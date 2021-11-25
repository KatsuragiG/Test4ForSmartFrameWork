namespace TradeStops.Contracts
{
    /// <summary>
    /// The result of login attempt by provided credentials
    /// </summary>
    public class LoginResultContract
    {
        /// <summary>
        /// Indicates whether login attempt was successful or not
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// API Error Code with additional information about failed login attempt.
        /// Check /documentation/errors endpoint for additional details
        /// </summary>
        public int? ErrorCode { get; set; }

        /// <summary>
        /// The user that was found during login. Equals NULL if 'Success' = false
        /// </summary>
        public TradeSmithUserContract TradeSmithUser { get; set; }
    }
}