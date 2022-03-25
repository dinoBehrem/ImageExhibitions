using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagery.Service.ViewModels.Image
{
    public class CoverImageVM
    {
        [Required(ErrorMessage = "Exhibition id is required!")]
        public int ExhibitionId { get; set; }
        
        [Required(ErrorMessage = "Cover image is required!")]
        public string CoverImage { get; set; }
    }
}
