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
    }
}