using System;

using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    // todo: consider changing this magical approach to use class BaseTriggerJson with N fields for each trigger type, so only one of them will be not-null.
    // todo: consider renaming to BaseTriggerContractJsonFields OR use BaseTriggerServerContract and BaseTriggerClientContract

    // Class is created from properties of all BaseTriggerContract descendants.
    // You can use SuperClassGenerator.Generate to generate class like this
    // This class is used only for JSON serialization/deserialization and must not be used in other scenarios
    // Alternative to this class is Dictionary<string FieldName, string FieldValue>
    // All properties were marked as nullable because Jil throws exception when trying to serialize Enums with 0 value
    // (because it serializes it into 'string' instead of 'int', like NewtonsoftJson do)
    // initially StartDate was the only nullable field, because TrailingStop allows us to put start date for position (on position card, but not on templates page)

    // class can used in the following scenarios:
    // + input in Services (but fields of this contract must not be used directly)
    // + output in Services
    // + input in Controllers - YES, because it's just not possible to use base class in controllers as input
    // + output in Controllers - YES, because we have api clients dll (for consistency)
    // + input in Clients - it's allowed to use both base-classes and json-fields-contracts with attribute [UseBaseTypeForInput(typeof(...))]
    // + output in Clients - YES, because when you de-serialize response from api, you can't de-serialize it easily into BaseTriggerContract

    /// <summary>
    /// All fields that are necessary to create trigger.
    /// Contract is created (generated) from properties of all BaseTriggerContract descendants.
    /// It is used to bypass limitation that we can't accept base-class as Json input.
    /// It is used only for JSON serialization/deserialization and must not be used in other scenarios.
    /// </summary>
    public class TriggerFieldsContract
    {
        /// <summary>
        /// Type of Position Trigger
        /// </summary>
        public TriggerTypes TriggerType { get; set; } // this is the only property of contract that allowed to be used in other than JSON-serialization scenarios

        /// <summary>
        /// Position Trigger price type.
        /// </summary>
        public PriceTypes? PriceType { get; set; }

        /// <summary>
        /// Numeric value of a specific period of time.
        /// </summary>
        public int? Period { get; set; }

        /// <summary>
        /// Type of the trigger Period .
        /// </summary>
        public PeriodTypes? PeriodType { get; set; }

        /// <summary>
        /// Type of the operation.
        /// </summary>
        public TriggerOperationTypes? OperationType { get; set; }

        /// <summary>
        /// Specific Position Trigger numeric value.
        /// </summary>
        public decimal?     /* int */ ThresholdValue { get; set; }

        /// <summary>
        /// Price value of the Underlying Stock.
        /// </summary>
        public decimal? StockPurchasePrice { get; set; }

        /// <summary>
        /// Numeric value of a specific period of time.
        /// </summary>
        public int? Period2 { get; set; }

        /// <summary>
        /// Type of the trigger period.
        /// </summary>
        public PeriodTypes? PeriodType2 { get; set; }

        /// <summary>
        /// Specific Position Trigger date value.
        /// </summary>
        public DateTime? ThresholdDate { get; set; }

        /// <summary>
        /// Trigger alert for the Entry Signal state.
        /// </summary>
        public bool? TrackEntrySignal { get; set; }

        /// <summary>
        /// Trigger alert for the Up Trend state.
        /// </summary>
        public bool? TrackUpTrend { get; set; }

        /// <summary>
        /// Trigger alert for the Side Trend state.
        /// </summary>
        public bool? TrackSideTrend { get; set; }

        /// <summary>
        /// Trigger alert for the Down Trend state.
        /// </summary>
        public bool? TrackDownTrend { get; set; }

        /// <summary>
        /// Trigger alert for the Stopped Out state.
        /// </summary>
        public bool? TrackStoppedOut { get; set; }

        /// <summary>
        /// Type of the  fundamental data.
        /// </summary>
        public TargetColumnNames? TargetColumnName { get; set; }

        /// <summary>
        /// Date to start the price analysis for the Position Trigger.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Position trade type.
        /// </summary>
        public TradeTypes? TradeType { get; set; }

        /// <summary>
        /// Determines if intraday prices has to be used.
        /// </summary>
        public bool? UseIntraday { get; set; }
    }
}
