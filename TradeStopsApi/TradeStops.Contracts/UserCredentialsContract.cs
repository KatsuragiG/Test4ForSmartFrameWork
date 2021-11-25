namespace TradeStops.Contracts
{
    /// <summary>
    /// User credentials
    /// </summary>
    public class UserCredentialsContract
    {
        /// <summary>
        /// Username (login)
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }
    }
}
