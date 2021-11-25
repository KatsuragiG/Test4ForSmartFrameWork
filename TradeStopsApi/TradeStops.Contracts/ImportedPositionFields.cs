using System;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class ImportedPositionFields
    {
        public bool? ChangingSharesPermission { get; set; }

        public string VendorHoldingId { get; set; }

        public string VendorSymbol { get; set; }

        public DateTime? PossibleCloseDate { get; set; }

        public bool? PossiblyClosedNotificationSent { get; set; }

        public decimal? SharesMultiplier { get; set; }
    }
}
