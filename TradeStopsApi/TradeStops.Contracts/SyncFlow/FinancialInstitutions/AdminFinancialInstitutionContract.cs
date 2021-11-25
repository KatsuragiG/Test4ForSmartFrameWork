using System;
using System.Collections.Generic;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Internal contract created before applying SA1600")]
    public class AdminFinancialInstitutionContract
    {
        public int FinancialInstitutionId { get; set; }

        public string Name { get; set; }

        public string VendorId { get; set; }
        
        public bool Visible { get; set; }

        public string LoginUrl { get; set; }

        public string Notes { get; set; }

        public DateTime? DateUpdated { get; set; }

        public string HomePage { get; set; }

        public FinancialInstitutionMfaTypes? MfaType { get; set; }

        public VendorTypes VendorType { get; set; }

        public List<FinancialInstitutionBetaTesterContract> BetaTesters { get; set; }
    }
}
