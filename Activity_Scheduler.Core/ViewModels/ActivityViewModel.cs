using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Activity_Scheduler.Core.ViewModels
{
    public class ActivityViewModel
    {
        public string Id {get;set;}
        public string Title {get;set;}
        public string Description{get;set;}
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime EndDate{get;set;}

    }
}