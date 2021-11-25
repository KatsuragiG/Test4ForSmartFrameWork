using System;

namespace TradeStops.Contracts
{
    ////[Obsolete("We have corresponding TradeStops.Common.TriggerTypes enum to replace this class")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Internal contract created before applying SA1600")]
    public class TriggerTypeContract
    {
        public int TriggerTypeId { get; set; }

        public string ClassName { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }
    }
}
