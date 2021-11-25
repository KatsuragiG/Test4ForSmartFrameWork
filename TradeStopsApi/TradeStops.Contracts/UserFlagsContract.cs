namespace TradeStops.Contracts
{
    /// <summary>
    /// User flags
    /// </summary>
    public class UserFlagsContract
    {
        /// <summary>
        /// Defines have user watched smart trailing stop video.
        /// </summary>
        public bool UserWatchedSmartTrailingStopVideo { get; set; }

        /// <summary>
        /// Defines have user watched position size calculator video.
        /// </summary>
        public bool UserWatchedPositionSizeCalculatorVideo { get; set; }

        /// <summary>
        /// Defines have user watched asset allocation video.
        /// </summary>
        public bool UserWatchedAssetAllocationVideo { get; set; }

        /// <summary>
        /// Defines have user watched volatility quotient video.
        /// </summary>
        public bool UserWatchedVolatilityQuotientVideo { get; set; }

        /// <summary>
        /// Defines have user watched newsletters video.
        /// </summary>
        public bool UserWatchedNewsLettersVideo { get; set; }

        /// <summary>
        /// Defines have user got upgrade of stop loss analyzer alert.
        /// </summary>
        public bool UserGotUpgradeOfStopLossAnalyzerAlert { get; set; }

        /// <summary>
        /// Defines have user got beacon for charts.
        /// </summary>
        public bool UserGotBeaconForCharts { get; set; }

        /// <summary>
        /// Defines have user accepted terms of service.
        /// </summary>
        public bool UserAcceptedTermsOfService { get; set; }

        /// <summary>
        /// Defines have user got reactivated expired users popup.
        /// </summary>
        public bool UserGotReactivatedExpiredUsersPopup { get; set; }

        /// <summary>
        /// Defines have user got help center popup.
        /// </summary>
        public bool UserGotHelpCenterPopup { get; set; }

        /// <summary>
        /// Defines have user got beacon for portfolios.
        /// </summary>
        public bool UserGotBeaconForPortfolios { get; set; }
    }
}
