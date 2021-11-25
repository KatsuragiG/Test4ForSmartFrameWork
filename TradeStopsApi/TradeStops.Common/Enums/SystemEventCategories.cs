using System;

namespace TradeStops.Common.Enums
{
    public enum SystemEventCategories
    {
        PurchaseOpen            = 1,
        SellClose               = 2,
        AlertTriggered          = 3,
        AlertCancelled          = 6,
        SplitIssued             = 7,
        DividendIssued          = 8,
        PartialSellClose        = 10,
        DeletePosition          = 11,
        AlertCreated            = 12,
        AlertEdited             = 13,
        PortfolioCreated        = 14,
        PortfolioDeleted        = 15,
        SpinoffIssued           = 16,
        StockDistributionIssued = 17,
        MovePosition            = 18,
        PortfolioEdited         = 19,
        CopyPosition            = 20,
        PositionEdited          = 21,

        // SymbolRenamed = 4 // does not exists in live database
        // SymbolDelisted = 5 // does not exists in live database
    }
}
