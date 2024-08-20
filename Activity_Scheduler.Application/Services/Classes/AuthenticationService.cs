using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Activity_Scheduler.Application.Services.Interfaces;
using Activity_Scheduler.Core.Models;
using Activity_Scheduler.Core.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Activity_Scheduler.Application.Services.Classes
{
    public class AuthenticationService:IAuthentication
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public AuthenticationService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<ApplicationUser> CheckUser(string email, string password = "password")
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(email);
            if (user == null){return null;}
            var response = await _userManager.CheckPasswordAsync(user,password);
            if (response == false){return null;}
            return user;
        }
        public async Task<ApplicationUser> Register(ApplicationUserViewModel user)
       {
        ApplicationUser userExists = await _userManager.FindByEmailAsync(user.Email);
        if (userExists != null)
        {
            return null;
        }
        ApplicationUser newUser = new ApplicationUser(){FirstName = user.FirstName, LastName = user.LastName, Password = user.Password, Email = user.Email, UserName = user.Email};
        var response = await _userManager.CreateAsync(newUser,newUser.Password);
        if (!response.Succeeded)
        {
            return null;
        }
        return newUser;
       }

    }
}
