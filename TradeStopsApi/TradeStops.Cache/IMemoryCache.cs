namespace TradeStops.Cache
{
    /// <summary>
    /// Interface is used to explicitly indicate that we need to use fast MemoryCache storage instead of Redis.
    /// </summary>
    // todo: consider to create BaseMemoryCache (single-instance) that will provide only Get-methods and won't require ICache and tempItemsExpiration parameters.
    // todo: consider to create BaseRedisCache (multi-instance) that will provide Get+Set/Clear methods and probably won't require ICache.
    public interface IMemoryCache : ICache
    {
    }
}
