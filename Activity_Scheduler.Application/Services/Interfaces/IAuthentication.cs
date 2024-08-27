using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Activity_Scheduler.Core.Models;
using Activity_Scheduler.Core.ViewModels;

namespace Activity_Scheduler.Application.Services.Interfaces
{
    public interface IAuthentication
    {
        Task<ApplicationUser> CheckUser(string email, string password = "password");
        Task<ApplicationUser> Register(ApplicationUserViewModel User);
        Task<bool> ConfirmEmail(ApplicationUser user, string emailContent);
        Task<string> UpdateConfirmationStatus(string userId);
        Task<bool> IsUserConfirmed(string userId);
    }
}
