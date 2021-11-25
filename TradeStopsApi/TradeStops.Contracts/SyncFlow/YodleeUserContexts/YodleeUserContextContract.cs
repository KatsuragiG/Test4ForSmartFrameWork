using System;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Yodlee user context required for authentication in Yodlee
    /// </summary>
    public class YodleeUserContextContract
    {
        /// <summary>
        ///  Yodlee user context ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///  User email address.
        /// </summary>
        public string UserEmail { get; set; }

        /// <summary>
        ///  Serialized user context.
        /// </summary>
        public string UserContext { get; set; }

        /// <summary>
        ///  Context creation date.
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        ///  Date when credentials were renewed last time.
        /// </summary>
        public DateTime? LastTouchConversationCredentials { get; set; }
    }
}
