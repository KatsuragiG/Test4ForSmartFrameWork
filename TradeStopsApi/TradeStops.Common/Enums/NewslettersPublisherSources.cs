namespace TradeStops.Common.Enums
{
    // ex-PortfolioSourceTypes enum
    // todo: better name is NewslettersDataSources
    // todo: Enum is used both for newsletters and for some others PortfolioSource-scenarios outside of API. Separate usages when obsolete projects will be disabled/removed
    // Enum determines the sources of the positions from newsletters portfolios.
    public enum NewslettersPublisherSources
    {
        TradeStops = 1, // used outside of API in non-newsletters scenarios :-/

        PortfolioTracker = 2,

        BillionaireTradeStops = 3,

        SymbolCryptoStops = 4,

        Indexes = 5,

        // LikeFolioResearch = 6, removed

        // StrategyIdeas = 7, removed

        PortfolioTracker2 = 8,
    }
}
