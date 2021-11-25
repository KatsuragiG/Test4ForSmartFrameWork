using TradeStops.Contracts.Types;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Basket fields to edit. Initialize only fields you want to patch
    /// </summary>
    public class EditBasketContract
    {
        /// <summary>
        /// Basket name.
        /// </summary>
        public Optional<string> BasketName { get; set; }
    }
}
