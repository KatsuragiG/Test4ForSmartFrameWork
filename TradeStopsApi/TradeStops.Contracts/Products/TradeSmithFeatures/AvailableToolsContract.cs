using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace TradeStops.Contracts
{
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements must be documented", Justification = "Temporary suppression")]
    public class AvailableToolsContract
    {
        public bool RiskRebalancer { get; set; }

        /// <summary>
        /// Indicates availability of Asset Allocation tool.
        /// This tool shows asset allocations by sectors and industries for portfolios. 
        /// </summary>
        public bool AssetAllocation { get; set; }

        /// <summary>
        /// Indicates availability of Portfolio VQ Analyzer (ex-VqAllocation) tool,
        /// that is used to display pie-chart with VQ allocation for portfolio. 
        /// </summary>
        public bool PvqAnalyzer { get; set; }

        /// <summary>
        /// Ex-QuantTool
        /// </summary>
        public bool PureQuant { get; set; }
        public bool PureQuantForStocks { get; set; }
        public bool PureQuantForCrypto { get; set; }

        public bool StockAnalyzer { get; set; }

        /// <summary>
        /// Indicates availability of Screener (ex-StockFinder) tool.
        /// </summary>
        public bool StockFinder { get; set; }

        /// <summary>
        /// Screener (ex-StockFinder): indicates availability of stocks. Used in Assets section on UI.
        /// </summary>
        public bool StockFinderForStocks { get; set; }

        /// <summary>
        /// Screener (ex-StockFinder): indicates availability of Crypto. Used in Assets section on UI.
        /// </summary>
        public bool StockFinderForCrypto { get; set; }

        public bool PositionSizeDollarInvestmentRisk { get; set; } // Ex-PositionSize
        public bool PositionSizeRiskPercentageOfPortfolio { get; set; } // Ex-PositionSize
        public bool PositionSizeEqualPositionRisk { get; set; } // Ex-PositionSize
        public bool BulkPositionSize { get; set; }

        /// <summary>
        /// Backtester is used to analyze stocks performance in the past period using different strategies.
        /// Backtester strategy is used to determine the date to buy a stock,
        /// the necessary amount of shares and the necessary date to sell (close) the stock.
        /// </summary>
        public bool Backtester { get; set; }
    }
}
