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
            _dbContext.ActivityTable.Remove(activity);
            var response = await _dbContext.SaveChangesAsync();
            if (response == null)
            {
                return null;
            }
            return response.ToString();
            
        }
    }
}
