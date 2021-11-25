namespace TradeStops.Contracts
{
    /// <summary>
    /// Contract to store source entity consists from 2 params: id and value.
    /// For example: symbolId and symbol, sectorId and sectorName, savedSearchId amd savedSearchName
    /// </summary>
    public class PureQuantSourceEntityInputContract
    {
        /// <summary>
        /// Source entity id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Source entity value.
        /// </summary>
        public string Value { get; set; }
    }
}
