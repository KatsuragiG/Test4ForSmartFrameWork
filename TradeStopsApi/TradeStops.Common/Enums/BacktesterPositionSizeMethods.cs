namespace TradeStops.Common.Enums
{
    /// <summary>
    /// Position size methods determine how big should be position size of position (in dollars).
    /// In other words it determines how many shares of the stock should be bought by backtesting process.
    /// </summary>
    public enum BacktesterPositionSizeMethods
    {
        /// <summary>
        /// Original position size means that we just use the same number of shares that was originally bought by user
        /// </summary>
        OriginalPositionSize = 1, //Original Position Size    -> position_size_method = "orig_pos_size"

        EqualVqRisk  = 2, //Equal Risk VQ             -> position_size_method = "equal_risk_vq"
        EqualSize    = 3, //Dollar Limited            -> position_size_method = "equal_size"
        EqualSsiRisk = 4, //Equal Risk SSI            -> position_size_method = "equal_risk_ssi"
        FixedRisk    = 5, //Fixed Risk Percent        -> position_size_method = "fixed_risk_percent"

        //Equal Risk                -> position_size_method = "equal_risk"
        //Specified #Shares         -> position_size_method = "equal_shares" // decimal? 'shares' parameter required
        //Equal Dollar Distribution -> position_size_method = "equal_dollars" // seems like no other parameters required
    }
}
