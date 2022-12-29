using System;
using System.Collections.Generic;

namespace Core.Infrastructure.Paging
{
    public class GroupSelector<TElement>
    {
        public Func<TElement, object> Selector { get; set; }
        public string Field { get; set; }
        public IEnumerable<Aggregator> Aggregates { get; set; }
    }
}
