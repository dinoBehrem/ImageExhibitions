using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagery.Core.Models
{
    public class UserSubscription
    {
        [Required(ErrorMessage = "Subscriber is required")]
        public string SubscriberId { get; set; }

        [Required(ErrorMessage = "Creator is required")]
        public string CreatorId { get; set; }
    }
}
