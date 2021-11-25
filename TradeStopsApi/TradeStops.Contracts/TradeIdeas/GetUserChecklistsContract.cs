using System.Collections.Generic;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to get list of user's checklists
    /// </summary>
    public class GetUserChecklistsContract
    {
        /// <summary>
        /// Symbol asset type
        /// </summary>
        public List<SymbolTypes> AssetTypes { get; set; }

        /// <summary>
        /// Symbol trade type
        /// </summary>
        public List<TradeTypes> TradeTypes { get; set; }

        /// <summary>
        /// Option operation type (only for options)
        /// </summary>
        public List<OptionOperationTypes> OptionOperationTypes { get; set; }
    }
}
