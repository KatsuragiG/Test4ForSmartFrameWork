using System;

namespace TradeStops.Common.Enums
{
    // Possible statuses on options parsing during synchronization.
    // To recognize option we need 4 values:
    //   - Underlying Stock (Ticker)
    //   - Strike Price
    //   - Expiration Date
    //   - Option Type
    public enum IncompleteOptionStatusTypes
    {
        [Obsolete("Obsolete value for grid in old admin area.")]
        All = 0,

        // All values received from vendor in separate response fields.   
        AllDataReceived = 1,

        // All values have been parsed from the symbol description.
        DataParsed = 2,

        // We received not enough data to find an option.
        // Some response fields are empty and we were not able to parse data from the symbol description.
        IncompleteData = 3,

        // All values have been parsed from the symbol description, but the option is not found in the database.
        ParsedNotFound = 4,

        // All values received from vendor in separate response fields, but the option is not found in the database.
        ReceivedNotFound = 5
    }
}
