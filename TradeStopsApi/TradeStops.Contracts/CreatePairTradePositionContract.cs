using System.Collections.Generic;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to create Pair Trade position
    /// </summary>
    public class CreatePairTradePositionContract
    {
        /// <summary>
        /// Portfolio Id.
        /// </summary>
        public int PortfolioId { get; set; }

        /// <summary>
        /// Total amount user would invest into a Pair Trade.
        /// </summary>
        public decimal InvestmentCapital { get; set; }

        /// <summary>
        /// Brokerage commission to open a position.
        /// </summary>
        public decimal OpenFee { get; set; }

        /// <summary>
        /// Position notes.
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Values of subtrades.
        /// </summary>
        public List<CreatePairTradeSubtradeContract> Subtrades { get; set; }
    }
}
