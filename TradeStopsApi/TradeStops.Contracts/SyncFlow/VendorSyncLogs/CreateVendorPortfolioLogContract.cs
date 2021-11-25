using System;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to create Vendor portfolio log.
    /// </summary>
    public class CreateVendorPortfolioLogContract
    {
        /// <summary>
        ///  Portfolio name.
        /// </summary>
        public string PortfolioName { get; set; }

        /// <summary>
        ///  Vendor portfolio ID.
        /// </summary>
        public string VendorPortfolioId { get; set; }

        /// <summary>
        /// Vendor sync log ID. 
        /// </summary>
        public int VendorSyncLogId { get; set; }

        /// <summary>
        ///  Portfolio cash.
        /// </summary>
        public decimal Cash { get; set; }

        /// <summary>
        ///  Currency name.
        /// </summary>
        public string CurrencyName { get; set; }

        /// <summary>
        ///  Portfolio total balance.
        /// </summary>
        public decimal TotalBalance { get; set; }

        /// <summary>
        ///  Portfolio money market balance.
        /// </summary>
        public decimal MoneyMarketBalance { get; set; }

        /// <summary>
        ///  Margin balance.
        /// </summary>
        public decimal MarginBalance { get; set; }

        /// <summary>
        ///  Cash to withdrawal.
        /// </summary>
        public decimal CashToWithdrawal { get; set; }

        /// <summary>
        ///  Account type.
        /// </summary>
        public string AccountType { get; set; }

        /// <summary>
        ///  Positions amount.
        /// </summary>
        public int PositionsAmount { get; set; }

        /// <summary>
        ///  Transactions amount.
        /// </summary>
        public int TransactionsAmount { get; set; }

        /// <summary>
        ///  Uploaded file name.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        ///  (optional) From date for requested transactions of portfolio.
        /// </summary>
        public DateTime? TransactionsPeriodFrom { get; set; }

        /// <summary>
        ///  (optional) To date for requested transactions of portfolio.
        /// </summary>
        public DateTime? TransactionsPeriodTo { get; set; }
    }
}
