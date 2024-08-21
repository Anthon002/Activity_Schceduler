using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Activity_Scheduler.Core.ViewModels
{
    public class ApplicationUserViewModel
    {
        [Required]
        public string FirstName{get; set;}
        [Required]
        public string LastName{get; set;}
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email{get; set;}
        [Required]
        [DataType(DataType.Password)]
        public string Password {get; set;}
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage = "Password does not match confirm password")]
        public string ConfirmPassword {get; set;}

    }
}