using System;

namespace TradeStops.Common.Enums
{
    [Obsolete("Why do you need this enum? Use UserFeaturesContract instead.")]
    public enum SiteServiceLevels
    {
        Basic = 1,
        Plus = 2,
        FreeBasic = 3,
        LifetimePro = 4,
        FreePremium = 5,
        Premium = 6,
        FreePlus = 7,
        PortfolioLite = 8,
        PortfolioAnalyzer = 9,
        Pro = 10,
    }
}
