using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagery.Service.ViewModels.Image
{
    public class CollectionItemVM
    {
        public string Image { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Creator { get; set; }
        public double Price { get; set; }
        public string Dimensions { get; set; }
        public string Exhibition { get; set; }
        public string Organizer { get; set; }
        public string Customer { get; set; }
        public int ExhibitionId { get; set; }
        public int Quantity { get; set; }
    }
}
