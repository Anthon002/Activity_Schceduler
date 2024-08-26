using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Activity_Scheduler.Core.ViewModels;
using Activity_Scheduler.Core.Models;
using Activity_Scheduler.Core.DTO;

namespace Activity_Scheduler.Application.IRepositories
{
    public interface IActivitySchedulerRepository
    {
        Task<List<ActivityViewModel>> GetActivities(string UserId);
        Task<string> CreateActivity(Activity activity);
        Task<Activity> GetActivity(string Id);
        Task<string> DeleteActivity(string Id);
        Task<string> CompleteActivity(string Id);
        Task<List<ActivityViewModel>> GetCompletedActivities();
        Task<List<ActivityViewModel>> GetExpiredActivities();
        Task<string> RemoveExpiredActivity(string Id);
        Task<string> DeleteExpiredActivity(string Id);
        Task<string> DeleteCompletedActivity(string Id);
    }
}
