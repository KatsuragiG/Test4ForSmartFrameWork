using System.Collections.Generic;
using System.Threading.Tasks;
using TradeStops.Contracts;

namespace TradeStops.WebApi.ClientCache.NoUserContext
{
    public interface ICurrenciesClientCache
    {
        List<CurrencyContract> GetAll();

        Task<List<CurrencyContract>> GetAllAsync();

        Dictionary<int, Dictionary<int, decimal>> GetAllCrossCoursesNotNull();

        Task<Dictionary<int, Dictionary<int, decimal>>> GetAllCrossCoursesNotNullAsync();
    }
}
