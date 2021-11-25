using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to determine position size for backtesting process.
    /// </summary>
    public class BacktesterPositionSizeContract
    {
        /// <summary>
        /// Position size method determine how big should be position size of position (in dollars).
        /// In other words it determines how many shares of the stock should be bought by backtesting process.
        /// </summary>
        public BacktesterPositionSizeMethods Method { get; set; }

        /// <summary>
        /// Parameters for EqualVqRisk position size method.
        /// Required only in case if EqualVqRisk method is set in Method field.
        /// </summary>
        public BacktesterPositionSizeByEqualVqRiskParamsContract EqualVqRiskParams { get; set; }

        /// <summary>
        /// Parameters for EqualSsiRisk position size method.
        /// Required only in case if EqualSsiRisk method is set in Method field.
        /// </summary>
        public BacktesterPositionSizeByEqualSsiRiskParamsContract EqualSsiRiskParams { get; set; }

        /// <summary>
        /// Parameters for EqualSize position size method.
        /// Required only in case if EqualSize method is set in Method field.
        /// </summary>
        public BacktesterPositionSizeByEqualSizeParamsContract EqualSizeParams { get; set; }

        /// <summary>
        /// Parameters for FixedRisk position size method.
        /// Required only in case if FixedRisk method is set in Method field.
        /// </summary>
        public BacktesterPositionSizeByFixedRiskParamsContract FixedRiskParams { get; set; }
    }
}
