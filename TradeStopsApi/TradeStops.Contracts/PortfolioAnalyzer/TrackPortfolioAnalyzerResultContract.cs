using System.Diagnostics.CodeAnalysis;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Todo: remove suppression")]
    public class TrackPortfolioAnalyzerResultContract
    {
        public string VendorAccountId { get; set; }

        public VendorTypes? VendorType { get; set; }

        public int? PortfolioId { get; set; }

        public bool Success { get; set; }

        public string Snaid { get; set; }

        public int DigitalConversionId { get; set; }
    }
}
