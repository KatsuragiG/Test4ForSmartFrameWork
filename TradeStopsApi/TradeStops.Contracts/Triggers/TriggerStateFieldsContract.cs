using System;

using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    // todo: consider renaming to BaseTriggerStateContractJsonFields
    // Class is created from properties of all BaseTriggerStateContract descendants.
    // You can use SuperClassGenerator.Generate to generate class like this
    // This class is used only for JSON serialization/deserialization and must not be used in other scenarios
    // Alternative to this class is Dictionary<string FieldName, string FieldValue>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Todo: remove suppression")]
    public class TriggerStateFieldsContract
    {
        /// <summary>
        /// Type of Position Trigger.
        /// </summary>
        public TriggerTypes TriggerType { get; set; } // this is the only property of contract that allowed to be used in other than JSON-serialization scenarios

        /// <summary>
        /// Numeric value of a specific period of time.
        /// </summary>
        public int? Period { get; set; }

        /// <summary>
        /// Type of the trigger period.
        /// </summary>
        public PeriodTypes? PeriodType { get; set; }

        /// <summary>
        /// Numeric value of a specific period of time.
        /// </summary>
        public int? Period2 { get; set; }

        /// <summary>
        /// Type of the trigger period.
        /// </summary>
        public PeriodTypes? PeriodType2 { get; set; }

        /// <summary>
        /// Position Trigger price type.
        /// </summary>
        public PriceTypes? PriceType { get; set; }

        /// <summary>
        /// Average price value. Available for Moving Average Triggers.
        /// </summary>
        public decimal? AveragePrice { get; set; }

        /// <summary>
        /// Average price value. Available for Moving Average Triggers.
        /// </summary>
        public decimal? AveragePrice2 { get; set; }

        /// <summary>
        /// Current Position Trigger value, depend on a trigger type.
        /// </summary>
        public decimal? CurrentValue { get; set; }

        /// <summary>
        /// Current symbol SSI Status.
        /// </summary>
        public SsiStatuses? CurrentSsiStatus { get; set; }

        /// <summary>
        /// Currency symbol.
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Latest price of the stock.
        /// </summary>
        public decimal? LatestPrice { get; set; }

        /// <summary>
        /// Type of the operation.
        /// </summary>
        public TriggerOperationTypes? OperationType { get; set; }

        /// <summary>
        /// Position cost basis value.
        /// </summary>
        public decimal? CostBasisValue { get; set; }

        /// <summary>
        /// Highest or lowest price of the stock depends on position trade type (Long or Short).
        /// </summary>
        public decimal? ExtremumPrice { get; set; }

        /// <summary>
        /// Position purchase price not adjusted by corporate actions.
        /// </summary>
        public decimal? PurchasePrice { get; set; }

        /// <summary>
        /// Specific Position Trigger numeric value, depends on a trigger type.
        /// </summary>
        public decimal? ThresholdValue { get; set; }

        /// <summary>
        /// Type of the  fundamental data.
        /// </summary>
        public TargetColumnNames? TargetColumnName { get; set; }

        /// <summary>
        /// Latest time value available for Time Value Position Triggers.
        /// </summary>
        public decimal? LatestTimeValue { get; set; }

        /// <summary>
        /// Initial time value available for Time Value Position Triggers.
        /// </summary>
        public decimal? InitialTimeValue { get; set; }

        /// <summary>
        /// Date when the highest or lowest stock price was reached.
        /// </summary>
        public DateTime? ExtremumDate { get; set; }

        /// <summary>
        /// Position trade type.
        /// </summary>
        public TradeTypes? TradeType { get; set; }

        /// <summary>
        /// Price when the Position Trigger will be triggered if Stop Price will be reached.
        /// </summary>
        public decimal? StopPrice { get; set; }

        /// <summary>
        /// Determines if intraday prices has to be used.
        /// </summary>
        public bool? UseIntraday { get; set; }

        /// <summary>
        ///  Current global rank value
        /// </summary>
        public GlobalRankTypes? CurrentGlobalRank { get; set; }

        /// <summary>
        ///  Previous global rank value
        /// </summary>
        public GlobalRankTypes? PreviousGlobalRank { get; set; }
    }
}
