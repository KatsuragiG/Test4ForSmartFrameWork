using System;
using System.Collections.Generic;

namespace TradeStops.Contracts
{
    /// <summary>
    /// All necessary information to display in Zendesk
    /// </summary>
    public class ZendeskAppsCustomerInformationContract
    {
        /// <summary>
        /// Primary email of the user in TradeSmith database
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Date when user was created in TradeSmith database
        /// </summary>
        public DateTime? CreationDateUtc { get; set; }

        /// <summary>
        /// Date of the last successful login into any product
        /// </summary>
        public DateTime? LastLoginDateUtc { get; set; }

        /// <summary>
        /// User ID in TradeSmith
        /// </summary>
        public int? TradeSmithUserId { get; set; }

        /// <summary>
        /// User ID in Agora CRM
        /// </summary>
        public string AgoraCustomerNumber { get; set; }

        /// <summary>
        /// User ID in Stansberry CRM
        /// </summary>
        public string Snaid { get; set; }

        /// <summary>
        /// Customer's personal information
        /// </summary>
        public ZendeskAppsPersonalInfoContract PersonalInfo { get; set; }

        /// <summary>
        /// Information about user's active portfolios, alerts, newsletters
        /// </summary>
        public ZendeskAppsPortfoliosInfoContract PortfoliosInfo { get; set; }

        /// <summary>
        /// Email addresses to receive HTML notifications.
        /// These emails are indicated on website by user in 'Settings - Notification - Email Notifications' section.
        /// Can contain up to 3 emails.
        /// </summary>
        public List<string> SecondaryEmails { get; set; }

        /// <summary>
        /// Email addresses to receive plain text notifications.
        /// Usually these emails are generated from mobile phone number + corresponding mobile operator postfix.
        /// These emails are indicated on website by user in 'Settings - Notification - Text Message Notifications' section.
        /// Can contain up to 3 emails.
        /// </summary>
        public List<string> PlainTextAddresses { get; set; }

        /// <summary>
        /// All subscriptions that should be displayed in Zendesk.
        /// All subscriptions are active and there are no duplicated subscriptions.
        /// </summary>
        public List<ZendeskAppsSubscriptionInfoContract> Subscriptions { get; set; }
    }
}
