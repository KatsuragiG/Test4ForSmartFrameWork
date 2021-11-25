using System.Diagnostics.CodeAnalysis;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    // rule for all child classes: fields with the same name must have the same type
    // or types that can be converted using Convert.ChangeType

    // Classes is used in the following scenarios:
    // - input in Services (but TriggerFieldsContract fields must not be used directly, so usually mapping to BaseTriggerContract should be performed in the start of method)
    // - output in Services
    // - input in Controllers - NO, it's just not possible to use base class in controllers, because only base-class fields will be filled (in our case TriggerType) and also because of protected constructor here
    // - output in Controllers
    // + input in Clients - YES, it's allowed to use both base-classes and json-fields-contracts with attribute [UseBaseTypeForInput(typeof(...))]
    // - output in Clients

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Todo: remove suppression")]
    public class BaseTriggerContract
    {
        protected BaseTriggerContract(TriggerTypes triggerType)
        {
            TriggerType = triggerType;
        }

        public TriggerTypes TriggerType { get; protected set; }
    }
}
