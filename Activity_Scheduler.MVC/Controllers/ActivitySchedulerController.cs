using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Activity_Scheduler.Application.Services.Interfaces;
using Activity_Scheduler.Core.DTO;
using Activity_Scheduler.Core.Models;
using Activity_Scheduler.Core.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
        [HttpGet]
        public async Task<ActionResult> CreateNewActivity()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> CreateNewActivity(ActivityViewModel newActivity)
        {
            if (newActivity == null)
            {
                return View();
            }
            if (ModelState.IsValid)
            {
                System.Security.Claims.ClaimsPrincipal user = User;
                var respnose = await _activityScheduler.CreateActivity(newActivity, user);
                if (respnose != null)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(newActivity);
        }
        [HttpGet]
        public async Task<ActionResult> Details(string Id)
        {
            ViewData["ErrorMessage"] = "";
            if (Id == null)
            {
                ViewData["ErrorMessage"] = "invalid activity";
                return View();
            }
            ActivityDTO activity = await _activityScheduler.GetActivity(Id);
            if(activity == null)
            {
                ViewData["ErrorMessage"] = "activity not found";
                return View();
            }
            return View(activity);
        }
        [HttpGet]
        public async Task<ActionResult> Delete(string Id)
        {
            ViewData["ErrorMessage"] ="";
            if (Id == null)
            {
                ViewData["ErrorMessage"] = "Invalid activity";
                return View();
            }
           ActivityDTO activity = await _activityScheduler.GetActivity(Id);
            return View(activity);
        }
        [HttpPost]
        public async Task<ActionResult> DeleteActivity(string Id)
        {
            var respone = await _activityScheduler.DeleteActivity(Id);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<ActionResult> CompleteActivity(string Id)
        {
            var response = await _activityScheduler.CompleteActivity(Id);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<ActionResult> CompletedActivities()
        {
            List<ActivityViewModel> activities =await _activityScheduler.GetCompletedActivities();
            return Json(activities);
        }
        [HttpGet]
        public async Task<ActionResult> CompletedActivitiesPage()
        {
            return View();
        }
        public async Task<ActionResult> ExpiredActivities()
        {
            List<ActivityViewModel> activities = await _activityScheduler.GetExpiredActivities();
            return Json(activities);
        }
        [HttpGet]
        public async Task<ActionResult> DeleteExpiredActivities(string Id)
        {
            await _activityScheduler.DeleteExpiredActivity(Id);
            return RedirectToAction("ExpiredActivitiesPage");
        }
        [HttpGet]
        public async Task<ActionResult> ExpiredActivitiesPage()
        {
            return View();
        }
        [HttpGet]
        public async Task<ActionResult> DeleteCompletedActivities(string Id)
        {
            await _activityScheduler.DeleteCompletedActivity(Id);
            return RedirectToAction("CompletedActivitiesPage");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
