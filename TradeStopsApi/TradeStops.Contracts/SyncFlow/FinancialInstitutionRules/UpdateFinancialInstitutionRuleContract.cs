using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Initialize all fields to update financial institution rule
    /// </summary>
    public class UpdateFinancialInstitutionRuleContract
    {
        /// <summary>
        /// Defines the rule's financial institution ID.
        /// </summary>
        public int FinancialInstitutionId { get; set; }

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
        /// Notification Type.
        /// </summary>
        public FinancialInstitutionRuleNotificationTypes NotificationType { get; set; }

        /// <summary>
        /// Warning message title.
        /// </summary>
        public string WarningMessageTitle { get; set; }

        /// <summary>
        /// Warning message description.
        /// </summary>
        public string WarningMessageDescription { get; set; }
    }
}
