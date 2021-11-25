using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to send Reset Password email
    /// </summary>
    public class SendResetPasswordEmailContract
    {
        /// <summary>
        /// User's email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The product the user tried to login. This parameter is used to generate return links from email.
        /// </summary>
        public Products ProductId { get; set; }
    }
}
