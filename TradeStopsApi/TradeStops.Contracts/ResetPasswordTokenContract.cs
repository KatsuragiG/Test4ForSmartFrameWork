namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class ResetPasswordTokenContract
    {
        /// <summary>
        /// Security token.
        /// </summary>
        public string Token { get; set; }
    }
}
