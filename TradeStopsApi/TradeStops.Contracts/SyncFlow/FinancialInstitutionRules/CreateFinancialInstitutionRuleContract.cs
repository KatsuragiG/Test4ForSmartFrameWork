using System.Collections.Generic;

using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to create financial institution rule
    /// </summary>
    public class CreateFinancialInstitutionRuleContract
    {
        /// <summary>
        /// Financial institution ID.
        /// </summary>
        public int FinancialInstitutionId { get; set; }

        /// <summary>
        /// Notification Type.
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
        /// Warning message description.
        /// </summary>
        public string WarningMessageDescription { get; set; }
    }
}
