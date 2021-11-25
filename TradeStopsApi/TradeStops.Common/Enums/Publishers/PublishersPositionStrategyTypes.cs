namespace TradeStops.Common.Enums
{
    public enum PublishersPositionStrategyTypes
    {
        DefaultCombinedStrategy = 10,

        PairTradeCombinedStrategy = 20,

        OnlyOneSymbolCombinedStrategy = 30,

        NakedPutCombinedStrategy = 40,

        BoughtOptionsWithSoldPutCombinedStrategy = 50,

        BoughtOptionsWithoutSoldPutCombinedStrategy = 60,

        CoveredCallsCombinedStrategy = 70,

        OptionSpreadsExercisedCombinedStrategy = 80,

        OptionSpreadsExpiredWorthlessCombinedStrategy = 90,

        OptionSpreadsHoldAndCloseCombinedStrategy = 100,

        PutSharesSellCallCombinedStrategy = 110,

        PutSharesBuyCallCombinedStrategy = 120,

        DefaultRegularStrategy = 130,

        CspRegularStrategy = 140,

        WsdRegularStrategy = 150
    }
}
