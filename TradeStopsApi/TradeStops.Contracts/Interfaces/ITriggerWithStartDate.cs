using System;
using System.Diagnostics.CodeAnalysis;

namespace TradeStops.Contracts.Interfaces
{
    // its okay to use interface if its simplifies code even in contracts, its not okay to use interfaces when you dont need them
    // some common sense rules here: https://lostechies.com/jamesgregory/2009/05/09/entity-interface-anti-pattern/
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Todo: remove suppression")]
    public interface ITriggerWithStartDate
    {
        DateTime? StartDate { get; set; }
    }
}