using System.Collections.Generic;

namespace TradeStops.Common.Constants
{
    public static class SyncSymbolsConstants
    {
        //todo - Readonly members is not available in C# 7.3
        public static Dictionary<string, List<string>> ForeignSymbolPostfixes => new Dictionary<string, List<string>>
        {
            { CurrencyNames.Cad, new List<string> { "-T", "-V", "-CN" } },
            { CurrencyNames.Gbp, new List<string> { "-L" } },
            { CurrencyNames.Aud, new List<string> { ".AX" } },
            { CurrencyNames.Eur, new List<string> { ".F", ".SG", "-PA" } },
        };
    }
}
