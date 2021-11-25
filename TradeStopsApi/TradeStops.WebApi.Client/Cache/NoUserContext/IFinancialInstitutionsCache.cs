using System.Collections.Generic;
using System.Threading.Tasks;
using TradeStops.Contracts;

namespace TradeStops.WebApi.ClientCache.NoUserContext
{
    public interface IFinancialInstitutionsClientCache
    {
        List<AdminFinancialInstitutionContract> GetAllFinancialInstitutionsWithBetaTesters();

        Task<List<AdminFinancialInstitutionContract>> GetAllFinancialInstitutionsWithBetaTestersAsync();
    }
}
