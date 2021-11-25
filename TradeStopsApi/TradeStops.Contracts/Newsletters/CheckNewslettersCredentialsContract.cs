using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Information about newsletters portfolio that user tries to access
    /// and credentials that user entered
    /// </summary>
    public class CheckNewslettersCredentialsContract
    {
        /// <summary>
        /// ID of the Publisher
        /// </summary>
        public PublisherTypes PublisherId { get; set; }

        /// <summary>
        /// ID of the Publisher's portfolio
        /// </summary>
        public int PortfolioId { get; set; }

        /// <summary>
        /// Username
        /// </summary>
        public string Username { get; set; } 

        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }
    }
}
