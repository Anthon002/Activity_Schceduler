using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Activity_Scheduler.Core.Models
{
    public class Activity
    {
        [Required]
        public string Id {get ; set; }
        [Required]
        public string Title {get; set;}
        [Required]
        public string Description {get; set;}
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Created {get; set;}
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime StartDate {get;set;}
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime EndDate{get;set;}
        [Required]
        public string UserId{get;set;}
    }
}