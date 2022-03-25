using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagery.Core.Models
{
    public class Dimensions
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Dimensions are required")]
        public string Dimension { get; set; }

        [Required(ErrorMessage = "Price is required")]
        public double Price { get; set; }

        [ForeignKey(nameof(ExponentItemId))]
        public ExponentItem ExponentItem { get; set; }

        [Required(ErrorMessage = "Item is required")]
        public int ExponentItemId { get; set; }
    }
}
