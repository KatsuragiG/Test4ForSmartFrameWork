using System;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters for TradeSmith user
    /// </summary>
    public class TradeSmithUserContract
    {
        /// <summary>
        /// Main user identifier in the database
        /// </summary>
        public int TradeSmithUserId { get; set; }

        /// <summary>
        /// User guid
        /// </summary>
        public Guid Guid { get; set; }

        /// <summary>
        /// Primary e-mail address assigned to the user account.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Identifier used to synchronize subscriptions with Stansberry CRM
        /// </summary>
        public string Snaid { get; set; }

        /// <summary>
        /// Identifier used to synchronize subscriptions with Agora CRM
        /// </summary>
        public string AgoraCustomerNumber { get; set; }

        /// <summary>
        /// The date when user was created in the database
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// User notes
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// User personal information
        /// </summary>
        public UserPersonalInfoContract PersonalInfo { get; set; }
    }
}