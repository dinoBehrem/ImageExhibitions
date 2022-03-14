using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagery.Service.ViewModels.Image
{
    public class DimensionsVM
    {
        [Required(ErrorMessage = "Dimension is a required field!")]
        public string Dimension { get; set; }

        [Required(ErrorMessage = "Price is a required field!")]
        public double Price { get; set; }

        public int Id { get; set; }
    }
}
