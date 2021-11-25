namespace TradeStops.Common.Enums
{
    // usually (always?) PriceAdjustmentType should be the same as SharesAdjustmentType
    public enum PriceAdjustmentTypes
    {
        Unadjusted = 0, // hey API, my price is unajusted. I spent X dollars to buy this stock, and I don't know the adjusted price. Please adjust by everything

        SplitAdjusted = 1,

        SplitAndDividendAdjusted = 2
    }
}
