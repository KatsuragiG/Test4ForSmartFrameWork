using System.Diagnostics.CodeAnalysis;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts.Extensions
{
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Todo: remove suppression")]
    public static class PositionContractExtensions
    {
        public static bool IsStock(this PositionContract position)
        {
            return position.PositionType == PositionTypes.Regular && position.Symbol.Option == null;
        }

        public static bool IsOption(this PositionContract position)
        {
            return position.PositionType == PositionTypes.Regular && position.Symbol.Option != null;
        }

        ////public static bool IsPairTrade(this PositionContract position)
        ////{
        ////    return position.PositionType == PositionTypes.PairsTrade;
        ////}

        public static string GetSymbol(this PositionContract position)
        {
            if (position.PositionType == PositionTypes.Regular)
            {
                return position.Symbol.Stock.Symbol;
            }

            return position.PairTradeSymbol.Ticker;
        }

        public static string GetSymbolName(this PositionContract position)
        {
            if (position.PositionType == PositionTypes.Regular)
            {
                return position.Symbol.Stock.Name;
            }

            return position.PairTradeSymbol.Name;
        }

        public static string GetExchangeName(this PositionContract position)
        {
            if (position.PositionType == PositionTypes.Regular)
            {
                return position.Symbol.Stock.ExchangeName;
            }

            return position.PairTradeSymbol.ExchangeName;
        }

        public static bool IsImported(this PositionContract position)
        {
            return position.ImportedPosition != null;
        }
    }
}
