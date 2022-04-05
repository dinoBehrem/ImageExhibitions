using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagery.Service.ViewModels.Image
{
    public class CollectionVM
    {
        public string Username { get; set; }
        public List<CollectionItemVM> Collection { get; set; }
    }
}
