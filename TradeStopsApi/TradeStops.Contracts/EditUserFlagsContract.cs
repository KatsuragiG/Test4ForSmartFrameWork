using TradeStops.Contracts.Types;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Contract with user action flags to patch
    /// </summary>
    public class EditUserFlagsContract
    {
        /// <summary>
        /// (optional) User accepted terms of service.
        /// </summary>
        public Optional<bool> UserAcceptedTermsOfService { get; set; }
    }
}
