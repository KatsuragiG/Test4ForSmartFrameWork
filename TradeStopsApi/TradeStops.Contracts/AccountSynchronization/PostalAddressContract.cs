namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Internal contract created before applying SA1600")]
    public class PostalAddressContract
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Street { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string CountryName { get; set; } // for stansberry "country" field may contain anything, there's no strict format
        public string CountryIso3Code { get; set; }
        public string ZipCode { get; set; }
        public string State { get; set; }
        public string Email { get; set; }
    }
}