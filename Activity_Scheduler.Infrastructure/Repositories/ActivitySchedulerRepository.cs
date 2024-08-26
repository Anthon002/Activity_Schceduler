using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Activity_Scheduler.Application.IRepositories;
using Activity_Scheduler.Core.DTO;
using Activity_Scheduler.Core.Models;
using Activity_Scheduler.Core.ViewModels;
using Activity_Scheduler.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Activity_Scheduler.Infrastructure.Repositories
{
    public class ActivitySchedulerRepository:IActivitySchedulerRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public ActivitySchedulerRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<ActivityViewModel>> GetActivities(string UserId)
        {
            try{
            List<ActivityViewModel> activities =  _dbContext.ActivityTable.Where(x => x.UserId == UserId).Select(x => new ActivityViewModel{
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                EndDate = x.EndDate,
                }).ToList();
                if (activities == null)
                {
                    return null;
                }
                return activities;
                
            } catch (Exception e){
                Console.WriteLine(e.Message);
                Console.WriteLine("Inner Exception");
                Console.WriteLine(e.InnerException);
            };
            return null;
        }
        public async Task<string> CreateActivity(Activity activity)
        {
            await _dbContext.ActivityTable.AddAsync(activity);
            int response = await _dbContext.SaveChangesAsync();
            if (response > 0)
            {
                return "true";
            }

            return ("false");
        }
        public async Task<Activity> GetActivity(string Id)
        {
            Activity activity= await _dbContext.ActivityTable.FirstOrDefaultAsync(x => x.Id == Id);
            if (activity == null)
            {
             Activity _error = new Activity(){Description = "activity not found"};
             return _error;   
            }
            return activity;
        }
        public async Task<string> DeleteActivity(string Id)
        {
            Activity activity= await _dbContext.ActivityTable.FirstOrDefaultAsync(x => x.Id == Id);
            if (activity == null)
            {
                return null;
            }
            _dbContext.ActivityTable.Remove(activity);
            var response = await _dbContext.SaveChangesAsync();
            if (response == null)
            {
                return null;
            }
            return response.ToString();  
        }
        public async Task<string> RemoveExpiredActivity(string Id)
        {
            Activity activity= await _dbContext.ActivityTable.FirstOrDefaultAsync(x => x.Id == Id);
            if (activity == null)
            {
                return null;
            }
            ExpiredActivity expiredActivity = new ExpiredActivity(){
                Id = activity.Id,
                Title = activity.Title,
                Description = activity.Description,
                EndDate = activity.EndDate,
                UserId = activity.UserId,
            };
            _dbContext.ExpiredActivityTable.Add(expiredActivity);
            _dbContext.ActivityTable.Remove(activity);
            var response = await _dbContext.SaveChangesAsync();
            if (response == null)
            {
                return null;
            }
            return response.ToString();  
        }
        public async Task<string> CompleteActivity(string Id)
        {
            Activity activity = await _dbContext.ActivityTable.FirstOrDefaultAsync(x=> x.Id == Id);
            if (activity == null)
            {
                return null;
            }
            CompletedActivity completedActivity = new CompletedActivity(){
                Id = activity.Id,
                Title = activity.Title,
                Description = activity.Description,
                EndDate = activity.EndDate,
                UserId = activity.UserId,
            };
            activity.Status ="Completed";
            _dbContext.CompletedActivityTable.Add(completedActivity);
            _dbContext.ActivityTable.Remove(activity);
            var respone = await _dbContext.SaveChangesAsync();
            if (respone == null)
            {
                return null;
            }
            return respone.ToString();
        }
        public async Task<List<ActivityViewModel>> GetCompletedActivities()
        {
            List<ActivityViewModel> activities =await _dbContext.CompletedActivityTable.Select(x => new ActivityViewModel{
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                EndDate = x.EndDate,

            }).ToListAsync();
            if (activities == null || activities.Count == 0) 
            {
                return null;
            }
            return activities;
        }

        public async Task<string> DeleteExpiredActivity(string Id)
        {
            ExpiredActivity activity = _dbContext.ExpiredActivityTable.FirstOrDefault(x => x.Id == Id);
            if (activity == null)
            {
                return null;
            }
            _dbContext.ExpiredActivityTable.Remove(activity);
            var response = await _dbContext.SaveChangesAsync();
            return response.ToString();
        }

        public async Task<string> DeleteCompletedActivity(string Id)
        {
            CompletedActivity activity = _dbContext.CompletedActivityTable.FirstOrDefault(x => x.Id == Id);
            if (activity == null)
            {
                return null;
            }
            _dbContext.CompletedActivityTable.Remove(activity);
            var response = await _dbContext.SaveChangesAsync();
            return response.ToString();
        }

        public async Task<List<ActivityViewModel>> GetExpiredActivities()
        {
          List<ActivityViewModel> activities =await _dbContext.ExpiredActivityTable.Select(x => new ActivityViewModel{
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                EndDate = x.EndDate,
          }).ToListAsync();  
            if (activities == null || activities.Count == 0) 
            {
                return null;
            }
            return activities;
        }
    }
}
