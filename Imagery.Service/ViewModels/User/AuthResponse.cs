using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagery.Service.ViewModels.User
{
    public class AuthResponse
    {
        public string Token { get; set; }
        public DateTime Expiriation { get; set; }
    }
}
