using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Information about user's login attempt
    /// </summary>
    public class LoginAttemptContract // todo: consider to rename to CreateLoginAttemptContract as it's used only to create login attempt
    {
        /////// <summary>
        /////// The time of user's login
        /////// </summary>
        ////public DateTime TimeStamp { get; set; }

        /// <summary>
        /// The product that user is logging in
        /// </summary>
        public Products ProductId { get; set; }

        /// <summary>
        /// ID of TradeSmithUser (same ID for all products)
        /// </summary>
        public int? TradeSmithUserId { get; set; }

        /// <summary>
        /// Username (Email) used for login
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Result of login
        /// </summary>
        public LoginResults LoginResultId { get; set; }

        /// <summary>
        /// The ID of the server used for login
        /// </summary>
        public string MachineName { get; set; }
    }
}
