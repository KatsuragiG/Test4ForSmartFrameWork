namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters for defining parent and child item in multiple dropdown
    /// </summary>
    public class MultipleFilterItemContract
    {
        /// <summary>
        /// Parent item
        /// </summary>
        public int Item1 { get; set; }

        /// <summary>
        /// (optional) Child item
        /// </summary>
        public int? Item2 { get; set; }
    }
}
