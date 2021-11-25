namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class SystemEventCategoryContract
    {
        public int SystemEventCategoryId { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }
    }
}
