using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagery.Core.Models
{
    public class CollectionItem
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Image is required")]
        public string Image { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required(ErrorMessage = "Creator is required")]
        public string Creator { get; set; }

        [Required(ErrorMessage = "Price is required")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Dimensions are required")]
        public string Dimensions { get; set; }

        [Required(ErrorMessage = "Exhibition is required")]
        public string ExhibitionTitle { get; set; }

        [Required(ErrorMessage = "Organizer is required")]
        public string Organizer { get; set; }

        public int ExhibitionId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        [Required(ErrorMessage = "Customer is required")]
        public string UserId { get; set; }
    }
}
