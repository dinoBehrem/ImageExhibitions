using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagery.Core.Models
{
    [Table("Users")]
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [StringLength(450,ErrorMessage ="Biography should be up to 450 characters length!")]
        public string Biography { get; set; }
        public string ProfilePicture { get; set; }
    }
}
