using System;
using System.Collections.Generic;
using System.Text;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to send email
    /// </summary>
    public class SendEmailContract
    {
        /// <summary>
        /// Sender. For example: Investor's Toolbox.
        /// </summary>
        public string Sender { get; set; }

        /// <summary>
        /// Email subject.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Email body.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Indicates whether email body is HTML (true) or plain text (false).
        /// </summary>
        public bool IsBodyHtml { get; set; }

        /// <summary>
        /// Sender name.
        /// </summary>
        public string FromName { get; set; }

        /// <summary>
        /// Sender address.
        /// </summary>
        public string FromAddress { get; set; }

        /// <summary>
        /// Recipient address.
        /// </summary>
        public string ToAddress { get; set; }

        /// <summary>
        /// Email category.
        /// </summary>
        public string Category { get; set; }
    }
}
