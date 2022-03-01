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
        public string Name { get; set; }
        public string Creator { get; set; }
        public string Description { get; set; }
        public string Dimensions { get; set; } // napraviti kao listu 
        public double Price { get; set; }
        public string Image { get; set; } 

        [ForeignKey(nameof(ExhibitionId))]
        public Exhibition Exhibition { get; set; }
        public int ExhibitionId { get; set; }
    }
}
