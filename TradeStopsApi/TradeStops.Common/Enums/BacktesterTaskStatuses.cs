namespace TradeStops.Common.Enums
{
    // todo: consider to create separate static-data table and enum for subtask statuses
    public enum BacktesterTaskStatuses
    {
        Pending = 1,
        InProcess = 2,
        Failed = 3,
        Success = 4
    }
}