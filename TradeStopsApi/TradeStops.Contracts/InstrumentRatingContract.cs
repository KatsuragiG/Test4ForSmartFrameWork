using System;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Instrument rating data.
    /// </summary>
    public class InstrumentRatingContract
    {
        /// <summary>
        /// Symbol.
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// Last trade date.
        /// </summary>
        public DateTime LastTradeDate { get; set; }

        /// <summary>
        /// Recalculation time.
        /// </summary>
        public DateTime RecalculationTime { get; set; }

        /// <summary>
        /// Momentum is the rate of acceleration of a security's price or volume.
        /// </summary>
        public decimal? Momentum { get; set; }

        /// <summary>
        /// Directional Movement Index identifies in which direction the price of an asset is moving.
        /// </summary>
        public decimal? DirectionalMovementIndex { get; set; }

        /// <summary>
        /// Up and down volume is a common standard to mark volume as volume traded during price advance (green volume bars) and as volume traded during price decline (red volume bars).
        /// </summary>
        public decimal? UpDownVolume { get; set; }

        /// <summary>
        /// Trailing Returns is jsut gain for fixing time frames.
        /// </summary>
        public decimal? TrailingReturns { get; set; }

        /// <summary>
        /// Size of company.
        /// </summary>
        public decimal? Size { get; set; }

        /// <summary>
        /// Total enterprise value.
        /// </summary>
        public decimal? TotalEnterpriseValue { get; set; }

        /// <summary>
        /// Liquidity.
        /// </summary>
        public decimal? Liquidity { get; set; }

        /// <summary>
        /// Volatility is a statistical measure of the dispersion of returns for a given security or market index.
        /// </summary>
        public decimal? Volatility { get; set; }

        /// <summary>
        /// Beta is a measure of a stock's volatility in relation to the overall market.
        /// </summary>
        public decimal? Beta { get; set; }

        /// <summary>
        /// Average True Range is technical analysis indicator that measures market volatility by decomposing the entire range of an asset price for that period.
        /// </summary>
        public decimal? AverageTrueRange { get; set; }

        /// <summary>
        /// Risk Adjusted is harmful volatility from total overall volatility by using the asset's standard deviation of negative portfolio returns.
        /// </summary>
        public decimal? RiskAdjusted { get; set; }

        /// <summary>
        /// Value is calculated using fundamental data.
        /// </summary>
        public decimal? Value { get; set; }

        /// <summary>
        /// Price Book ratio.
        /// </summary>
        public decimal? PriceBookRatio { get; set; }

        /// <summary>
        /// Price Cash Flow ratio.
        /// </summary>
        public decimal? PriceCashFlowRatio { get; set; }

        /// <summary>
        /// Price Earnings ratio.
        /// </summary>
        public decimal? PriceEarningsRatio { get; set; }

        /// <summary>
        /// Price Sales ratio.
        /// </summary>
        public decimal? PriceSalesRatio { get; set; }

        /// <summary>
        /// Total enterprise value / earnings before interest, taxes, depreciation, and amortization.
        /// </summary>
        public decimal? TotalEnterpriseValueEBITDA { get; set; }

        /// <summary>
        /// Quality.
        /// </summary>
        public decimal? Quality { get; set; }

        /// <summary>
        /// Return On: Assets, Equity, Invested Capital.
        /// </summary>
        public decimal? ReturnOnAEIC { get; set; }

        /// <summary>
        /// Fundamental data.
        /// </summary>
        public decimal? CashFlow { get; set; }

        /// <summary>
        /// Margins.
        /// </summary>
        public decimal? Margins { get; set; }

        /// <summary>
        /// Debt.
        /// </summary>
        public decimal? Debt { get; set; }

        /// <summary>
        /// Efficiency.
        /// </summary>
        public decimal? Efficiency { get; set; }

        /// <summary>
        /// Growth.
        /// </summary>
        public decimal? Growth { get; set; }

        /// <summary>
        /// Earnings per sales.
        /// </summary>
        public decimal? EarningsPerSales { get; set; }

        /// <summary>
        /// Sales.
        /// </summary>
        public decimal? Sales { get; set; }

        /// <summary>
        /// Net income.
        /// </summary>
        public decimal? NetIncome { get; set; }

        /// <summary>
        /// Total Rank.
        /// </summary>
        public decimal? TotalRank { get; set; }
    }
}
