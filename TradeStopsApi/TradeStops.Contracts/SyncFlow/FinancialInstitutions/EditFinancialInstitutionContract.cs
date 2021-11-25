using System;
using TradeStops.Common.Enums;
using TradeStops.Contracts.Types;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Internal contract created before applying SA1600")]
    public class EditFinancialInstitutionContract
    {
        public Optional<string> Name { get; set; }
        public Optional<string> VendorId { get; set; }
        public Optional<bool> Visible { get; set; }
        public Optional<string> LoginUrl { get; set; }
        public Optional<string> Notes { get; set; }
        public Optional<DateTime?> DateUpdated { get; set; }
        public Optional<string> HomePage { get; set; }
        public Optional<FinancialInstitutionMfaTypes?> MfaType { get; set; }
        public Optional<bool> WasMigrated { get; set; }
    }
}
