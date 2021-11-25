using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace TradeStops.Contracts.Generic
{
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class SearchResultContract<T>
    {
        public int TotalCount { get; set; }

        public IEnumerable<T> Items { get; set; }
    }
}
