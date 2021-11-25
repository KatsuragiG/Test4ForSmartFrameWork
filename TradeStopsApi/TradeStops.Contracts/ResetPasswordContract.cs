namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class ResetPasswordContract
    {
        //Token values.
        public ResetPasswordTokenContract Token { get; set; }

        /// <summary>
        /// New password.
        /// </summary>
        public string NewPassword { get; set; }

        /// <summary>
        /// Repeated new password.
        /// </summary>
        public string ConfirmPassword { get; set; }
    }
}
