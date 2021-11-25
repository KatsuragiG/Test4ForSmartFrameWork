namespace TradeStops.Contracts
{
    /// <summary>
    /// Information about available baskets in Platform (Finance) website.
    /// Each basket is a set of symbols combined under one name.
    /// </summary>
    public class AvailableBasketsContract
    {
        /// <summary>
        /// Indicates availability for all cryptocurrencies basket.
        /// </summary>
        public bool AllCryptocurrencies { get; set; }

        /// <summary>
        /// Indicates availability for CCI30 basket.
        /// </summary>
        public bool Cci30 { get; set; }

        /// <summary>
        /// Indicates availability for binance basket.
        /// </summary>
        public bool Binance { get; set; }

        /// <summary>
        /// Indicates availability for binance US basket.
        /// </summary>
        public bool BinanceUs { get; set; }

        /// <summary>
        /// Indicates availability for coinbase basket.
        /// </summary>
        public bool Coinbase { get; set; }

        /// <summary>
        /// Indicates availability for poloniex basket.
        /// </summary>
        public bool Poloniex { get; set; }

        /// <summary>
        /// Indicates availability for markets basket.
        /// </summary>
        public bool Markets { get; set; }

        /// <summary>
        /// Indicates availability for Copilot Companion basket.
        /// </summary>
        public bool CopilotCompanion { get; set; }

        /// <summary>
        /// Indicates availability for LikeFolio Bullish basket.
        /// </summary>
        public bool LikeFolioBullish { get; set; }

        /// <summary>
        /// Indicates availability for LikeFolio Bearish basket.
        /// </summary>
        public bool LikeFolioBearish { get; set; }

        /// <summary>
        /// Indicates availability for LikeFolio Neutral basket.
        /// </summary>
        public bool LikeFolioNeutral { get; set; }

        /// <summary>
        /// Indicates availability for PossibleShortSqueeze basket.
        /// </summary>
        public bool PossibleShortSqueeze { get; set; }

        /// <summary>
        /// Indicates availability for baskets management.
        /// </summary>
        public bool CustomBaskets { get; set; }
    }
}
