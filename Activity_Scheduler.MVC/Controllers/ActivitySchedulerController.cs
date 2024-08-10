using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Activity_Scheduler.MVC.Controllers
{
    [Route("[controller]")]
    public class ActivitySchedulerController : Controller
    {
        private readonly ILogger<ActivitySchedulerController> _logger;

        public ActivitySchedulerController(ILogger<ActivitySchedulerController> logger)
        {
            _logger = logger;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}