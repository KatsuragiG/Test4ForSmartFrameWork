using System.Collections.Generic;

namespace TradeStops.Common.Constants
{
    public static class CurrencyIdsConstants
    {
        public const int Usd = 1;
        public const int Cad = 2;
        public const int Gbp = 3;
        public const int Eur = 4;
        public const int Jpy = 5;
        public const int Aud = 6;
        public const int Hkd = 12;
        public const int Gbx = 7;

        public const int Btc = 77;
        public const int Eth = 78;
        public const int Ltc = 79;

        public static List<int> TradeStopsPortfolioCurrencyIds => new List<int> { Usd, Cad, Gbp, Eur, Aud };

        public static List<int> CryptoStopsCurrencyIds => new List<int> { Usd, Cad, Gbp, Eur, Aud, Btc, Eth, Ltc };

        public static List<int> TradeStopsNotAvailableCurrencyIds => new List<int> { Gbx };
    }
}
