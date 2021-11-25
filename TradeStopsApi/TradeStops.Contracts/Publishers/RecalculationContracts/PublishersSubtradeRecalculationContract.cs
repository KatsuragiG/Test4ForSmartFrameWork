using System;
using System.Collections.Generic;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Subtrade data for calculations. 
    /// </summary>
    public class PublishersSubtradeRecalculationContract
    {
        /// <summary>
        /// Subtrade Id.
        /// </summary>
        public int SubtradeId { get; set; }

        /// <summary>
        /// Symbol Id.
        /// </summary>
        public long SymbolId { get; set; }

        /// <summary>
        /// Open price.
        /// </summary>
        public decimal? OpenPrice { get; set; }

        /// <summary>
        /// Open date.
        /// </summary>
        public DateTime? OpenDate { get; set; }

        /// <summary>
        /// Current price.
        /// </summary>
        public decimal CurrentPrice { get; set; }

        /// <summary>
        /// Close price.
        /// </summary>
        public decimal? ClosePrice { get; set; }

        /// <summary>
        /// Close date.
        /// </summary>
        public DateTime? CloseDate { get; set; }

        /// <summary>
        /// Contains the current price for open trades and close price for close trades.
        /// </summary>
        public decimal CurrentClosePrice { get; set; }

        /// <summary>
        /// Contains the current date for open trades and close date for close trades.
        /// </summary>
        public DateTime CurrentCloseDate { get; set; }

        /// <summary>
        /// Price for calculation YTD gain.
        /// </summary>
        public decimal? YTDPrice { get; set; }

        /// <summary>
        /// Last open price.
        /// </summary>
        public decimal? LastOpenPrice { get; set; }

        /// <summary>
        /// Last open date.
        /// </summary>
        public DateTime? LastOpenDate { get; set; }

        /// <summary>
        /// Defines whether symbol is custom.
        /// </summary>
        public bool IsCustomSymbol { get; set; }

        /// <summary>
        /// Subtrade type.
        /// </summary>
        public PublishersSubtradeTypes SubtradeType { get; set; }

        /// <summary>
        /// Stock type.
        /// </summary>
        public PublishersStockTypes? StockType { get; set; }

        /// <summary>
        /// Option type.
        /// </summary>
        public PublishersOptionTypes? OptionType { get; set; }

        /// <summary>
        /// Trade status.
        /// </summary>
        public PublishersTradeStatuses? TradeStatus { get; set; }

        /// <summary>
        /// Trade type.
        /// </summary>
        public PublishersTradeTypes? TradeType { get; set; }

        /// <summary>
        /// Option status.
        /// </summary>
        public PublishersOptionStatuses? OptionStatus { get; set; }

        /// <summary>
        /// Current price of underlying stock for option symbol.
        /// </summary>
        public decimal? UnderlyingStockCurrentPrice { get; set; }

        /// <summary>
        /// Currency type.
        /// </summary>
        public PublishersCurrencyTypes CurrencyType { get; set; }

        /// <summary>
        /// Weight on open date.
        /// </summary>
        public decimal OpenWeight { get; set; }

        /// <summary>
        /// Shares per contract.
        /// </summary>
        public decimal SharesPerContract { get; set; }

        /// <summary>
        /// Price for calculation YTD Return gain.
        /// </summary>
        public decimal? YTDReturnPrice { get; set; }

        /// <summary>
        /// YTD Return gain.
        /// </summary>
        public decimal? YTDReturn { get; set; }

        /// <summary>
        /// Strike price.
        /// </summary>
        public decimal? StrikePrice { get; set; }

        /// <summary>
        /// Margin.
        /// </summary>
        public decimal? Margin { get; set; }

        /// <summary>
        /// Defines whether trade is based on a put option.
        /// </summary>
        public bool IsBasedOnPutOption { get; set; }

        /// <summary>
        /// Defines whether trade is opened.
        /// </summary>
        public bool IsOpened { get; set; }

        /// <summary>
        /// Defines whether trade is opened or waiting.
        /// </summary>
        public bool IsOpenedOrWaiting { get; set; }

        /// <summary>
        /// Dollar gain.
        /// </summary>
        public decimal? DollarGain { get; set; }

        /// <summary>
        /// Total dollar gain.
        /// </summary>
        public decimal? TotalDollarGain { get; set; }

        /// <summary>
        /// Defines whether a trade is long.
        /// </summary>
        public bool IsLong { get; set; }

        /// <summary>
        /// Total dividends.
        /// </summary>
        public decimal? TotalDividends { get; set; }

        /// <summary>
        /// Expiration date for option.
        /// </summary>
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// Buyback price.
        /// </summary>
        public decimal? BuybackPrice { get; set; }

        /// <summary>
        /// Sum of dividends.
        /// </summary>
        public decimal? Dividends { get; set; }

        /// <summary>
        /// Weight on current or close date.
        /// </summary>
        public decimal Weight { get; set; }

        /// <summary>
        /// Value (Cash allocation) on open date.
        /// </summary>
        public decimal? OpenValue { get; set; }

        /// <summary>
        /// Value (Cash allocation) in US dollars on open date.
        /// </summary>
        public decimal? DollarOpenValue { get; set; }

        /// <summary>
        /// Capital gains yield.
        /// </summary>
        public decimal? CapitalGainsYield { get; set; }

        /// <summary>
        /// Cross rate to the US dollar on open date.
        /// </summary>
        public decimal? OpenCurrency { get; set; }

        /// <summary>
        /// Cross rate to the US dollar on close date.
        /// </summary>
        public decimal? CloseCurrency { get; set; }

        /// <summary>
        /// Cross rate to the US dollar on current date.
        /// </summary>
        public decimal? CurrentCurrency { get; set; }

        /// <summary>
        /// Shares (Weight for Pair trades).
        /// </summary>
        public decimal Shares { get; set; }

        /// <summary>
        /// Dividends per share.
        /// </summary>
        public decimal? DividendsPerShare { get; set; }

        /// <summary>
        /// Total DRIP Dividends.
        /// </summary>
        public decimal? TotalDRiPDividends { get; set; }

        /// <summary>
        /// Casey Research margin.
        /// </summary>
        public decimal? MarginCR { get; set; }

        /// <summary>
        /// Investor Place Media margin.
        /// </summary>
        public decimal? MarginIPM { get; set; }

        /// <summary>
        /// Total credit.
        /// </summary>
        public decimal? TotalCredit { get; set; }

        /// <summary>
        /// Cross rate to the US dollar for gain calculation.
        /// </summary>
        public decimal? GainCurrency { get; set; }

        /// <summary>
        /// High close date.
        /// </summary>
        public DateTime? HighCloseDate { get; set; }

        /// <summary>
        /// High close price.
        /// </summary>
        public decimal? HighClosePrice { get; set; }

        /// <summary>
        /// Adjusted high close price.
        /// </summary>
        public decimal? AdjHighClosePrice { get; set; }

        /// <summary>
        /// High high date.
        /// </summary>
        public DateTime? HighHighDate { get; set; }

        /// <summary>
        /// High high price.
        /// </summary>
        public decimal? HighHighPrice { get; set; }

        /// <summary>
        /// Low close date.
        /// </summary>
        public DateTime? LowCloseDate { get; set; }

        /// <summary>
        /// Low close price.
        /// </summary>
        public decimal? LowClosePrice { get; set; }

        /// <summary>
        /// Adjusted low close price.
        /// </summary>
        public decimal? AdjLowClosePrice { get; set; }

        /// <summary>
        /// Low low date.
        /// </summary>
        public DateTime? LowLowDate { get; set; }

        /// <summary>
        /// Low low price.
        /// </summary>
        public decimal? LowLowPrice { get; set; }

        /// <summary>
        /// Trade holding period.
        /// </summary>
        public int? HoldPeriod { get; set; }

        /// <summary>
        /// Defines conversion status.
        /// </summary>
        public PublishersConversionStatuses ConversionStatus { get; set; }

        /// <summary>
        /// Customer strategy.
        /// </summary>
        public PublishersCustomerStrategyTypes CustomerStrategy { get; set; }

        /// <summary>
        /// Currency type of portfolio.
        /// </summary>
        public PublishersCurrencyTypes? PortfolioCurrencyType { get; set; }

        /// <summary>
        /// Parent position type.
        /// </summary>
        public PublishersPositionTypes PositionType { get; set; }

        /// <summary>
        /// Parent position strategy.
        /// </summary>
        public PublishersPositionStrategyTypes PositionStrategy { get; set; }

        /// <summary>
        /// Defines whether Dividend Reinvestment Plan is set for parent position.
        /// </summary>
        public bool IsDripPosition { get; set; }

        /// <summary>
        /// Max down side price for parent regular position.
        /// </summary>
        public decimal? PositionMaxDownSide { get; set; }

        /// <summary>
        /// Custom symbol.
        /// </summary>
        public PublishersCustomSymbolRecalculationContract CustomSymbol { get; set; }

        /// <summary>
        /// Subtrade settings.
        /// </summary>
        public PublishersSubtradeSettingRecalculationContract SubtradeSetting { get; set; }

        /// <summary>
        /// Dividends of HD symbol.
        /// </summary>
        public List<PublishersDividendRecalculationContract> DividendsList { get; set; }
    }
}
