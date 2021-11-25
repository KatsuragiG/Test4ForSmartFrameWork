using System;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Data of a report whose fields contain information that I do not understand")]
    public class CashFlowStatementReportContract
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

        public long? CfNetIncome { get; set; }

        public long? CfDepreciationAndAmortization { get; set; }

        public long? EmployeeCompensation { get; set; }

        public long? DeferredIncomeTaxes { get; set; }

        public long? RealizedGainsAndLosses { get; set; }

        public long? OtherAdjustments { get; set; }

        public long? ChangeInAccountsReceivable { get; set; }

        public long? ChangeInOtherCurrentAssets { get; set; }

        public long? ChangeInCurrentAssets { get; set; }

        public long? ChangeInOtherAssets { get; set; }

        public long? ChangeInAccountsPayableAndAccruedExpenses { get; set; }

        public long? ChangeInDeferredRevenue { get; set; }

        public long? ChangeInIncomeTaxesPayable { get; set; }

        public long? ChangeInOtherCurrentLiabilities { get; set; }

        public long? ChangeInCurrentLiabilities { get; set; }

        public long? ChangeInOtherLiabilities { get; set; }

        public long? OtherAssetAndLiabilityChangesAndNet { get; set; }

        public long? ChangeInOperatingAssetsAndLiabilities { get; set; }

        public long? TotalAdjustments { get; set; }

        public long? CashFromOperatingActivities { get; set; }

        public long? CapitalExpenditures { get; set; }

        public long? PurchaseOfInvestments { get; set; }

        public long? SaleOfInvestments { get; set; }

        public long? AcquisitionAndSaleOfBusinessAndNet { get; set; }

        public long? OtherInvestmentChangesAndNet { get; set; }

        public long? InvestmentChangesAndNet { get; set; }

        public long? SaleOfPropertyAndPlantAndEquipment { get; set; }

        public long? OtherInvestingActivities { get; set; }

        public long? CashFromInvestingActivities { get; set; }

        public long? ChangeInShorttermBorrowingsAndNet { get; set; }

        public long? LongtermDebtProceeds { get; set; }

        public long? LongtermDebtPayments { get; set; }

        public long? OtherDebtAndNet { get; set; }

        public long? ChangeInLongtermDebtAndNet { get; set; }

        public long? ChangeInDebtAndNet { get; set; }

        public long? IssuanceOfEquity { get; set; }

        public long? RepurchaseOfEquity { get; set; }

        public long? OtherEquityTransactionsAndNet { get; set; }

        public long? ChangeInEquityAndNet { get; set; }

        public long? DividendsPaid { get; set; }

        public long? OtherFinancingActivitiesAndNet { get; set; }

        public long? CashFromFinancingActivities { get; set; }

        public long? CashFromDiscontinuedOperations { get; set; }

        public long? EffectOfExchangeRateOnCash { get; set; }

        public long? NetChangeInCash { get; set; }

        public long? CashAndCashEquivalentsAndBeginningOfYear { get; set; }

        public long? CashAndCashEquivalentsAndEndOfYear { get; set; }

        public long? CashPaidForIncomeTaxes { get; set; }

        public long? CashPaidForInterestExpense { get; set; }

        public long? StockOptionTaxBenefits { get; set; }

        public long? AdjustmentForSpecialCharges { get; set; }

        public long? AdjustmentForMinorityInterest { get; set; }

        public long? AdjustmentForEquityEarnings { get; set; }
    }
}
