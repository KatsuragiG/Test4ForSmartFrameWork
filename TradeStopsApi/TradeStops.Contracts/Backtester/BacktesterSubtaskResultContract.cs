using System;
using System.Collections.Generic;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Result of backtesting process.
    /// Each SubtaskResult represents result for each strategy in the task.
    /// So if a task has 3 strategies to backtest, then there will be 3 SubtaskResults for the task.
    /// </summary>
    public class BacktesterSubtaskResultContract
    {
        /// <summary>
        /// ID of the Backtester task
        /// </summary>
        public int TaskId { get; set; }

        /// <summary>
        /// ID of the Backtester Strategy
        /// </summary>
        public int StrategyId { get; set; }

        /// <summary>
        /// Backtesting results that should be displayed in form of chart
        /// </summary>
        public List<BacktesterSubtaskResultChartPointContract> ChartPoints { get; set; }

        /// <summary>
        /// Net Profit $. The last point from the Dollar Gain chart line.
        /// </summary>
        public decimal? NetProfit { get; set; }

        /// <summary>
        /// Net Profit %. The last point from the Percent Gain chart line.
        /// </summary>
        public decimal? NetProfitPercent { get; set; }

        /// <summary>
        /// Compound Annual Growth Return % (CAGR %)
        /// </summary>
        public decimal? CagrPercent { get; set; }

        /// <summary>
        /// Average Gain $
        /// </summary>
        public decimal? AverageGain { get; set; }

        /// <summary>
        /// Average Gain %
        /// </summary>
        public decimal? AverageGainPercent { get; set; }

        /// <summary>
        /// Average Days Held.
        /// </summary>
        public decimal? AverageDaysHeld { get; set; }

        /// <summary>
        /// Winners %
        /// </summary>
        public decimal? WinnersPercent { get; set; }

        /// <summary>
        /// Average Winner Gain %
        /// </summary>
        public decimal? AverageWinnerGainPercent { get; set; }

        /// <summary>
        /// Average Winner Days Held
        /// </summary>
        public decimal? AverageWinnerDaysHeld { get; set; }

        /// <summary>
        /// Maximum System Drawdown %
        /// </summary>
        public decimal? MaxDrawdownPercent { get; set; }

        /// <summary>
        /// Maximum System Drawdown $
        /// </summary>
        public decimal? MaxDrawdown { get; set; }

        /// <summary>
        /// Sharpe Ratio. It's just a number, not % or $.
        /// </summary>
        public decimal? SharpeRatio { get; set; }

        /// <summary>
        /// The list of Trades that were done for corresponding strategy.
        /// </summary>
        public List<BacktesterSubtaskResultTradeContract> Trades { get; set; }

        /////// <summary>
        /////// PVQ%
        /////// </summary>
        ////public decimal PortfolioVqPercent { get; set; }

        /////// <summary>
        /////// Net Profit % / PVQ %
        /////// </summary>
        ////public decimal NetProfitPercentDividedByPortfolioVqPercent { get; set; }

        /////// <summary>
        /////// CAGR % / Maximum System Drawdown % 
        /////// </summary>
        ////public decimal CagrPercentDividedByMaximumSystemDrawdownPercent { get; set; }

        /////// <summary>
        /////// Diversification Quotient
        /////// </summary>
        ////public decimal Dq { get; set; }
    }
}
