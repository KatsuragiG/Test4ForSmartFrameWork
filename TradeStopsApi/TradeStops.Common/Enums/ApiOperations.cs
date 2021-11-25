namespace TradeStops.Common.Enums
{
    public enum ApiOperations
    {
        /// <summary>
        /// Additional required operation for methods with access to Current User data (these methods require UserContext because they use Current[TradeSmith]UserId in the code)
        /// Such methods are usually used in Website-projects
        /// </summary>
        CurrentUser = 1,

        /// <summary>
        /// Additional required operation for methods with access to Another User data (usually [TradeSmith]UserId is passed as input to such methods)
        /// Such methods are usually available only for internal projects
        /// </summary>
        Admin = 2,

        /// <summary>
        /// Additional required operation for methods that are obsolete or too specific and not recommended to be used for third-party
        /// Such methods are used only for internal projects
        /// Lazy developers can use this operation type for Controllers without specific restrictions on operations availability,
        /// but ideally you have to create separate operation type instead of using this one
        /// </summary>
        Internal = 3,

        /// <summary>
        /// Additional required operation for methods that modify any data
        /// Used in HttpPut, HttpDelete and some HttpPost methods
        /// If the HttpPost method does not modify data, such as methods with the prefixes "Get", "Search", "AdminGet", "AdminSearch", "Find", this attribute should not be
        /// Shouldn't be used for HttpGet methods
        /// </summary>
        ModifyData = 4,

        // Modify = 203,  // additional required operation for data-modification
        // AvailableForAuthenticated = 200, // Operation type to use for methods that are available for all authenticated users (like healthcheck)

        // historical data and common static database stuff
        Symbols = 101,
        Currencies = 102,
        Prices = 103,
        CorporateActions = 104,
        Countries = 105,
        MobileOperators = 106,
        IntradayPrices = 107,
        Exchanges = 108,
        IntradayOptionsData = 109,

        // users and subscription-related stuff
        NotificationAddresses = 201,
        Subscriptions = 202,
        Users = 203,
        ResetPasswords = 204,
        TradeSmithUsers = 205,
        SynchronizeAccounts = 206,
        TradeSmithProducts = 207,
        UserContextsUntrusted = 208,
        RefreshUserContexts = 209,
        Login = 210,

        // portfolio management (TradeStops)
        Tags = 301,
        Portfolios = 302,
        Positions = 303,
        Templates = 304,
        PositionTriggers = 305,
        Views = 306,
        PairTrades = 307,

        // portfolio synchronization
        VendorSyncErrors = 401,
        FinancialInstitutions = 402,
        BrokerPills = 403,
        YodleeUserContexts = 404,
        UnconfirmedPositions = 405,
        SynchronizedPortfolios = 406,
        ImportProcesses = 407,
        DeletedSyncPositions = 408,
        IncompleteOptions = 409,
        TradeIt = 410,
        SyncReports = 411,
        SyncFlowAuthentication = 412,
        VendorUsernames = 413,

        // research values, tools and additional analytic stuff
        Vq = 501,
        Ssi = 502,
        RiskRebalancer = 503,
        VqAnalyzer = 504,
        AssetAllocation = 505,
        PositionSize = 506,
        MagicCalculator = 507, // todo: delete
        StopLossAnalyzer = 508, // todo: delete
        QuantTool = 509,
        DiversificationRatio = 510,
        LikeFolio = 511,
        Cycles = 512, // todo: delete
        TradeIdeas = 513,
        PortfolioAnalyzer = 514,
        Newsletters = 515,
        Roc = 516,
        BullBear = 517,
        InstrumentRatings = 518,
        Timings = 519,
        Backtester = 520,
        PlatformTasks = 521,
        SmithRank = 522,
        NotificationEvents = 523,
        GlobalRank = 524,
        FunFacts = 525,
        Baskets = 526,
        GlobalRankAllocation = 527,
        OptionStatistics = 528,

        // publishers
        Publishers = 600,
        PortfolioTracker = 601,

        // todo: put ZendeskApps, StansberryPushNotifications, AgoraPushNotifications into separate Third-party group,
        // because we don't need these operations anywhere.
        // other
        ZendeskApps = 901,
        SystemEvents = 902,
        Charts = 903,
        PerformanceLine = 904,
        BehavioralData = 905,
        News = 910,
        SystemSettings = 911,
        StansberryPushNotifications = 912,
        MobileDevices = 913,
        AgoraPushNotifications = 914,
        EmailMessages = 915,
        Publications = 916,
        PortfolioLite = 917,
        PortfolioRules = 918,
        DraftManagement = 919,
        RightWayCharts = 920
    }
}
