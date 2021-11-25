namespace TradeStops.Contracts
{
    /// <summary>
    /// Financial institution rule for admin area.
    /// </summary>
    public class AdminFinancialInstitutionRuleContract
    {
        /// <summary>
        /// Financial institution rule.
        /// </summary>
        public FinancialInstitutionRuleContract FinancialInstitutionRule { get; set; }

        /// <summary>
        /// Financial institution name.
        /// </summary>
        public string FinancialInstitutionName { get; set; }
    }
}
