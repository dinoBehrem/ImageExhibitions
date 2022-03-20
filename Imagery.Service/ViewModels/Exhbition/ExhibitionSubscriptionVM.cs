using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagery.Service.ViewModels.Exhbition
{
    public class ExhibitionSubscriptionVM
    {
        [Required(ErrorMessage ="Exhibition id is required")]
        public int ExhibitionId { get; set; }
        
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }
    }
}
