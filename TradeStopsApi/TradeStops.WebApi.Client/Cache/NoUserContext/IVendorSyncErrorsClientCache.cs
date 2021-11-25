using System.Collections.Generic;
using System.Threading.Tasks;
using TradeStops.Contracts;

namespace TradeStops.WebApi.ClientCache.NoUserContext
{
    public interface IVendorSyncErrorsClientCache
    {
        Task<List<VendorSyncErrorMessageContract>> GetAllAsync();

        List<VendorSyncErrorMessageContract> GetAll();
    }
}
