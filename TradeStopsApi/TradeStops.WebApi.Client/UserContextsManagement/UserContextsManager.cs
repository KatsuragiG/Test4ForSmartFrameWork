using System;
using System.Threading.Tasks;

using JWT;
using JWT.Algorithms;
using JWT.Serializers;

using TradeStops.Contracts;
using TradeStops.WebApi.Client.Configuration;
using TradeStops.WebApi.Client.Generated;
using TradeStops.WebApi.Client.UserContextsManagement.Caching;

namespace TradeStops.WebApi.Client.UserContextsManagement
{
    // TODO: Use BaseCache.cs here instead of InMemoryCache.
    // In the BaseCache.cs use method with expirationTimeMethod parameter to calculate expiration time based on the item expiration.
    public class UserContextsManager : IUserContextsManager
    {
        private readonly IUserContextsClient _userContextsClient;
        private readonly IClientSettings _clientSettings;

        private readonly IUserContextsCache _userContextsCache;
        private readonly IJwtEncoder _jwtEncoder;

        public UserContextsManager(IUserContextsClient userContextsClient, IClientSettings clientSettings)
        {
            _userContextsClient = userContextsClient;
            _clientSettings = clientSettings;

            _userContextsCache = new UserContextsCache();
            _jwtEncoder = new JwtEncoder(new HMACSHA256Algorithm(), new JsonNetSerializer(), new JwtBase64UrlEncoder());
        }

        public UserContextContract GetUserContext(Guid userGuid, Guid? agentGuid = null)
        {
            return GetTradeSmithUserContext(userGuid, agentGuid);
        }

        public async Task<UserContextContract> GetUserContextAsync(Guid userGuid, Guid? agentGuid = null)
        {
            return await GetTradeSmithUserContextAsync(userGuid, agentGuid);
        }

        public UserContextContract GetTradeSmithUserContext(Guid tradeSmithUserGuid, Guid? agentGuid = null)
        {
            var userContextCacheKey = $"{tradeSmithUserGuid}_{agentGuid}";
            var userContext = _userContextsCache.Get(userContextCacheKey);

            if (userContext != null && !userContext.IsExpired())
            {
                return new UserContextContract { ContextKey = userContext.ContextKey };
            }

            var apiTokenPayload = new CreateTradeSmithUserContextPayloadContract
            {
                JwtId = Guid.NewGuid(),
                IssuedAt = DateTime.UtcNow,
                ExpirationAt = DateTime.UtcNow.AddSeconds(_clientSettings.ApiTokenExpirationInSeconds),
                TradeSmithUserGuid = tradeSmithUserGuid,
                AgentGuid = agentGuid,
            };

            var apiToken = _jwtEncoder.Encode(apiTokenPayload, _clientSettings.ApiTokenEncryptionKey);
            var apiTokenContract = new JsonWebTokenContract { Value = apiToken };
            var userContextContract = _userContextsClient.CreateTradeSmithUserContext(apiTokenContract);

            userContext = new UserContext(userContextContract.ContextKey);
            _userContextsCache.Set(userContextCacheKey, userContext);

            return userContextContract;
        }

        public async Task<UserContextContract> GetTradeSmithUserContextAsync(Guid tradeSmithUserGuid, Guid? agentGuid = null)
        {
            var userContextCacheKey = $"{tradeSmithUserGuid}_{agentGuid}";
            var userContext = _userContextsCache.Get(userContextCacheKey);

            if (userContext != null && !userContext.IsExpired())
            {
                return new UserContextContract { ContextKey = userContext.ContextKey };
            }

            var apiTokenPayload = new CreateTradeSmithUserContextPayloadContract
            {
                JwtId = Guid.NewGuid(),
                IssuedAt = DateTime.UtcNow,
                ExpirationAt = DateTime.UtcNow.AddSeconds(_clientSettings.ApiTokenExpirationInSeconds),
                TradeSmithUserGuid = tradeSmithUserGuid,
                AgentGuid = agentGuid,
            };

            var apiToken = _jwtEncoder.Encode(apiTokenPayload, _clientSettings.ApiTokenEncryptionKey);
            var apiTokenContract = new JsonWebTokenContract { Value = apiToken };
            var userContextContract = await _userContextsClient.CreateTradeSmithUserContextAsync(apiTokenContract);

            userContext = new UserContext(userContextContract.ContextKey);
            _userContextsCache.Set(userContextCacheKey, userContext);

            return userContextContract;
        }

        // this method is preferable to use because it doesn't require reference to TradeStops.Contracts in the code where it's used
        public async Task<Guid> GetContextKeyAsync(Guid tradeSmithUserGuid, Guid? agentGuid = null)
        {
            var contract = await GetTradeSmithUserContextAsync(tradeSmithUserGuid, agentGuid);

            return contract.ContextKey;
        }
    }
}
