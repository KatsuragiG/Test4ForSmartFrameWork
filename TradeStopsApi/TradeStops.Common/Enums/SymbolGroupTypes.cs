namespace TradeStops.Common.Enums
{
    public enum SymbolGroupTypes
    {
        // Symbol Group Ids are taken from SymbolGroups table in HistoricalDataNew DB

        // select *
        // from HistoricalDataNew..SymbolGroup sg
        // left join tradeideas..MarketOutlooks mo on sg.SymbolId = mo.symbolid

        None = 0,

        Dow30 = 1,
        Nasdaq100 = 5,
        Russell1000 = 11,
        Russell2000 = 12,
        SP100 = 14,
        SP400 = 15,
        SP500 = 16,

        XLE = 40,
        XLY = 41,
        XLP = 42,
        XLF = 43,
        XLV = 44,
        XLI = 45,
        XLB = 46,
        XLK = 47,
        XLU = 48,
        XLC = 64,
        XLRE = 406,

        BloombergCommodityIndex = 55,

        SP600 = 57,

        Australia = 51,
        LondonFtse100 = 52, // has name 'United Kingdom' in TradeIdeas
        Canada = 61,
        AmexGoldBugs = 62,

        HongKong = 50, // not supported currency in TS
        Japan = 63, // not supported currency in TS
    }
}
