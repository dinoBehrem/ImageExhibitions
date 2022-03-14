using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagery.Service.ViewModels.Image
{
    public class EditItemVM
    {
        [Required(ErrorMessage = "Name is required!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Creator is required!")]
        public string Creator { get; set; }
        public string ImageDescription { get; set; }
        public string ImagePath { get; set; }
        public IFormFile Image { get; set; }
    }
}
