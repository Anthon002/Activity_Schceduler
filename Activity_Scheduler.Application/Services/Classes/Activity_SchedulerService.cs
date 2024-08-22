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
             string endDateStr = newActivity.EndDate.ToString();
             DateTime endDateUtc = DateTime.Parse(endDateStr).ToUniversalTime();
             Activity activity = new Activity(){Id = Guid.NewGuid().ToString(), Title = newActivity.Title, Description = newActivity.Description, EndDate = endDateUtc, StartDate = DateTime.Now, UserId = userId};
             return await _activitySchedulerRepository.CreateActivity(activity);
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
                UserId = activity.UserId
            };
            return activityDTO;
        }
    }
}
