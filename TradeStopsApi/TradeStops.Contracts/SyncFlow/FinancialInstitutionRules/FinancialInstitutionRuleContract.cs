using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Financial institution rule is a message that should be displayed for the specific broker to a user when there are some issues with that broker.
    /// </summary>
    public class FinancialInstitutionRuleContract
    {
        /// <summary>
        /// Financial institution rule ID.
        /// </summary>
        public int FinancialInstitutionRuleId { get; set; }

        /// <summary>
        /// Financial institution ID.
        /// </summary>
        public int FinancialInstitutionId { get; set; }

        /// <summary>
        /// Notification type.
        /// </summary>
        public FinancialInstitutionRuleNotificationTypes NotificationType { get; set; }

        /// <summary>
        /// Defines is the rule applied for import.
        /// </summary>
        public bool IsAppliedForImport { get; set; }

        /// <summary>
        /// Defines is the rule applied for refresh.
        /// </summary>
        public bool IsAppliedForRefresh { get; set; }

        /// <summary>
        /// Defines is the rule applied for restore portfolios.
        /// </summary>
        public bool IsAppliedForRestore { get; set; }

        /// <summary>
        /// Defines is the rule applied for update credentials.
        /// </summary>
        public bool IsAppliedForUpdateCredentials { get; set; }

        /// <summary>
        /// Warning message title.
        /// </summary>
        public string WarningMessageTitle { get; set; }

        /// <summary>
        /// Warning message message.
        /// </summary>
        public string WarningMessageDescription { get; set; }
    }
}
