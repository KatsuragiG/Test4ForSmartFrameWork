using System.Diagnostics.CodeAnalysis;

namespace TradeStops.Contracts
{
    /// <summary>
    /// TradeIdeas (Ideas Lab) Strategies
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements must be documented", Justification = "Temporary suppression")]
    public class AvailableStrategiesContract
    {
        /// <summary>
        /// Indicates availability of Best of Billionaires strategy
        /// </summary>
        public bool BestOfBillionaires { get; set; }

        /// <summary>
        /// Indicates availability of Kinetic VQ strategy
        /// </summary>
        public bool KineticVq { get; set; }

        /// <summary>
        /// Indicates availability of Low Risk Runners strategy
        /// </summary>
        public bool LowRiskRunners { get; set; }
        public bool SectorSelects { get; set; }
        public bool DividendGrowers { get; set; }
        public bool Growth { get; set; }
        public bool Value { get; set; }

        // Crypto Ideas Lab
        public bool CryptoKineticVq { get; set; }
        public bool CryptoLowRiskRunners { get; set; }
        public bool CryptoMomentumPairing { get; set; }
        public bool CryptoMomentumAndVqPairing { get; set; }

        /// <summary>
        /// Indicates availability of Timing strategy
        /// </summary>
        public bool TimingRsiRebounds { get; set; }

        // Option strategies
        public bool TradeSmithCoveredCall { get; set; }
        public bool TradeSmithNakedPut { get; set; }
        public bool TradeSmithBuyCall { get; set; }
        ////public bool CoveredCall { get; set; } // 50/50 Heidi may ask to restore this strategy.
    }
}
