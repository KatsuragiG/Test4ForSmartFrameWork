using System;
using System.Threading.Tasks;

using TradeStops.Contracts;

namespace TradeStops.WebApi.Client.UserContextsManagement
{
    public interface IUserContextsManager
    {
        UserContextContract GetUserContext(Guid userGuid, Guid? agentGuid = null);
        Task<UserContextContract> GetUserContextAsync(Guid userGuid, Guid? agentGuid = null);
        UserContextContract GetTradeSmithUserContext(Guid tradeSmithUserGuid, Guid? agentGuid = null);
        Task<UserContextContract> GetTradeSmithUserContextAsync(Guid tradeSmithUserGuid, Guid? agentGuid = null);
        Task<Guid> GetContextKeyAsync(Guid tradeSmithUserGuid, Guid? agentGuid = null);
    }
}