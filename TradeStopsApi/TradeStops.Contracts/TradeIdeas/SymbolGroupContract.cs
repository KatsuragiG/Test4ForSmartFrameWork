using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Symbol group
    /// </summary>
    public class SymbolGroupContract
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SymbolGroupContract()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">Symbol group type</param>
        /// <param name="name">Symbol group name</param>
        public SymbolGroupContract(SymbolGroupTypes type, string name)
        {
            SymbolGroupId = type;
            SymbolGroupName = name;
        }

        /// <summary>
        /// Symbol group
        /// </summary>
        public SymbolGroupTypes SymbolGroupId { get; set; }

        /// <summary>
        /// Name of the symbol group
        /// </summary>
        public string SymbolGroupName { get; set; }
    }
}
