using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagery.Service.ViewModels.Exhbition
{
    public class FilterVM
    {
        public string CreatorName { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public double AvgPriceMin { get; set; }
        public double AvgPriceMax { get; set; }
        public List<TopicVM> Topics { get; set; }
    }
}
