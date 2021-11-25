using System;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Internal contract created before applying SA1600")]
    public class UpdatePostalAddressResultContract
    {
        public UpdatePostalAddressResultContract()
        {
        }

        public UpdatePostalAddressResultContract(Exception ex)
        {
            Success = false;
            ErrorMessage = ex.Message;
        }

        public bool Success { get; set; }

        public string ErrorMessage { get; set; }
    }
}
