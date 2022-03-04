using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagery.Service.ViewModels.Image
{
    public class ExponentItemVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Creator { get; set; }
        public string Description { get; set; }
        public List<DimensionsVM> Dimensions { get; set; }
        public string Image { get; set; }

        public double AveragePrice
        {
            get
            {
                if (Dimensions.Count > 0)
                {
                    return Dimensions.Average(dimension => dimension.Price);
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if (Dimensions.Count > 0)
                {
                    AveragePrice = Dimensions.Average(dimension => dimension.Price);
                }
                else
                {
                    AveragePrice = 0;
                }
            }
        }
    }
}
