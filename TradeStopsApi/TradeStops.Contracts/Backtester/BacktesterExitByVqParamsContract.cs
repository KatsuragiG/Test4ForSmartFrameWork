namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to use ExitByVq exit strategy
    /// </summary>
    public class BacktesterExitByVqParamsContract
    {
        /// <summary>
        /// Max VQ (Usually it's 95)
        /// </summary>
        public decimal MaxVq { get; set; }

        /// <summary>
        /// Max VQ (Usually it's 5)
        /// </summary>
        public decimal MinVq { get; set; }

        /// <summary>
        /// Max VQ (Usually it's 25)
        /// </summary>
        public decimal DefaultVq { get; set; }
    }
}
