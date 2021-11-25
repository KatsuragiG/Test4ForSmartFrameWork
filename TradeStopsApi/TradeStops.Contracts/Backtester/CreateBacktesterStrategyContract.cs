using TradeStops.Common.Enums;

// todo: Consider to use the resource-based approach for Create/Get contracts:
// There will be main resource Contract that is used to get/create
// (and probably for put, but there will be an issue with editing in mobile app when you add new fields into contract, so we have to have Patch-approach anyway).
// If Get contract contains additional fields, like creation date and probably ID (for ID it's better to have some generic ResourceWithId wrapper or just have zero ID),
// then you just return some extended Contract inherited from (or containing) main resource Contract.
// For example, for triggers MainResourceContract is BaseTriggerContract type, and for Extended resource there's added TriggerStats+Creation/Process date, info about symbol, etc.
namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to create Backtester strategy
    /// </summary>
    public class CreateBacktesterStrategyContract
    {
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
        /// Determines whether the strategy should be marked by user as favorite or not
        /// </summary>
        public bool IsFavorite { get; set; }
    }
}
