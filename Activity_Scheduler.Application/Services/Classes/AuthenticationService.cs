using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Activity_Scheduler.Application.Services.Interfaces;
using Activity_Scheduler.Core.Models;
using Activity_Scheduler.Core.ViewModels;
using Microsoft.AspNetCore.Identity;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Model;

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
       public async Task<bool> ConfirmEmail(ApplicationUser user, string emailContent)
       {
            Configuration.Default.ApiKey["api-key"] = "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";

            var apiInstance = new TransactionalEmailsApi();
            string SenderName = "Chinedu Anulugwo";
            string SenderEmail = "chineduanulugwo@gmail.com";
            SendSmtpEmailSender Email = new SendSmtpEmailSender(SenderName, SenderEmail);
            string ToEmail = user.Email.ToString().ToLower();
            string ToName = user.UserName.ToString();
            SendSmtpEmailTo smtpEmailTo = new SendSmtpEmailTo(ToEmail, ToName);
            List<SendSmtpEmailTo> To = new List<SendSmtpEmailTo>();
            To.Add(smtpEmailTo);
            string HtmlContent = null;
            string TextContent = emailContent;
            string Subject = "Email Confirmation";            

            try
            {
                var sendSmtpEmail = new SendSmtpEmail(Email, To, null, null, HtmlContent, TextContent, Subject);
                CreateSmtpEmail result = apiInstance.SendTransacEmail(sendSmtpEmail);
                Console.WriteLine("Response: \n" + result.ToJson());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }
            return true;
       }
       public async Task<string> UpdateConfirmationStatus(string id)
       {
        ApplicationUser user = await _userManager.FindByIdAsync(id)?? new ApplicationUser();
        user.EmailConfirmed = true;
        await _userManager.UpdateAsync(user);
        return "done";
       }

    }
}
