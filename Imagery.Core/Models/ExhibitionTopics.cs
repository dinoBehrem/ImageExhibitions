using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagery.Core.Models
{
    public class ExhibitionTopics
    {
        [Required(ErrorMessage = "Exhibition is required")]
        public int ExhibitionId { get; set; }

        [Required(ErrorMessage = "Topic is required")]
        public int TopicId { get; set; }
    }
}
