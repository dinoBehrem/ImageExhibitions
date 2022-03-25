using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagery.Core.Models
{
    public class ExhibitionSubscription
    {
        [Required(ErrorMessage = "Exhibition is required")]
        public int ExhibitionId { get; set; }

        [Required(ErrorMessage = "User is required")]
        public string UserId { get; set; }
    }
}
