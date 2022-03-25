using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagery.Core.Models
{
    public class ExponentItem
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Creator is required")]
        public string Creator { get; set; }
        public string Description { get; set; }

        [Required(ErrorMessage = "Image is required")]
        public string Image { get; set; } 

        [ForeignKey(nameof(ExhibitionId))]
        public Exhibition Exhibition { get; set; }

        [Required(ErrorMessage = "Exhibition is required")]
        public int ExhibitionId { get; set; }
    }
}
