using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to set exit signals for backtesting process.
    /// Exit strategy is the way to determine the date when to sell (close) position.
    /// </summary>
    public class BacktesterExitStrategyContract
    {
        /// <summary>
        /// Original exit means that we just use the date when position was sold.
        /// </summary>
        public bool Original { get; set; }
        
        /// <summary>
        /// Exit by VQ means that we use VQ to determine exit date.
        /// If this exit signal is enabled, then ExitByVqParams field must be set.
        /// </summary>
        public bool Vq { get; set; }
        
        /// <summary>
        /// Exit by SSI means that we use SSI (Health) to determine exit date.
        /// </summary>
        public bool Ssi { get; set; }

        /// <summary>
        /// Exit by Trailing Stop means that we sell position when it's current price X% lower than the highest price after purchase date.
        /// If this exit signal is enabled, then ExitByTrailingStopParams field must be set.
        /// </summary>
        public bool TrailingStop { get; set; }

        /// <summary>
        /// Parameters to use 'Vq' exit strategy.
        /// Required only in case if ExitByVq strategy is set in ExitStrategy field.
        /// </summary>
        public BacktesterExitByVqParamsContract VqParams { get; set; }

        /// <summary>
        /// Parameters to use 'TrailingStop' exit strategy.
        /// Required only in case if ExitByTrailingStop strategy is set in ExitStrategy field.
        /// </summary>
        public BacktesterExitByTrailingStopParamsContract TrailingStopParams { get; set; }

        /// <summary>
        /// Determines if exit must be done when any of the signals is triggered or when all of them are triggered at once.
        /// </summary>
        public BacktesterSignalsCombinationTypes ExitSignalsCombination { get; set; }
    }
}
