namespace TradeStops.Common.Constants
{
    public static class SystemEventCategoryNames
    {
        public const string PortfolioCreated = "portfolioCreated";

        public const string PortfolioDeleted = "portfolioDeleted";

        public const string PortfolioEdited = "PortfolioEdited";

        public const string PositionCreated = "purchase(open)";

        public const string PositionClosed = "sell(close)";

        public const string PositionPartialClosed = "partialSell(close)";

        public const string PositionMoved = "movePosition";

        public const string PositionDeleted = "deletePosition";

        public const string PositionEdited = "PortfolioEdited";

        public const string AlertCreated = "alertCreated";

        public const string AlertEdited = "alertEdited";

        public const string AlertCancelled = "alertCancelled";
    }
}
