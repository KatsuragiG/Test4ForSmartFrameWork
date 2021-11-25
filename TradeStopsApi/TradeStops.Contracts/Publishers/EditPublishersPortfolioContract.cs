using TradeStops.Common.Enums;
using TradeStops.Contracts.Types;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Contract with portfolio fields to patch.
    /// </summary>
    public class EditPublishersPortfolioContract
    {
        /// <summary>
        /// (optional) Portfolio name.
        /// </summary>
        public Optional<string> Name { get; set; }

        /// <summary>
        /// (optional) Editor name.
        /// </summary>
        public Optional<string> EditorName { get; set; }

        /// <summary>
        /// (optional) Notes.
        /// </summary>
        public Optional<string> Notes { get; set; }

        /// <summary>
        /// (optional) Defines whether to use intraday data.
        /// </summary>
        public Optional<bool> UseIntradayData { get; set; }

        /// <summary>
        /// (optional) Defines if the Dividend Reinvestment Plan will be set for positions by default.
        /// </summary>
        public Optional<bool> IsDrip { get; set; }

        /// <summary>
        /// (optional) Defines whether Split Adjustment is set by default.
        /// </summary>
        public Optional<bool> AdjustBySplit { get; set; }

        /// <summary>
        /// (optional) Initial cash.
        /// </summary>
        public Optional<decimal> InitialCash { get; set; }

        /// <summary>
        /// (optional) Amount of trading days to calculate a custom percentage gain.
        /// </summary>
        public Optional<int> CalendarX { get; set; }

        /// <summary>
        /// (optional) Portfolio status.
        /// </summary>
        public Optional<PublishersPortfolioStatuses> PortfolioStatus { get; set; }

        /// <summary>
        /// (optional) Portfolio currency type.
        /// </summary>
        public Optional<PublishersCurrencyTypes?> CurrencyType { get; set; }

        /// <summary>
        /// (optional) Admin name.
        /// </summary>
        public Optional<string> AdminName { get; set; }

        /// <summary>
        /// (optional) Admin email.
        /// </summary>
        public Optional<string> AdminEmail { get; set; }

        /// <summary>
        /// (optional) Other emails.
        /// </summary>
        public Optional<string> OtherEmails { get; set; }

        /// <summary>
        /// (optional) Defines whether to receive email alerts.
        /// </summary>
        public Optional<bool> ReceiveEmailAlerts { get; set; }

        /// <summary>
        /// (optional) Business unit Id of the portfolio.
        /// </summary>
        public Optional<int?> BusinessUnitId { get; set; }

        /// <summary>
        /// (optional) Pub code Id of the portfolio.
        /// </summary>
        public Optional<int?> PubCodeId { get; set; }

        /// <summary>
        /// (optional) Publisher Id of the portfolio.
        /// </summary>
        public Optional<int?> PublisherId { get; set; }

        /// <summary>
        /// (optional) Defines portfolio visibility in TradeStops.
        /// </summary>
        public Optional<bool> VisibilityTS { get; set; }

        /// <summary>
        /// (optional) Defines portfolio visibility in CryptoTradesmith.
        /// </summary>
        public Optional<bool> VisibilityCS { get; set; }
    }
}
