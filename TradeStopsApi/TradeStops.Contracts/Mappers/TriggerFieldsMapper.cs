using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts.Mappers
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Not a contract")]
    public static class TriggerFieldsMapper
    {
        public static BaseTriggerContract ToBaseTrigger(this TriggerFieldsContract fieldsContract)
        {
            if (fieldsContract == null)
            {
                return null;
            }

            var baseTriggerContract = CreateInstanceByTriggerType(fieldsContract.TriggerType);

            PropertiesMapper.Map(fieldsContract, baseTriggerContract, true, false);

            return baseTriggerContract;
        }

        public static TriggerFieldsContract ToTriggerFields(this BaseTriggerContract baseTriggerContract)
        {
            if (baseTriggerContract == null)
            {
                return null;
            }

            var fieldsContract = new TriggerFieldsContract();

            PropertiesMapper.Map(baseTriggerContract, fieldsContract, false, false);

            return fieldsContract;
        }

        private static BaseTriggerContract CreateInstanceByTriggerType(TriggerTypes triggerType)
        {
            switch (triggerType)
            {
                case TriggerTypes.PercentOfAverageVolume: return new PercentOfAverageVolumeTriggerContract();
                case TriggerTypes.MovingAveragePrice: return new MovingAveragePriceTriggerContract();
                case TriggerTypes.MovingAverageCrosses: return new MovingAverageCrossesTriggerContract();
                case TriggerTypes.CalendarDays: return new CalendarDaysTriggerContract();
                case TriggerTypes.TradingDays: return new TradingDaysTriggerContract();
                case TriggerTypes.SpecificDate: return new SpecificDateTriggerContract();
                case TriggerTypes.ProfitableCloses: return new ProfitableClosesTriggerContract();
                case TriggerTypes.PercentageGainLoss: return new PercentageGainLossTriggerContract();
                case TriggerTypes.DollarGainLoss: return new DollarGainLossTriggerContract();
                case TriggerTypes.FixedPrice: return new FixedPriceTriggerContract();
                case TriggerTypes.NakedPut: return new NakedPutTriggerContract();
                case TriggerTypes.CoveredCall: return new CoveredCallTriggerContract();
                case TriggerTypes.UnStockFixedPrice: return new UnStockFixedPriceTriggerContract();
                case TriggerTypes.PercentageTimeValue: return new PercentageTimeValueTriggerContract();
                case TriggerTypes.DollarTimeValue: return new DollarTimeValueTriggerContract();
                case TriggerTypes.Expiry: return new ExpiryTriggerContract();
                case TriggerTypes.Breakout: return new BreakoutTriggerContract();
                case TriggerTypes.Target: return new TargetTriggerContract();
                case TriggerTypes.UnStockTrailingStopPercent: return new UnStockTrailingStopPercentTriggerContract();
                case TriggerTypes.UnStockVolatilityQuotinent: return new UnStockVolatilityQuotinentTriggerContract();
                case TriggerTypes.TrailingStopPercent: return new TrailingStopPercentTriggerContract();
                case TriggerTypes.VolatilityQuotinent: return new VolatilityQuotinentTriggerContract();
                case TriggerTypes.StockStateIndicator: return new StockStateIndicatorTriggerContract();
                case TriggerTypes.TwoVolatilityQuotient: return new TwoVolatilityQuotientTriggerContract();
                case TriggerTypes.TrailingStopMinusDividend: return new TrailingStopMinusDividendContract();
                case TriggerTypes.EntrySignal: return new EntrySignalTriggerContract();
                case TriggerTypes.EarlyEntrySignal: return new EarlyEntrySignalTriggerContract();
                case TriggerTypes.NewHighProfit: return new NewHighProfitTriggerContract();
                case TriggerTypes.StockRating: return new StockRatingTriggerContract();

                default: throw new Exception("unknown trigger configuration");
            }
        }
    }
}
