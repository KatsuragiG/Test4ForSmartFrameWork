namespace TradeStops.Contracts
{
    /// <summary>
    /// Information about available publications in Platform (Finance) website
    /// </summary>
    public class AvailablePublicationsContract
    {
        /// <summary>
        /// Indicates availability of 'Inside TradeSmith' publication type
        /// </summary>
        public bool InsideTradeSmith { get; set; }

        /// <summary>
        /// Indicates availability of 'TradeSmith Daily' publication type
        /// </summary>
        public bool TradeSmithDaily { get; set; }

        /// <summary>
        /// Indicates availability of 'TradeSmith Decoder' publication type
        /// </summary>
        public bool TradeSmithDecoder { get; set; }

        /// <summary>
        /// Indicates availability of 'Timing by TradeSmith' publication type
        /// </summary>
        public bool TimingByTradeSmith { get; set; }

        /// <summary>
        /// Indicates availability of 'TradeSmith Trends' publication type
        /// </summary>
        public bool TradeSmithTrends { get; set; }

        /// <summary>
        /// Indicates availability of 'LikeFolio Megatrends' publication type
        /// </summary>
        public bool LikeFolioMegatrends { get; set; }

        /// <summary>
        /// Indicates availability of 'LikeFolio Opportunity Alerts' publication type
        /// </summary>
        public bool LikeFolioOpportunityAlerts { get; set; }

        /// <summary>
        /// Indicates availability of 'Earnings Reports' publication type
        /// </summary>
        public bool EarningsReports { get; set; }

        /// <summary>
        /// Indicates availability of 'Market Weekly Update' publication type
        /// </summary>
        public bool MarketWeeklyUpdate { get; set; }

        /// <summary>
        /// Indicates availability of 'Money Talks' publication type
        /// </summary>
        public bool MoneyTalks { get; set; }

        /// <summary>
        /// Indicates availability of 'Platinum Only Announcements' publication type
        /// </summary>
        public bool PlatinumOnlyAnnouncements { get; set; }

        /// <summary>
        /// Indicates availability of 'Platinum Benefits' publication type
        /// </summary>
        public bool PlatinumBenefits { get; set; }
    }
}
