namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to create token to reset password
    /// </summary>
    public class CreateResetPasswordTokenContract
    {
        /// <summary>
        /// E-mail address assigned to the user account
        /// </summary>
        public string Email { get; set; }
    }
}
