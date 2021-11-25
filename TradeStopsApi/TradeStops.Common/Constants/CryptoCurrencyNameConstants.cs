using System.Collections.Generic;

namespace TradeStops.Common.Constants
{
    public static class CryptoCurrencyNameConstants
    {
        public const string Bitcoin = "BTC";
        public const string Ethereum = "ETH";
        public const string Litecoin = "LTC";

        public static HashSet<string> All => new HashSet<string>
        {
            Bitcoin,
            Ethereum,
            Litecoin
        };
    }
}
