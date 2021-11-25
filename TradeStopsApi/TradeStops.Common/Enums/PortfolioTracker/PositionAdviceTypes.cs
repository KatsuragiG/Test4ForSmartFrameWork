namespace TradeStops.Common.Enums
{
    public enum PositionAdviceTypes : byte
    {
        None = 0,
        BuyCover = 1, 
        BuyToClose = 2, 
        BuyToOpen = 3, 
        Hold = 4, 
        HoldShort = 5, 
        Reduce = 6, 
        Sell = 7, 
        SellPut = 8, 
        SellShort = 9, 
        SellToOpen = 10
    }
}
