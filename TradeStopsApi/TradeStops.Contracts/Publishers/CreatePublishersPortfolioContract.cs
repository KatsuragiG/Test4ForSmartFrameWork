using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to create portfolio.
    /// </summary>
    public class CreatePublishersPortfolioContract
    {
        /// <summary>
        /// Portfolio name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Editor name.
        /// </summary>
        public string EditorName { get; set; }

        /// <summary>
        /// Notes.
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Defines whether to use intraday data.
        /// </summary>
        public bool UseIntradayData { get; set; }

        /// <summary>
        /// Defines if the Dividend Reinvestment Plan will be set for positions by default.
        /// </summary>
        public bool IsDrip { get; set; }

        /// <summary>
        /// Defines whether Split Adjustment is set by default.
        /// </summary>
        public bool AdjustBySplit { get; set; }

        /// <summary>
        /// Initial cash.
        /// </summary>
        public decimal InitialCash { get; set; }

        /// <summary>
        /// Amount of trading days to calculate a custom percentage gain.
        /// </summary>
        public int CalendarX { get; set; }

        /// <summary>
        /// Portfolio status.
        /// </summary>
        public PublishersPortfolioStatuses PortfolioStatus { get; set; }

        /// <summary>
        /// Portfolio currency type.
        /// </summary>
        public PublishersCurrencyTypes? CurrencyType { get; set; }

        /// <summary>
        /// Admin name.
        /// </summary>
        public string AdminName { get; set; }

        /// <summary>
        /// Admin email.
        /// </summary>
        public string AdminEmail { get; set; }

        /// <summary>
        /// Other emails.
        /// </summary>
        public string OtherEmails { get; set; }

        /// <summary>
        /// Defines whether to receive email alerts.
        /// </summary>
        public bool ReceiveEmailAlerts { get; set; }

        /// <summary>
        /// Business unit Id of the portfolio.
        /// </summary>
        public int? BusinessUnitId { get; set; }

        /// <summary>
        /// Pub code Id of the portfolio.
        /// </summary>
        public int? PubCodeId { get; set; }

        /// <summary>
        /// Publisher Id of the portfolio.
        /// </summary>
        public int? PublisherId { get; set; }

        /// <summary>
        /// Defines portfolio visibility in TradeStops.
        /// </summary>
        public bool VisibilityTS { get; set; }

        /// <summary>
        /// Defines portfolio visibility in CryptoTradesmith.
        /// </summary>
        public bool VisibilityCS { get; set; }
    }
}
