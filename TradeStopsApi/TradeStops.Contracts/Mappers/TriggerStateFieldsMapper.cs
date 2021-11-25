using System;

using TradeStops.Common.Enums;

namespace TradeStops.Contracts.Mappers
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Not a contract")]
    public static class TriggerStateFieldsMapper
    {
        public static BaseTriggerStateContract ToBaseTriggerState(this TriggerStateFieldsContract fieldsContract)
        {
            if (fieldsContract == null)
            {
                return null;
            }

            var baseTriggerContract = CreateInstanceByTriggerType(fieldsContract.TriggerType);

            PropertiesMapper.Map(fieldsContract, baseTriggerContract, true, false);

            return baseTriggerContract;
        }

        public static TriggerStateFieldsContract ToTriggerStateFields(this BaseTriggerStateContract baseTriggerStateContract)
        {
            if (baseTriggerStateContract == null)
            {
                return null;
            }

            var fieldsContract = new TriggerStateFieldsContract();

            PropertiesMapper.Map(baseTriggerStateContract, fieldsContract, false, false);

            return fieldsContract;
        }

        public static BaseTriggerStateContract CreateInstanceByTriggerType(TriggerTypes triggerType)
        {
            switch (triggerType)
            {
                case TriggerTypes.PercentOfAverageVolume: return new PercentOfAverageVolumeTriggerStateContract();
                case TriggerTypes.MovingAveragePrice: return new MovingAveragePriceTriggerStateContract();
                case TriggerTypes.MovingAverageCrosses: return new MovingAverageCrossesTriggerStateContract();
                case TriggerTypes.CalendarDays: return new CalendarDaysTriggerStateContract();
                case TriggerTypes.TradingDays: return new TradingDaysTriggerStateContract();
                case TriggerTypes.SpecificDate: return new SpecificDateTriggerStateContract();
                case TriggerTypes.ProfitableCloses: return new ProfitableClosesTriggerStateContract();
                case TriggerTypes.PercentageGainLoss: return new PercentageGainLossTriggerStateContract();
                case TriggerTypes.DollarGainLoss: return new DollarGainLossTriggerStateContract();
                case TriggerTypes.FixedPrice: return new FixedPriceTriggerStateContract();
                case TriggerTypes.NakedPut: return new NakedPutTriggerStateContract();
                case TriggerTypes.CoveredCall: return new CoveredCallTriggerStateContract();
                case TriggerTypes.UnStockFixedPrice: return new UnStockFixedPriceTriggerStateContract();
                case TriggerTypes.PercentageTimeValue: return new PercentageTimeValueTriggerStateContract();
                case TriggerTypes.DollarTimeValue: return new DollarTimeValueTriggerStateContract();
                case TriggerTypes.Expiry: return new ExpiryTriggerStateContract();
                case TriggerTypes.Breakout: return new BreakoutTriggerStateContract();
                case TriggerTypes.Target: return new TargetTriggerStateContract();
                case TriggerTypes.UnStockTrailingStopPercent: return new UnStockTrailingStopPercentTriggerStateContract();
                case TriggerTypes.UnStockVolatilityQuotinent: return new UnStockVolatilityQuotinentTriggerStateContract();
                case TriggerTypes.TrailingStopPercent: return new TrailingStopPercentTriggerStateContract();
                case TriggerTypes.VolatilityQuotinent: return new VolatilityQuotinentTriggerStateContract();
                case TriggerTypes.StockStateIndicator: return new StockStateIndicatorTriggerStateContract();
                case TriggerTypes.TwoVolatilityQuotient: return new TwoVolatilityQuotientTriggerStateContract();
                case TriggerTypes.EntrySignal: return new EntrySignalTriggerStateContract();
                case TriggerTypes.EarlyEntrySignal: return new EarlyEntrySignalTriggerStateContract();
                case TriggerTypes.NewHighProfit: return new NewHighProfitTriggerStateContract();
                case TriggerTypes.StockRating: return new StockRatingTriggerStateContract();

                default: throw new Exception($"Unknown trigger type: {triggerType}");
            }
        }
    }
}
