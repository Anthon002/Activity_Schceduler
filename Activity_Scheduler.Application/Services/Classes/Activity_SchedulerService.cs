using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Activity_Scheduler.Application.IRepositories;
using Activity_Scheduler.Application.Services.Interfaces;
using Activity_Scheduler.Core.ViewModels;

namespace Activity_Scheduler.Application.Services.Classes
{
    public class Activity_SchedulerService:IActivityScheduler
    {
        private readonly IActivitySchedulerRepository _activitySchedulerRepository;
        public Activity_SchedulerService(IActivitySchedulerRepository activitySchedulerRepository)
        {
            _activitySchedulerRepository = activitySchedulerRepository;
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
    }
}