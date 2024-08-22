using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Activity_Scheduler.Core.DTO;
using Activity_Scheduler.Core.ViewModels;

namespace Activity_Scheduler.Application.Services.Interfaces
{
    public interface IActivityScheduler
    {
        Task<List<ActivityViewModel>> GetActivites(string UserId);
        Task<string> CreateActivity(ActivityViewModel newActivity, System.Security.Claims.ClaimsPrincipal user);
        Task<ActivityDTO> GetActivity(string Id);
    }
}
