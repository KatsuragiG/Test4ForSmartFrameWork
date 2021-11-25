using System;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Data of a report whose fields contain information that I do not understand")]
    public class IncomeStatementReportContract
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

        public long? Revenue { get; set; }

        public long? OtherRevenue { get; set; }

        public long? TotalOperatingRevenue { get; set; }

        public long? GrossProfit { get; set; }

        public long? SalesAndMarketingExpense { get; set; }

        public long? GeneralAndAdministrativeExpense { get; set; }

        public long? OtherSellingAndGeneralAndAdministrativeExpenses { get; set; }

        public long? SellingAndGeneralAndAdministrativeExpenses { get; set; }

        public long? ResearchAndDevelopmentExpense { get; set; }

        public long? DepreciationExpense { get; set; }

        public long? AmortizationExpense { get; set; }

        public long? DepreciationAndAmortizationExpense { get; set; }

        public long? RestructuringAndRemediationAndImpairmentProvisions { get; set; }

        public long? OtherOperatingExpenses { get; set; }

        public long? OperatingExpenses { get; set; }

        public long? CostAndOperatingExpenses { get; set; }

        public long? TotalOperatingExpenses { get; set; }

        public long? OperatingProfit { get; set; }

        public long? InterestExpense { get; set; }

        public long? InterestIncome { get; set; }

        public long? OtherInterestIncomeAndExpenseAndNet { get; set; }

        public long? InterestIncomeAndExpenseAndNet { get; set; }

        public long? NonoperatingGainsAndLosses { get; set; }

        public long? OtherNonoperatingIncomeAndExpense { get; set; }

        public long? TotalNonoperatingIncomeAndExpense { get; set; }

        public long? IncomeBeforeTaxes { get; set; }

        public long? IncomeTaxes { get; set; }

        public long? IncomeAfterTaxes { get; set; }

        public long? MinorityInterestAndEquityEarnings { get; set; }

        public long? IncomeBeforeExtraordinaryItems { get; set; }

        public long? ExtraordinaryItems { get; set; }

        public long? AccountingChange { get; set; }

        public long? DiscontinuedOperations { get; set; }

        public long? NetIncome { get; set; }

        public long? PreferredDividends { get; set; }

        public long? NetIncomeApplicableToCommon { get; set; }

        public long? NetIncomeFromContinuingOperationsApplicableToCommon { get; set; }

        public long? BasicWeightedAverageShares { get; set; }

        public long? BasicEpsAndNetIncome { get; set; }

        public long? DilutedWeightedAverageShares { get; set; }

        public long? DilutedEpsAndNetIncome { get; set; }

        public long? LaborExpense { get; set; }

        public long? OtherGeneralAndAdministrativeExpense { get; set; }

        public long? NoncontrollingInterest { get; set; }

        public long? EquityEarnings { get; set; }

        public long? BasicEpsAndNetIncomeFromContinuingOperations { get; set; }

        public long? DilutedEpsAndNetIncomeFromContinuingOperations { get; set; }

        public long? DilutedWeightedAverageSharesFirst { get; set; }

        public long? BasicWeightedAverageSharesFirst { get; set; }

        public long? NetIncomeFromContinuingOperationsApplicableToCommonFirst { get; set; }

        public long? RightDilutedAverageShares { get; set; }

        public long? RightBasicAverageShares { get; set; }

        public long? DilutedWeightedAverageSharesCalculated { get; set; }

        public long? BasicWeightedAverageSharesCalculated { get; set; }
    }
}
