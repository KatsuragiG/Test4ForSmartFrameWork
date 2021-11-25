using System;
using System.Diagnostics.CodeAnalysis;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class SystemEventContract
    {
        public int SystemEventId { get; set; }

        public DateTime EventDate { get; set; }
        public SystemEventCategories SystemEventCategoryId { get; set; }

        /// <summary>
        /// The main description of the event, like 'Alert Triggered' or similar
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The additional description of the event
        /// </summary>
        public string AdditionalDescription { get; set; }

        /// <summary>
        /// Additional information about event that we usually display on mouse hover on UI
        /// </summary>
        public string DescriptionHover { get; set; }

        public int? PortfolioId { get; set; }
        public int? PositionId { get; set; }
        public PortfolioContract Portfolio { get; set; }
        public PositionContract Position { get; set; }

        public string ItemName { get; set; }
        public short? ItemType { get; set; }
        public string Currency { get; set; }
        public bool? IsTriggered { get; set; }
        public decimal? ThresholdValue { get; set; }
        public decimal? CurrentValue { get; set; }
        public DateTime? ExtremumDate { get; set; }
        public decimal? ExtremumPrice { get; set; }
        public PriceTypes? PriceType { get; set; }
        public TradeTypes? TradeType { get; set; }
        public TriggerOperationTypes? OperationType { get; set; }
        public PeriodTypes? PeriodType { get; set; }
        public int? Period { get; set; }
        public decimal? LatestPrice { get; set; }
        public decimal? StopPrice { get; set; }
        public DateTime? Date1 { get; set; }
        public string Text1 { get; set; }
        public string Text2 { get; set; }
        public decimal? Decimal1 { get; set; }
        public decimal? Decimal2 { get; set; }
        public byte? TinyInt1 { get; set; }
        public int? Int1 { get; set; }
        public bool? Bool1 { get; set; }
        public bool? UseIntraday { get; set; }
    }
}
