namespace TradeStops.Contracts
{
    /// <summary>
    /// Defines that beta financial institution allowed for usage by user.
    /// </summary>
    public class FinancialInstitutionBetaTesterContract
    {
        /// <summary>
        /// Financial Institution Beta Tester ID.
        /// </summary>
        public int FinancialInstitutionBetaTesterId { get; set; }

        /// <summary>
        /// Broker ID.
        /// </summary>
        public int FinancialInstitutionId { get; set; }

        /// <summary>
        /// TradeSmith user ID.
        /// </summary>
        public int TradeSmithUserId { get; set; }

        /// <summary>
        /// User email.
        /// </summary>
        public string Email { get; set; }
    }
}
