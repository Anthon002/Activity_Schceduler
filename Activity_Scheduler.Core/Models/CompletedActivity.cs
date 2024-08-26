using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Activity_Scheduler.Core.Models
{
    public class CompletedActivity
    {
        public string Id {get ; set; }
        public string Title {get; set;}
        public string Description {get; set;}
        public DateTime StartDate {get;set;}
        public DateTime EndDate{get;set;}
        public int reminderTime{get; set;}
        public string UserId{get;set;}
        public string Status {get; set;} = "Active";
    }
}
