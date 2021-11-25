using System;

namespace TradeStops.Contracts
{
    /// <summary>
    /// The contract returns symbol configuration.
    /// </summary>
    public class RwcSymbolConfigurationContract
    {
        /// <summary>
        /// Symbol Id.
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// Configuration data type class name
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// Configuration data type columns
        /// </summary>
        public string Columns { get; set; }

        /// <summary>
        /// Configuration type
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// Configuration data type ID
        /// </summary>
        public int DataTypeId { get; set; }

        /// <summary>
        /// Configuration type description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Configuration finish date
        /// </summary>
        public DateTime? FinishDate { get; set; }

        /// <summary>
        /// Configuration import params
        /// </summary>
        public string ImportParams { get; set; }

        /// <summary>
        /// Configuration last price update
        /// </summary>
        public DateTime? LastPriceUpdate { get; set; }

        /// <summary>
        /// Configuration last import result
        /// </summary>
        public string RecentImportResults { get; set; }

        /// <summary>
        /// Configuration start date
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Configuration updatable or not
        /// </summary>
        public bool Updatable { get; set; }
    }
}
