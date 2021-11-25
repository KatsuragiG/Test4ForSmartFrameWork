using System;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to get latest price
    /// </summary>
    public class GetLatestPriceContract
    {
        /// <summary>
        /// Initializes a new instance of GetLatestPriceContract class.
        /// </summary>
        public GetLatestPriceContract()
        {
        }

        /// <summary>
        /// Initializes a new instance of GetLatestPriceContract class.
        /// </summary>
        /// <param name="symbolId">Symbol ID</param>
        /// <param name="tradeDate">Date to load price</param>
        public GetLatestPriceContract(int symbolId, DateTime tradeDate)
        {
            SymbolId = symbolId;
            TradeDate = tradeDate;
        }

        /// <summary>
        /// Symbol ID
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// Date to load price
        /// </summary>
        public DateTime TradeDate { get; set; }
    }
}
