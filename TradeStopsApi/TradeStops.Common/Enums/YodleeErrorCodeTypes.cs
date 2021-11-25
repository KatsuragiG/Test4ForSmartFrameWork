namespace TradeStops.Common.Enums
{
    public enum YodleeErrorCodeTypes
    {
        Unknown = -1,
        Success = 0,

        NoConnection = 401,
        StatusLoginFailed = 402,
        InternalError = 403,
        LostRequest = 404,
        StatusUserAbortedRequest = 405,
        StatusPasswordExpired = 406,
        StatusAccountLocked = 407,
        DataExpected = 408,
        StatusSiteUnavailable = 409,
        Pop3ServerFailed = 410,
        StatusSiteOutOfBusiness = 411,
        StatusSiteApplicationError = 412,
        RequiredFieldUnavailable = 413,
        StatusNoAcctFound = 414,
        StatusSiteTerminatedSession = 415,
        StatusSiteSessionAlreadyEstablished = 416,
        StatusDataModelNoSupport = 417,
        StatusHttpDnsErrorException = 418,
        LoginNotCompleted = 419,
        StatusSiteMergedError = 420,
        StatusUnsupportedLanguageError = 421,
        StatusAccountCanceled = 422,
        StatusAcctInfoUnavailable = 423,
        StatusSiteDownForMaintenance = 424,
        StatusCertificateError = 425,
        StatusSiteBlocking = 426,
        StatusSplashPageException = 427,
        StatusTermsAndConditionsException = 428,
        StatusUpdateInformationException = 429,
        StatusSiteNotSupported = 430,
        HttpFileNotFoundError = 431,
        HttpInternalServerError = 432,
        StatusRegistrationPartialSuccess = 433,
        StatusRegistrationFailedError = 434,
        StatusRegistrationInvalidData = 435,
        RegistrationAccountAlreadyRegistered = 436,

        StatusInvalidEmailAddress = 459,

        MultipleError = 504,
        StatusSiteCurrentlyNotSupported = 505,
        NewLoginInfoRequiredForSite = 506,
        BetaSiteWorkInProgress = 507,
        InstantRequestTimedout = 508,
        TokenIdInvalid = 509,
        StatusPropertyRecordNotFound = 510,
        StatusHomeValueNotFound = 511,
        NoPayeesAreFoundOnSource = 512,

        GeneralExceptionWhileGatheringMfaData = 517,
        NewMfaInfoRequiredForAgents = 518,
        MfaInfoNotProvidedToYodleeByUserForAgents = 519,
        MfaInfoMismatchForAgents = 520,
        EnrollInMfaAtSite = 521,
        MfaInfoNotProvidedInRealTimeByUserViaApp = 522,
        InvalidMfaInfoInRealTimeByUserViaApp = 523,
        UserProvidedRealTimeMfaDataExpired = 524,
        MfaInfoNotProvidedInRealTimeByGatherer = 525,
        InvalidMfaInfoOrCredentials = 526,

        StatusDbFilterSummarySaveError = 601,

        StatusValueNotCompliant = 708,
        StatusFieldNotAvailable = 709,

        RefreshNeverDone = 801,
        RefreshNeverDoneAfterCredentialsUpdate = 802,

        PartialSuccess = 811
    }
}