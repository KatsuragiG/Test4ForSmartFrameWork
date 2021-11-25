using System;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Data of a report whose fields contain information that I do not understand")]
    public class FinancialsRatioReportContract
    {
        /// <summary>
        /// Report Id
        /// </summary>
        public int ReportId { get; set; }

        /// <summary>
        /// End of the financial period in UTC
        /// </summary>
        public DateTime PeriodEndDate { get; set; }

        /// <summary>
        /// Indicates that the report is TTM
        /// </summary>
        public bool IsTrailingTwelveMonths { get; set; }

        public long? Ebit { get; set; }

        public long? Ebitda { get; set; }

        public long? AdjustedEbit { get; set; }

        public long? AdjustedEbitda { get; set; }

        public long? FreeCashFlow { get; set; }

        public long? NetOperatingProfitAfterTax { get; set; }

        public long? GrossMargin { get; set; }

        public long? OperatingMargin { get; set; }

        public long? PretaxMargin { get; set; }

        public long? AftertaxMargin { get; set; }

        public long? FreeCashFlowMargin { get; set; }

        public long? TaxRate { get; set; }

        public long? ReturnOnAssets { get; set; }

        public long? ReturnOnEquity { get; set; }

        public long? ReturnOnInvestedCapital { get; set; }

        public long? FreeCashFlowReturnOnAssets { get; set; }

        public long? RevenuePerEmployee { get; set; }

        public long? NetIncomePerEmployee { get; set; }

        public long? DegreeOfFinancialLeverage { get; set; }

        public long? DegreeOfCombinedLeverage { get; set; }

        public long? TotalDebt { get; set; }

        public long? NetDebt { get; set; }

        public long? BookEquity { get; set; }

        public long? NetWorkingCapital { get; set; }

        public long? LongTermCapital { get; set; }

        public long? TotalCapital { get; set; }

        public long? CurrentRatio { get; set; }

        public long? QuickRatio { get; set; }

        public long? CashRatio { get; set; }

        public long? DebtToEquity { get; set; }

        public long? DebtToAssets { get; set; }

        public long? LongTermDebtToLongTermCapital { get; set; }

        public long? LongTermDebtToTotalCapital { get; set; }

        public long? AdjustedNetIncome { get; set; }

        public long? AdjustedEpsAndBasic { get; set; }

        public long? AdjustedEpsAndDiluted { get; set; }
    }
}
