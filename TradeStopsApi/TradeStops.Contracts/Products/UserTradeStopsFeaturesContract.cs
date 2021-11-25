using System;
using System.Diagnostics.CodeAnalysis;

namespace TradeStops.Contracts
{
    /// <summary>
    /// TradeStops product features available for user
    /// </summary>
    [Obsolete("Use UserFeaturesContract instead, Will be removed in nearest future")]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed")]
    public class UserTradeStopsFeaturesContract
    {
        // only these fields were used by mobile app in july 2020.
        public bool PriceTargetAlerts { get; set; }
        public bool TimeBasedAlerts { get; set; }
        public bool VolumeAlerts { get; set; }
        public bool MovingAverageAlerts { get; set; }
        public bool TrailingStopsAlerts { get; set; }
        public bool OptionCostBasisAlerts { get; set; }
        public bool UnderlyingStockAlerts { get; set; }
        public bool TimeValueExpiryAlerts { get; set; }
        public bool FundamentalsAlerts { get; set; }
        public bool SsiAlerts { get; set; }

        [Obsolete("Temporary method, will be removed as soon as possible")]
        public static UserTradeStopsFeaturesContract Map(UserFeaturesContract contract)
        {
            if (contract == null)
            {
                return null;
            }

            return new UserTradeStopsFeaturesContract
            {
                PriceTargetAlerts                 = contract.Alerts.PriceTarget,
                TimeBasedAlerts                   = contract.Alerts.TimeBased,
                VolumeAlerts                      = contract.Alerts.Volume,
                MovingAverageAlerts               = contract.Alerts.MovingAverage,
                TrailingStopsAlerts               = contract.Alerts.TrailingStops,
                OptionCostBasisAlerts             = contract.Alerts.OptionCostBasis,
                UnderlyingStockAlerts             = contract.Alerts.UnderlyingStock,
                TimeValueExpiryAlerts             = contract.Alerts.TimeValueExpiry,
                FundamentalsAlerts                = contract.Alerts.Fundamentals,
                SsiAlerts                         = contract.Alerts.Ssi,
            };
        }
    }
}
