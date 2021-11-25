using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Checklist check results base parameters
    /// </summary>
    public class ChecklistBaseCheckContract
    {
        /// <summary>
        /// Checklist check type
        /// </summary>
        public StockFinderFilterTypes CheckType { get; set; }

        /// <summary>
        /// Is check passed
        /// </summary>
        public bool IsCheckPassed { get; set; }
    }
}
