using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Activity_Scheduler.Application.Services.Interfaces;
using Activity_Scheduler.Core.Models;
using Activity_Scheduler.Core.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Activity_Scheduler.MVC.Controllers
{
    public class ActivitySchedulerController : Controller
    {
        private readonly ILogger<ActivitySchedulerController> _logger;
        private readonly IActivityScheduler _activityScheduler;
        private readonly UserManager<ApplicationUser> _userManager;

        public ActivitySchedulerController(ILogger<ActivitySchedulerController> logger, IActivityScheduler activityScheduler, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _activityScheduler = activityScheduler;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<ActionResult> GetActivities()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login","Authentication");
            }
            var userId = _userManager.GetUserId(User).ToString();
            if (userId == null)
            {
                return NotFound();
            }
            List<ActivityViewModel> activities = await _activityScheduler.GetActivites(userId);
            return Json(activities);
        }
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login","Authentication");
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
