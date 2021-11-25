using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Strategy for the Backtester.
    /// </summary>
    public class BacktesterStrategyContract
    {
        /// <summary>
        /// ID of the strategy.
        /// </summary>
        public int StrategyId { get; set; }

        /// <summary>
        /// The name of the strategy.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Entry strategy is the way to determine date when to buy position in backtesting process.
        /// This field may be replaced with array of entry strategies in the future.
        /// </summary>
        public BacktesterEntryStrategies EntryStrategy { get; set; }

        /// <summary>
        /// Original Trade settings: Minimum trade length to process (in days).
        /// It is used to avoid cases when we buy and sell stocks too often, like every hour or every day.
        /// </summary>
        public int MinTradeLength { get; set; }

        /// <summary>
        /// Original Trade settings: Minimum position size to process (in dollars).
        /// It is used to avoid cases when we purchase positions with really small position size, like $0.1 or similar.
        /// </summary>
        public decimal MinPositionSize { get; set; }

        /// <summary>
        /// Parameters to set exit signals for backtesting process.
        /// Exit strategy is the way to determine the date when to sell (close) position.
        /// </summary>
        public BacktesterExitStrategyContract ExitStrategy { get; set; }

        /// <summary>
        /// Parameters to determine position size for backtesting process.
        /// </summary>
        public BacktesterPositionSizeContract PositionSize { get; set; }

        /// <summary>
        /// Determines whether the strategy is marked by user as favorite or not
        /// </summary>
        public bool IsFavorite { get; set; }
    }
}
