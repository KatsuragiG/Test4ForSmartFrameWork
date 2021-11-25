using System.Collections.Generic;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Information about customer
    /// </summary>
    public class PublishersCustomerContract
    {
        /// <summary>
        /// Customer Id
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Customer name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Weiss customer
        /// </summary>
        public virtual bool IsWeiss { get; set; }

        /// <summary>
        /// Active customer
        /// </summary>
        public virtual bool IsActive { get; set; }

        /// <summary>
        /// Oxford Report Available
        /// </summary>
        public virtual bool IsOxfordReportAvailable { get; set; }

        /// <summary>
        /// Customer strategy
        /// </summary>
        public virtual PublishersCustomerStrategyTypes Strategy { get; set; }

        /// <summary>
        /// Display total weight
        /// </summary>
        public virtual bool DisplayTotalWeight { get; set; }

        /// <summary>
        /// Api keys
        /// </summary>
        public IList<PublishersApiKeyContract> ApiKeys { get; set; }

        /// <summary>
        /// Business units
        /// </summary>
        public IList<PublishersBusinessUnitContract> BusinessUnits { get; set; }

        /// <summary>
        /// Pub codes
        /// </summary>
        public IList<PublishersPubCodeContract> PubCodes { get; set; }
    }
}
