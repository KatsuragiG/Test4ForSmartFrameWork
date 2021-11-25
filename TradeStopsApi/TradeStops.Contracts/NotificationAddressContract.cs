using System.Diagnostics.CodeAnalysis;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    // todo: consider to add NotificationAddressesContract { TradeSmithUserId, List<string> Html, List<string> Text } 
    // There are 3 types of Notification addresses:
    // 1. Email for HTML messages (AddressType = email)
    // 2. Email for Text messages (AddressType = other and MobileOperator = 10 Other)
    // 3. Mobile for Text messages (AddressType = other and MobileOperator != 10 Other) 
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class NotificationAddressContract
    {
        /// <summary>
        /// Notification address Id.
        /// </summary>
        public int NotificationAddressId { get; set; }

        /// <summary>
        /// Address type.
        /// </summary>
        public AddressTypes AddressType { get; set; }

        /// <summary>
        /// User address.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Mobile operator values.
        /// </summary>
        public MobileOperatorContract MobileOperator { get; set; }
    }
}
