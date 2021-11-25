namespace TradeStops.Common.Enums
{
    public enum InvestmentStrategyTypes
    {
        SectorSelects = 1,
        KineticVq = 2,
        BestOfBillionaires = 3,
        LowRiskRunners = 4,
        BestFromAllStrategies = 5,

        LargeCapTop = 6,
        MidCapTop = 7,
        SmallCapTop = 8,
        Sp500Top = 9,
        NasdaqTop = 10,
        DowTop = 11,

        DividendGrowers = 12,
        RevenueGrowthAndShrinkingDebt = 13,
        Growth = 14,
        Value =  15,

        CryptoKineticVq = 16,
        CryptoMomentumPairing = 17,
        CryptoMomentumAndVqPairing = 18,
        CryptoLowRiskRunners = 19,
        CryptoUnderestimatedVq = 20,

        TimingRsiRebounds = 21,

        TradeSmithNakedPut = 22,
        TradeSmithCoveredCall = 23,
        CoveredCall = 24, // This strategy doesn't have any results in CurrentStrategyResult and used only to filter results in OptionsScreener.
        TradeSmithBuyCall = 25
    }
}
