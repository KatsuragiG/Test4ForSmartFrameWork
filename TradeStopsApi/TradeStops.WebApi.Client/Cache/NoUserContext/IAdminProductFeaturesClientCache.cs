using System;
using System.Threading.Tasks;

using TradeStops.Contracts;

namespace TradeStops.WebApi.Client.Cache.NoUserContext
{
    public interface IAdminProductFeaturesClientCache
    {
        Task<AdminFeaturesContract> GetAdminFeaturesAsync(int tradeSmithUserId, DateTime lastLoginDate);
    }
}
