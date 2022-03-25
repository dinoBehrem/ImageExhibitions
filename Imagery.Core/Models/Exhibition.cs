using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagery.Core.Models
{
    public class Exhibition
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }
        public string Description { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; }
        public DateTime ExpiringTime { get; set; }
        public string CoverImage { get; set; }

        [ForeignKey(nameof(OrganizerId))]
        public User Organizer { get; set; }

        [Required(ErrorMessage = "Organizer is required")]
        public string OrganizerId { get; set; }
    }
}
