using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagery.Service.ViewModels.User
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "First name is required!")]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "Last name is required!")]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "Username is required!")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is required!")]
        [EmailAddress]
        [StringLength(30,MinimumLength = 8)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required!")]
        [StringLength(40, MinimumLength = 8)]
        public string Password { get; set; }
        public string Image { get; set; }
    }
}
