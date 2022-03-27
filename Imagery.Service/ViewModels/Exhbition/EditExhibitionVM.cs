using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagery.Service.ViewModels.Exhbition
{
    public class EditExhibitionVM
    {

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }
        public string Description { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; }
        public List<TopicVM> Topics { get; set; }

    }
}
