using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagery.Service.ViewModels.Image
{
    public class ItemUploadVM
    {
        [Required(ErrorMessage = "Exhibtion is required!")]
        public int ExhibitionId { get; set; }
        [Required(ErrorMessage = "Name is a required field!")]
        public string Name { get; set; }
        public string Creator { get; set; }
        public string ImageDescription { get; set; }
        public IFormFile Image { get; set; }

    }
}
