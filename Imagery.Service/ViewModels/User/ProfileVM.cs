using Imagery.Service.ViewModels.Exhbition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagery.Service.ViewModels.User
{
    public class ProfileVM
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Biography { get; set; }
        public string ProfilePicture { get; set; }
        public List<UserVM> Followers { get; set; }
        public List<UserVM> Following { get; set; }
        public List<ExhibitionVM> Exhibitions { get; set; }
        //public List<ExhibitionProfileVM> Exhibitions { get; set; }
        public List<string> Roles { get; set; }
    }
}
