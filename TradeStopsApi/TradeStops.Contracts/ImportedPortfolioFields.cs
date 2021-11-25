using System;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class ImportedPortfolioFields
    {
        /// <summary>
        /// (sync) Unique identifier for the synchronized portfolio in the vendor system.
        /// </summary>
        public string       VendorPortfolioId         { get; set; } // it can be non-nullable

        /// <summary>
        /// (sync) Unique identifier of the user account in the vendor system.
        /// </summary>
        public string       VendorAccountId           { get; set; } // it can be non-nullable

        /// <summary>
        /// (sync) Brokerage identifier of the synchronized portfolio.
        /// </summary>
        public int?         FinancialInstitutionId    { get; set; } // it can be non-nullable: select * from tradestops3.dbo.portfolios where vendorPortfolioId is not null and financialinstitutionid is null -- return 0 rows

        /// <summary>
        /// (sync) Total value of the synchronized portfolio.
        /// </summary>
        public decimal?     VendorPortfolioTotalValue { get; set; }

        /// <summary>
        /// (sync) Property to identify whether the portfolio requires new brokerage credentials.
        /// </summary>
        public bool         IsRequireNewCredentials   { get; set; }

        /// <summary>
        /// (sync) Status of the credentials update process for the portfolio in the vendor system.
        /// </summary>
        public string       UpdateCredentialsStatus   { get; set; }

        /// <summary>
        /// (sync) Initial portfolio name on a brokerage website.
        /// </summary>
        public string       AccountNumber             { get; set; }

        /// <summary>
        /// (sync) Latest refresh date of the synchronized portfolio.
        /// </summary>
        public DateTime?    UpdateDate                { get; set; }

        /// <summary>
        /// (sync) Property to identify whether the System has to show a pop-up notification for a user.
        /// </summary>
        public int?         ShowInfoPopupForError     { get; set; }

        /// <summary>
        /// (sync) Number of already sent emails to user.
        /// </summary>
        public int?         NumberOfSentEmails        { get; set; }
    }
}