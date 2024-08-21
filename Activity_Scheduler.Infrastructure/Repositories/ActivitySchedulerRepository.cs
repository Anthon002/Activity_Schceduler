using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Activity_Scheduler.Application.IRepositories;
using Activity_Scheduler.Core.Models;
using Activity_Scheduler.Core.ViewModels;
using Activity_Scheduler.Infrastructure.Data;

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
    }
}
