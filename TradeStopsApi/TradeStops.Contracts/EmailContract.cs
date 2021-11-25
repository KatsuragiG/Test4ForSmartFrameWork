namespace TradeStops.Contracts
{
    /// <summary>
    /// Email data
    /// </summary>
    public class EmailContract
    {
        /// <summary>
        /// Initializes a new instance of EmailContract class.
        /// </summary>
        public EmailContract()
        {
        }

        /// <summary>
        /// Initializes a new instance of EmailContract class.
        /// </summary>
        /// <param name="email">Email</param>
        public EmailContract(string email)
        {
            Email = email;
        }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
    }
}
