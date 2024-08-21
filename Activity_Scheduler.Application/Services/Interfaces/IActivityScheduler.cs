using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Activity_Scheduler.Core.ViewModels;

namespace Activity_Scheduler.Application.Services.Interfaces
{
    public interface IActivityScheduler
    {
        Task<List<ActivityViewModel>> GetActivites(string UserId);
    }
}
