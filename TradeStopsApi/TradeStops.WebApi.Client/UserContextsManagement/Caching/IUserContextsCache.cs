namespace TradeStops.WebApi.Client.UserContextsManagement.Caching
{
    internal interface IUserContextsCache
    {
        UserContext Get(string key);

        void Set(string key, UserContext userContext);

        void Remove(string key);
    }
}
