namespace TradeStops.Contracts
{
    /// <summary>
    /// Pure Quant task description.
    /// </summary>
    public class PureQuantTaskContract
    {
        /// <summary>
        /// Pure Quant task.
        /// </summary>
        public PlatformTaskContract PureQuantTask { get; set; }

        /// <summary>
        /// Pure Quant saved input parameters.
        /// </summary>
        public PureQuantInputParamsContract PureQuantInputParams { get; set; }
    }
}
