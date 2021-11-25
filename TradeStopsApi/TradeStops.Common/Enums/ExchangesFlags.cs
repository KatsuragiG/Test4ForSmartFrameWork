using System;

namespace TradeStops.Common.Enums
{
    [Flags]
    public enum ExchangesFlags : byte
    {
        None = 0,

        USA = 1,

        UK = 1 << 1, // 2

        Canada = 1 << 2, // 4

        Germany = 1 << 3, // 8

        Australia = 1 << 4, // 16

        CryptoCurrencies = 1 << 5, // 32

        Other = 1 << 7, // 128

        StockExchanges = USA | UK | Canada | Germany | Australia | Other,

        All = 255
    }
}