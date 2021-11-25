using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Alerts grid row
    /// </summary>
    public class AlertsGridRowContract
    {
        /// <summary>
        /// See Position Trigger contract values.
        /// </summary>
        public PositionTriggerContract PositionTrigger { get; set; }

        /// <summary>
        /// Position ID.
        /// </summary>
        public int PositionId { get; set; }

        /// <summary>
        /// Position notes.
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Position Trigger on Position that is not adjusted by dividends.
        /// </summary>
        public bool IgnoreDividends { get; set; }

        /// <summary>
        /// Position type in the TradeStops
        /// </summary>
        public PositionTypes PositionType { get; set; }

        /// <summary>
        /// Position trade type. 
        /// </summary>
        public TradeTypes TradeType { get; set; }

        /// <summary>
        /// Position purchase date.
        /// </summary>
        public DateTime? PurchaseDate { get; set; }

        /// <summary>
        /// Position purchase price not adjusted by corporate actions.
        /// </summary>
        public decimal? PurchasePrice { get; set; }

        /// <summary>
        /// Purchase price adjusted by all corporate actions.
        /// </summary>
        public decimal? PurchasePriceAdj { get; set; }

        /// <summary>
        /// Purchase price not adjusted by dividends.
        /// </summary>
        public decimal? SplitsAdj { get; set; }

        /// <summary>
        /// Number of the position shares.
        /// </summary>
        public decimal? Shares { get; set; }

        /// <summary>
        /// Position close price.
        /// </summary>
        public decimal? LatestClose { get; set; }

        /// <summary>
        /// Position cost basis.
        /// </summary>
        public decimal? CostBasis { get; set; }

        /// <summary>
        /// Percentage that represents how much the latest close is below the high close.
        /// </summary>
        public decimal? PercentOffHigh { get; set; }

        /// <summary>
        /// Date when the max profitable close price was reached.
        /// </summary>
        public DateTime? MaxProfitableCloseDate { get; set; }

        /// <summary>
        /// The max profitable closing price since entry.
        /// </summary>
        public decimal? MaxProfitableClosePrice { get; set; }

        /// <summary>
        /// Max Gain %.
        /// </summary>
        public decimal? MaxGainPercent { get; set; }

        /// <summary>
        /// Min Gain %.
        /// </summary>
        public decimal? MinGainPercent { get; set; }

        /// <summary>
        /// 10 day Simple Moving Average.
        /// </summary>
        public decimal? MovingAvg10 { get; set; }

        /// <summary>
        /// 200 day Simple Moving Average.
        /// </summary>
        public decimal? MovingAvg200 { get; set; }

        /// <summary>
        /// 50 day Simple Moving Average.
        /// </summary>
        public decimal? MovingAvg50 { get; set; }

        /// <summary>
        /// Position portfolio ID.
        /// </summary>
        public int PortfolioId { get; set; }

        /// <summary>
        /// Position portfolio name.
        /// </summary>
        public string PortfolioName { get; set; }

        /// <summary>
        /// Position Trigger on Position from synchronized portfolio.
        /// </summary>
        public bool IsSyncPortfolio { get; set; }

        /// <summary>
        /// The number of calendar days since the Position Trigger has been triggered.
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// Position symbol.
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// Company name.
        /// </summary>
        public string SymbolName { get; set; }

        /// <summary>
        /// Symbol type
        /// </summary>
        [Obsolete("2020-10-26. Use SymbolType instead.")]
        public SymbolDataTypes SymbolDataType { get; set; }

        /// <summary>
        /// SymbolType (stock, option, fund, etc.)
        /// </summary>
        public SymbolTypes SymbolType { get; set; }

        /// <summary>
        /// Sign of the Position Trigger currency.
        /// </summary>
        public string CurrencySign { get; set; }

        /// <summary>
        /// Currency code of the Position Trigger.
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Position VQ value.
        /// </summary>
        public decimal? VolatilityQuotient { get; set; }

        /// <summary>
        /// Contract containing SSI values.
        /// </summary>
        public SsiValueContract SsiValue { get; set; }

        /// <summary>
        /// Underlying stock ID of option.
        /// </summary>
        public int? ParentSymbolId { get; set; }

        /// <summary>
        /// Underlying symbol of option.
        /// </summary>
        public string ParentSymbol { get; set; }

        /// <summary>
        /// Company name of the underlying symbol of option.
        /// </summary>
        public string ParentSymbolName { get; set; }

        /// <summary>
        /// Latest close price of underlying stock for option.
        /// </summary>
        public decimal? ParentLatestClose { get; set; }

        /// <summary>
        /// Price when the Position Trigger will be triggered. 
        /// </summary>
        public decimal? StopPrice { get; set; }

        /// <summary>
        /// Amount of money to risk. 
        /// </summary>
        public decimal? Risk { get; set; }

        /// <summary>
        /// Defines that Position Tigger has been triggered.
        /// </summary>
        public bool IsTriggered { get; set; }

        /// <summary>
        /// The number of calendar days since the Position Trigger has been triggered.
        /// </summary>
        public int DaysTriggered { get; set; }

        /// <summary>
        /// Earliest date when Position Trigger has been triggered.
        /// </summary>
        public DateTime? FirstTimeTriggered { get; set; }

        /// <summary>
        /// Latest date when Position Trigger has been triggered.
        /// </summary>
        public DateTime? LastTriggered { get; set; }

        /// <summary>
        /// Trailing Stop percent value of the first Trailing Stop or VQ trigger.
        /// </summary>
        public decimal? TrailingStopPercent { get; set; }

        /// <summary>
        /// Latest symbol intraday price, if alert has been set up for using intraday prices.
        /// </summary>
        public decimal? LatestIntradayPrice { get; set; }
    }
}
