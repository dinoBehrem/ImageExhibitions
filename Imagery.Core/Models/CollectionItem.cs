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
        public string Image { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Creator { get; set; }
        public double Price { get; set; }
        public string Dimensions { get; set; }
        public string ExhibitionTitle { get; set; }
        public string Organizer { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        public string UserId { get; set; }
    }
}
