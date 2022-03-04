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
        public string Dimension { get; set; }
        public double Price { get; set; }

        [ForeignKey(nameof(ExponentItemId))]
        public ExponentItem ExponentItem { get; set; }
        public int ExponentItemId { get; set; }
    }
}
