using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Top newsletter recommendation
    /// </summary>
    public class TopNewsletterPositionContract
    {
        /// <summary>
        /// Id of the newsletter portfolio
        /// </summary>
        public int PortfolioId { get; set; }

        /// <summary>
        /// The type of publisher
        /// </summary>
        public PublisherTypes PublisherType { get; set; }

        /// <summary>
        /// Publisher name
        /// </summary>
        public string PublisherName { get; set; }

        /// <summary>
        /// The trade type of newsletter position
        /// </summary>
        public TradeTypes? TradeType { get; set; }

        /// <summary>
        /// Symbol values
        /// </summary>
        public SymbolContract Symbol { get; set; }

        /// <summary>
        /// Currency values.
        /// </summary>
        public CurrencyContract Currency { get; set; }

        /// <summary>
        /// Current VQ value
        /// </summary>
        public decimal CurrentVq { get; set; }

        /// <summary>
        /// Current SSI state
        /// </summary>
        public SsiCurrentValueContract SsiCurrentValue { get; set; }

        /// <summary>
        /// Ref date value
        /// </summary>
        public DateTime? RefDate { get; set; }

        /// <summary>
        /// Ref price value
        /// </summary>
        public decimal? RefPrice { get; set; }

        /// <summary>
        /// Date when position was sold.
        /// </summary>
        public DateTime? ExitDate { get; set; }

        /// <summary>
        /// Price at which position was sold.
        /// </summary>
        public decimal? ExitPrice { get; set; }

        /// <summary>
        /// Position total gain per share
        /// </summary>
        public decimal? TotalGainPerShare { get; set; }

        /// <summary>
        /// Position total gain percent per share
        /// </summary>
        public decimal? TotalPercentGainPerShare { get; set; }
    }
}
