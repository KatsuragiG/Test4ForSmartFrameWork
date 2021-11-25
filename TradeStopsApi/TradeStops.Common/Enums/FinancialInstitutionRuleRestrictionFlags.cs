using System;

namespace TradeStops.Common.Enums
{
    [Flags]
    public enum FinancialInstitutionRuleRestrictionFlags
    {
        None = 0,

        Import = 1 << 0,

        Refresh = 1 << 1,

        Restore = 1 << 2,

        UpdateCredentials = 1 << 3
    }
}
