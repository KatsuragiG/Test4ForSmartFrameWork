using System.Collections.Generic;
using System.Collections.ObjectModel;
using TradeStops.Common.DataStructures;

namespace TradeStops.Common.Enums
{
    public static class MarketCapFilters
    {
        public static IReadOnlyDictionary<MarketCapTypes, DecimalFilter> Filters { get; } = new ReadOnlyDictionary<MarketCapTypes, DecimalFilter>(new Dictionary<MarketCapTypes, DecimalFilter>
        {
            { MarketCapTypes.Nano, new DecimalFilter { MinValue = 0, MaxValue = 50000000 } },
            { MarketCapTypes.Micro, new DecimalFilter { MinValue = 50000000, MaxValue = 300000000 } },
            { MarketCapTypes.Small, new DecimalFilter { MinValue = 300000000, MaxValue = 2000000000 } },
            { MarketCapTypes.Mid, new DecimalFilter { MinValue = 2000000000, MaxValue = 10000000000 } },
            { MarketCapTypes.Large, new DecimalFilter { MinValue = 10000000000, MaxValue = 200000000000 } },
            { MarketCapTypes.Mega, new DecimalFilter { MinValue = 200000000000, MaxValue = decimal.MaxValue } }
        });
    }
}
