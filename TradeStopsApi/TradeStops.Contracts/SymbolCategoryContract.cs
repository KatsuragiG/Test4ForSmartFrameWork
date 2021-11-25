namespace TradeStops.Contracts
{
    /// <summary>
    /// Symbol Category
    /// </summary>
    public class SymbolCategoryContract
    {
        /// <summary>
        /// Symbol category ID.
        /// </summary>
        public int SymbolCategoryId { get; set; }

        /// <summary>
        /// Symbol Category Parent ID.
        /// </summary>
        public int SymbolCategoryParentId { get; set; }

        /// <summary>
        /// Symbol Category Name.
        /// </summary>
        public string Name { get; set; }
    }
}
