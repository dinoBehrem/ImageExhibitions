using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagery.Service.ViewModels.Exhbition
{
    public class ExhbitionCreationVM
    {
        [Required(ErrorMessage = "Title is required field!")]
        public string Title { get; set; }
        public string Description { get; set; }

        [Required(ErrorMessage = "Date is required field!")]
        public DateTime StartingDate { get; set; }

        [Required (ErrorMessage = "You are not signed user, please sign in!")]
        public string Organizer { get; set; }
    }
}
