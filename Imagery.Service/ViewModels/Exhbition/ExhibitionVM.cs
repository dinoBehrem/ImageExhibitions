using Imagery.Service.ViewModels.Image;
using Imagery.Service.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagery.Service.ViewModels.Exhbition
{
    public class ExhibitionVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public UserVM Organizer { get; set; }
        public string Cover { get; set; }
        public List<ExponentItemVM> Items { get; set; }
        public double AveragePrice { get 
            {
                if (Items.Count > 0)
                {
                    return Items.Average(item => item.Price);
                }else
                {
                    return 0;
                }
            } set 
            {
                if(Items.Count > 0)
                {
                    AveragePrice = Items.Average(item => item.Price);
                }else
                {
                    AveragePrice = 0;
                }
            } 
        }

    }
}
