namespace TradeStops.Common.Enums
{
    public enum LoginResults : byte
    {
        Unknown = 0,

        Success = 1,

        InvalidPassword = 2,

        InvalidUserName = 3,

        UnhandledException = 4,

        Locked = 5
    }
}
