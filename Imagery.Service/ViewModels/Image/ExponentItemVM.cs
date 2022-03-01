using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagery.Service.ViewModels.Image
{
    public class ExponentItemVM
    {
        public string Name { get; set; }
        public string Creator { get; set; }
        public string Description { get; set; }
        public string Dimensions { get; set; } // napraviti kao listu 
        public double Price { get; set; }
        public string Image { get; set; }
    }
}
