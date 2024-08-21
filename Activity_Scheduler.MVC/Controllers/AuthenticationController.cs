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

        public AuthenticationController(ILogger<AuthenticationController> logger, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signinManager, IAuthentication authentication)
        {
            _logger = logger;
            _userManager = userManager;
            _signinManager = signinManager;
            _authentication = authentication;
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
                await _signinManager.SignInAsync(user,isPersistent: false);
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
            ApplicationUser newUser = await _authentication.Register(user);
            if (user == null)
            {
                return null;
            }
            _signinManager.SignInAsync(newUser , isPersistent: false);
            return RedirectToAction("Index","ActivityScheduler");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}