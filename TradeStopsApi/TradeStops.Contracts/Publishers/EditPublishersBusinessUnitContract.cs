namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters for editing a business unit
    /// </summary>
    public class EditPublishersBusinessUnitContract
    {
        /// <summary>
        /// Business unit Id
        /// </summary>
        public int BusinessUnitId { get; set; }

        /// <summary>
        /// Business unit name
        /// </summary>
        public string BusinessUnitName { get; set; }
    }
}
