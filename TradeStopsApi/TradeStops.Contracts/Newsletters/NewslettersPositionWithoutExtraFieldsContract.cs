using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements must be documented", Justification = "Temporary")]
    public class NewslettersPositionWithoutExtraFieldsContract
    {
        public int? PositionId { get; set; }

        public int PortfolioId { get; set; }
        public PublisherTypes PublisherType { get; set; }

        public SymbolContract Symbol { get; set; }

        public NewsletterPositionTypes PositionType { get; set; }
        public TradeTypes TradeType { get; set; }
        public decimal? Shares { get; set; }
        public decimal? PreviousShares { get; set; }
        public DateTime? RefDate { get; set; }
        public decimal? RefPrice { get; set; }
        public PositionStatusTypes StatusType { get; set; }
        public DateTime? ExitDate { get; set; }
        public decimal? ExitPrice { get; set; }
        public DateTime? ReportDate { get; set; }
        public int CurrencyId { get; set; }
        public DateTime? PublishedDateUtc { get; set; }

        public List<NewslettersPositionWithoutExtraFieldsContract> Subtrades { get; set; }
    }
}
