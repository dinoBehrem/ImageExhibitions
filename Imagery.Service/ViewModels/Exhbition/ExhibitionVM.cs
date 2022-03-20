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
        public bool Expired { get; set; }
        public bool Started { get; set; }
        public UserVM Organizer { get; set; }
        public string Cover { get; set; }
        public int Subscribers { get; set; }
        public List<ExponentItemVM> Items { get; set; }
        public List<TopicVM> Topics { get; set; }

        public double AveragePrice
        {
            get
            {
                if (Items.Count > 0)
                {
                    return Items.Average(item => item.AveragePrice);
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if (Items.Count > 0)
                {
                    AveragePrice = Items.Average(item => item.AveragePrice);
                }
                else
                {
                    AveragePrice = 0;
                }
            }
        }

    }
}
