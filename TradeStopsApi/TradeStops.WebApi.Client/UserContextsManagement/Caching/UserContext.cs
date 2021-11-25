using System;

namespace TradeStops.WebApi.Client.UserContextsManagement.Caching
{
    internal class UserContext
    {
        private const int ValidMinutes = 25;

        public UserContext(Guid contextKey)
        {
            ContextKey = contextKey;
            ExpirationDate = DateTime.Now.AddMinutes(ValidMinutes);
        }

        public Guid ContextKey { get; set; }

        public DateTime ExpirationDate { get; set; }

        public bool IsExpired()
        {
            return ExpirationDate <= DateTime.Now;
        }
    }
}
