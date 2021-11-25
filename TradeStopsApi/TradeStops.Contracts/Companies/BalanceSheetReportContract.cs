using System;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Data of a report whose fields contain information that I do not understand")]
    public class BalanceSheetReportContract
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

        public long? CashAndCashEquivalents { get; set; }

        public long? ShortTermInvestments { get; set; }

        public long? CashAndCashEquivalentsAndShortTermInvestments { get; set; }

        public long? AccountsReceivableAndTradeAndNet { get; set; }

        public long? OtherAccountsAndNotesReceivable { get; set; }

        public long? OtherReceivables { get; set; }

        public long? TotalReceivablesAndNet { get; set; }

        public long? InventoriesAndNet { get; set; }

        public long? DeferredIncomeTaxesAndCurrent { get; set; }

        public long? PrepaidExpenses { get; set; }

        public long? OtherCurrentAssets { get; set; }

        public long? TotalCurrentAssets { get; set; }

        public long? PropertyAndEquipmentAndGross { get; set; }

        public long? AccumulatedDepreciation { get; set; }

        public long? PropertyAndPlantAndEquipmentAndNet { get; set; }

        public long? LongtermInvestments { get; set; }

        public long? OtherInvestments { get; set; }

        public long? Goodwill { get; set; }

        public long? IntangibleAssets { get; set; }

        public long? GoodwillAndIntangibleAssetsAndNet { get; set; }

        public long? DeferredCharges { get; set; }

        public long? AccruedInterest { get; set; }

        public long? OtherAssets { get; set; }

        public long? TotalLongtermAssets { get; set; }

        public long? TotalAssets { get; set; }

        public long? AccountsPayable { get; set; }

        public long? AccruedExpenses { get; set; }

        public long? AccountsPayableAndAccruedExpenses { get; set; }

        public long? DeferredLiabilityCharges { get; set; }

        public long? IncomeTaxesPayable { get; set; }

        public long? CurrentPortionOfLongtermDebt { get; set; }

        public long? ShortTermBorrowings { get; set; }

        public long? TotalShortTermDebt { get; set; }

        public long? OtherCurrentLiabilities { get; set; }

        public long? TotalCurrentLiabilities { get; set; }

        public long? LongtermDebt { get; set; }

        public long? OtherBorrowings { get; set; }

        public long? NotesPayable { get; set; }

        public long? TotalLongtermDebt { get; set; }

        public long? PensionAndPostretirementObligation { get; set; }

        public long? LongtermDeferredLiabilityCharges { get; set; }

        public long? OtherLiabilities { get; set; }

        public long? MinorityInterest { get; set; }

        public long? TotalLongtermLiabilities { get; set; }

        public long? TotalLiabilities { get; set; }

        public long? CommitmentsAndContingencies { get; set; }

        public long? TemporaryEquity { get; set; }

        public long? CommonStock { get; set; }

        public long? PreferredStock { get; set; }

        public long? AdditionalPaidinCapital { get; set; }

        public long? RetainedEarnings { get; set; }

        public long? TreasuryStock { get; set; }

        public long? OtherAccumulatedComprehensiveIncome { get; set; }

        public long? OtherEquity { get; set; }

        public long? TotalStockholdersEquity { get; set; }

        public long? PartnersCapital { get; set; }

        public long? LiabilitiesAndStockholdersEquity { get; set; }

        public long? TotalSharesOutstanding { get; set; }

        public long? NumberOfEmployees { get; set; }

        public long? NumberOfShareholders { get; set; }

        public long? OperatingLeases { get; set; }

        public long? RestrictedCash { get; set; }

        public long? DeferredIncomeTaxesAndLongterm { get; set; }

        public long? OtherAccountsPayableAndAccruedExpenses { get; set; }

        public long? DeferredIncomeTaxLiabilitiesAndShortTerm { get; set; }

        public long? LongtermDeferredIncomeTaxLiabilities { get; set; }

        public long? AdditionalPaidinCapitalPreferredStock { get; set; }
    }
}
