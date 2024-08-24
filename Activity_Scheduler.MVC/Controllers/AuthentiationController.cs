using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Activity_Scheduler.Core.Models;
using Activity_Scheduler.Application.Services.Interfaces;
using Activity_Scheduler.Core.ViewModels;

namespace Activity_Scheduler.MVC.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signinManager;
        private readonly IAuthentication _authentication;
        private readonly IHttpContextAccessor _httpcontextAccessor;

        public AuthenticationController(ILogger<AuthenticationController> logger, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signinManager, IAuthentication authentication, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _userManager = userManager;
            _signinManager = signinManager;
            _authentication = authentication;
            _httpcontextAccessor = httpContextAccessor;
        }
        [HttpGet]
        public async Task<ActionResult> Login()
        {
            if(User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "ActivityScheduler");
            }
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Login(string Email, string Password)
        {
            if(User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "ActivityScheduler");
            }   
            ApplicationUser user = await _authentication.CheckUser(Email,Password);
            if (user != null)
            {
                await _signinManager.SignInAsync(user,isPersistent: true);
            }
            return RedirectToAction("Index","ActivityScheduler");
        }
        [HttpGet]
        public async Task<ActionResult> Register()
        {
            if(User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "ActivityScheduler");
            }
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Register(ApplicationUserViewModel user)
        {
            if(User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "ActivityScheduler");
            }
            if(!ModelState.IsValid)
            {
                return View(user);
            }
            ViewData["UserExists"] = "";
            ApplicationUser newUser = await _authentication.Register(user);
            if(newUser == null)
            {
                ViewData["UserExists"] ="This User already exists";
            }
            if (user == null)
            {
                return null;
            }
            await _signinManager.SignInAsync(newUser , isPersistent: false);
            return RedirectToAction("Emailconfirmation");
        }
        [HttpPost]
        public async Task<ActionResult> Logout()
        {
            await _signinManager.SignOutAsync();
            return RedirectToAction("Login");
        }
        [HttpGet]
        public async Task<ActionResult>Emailconfirmation(string errorMessage=" ")
        {
            ViewData["errorMessage"] = errorMessage;
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> EmailConfirmation()
        {
            string confirmationToken = Guid.NewGuid().ToString();
            TempData["confirmationToken"] = confirmationToken;
            var request = _httpcontextAccessor.HttpContext.Request;
            string userName= User.Identity.Name;
            ApplicationUser user = await _userManager.GetUserAsync(User);
            string callbackUrl = Url.Action("ConfirmEmail","Authentication",new {userId = user.Id, token = confirmationToken, protocol = request.Scheme});
            string emailContent = $"{request.Scheme}://{request.Host}{callbackUrl}";
            Task<bool> response = _authentication.ConfirmEmail(user,emailContent);
            return RedirectToAction("WaitForResend");
        }
        [HttpGet]
        public async Task<ActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("Emailconfirmation",new{errorMessage = "Invalid userId or confirmation token"});
            }
            if (token != TempData["confirmationToken"].ToString())
            {
                return RedirectToAction("Emailconfirmation", new{errorMessage = "Confirmation Token does not match"});
            }
            await _authentication.UpdateConfirmationStatus(userId);
            return RedirectToAction("Index", "Activity_Scheduler");
        }
        [HttpGet]
        public async Task<ActionResult> WaitForResend()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
