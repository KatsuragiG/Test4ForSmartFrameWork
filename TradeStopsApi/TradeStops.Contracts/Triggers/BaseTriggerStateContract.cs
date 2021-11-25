using System.Diagnostics.CodeAnalysis;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    // rule for all child classes: fields with the same name must have the same type
    // or types that can be converted using Convert.ChangeType
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Todo: remove suppression")]
    public class BaseTriggerStateContract
    {
        protected BaseTriggerStateContract(TriggerTypes triggerType)
        {
            TriggerType = triggerType;
        }

        public TriggerTypes TriggerType { get; protected set; }
    }
}
