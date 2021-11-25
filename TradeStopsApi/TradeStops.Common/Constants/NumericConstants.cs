namespace TradeStops.Common.Constants
{
    public static class NumericConstants
    {
        public const int DefaultCurrencyId = 1; // USD currency id
        public const int DefaultCountryId = 223; // USA

        // These constants are the same as in the
        // TradeSmith.Research.Algorithms.Configuration.DefaultVqValues.
        // Please, keep them synced.
        public const decimal DefaultVqValue = 25;
        public const decimal MinVqValue = 5;
        public const decimal MaxVqValue = 95;
        public const int VqMinNumberOfPoints = 250;
        public const int VqOptimalNumberOfPoints = 750;

        public const int MaxPercentageValue = 100;
        
        public const int OptionDefaultContractSize = 100;
        public const int StockDefaultSharesMultiplier = 1;

        public const decimal DefaultPrecision = 0.00001m;
        
        public const int DefaultAutocompleteResultsCount = 20;

        public const int MinimalCredentialsLength = 3;

        public const int DaysInYear = 365;
        public const int DaysInWeek = 7;
        public const int WorkingDaysInWeek = 5;

        public const int SsiTrendNumberOfOmaPoints = 40;
        public const double SsiTrendPercentForRocLength = 0.2;
        public const int SsiTrendNumberTradeDaysPerYear = 252;
        public const int SsiTrendNumberOfYears = 3;

        public const int DoubleVqThreshold = 2;

        public const int PricePrecision = 8;
    }
}
