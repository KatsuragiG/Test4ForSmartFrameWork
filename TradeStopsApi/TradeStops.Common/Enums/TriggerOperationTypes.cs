namespace TradeStops.Common.Enums
{
    public enum TriggerOperationTypes : byte
    {
        None = 0,

        Less = 2,

        LessOrEqual = 4,

        Equal = 8,

        Greater = 16,

        GreaterOrEqual = 32,

        ModuleDeletion = 64
    }
}
