using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagery.Service.ViewModels.Exhbition
{
    public class MyExhibitionVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public bool Expired { get; set; }
        public bool Started { get; set; }
        public string Cover { get; set; }
        public int Subscribers { get; set; }
        public int Items { get; set; }
        public int SoldItems { get; set; }
        public double Profit { get; set; }
        public List<TopicVM> Topics { get; set; }
    }
}
