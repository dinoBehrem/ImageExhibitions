using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagery.Service.ViewModels.User
{
    public class UserEditVM
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Biography { get; set; }
        //public IFormFile ProfilePicture { get; set; }
        //public string Image { get; set; }
    }
}
