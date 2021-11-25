using System;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Alert Template
    /// </summary>
    public class AlertTemplateContract
    {
        /// <summary>
        /// Alert template Id.
        /// </summary>
        public int AlertTemplateId { get; set; }

        /// <summary>
        /// Title of the alert template.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Creation date of the alert template.
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Determines if the alert template is default on creating position.
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// Determines if the alert template is predefined by default.
        /// </summary>
        public bool IsPredefined { get; set; }
    }
}
