using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagery.Service.Helpers
{
    public class PageParameters
    {
        const int maxPageSize = 20;
        public int PageNumber { get; set; } = 1;

        int pageSize = 10;

        public int PageSize { get { return pageSize; } set { pageSize = (value > maxPageSize) ? maxPageSize : value; } }
    }
}
