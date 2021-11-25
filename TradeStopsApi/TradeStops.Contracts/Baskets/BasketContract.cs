using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Basket parameters
    /// </summary>
    public class BasketContract
    {
        /// <summary>
        /// Basket ID.
        /// </summary>
        public int BasketId { get; set; }

        /// <summary>
        /// User ID.
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// Basket name.
        /// </summary>
        public string BasketName { get; set; }

        /// <summary>
        /// Type of basket. Null for custom user's basket.
        /// </summary>
        public BasketTypes? BasketType { get; set; }

        /// <summary>
        /// Defines if the basket is system.
        /// </summary>
        public bool IsSystem { get; set; }

        /// <summary>
        /// Defined if the basket can be deleted by the user.
        /// </summary>
        public bool IsDeletable { get; set; }
    }
}
