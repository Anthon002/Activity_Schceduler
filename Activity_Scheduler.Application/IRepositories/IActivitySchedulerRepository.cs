using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Activity_Scheduler.Core.ViewModels;

namespace Activity_Scheduler.Application.IRepositories
{
    public interface IActivitySchedulerRepository
    {
        Task<List<ActivityViewModel>> GetActivities(string UserId);
    }
}