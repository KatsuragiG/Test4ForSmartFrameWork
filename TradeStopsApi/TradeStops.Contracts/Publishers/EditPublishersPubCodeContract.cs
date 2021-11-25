using TradeStops.Contracts.Types;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters for editing a pub code
    /// </summary>
    public class EditPublishersPubCodeContract
    {
        /// <summary>
        /// Pub code Id
        /// </summary>
        public int PubCodeId { get; set; }

        /// <summary>
        /// (optional) New pub code value
        /// </summary>
        public Optional<string> PubCode { get; set; }

        /// <summary>
        /// (optional) New own org value
        /// </summary>
        public Optional<string> OwnOrg { get; set; }

        /// <summary>
        /// (optional) New pub code description
        /// </summary>
        public Optional<string> PubTitle { get; set; }

        /// <summary>
        /// (optional) New pub code category
        /// </summary>
        public Optional<string> PubCategory { get; set; }
    }
}
