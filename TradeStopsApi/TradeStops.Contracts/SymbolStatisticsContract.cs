using System;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class SymbolStatisticsContract
    {
        public int SymbolId { get; set; }

        /// <summary>
        /// Trade date.
        /// </summary>
        public DateTime TradeDate { get; set; }

        /// <summary>
        /// Latest symbol close price.
        /// </summary>
        public decimal? LatestClose { get; set; }

        /// <summary>
        /// Previous symbol close price.
        /// </summary>
        public decimal? PreviousClose { get; set; }

        /// <summary>
        /// Latest symbol trade volume.
        /// </summary>
        public decimal? Volume { get; set; }

        /// <summary>
        /// Average trade volume of the symbol for a 100-day period. /p>
        /// </summary>
        public decimal? Avg100Volume { get; set; }

        /// <summary>
        /// Absolute percentage performance of the share price over the past one year.
        /// </summary>
        public decimal? OneYearChangeInPercent { get; set; }

        /// <summary>
        /// Absolute percentage performance of the share price over the past three years.
        /// </summary>
        public decimal? ThreeYearsChangeInPercent { get; set; }

        /// <summary>
        /// Absolute percentage performance of the share price over the past five years.
        /// </summary>
        public decimal? FiveYearsChangeInPercent { get; set; }

        /// <summary>
        /// Lowest price at which the stock has traded at in the last year.
        /// </summary>
        public decimal? OneYearLow { get; set; }

        /// <summary>
        /// Highest price at which the stock has traded at in the last year.
        /// </summary>
        public decimal? OneYearHigh { get; set; }

        /// <summary>
        /// Performance value of the share price over the last trade day.
        /// This value can be used to display 'Daily Gain' information
        /// </summary>
        public decimal? OneDayChange { get; set; }

        /// <summary>
        /// Absolute percentage performance of the share price over the last trade day.
        /// </summary>
        public decimal? OneDayChangeInPercent { get; set; }

        /// <summary>
        /// Absolute percentage price performance of the share price over the past one month.
        /// </summary>
        public decimal? OneMonthChangeInPercent { get; set; }

        /// <summary>
        /// Absolute percentage price performance of the share price over the past three months.
        /// </summary>
        public decimal? ThreeMonthsChangeInPercent { get; set; }

        /// <summary>
        /// Absolute change of the share price over the past one year to the current date.
        /// </summary>
        public decimal? YtdChange { get; set; }

        /// <summary>
        /// Absolute percentage price performance of the share price over the past one week.
        /// </summary>
        public decimal? OneWeekChangeInPercent { get; set; }

        /// <summary>
        /// Average trade volume of the symbol for a 10-day period.
        /// </summary>
        public decimal? Avg10Volume { get; set; }

        /// <summary>
        /// Average VQ value fot 30 years
        /// </summary>
        public decimal? Average30YearsVolatilityQuotient { get; set; }

        /// <summary>
        /// Highest price at which the stock has traded at in the last trade day.
        /// </summary>
        public decimal? OneDayHigh { get; set; }

        /// <summary>
        /// Lowest price at which the stock has traded at in the last trade day.
        /// </summary>
        public decimal? OneDayLow { get; set; }

        /// <summary>
        /// Highest price at which the stock has traded at in the last week.
        /// </summary>
        public decimal? OneWeekHigh { get; set; }

        /// <summary>
        /// Lowest price at which the stock has traded at in the last week.
        /// </summary>
        public decimal? OneWeekLow { get; set; }

        /// <summary>
        /// Highest price at which the stock has traded at in the last month.
        /// </summary>
        public decimal? OneMonthHigh { get; set; }

        /// <summary>
        /// Lowest price at which the stock has traded at in the last month.
        /// </summary>
        public decimal? OneMonthLow { get; set; }

        /// <summary>
        /// Trailing Dividend Yield.
        /// </summary>
        public decimal? TrailingDividendYield { get; set; }

        /// <summary>
        /// Forward Dividend Yield.
        /// </summary>
        public decimal? ForwardDividendYield { get; set; }

        /// <summary>
        /// Average trade volume of the symbol for a 1-year period.
        /// </summary>
        public decimal? OneYearAvgVolume { get; set; }

        /// <summary>
        /// Average trade volume value of the symbol for a 1-year period.
        /// </summary>
        public decimal? OneYearAvgVolumeValue { get; set; }

        /// <summary>
        /// Short Interest
        /// </summary>
        public decimal? ShortInterest { get; set; }

        /// <summary>
        ///  Average Volume for 30 days
        /// </summary>
        public decimal? Avg30Volume { get; set; }

        /// <summary>
        /// Absolute short interest percentage of float.
        /// </summary>
        public decimal? ShortPercentOfFloat { get; set; }
    }
}