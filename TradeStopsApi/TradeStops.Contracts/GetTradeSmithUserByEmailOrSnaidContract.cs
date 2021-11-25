namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to get User.
    /// </summary>
    // todo: rename to GetUserContract
    public class GetTradeSmithUserByEmailOrSnaidContract
    {
        /// <summary>
        /// (optional) User's primary email (or username).
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// (optional) SNAID - identifier of the user in Stansberry.
        /// </summary>
        public string Snaid { get; set; }
    }
}
