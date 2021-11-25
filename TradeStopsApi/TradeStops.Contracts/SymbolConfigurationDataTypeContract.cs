namespace TradeStops.Contracts
{
    /// <summary>
    /// Symbol Configuration Data type
    /// </summary>
    public class SymbolConfigurationDataTypeContract
    {
        /// <summary>
        /// Data type ID.
        /// </summary>
        public int DataTypeId { get; set; }

        /// <summary>
        /// Class name of data type.
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// Columns.
        /// </summary>
        public string Columns { get; set; }

        /// <summary>
        /// Type.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Description.
        /// </summary>
        public string Description { get; set; }
    }
}
