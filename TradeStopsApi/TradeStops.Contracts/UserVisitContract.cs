using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Information about user's visit (like login into site, opening site with saved credentials and so on)
    /// </summary>
    public class UserVisitContract
    {
        /// <summary>
        /// The product that user is visiting
        /// </summary>
        public Products ProductId { get; set; }

        /// <summary>
        /// (optional) Browser
        /// </summary>
        public string Browser { get; set; }

        /// <summary>
        /// (optional) OS
        /// </summary>
        public string OS { get; set; }
    }
}
