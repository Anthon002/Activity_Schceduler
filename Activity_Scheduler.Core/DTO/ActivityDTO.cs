using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Activity_Scheduler.Core.DTO
{
    public class ActivityDTO
    {
        public string Id {get ; set; }
        public string Title {get; set;}
        public string Description {get; set;}
        public string UserId{get;set;}
        public int Duration{get; set;}
    }
}