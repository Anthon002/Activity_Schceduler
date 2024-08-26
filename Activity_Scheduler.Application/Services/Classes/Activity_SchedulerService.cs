using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Activity_Scheduler.Application.IRepositories;
using Activity_Scheduler.Application.Services.Interfaces;
using Activity_Scheduler.Core.DTO;
using Activity_Scheduler.Core.Models;
using Activity_Scheduler.Core.ViewModels;
using Microsoft.AspNetCore.Identity;
using Hangfire;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Model;

namespace Activity_Scheduler.Application.Services.Classes
{
    public class Activity_SchedulerService:IActivityScheduler
    {
        private readonly IActivitySchedulerRepository _activitySchedulerRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        public Activity_SchedulerService(IActivitySchedulerRepository activitySchedulerRepository, UserManager<ApplicationUser> userManager)
        {
            _activitySchedulerRepository = activitySchedulerRepository;
            _userManager = userManager;
        }

        public async Task<List<ActivityViewModel>> GetActivites(string UserId)
        {
            List<ActivityViewModel> activities = await _activitySchedulerRepository.GetActivities(UserId);
            if (activities == null)
            {
                return null;
            }
            return activities;
        }
        public async Task<string> CreateActivity(ActivityViewModel newActivity, System.Security.Claims.ClaimsPrincipal user)
        {
             string userId = _userManager.GetUserId(user);
             string userEmail = _userManager.GetUserName(user);
             ApplicationUser _user = await _userManager.FindByIdAsync(userId);
             string endDateStr = newActivity.EndDate.ToString();
             DateTime endDateUtc = DateTime.Parse(endDateStr).ToUniversalTime();
             Activity activity = new Activity(){Id = Guid.NewGuid().ToString(), Title = newActivity.Title, Description = newActivity.Description, EndDate = endDateUtc, StartDate = DateTime.Now, UserId = userId, reminderTime = 30};
             string response = await _activitySchedulerRepository.CreateActivity(activity);
             if (response == "true")
             {
                //DateTime endDate = activity.EndDate;
                var _dateTime = activity.EndDate.AddMinutes(-(activity.reminderTime));
                var dateTimeOffSet = new DateTimeOffset(_dateTime);
                var endDateTimeOffSet = new DateTimeOffset(activity.EndDate);
                var emailResponse = SendReminderEmail(_user,activity,dateTimeOffSet, endDateTimeOffSet);
             }
             return response;
        }
        public async Task<string> SendReminderEmail(ApplicationUser user,Activity activity, DateTimeOffset sendDate, DateTimeOffset endDateTime)
        {
            BackgroundJob.Schedule(()=> ReminderEmail(user,activity),sendDate);
            BackgroundJob.Schedule(()=> AutoRemoveExpiredActivity(activity),endDateTime);
            return null;
        }
        public async Task<string> AutoRemoveExpiredActivity(Activity activity)
        {
            await _activitySchedulerRepository.RemoveExpiredActivity(activity.Id);

            return null;
        }
        public bool ReminderEmail(ApplicationUser user, Activity activity)
        {
            Configuration.Default.ApiKey["api-key"] = "api key";

            var apiInstance = new TransactionalEmailsApi();
            string SenderName = "Chinedu Anulugwo";
            string SenderEmail = "chineduanulugwo@gmail.com";
            SendSmtpEmailSender Email = new SendSmtpEmailSender(SenderName, SenderEmail);
            string ToEmail = user.Email.ToString().ToLower();
            string ToName = user.UserName.ToString();
            SendSmtpEmailTo smtpEmailTo = new SendSmtpEmailTo(ToEmail, ToName);
            List<SendSmtpEmailTo> To = new List<SendSmtpEmailTo>();
            To.Add(smtpEmailTo);
            string HtmlContent = null;
            string TextContent = $"Lets go complete it. {activity.Description} ";
            string Subject = $"{activity.Title} is due for completion";            

            try
            {
                var sendSmtpEmail = new SendSmtpEmail(Email, To, null, null, HtmlContent, TextContent, Subject);
                CreateSmtpEmail result = apiInstance.SendTransacEmail(sendSmtpEmail);
                Console.WriteLine("Response: \n" + result.ToJson());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }
            return true;
        }
        public async Task<ActivityDTO> GetActivity(string Id)
        {
            Activity activity = await _activitySchedulerRepository.GetActivity(Id);
            if (activity.Description == "activity not found")
            {
                ActivityDTO _activityDTO = new ActivityDTO(){ Description = activity.Description};
                return _activityDTO;
            }
            TimeSpan _duration = activity.EndDate - activity.StartDate;
            ActivityDTO activityDTO = new ActivityDTO(){
                Id = activity.Id,
                Title = activity.Title,
                Description = activity.Description,
                Duration = _duration.TotalHours,
                UserId = activity.UserId,
            };
            return activityDTO;
        }
        public async Task<string> DeleteActivity(string Id)
        {
            return await _activitySchedulerRepository.DeleteActivity(Id);
        }
        public async Task<string> CompleteActivity(string Id)
        {
            return await _activitySchedulerRepository.CompleteActivity(Id);
        }

        public async Task<List<ActivityViewModel>> GetCompletedActivities()
        {
            return await _activitySchedulerRepository.GetCompletedActivities();
        }

        public async Task<List<ActivityViewModel>> GetExpiredActivities()
        {
            return await _activitySchedulerRepository.GetExpiredActivities();
        }

        public async Task<string> DeleteExpiredActivity(string Id)
        {
            return await _activitySchedulerRepository.DeleteExpiredActivity(Id);
        }

        public async Task<string> DeleteCompletedActivity(string Id)
        {
            return await _activitySchedulerRepository.DeleteCompletedActivity(Id);
        }
    }
}
