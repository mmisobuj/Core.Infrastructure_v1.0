using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Infrastructure.Grid
{
    public class KendoGridFilter
    {
        public class GridFilter
        {
            public string Operator { get; set; }
            public string Field { get; set; }
            public string Value { get; set; }
        }

        public class AutoCompFilter
        {
            public string field { get; set; }
            public bool ignore { get; set; }
            public string @operator { get; set; }
            public string value { get; set; }
        }

        public class GridFilters
        {
            public List<GridFilter> Filters { get; set; }
            public string Logic { get; set; }
        }

        public class AutoCompFilters
        {
            public List<AutoCompFilter> filters { get; set; }
            public string logic { get; set; }
        }


        public class GridSort
        {
            public string field { get; set; }

            public string dir { get; set; }
        }


        //-------------------------------------------------------------------
        public class SortDescription
        {
            public string field { get; set; }

            public string dir { get; set; }
        }

        public class FilterContainer
        {
            public List<FilterDescription> filters { get; set; }

            public string logic { get; set; }
        }

        public class FilterDescription
        {
            public string @operator { get; set; }

            public string field { get; set; }

            public string value { get; set; }
        }

        public class OrderForPaggging
        {
            public string OrderByField { get; set; }

            public string OrderByType { get; set; }
        }
    }
}
