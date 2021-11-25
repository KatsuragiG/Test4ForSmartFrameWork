namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to combine positions
    /// </summary>
    public class CombinePositionsContract
    {
        /// <summary>
        /// Id of parent position.
        /// </summary>
        public int ParentPositionId { get; set; }

        /// <summary>
        /// Id of child position.
        /// </summary>
        public int ChildPositionId { get; set; }
    }
}
