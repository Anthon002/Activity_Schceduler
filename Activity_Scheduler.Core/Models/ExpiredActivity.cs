
namespace Activity_Scheduler.Core.Models
{
    public class ExpiredActivity
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