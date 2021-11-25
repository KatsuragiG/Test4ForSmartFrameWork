using System.Diagnostics.CodeAnalysis;
using TradeStops.Common.Constants;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts.Extensions
{
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Todo: remove suppression")]
    public static class UserSettingsContractExtensions
    {
        public static ExchangesFlags GetExchangesFlagsBySubscription(this UserSettingsContract userSettings, bool hasStockExchanges, bool hasCryptoExchange)
        {
            var result = (ExchangesFlags)userSettings.ExchangesFlags;

            if (!hasStockExchanges)
            {
                // User is not subscribed to any stocks
                result = DefaultUserSettings.DefaultVisibleCryptoStopsExchanges;
            }

            if (!hasCryptoExchange)
            {
                // User is not subscrined to any cryptos
                result = result & ~ExchangesFlags.CryptoCurrencies;
            }

            return result;
        }
    }
}
