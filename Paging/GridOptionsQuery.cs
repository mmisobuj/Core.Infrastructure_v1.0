using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure.Paging
{
    public class GridOptionsQuery
    {
        public int take { get; set; }
        public int skip { get; set; }
        public List<Sort> sort { get; set; }
        public Filter filter { get; set; }
    }
}
