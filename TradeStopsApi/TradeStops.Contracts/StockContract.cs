using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// General information about symbol that applicable for all types of symbols
    /// </summary>
    public class StockContract
    {
        /// <summary>
        /// Symbol ID.
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// Symbol's ticker, like 'AAPL'.
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// Company name, like 'Apple'.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Indicates whether symbol has been de-listed (removed, deactivated) or not.
        /// </summary>
        public bool Delisted { get; set; }

        /// <summary>
        /// Symbol type (StockData of OptionsData).
        /// </summary>
        [Obsolete("2020-10-26. Use SymbolType instead.")]
        public string DataType { get; set; }

        /// <summary>
        /// ID of the currency.
        /// </summary>
        public int CurrencyId { get; set; }

        /// <summary>
        /// Internal TradeStops value
        /// </summary>
        public int InstrumentId { get; set; }

        /// <summary>
        /// Symbol exchange name.
        /// </summary>
        public string ExchangeName { get; set; }

        /// <summary>
        /// Symbol exchange ID.
        /// </summary>
        public int ExchangeId { get; set; }

        /// <summary>
        /// Symbol has announced corporate actions. Prices will be corrected.
        /// </summary>
        public bool HasDelayedCorporateActions { get; set; }

        /// <summary>
        /// Crypto currency symbol.
        /// </summary>
        [Obsolete("2020-10-26. Use SymbolType instead.")]
        public bool IsCryptoCurrency { get; set; }

        /// <summary>
        /// SymbolType (stock, option, fund, etc.)
        /// </summary>
        public SymbolTypes SymbolType { get; set; }

        /// <summary>
        /// Symbol category ID.
        /// </summary>
        public int SymbolCategoryId { get; set; }
    }
}
