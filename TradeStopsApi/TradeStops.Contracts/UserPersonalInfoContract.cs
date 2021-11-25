namespace TradeStops.Contracts
{
    /// <summary>
    /// User personal information
    /// </summary>
    public class UserPersonalInfoContract
    {
        /// <summary>
        /// User last name
        /// </summary>
        public string FirstName { get; set; } // not null!

        /// <summary>
        /// User last name.
        /// </summary>
        public string LastName { get; set; } // not null!

        /// <summary>
        /// User address.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// User additional address.
        /// </summary>
        public string Address2 { get; set; }

        /// <summary>
        /// User city name.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// User state name.
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// User Zip code.
        /// </summary>
        public string ZipCode { get; set; }

        /// <summary>
        /// User phone number.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// User country Id.
        /// </summary>
        public int CountryId { get; set; }
    }
}
