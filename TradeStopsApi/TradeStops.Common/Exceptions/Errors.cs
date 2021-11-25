namespace TradeStops.Common.Exceptions
{
    // todo: Consider to make these errors more generic: owner validations, item not found, invalid value.
    // We don't really need specific error messages unless validator returns duplicated error codes.
    // todo: Consider to shift existing error codes from 10-99 somewhere else and put generic error codes in <100 section.
    // To align use Code Alignment extension from Visual Studio Gallery, shortcut Ctrl + Shift + =
    public static class Errors
    {
        public static readonly Error TestError                                      = new Error(-1, "Test Error");

        // todo: Change error code to not-zero
        public static readonly Error InternalServerError                            = new Error(0, "API internal server error"); // server name is added dynamically in code

        public static readonly Error AuthenticationDenied                           = new Error(1, "Authentication has been denied for this request."); // Unauthorized (401)
        public static readonly Error AuthorizationDenied                            = new Error(2, "Authorization has been denied for this request."); // Forbidden (403). Message is dynamically extended to indicate operations with missing permissions

        public static readonly Error InvalidUserContext                             = new Error(3, "User context is invalid.");
        public static readonly Error InvalidCredentials                             = new Error(4, "User credentials are invalid.");
        public static readonly Error InputContractIsEmpty                           = new Error(5, "Input contract cannot be empty");
        public static readonly Error InputParameterIsEmpty                          = new Error(6, "Input parameter cannot be empty"); // parameter name in error message is set dynamically in code (InputValidator)
        public static readonly Error InputListIsEmpty                               = new Error(7, "Input list of parameters cannot be empty");
        public static readonly Error InputContractDeserializationError              = new Error(8, "JSON deserialization error."); // exception message from deserializer is set dynamically in code
        public static readonly Error InvalidUserContextForEndpoint                  = new Error(9, "User context is not valid for requested endpoint.");

        public static readonly Error InvalidSymbolId                                = new Error(10, "SymbolId value is invalid.");
        public static readonly Error SymbolNotFound                                 = new Error(11, "Symbol not found.");
        public static readonly Error SymbolIsNotStock                               = new Error(12, "Symbol is not a stock");
        public static readonly Error SearchQueryAndFieldMustBeSetTogether           = new Error(13, "If SearchQuery or SearchField is set, another value should be set also.");
        public static readonly Error InvalidMonth                                   = new Error(14, "Month should be greater or equal 1 and less or equal 12.");
        public static readonly Error InvalidYear                                    = new Error(15, "Year should be greater or equal zero.");
        public static readonly Error InvalidStrikePrice                             = new Error(16, "Strike price should be greater than zero.");
        public static readonly Error InvalidExchangeId                              = new Error(17, "ExchangeId value is invalid.");
        public static readonly Error SectorNotFound                                 = new Error(18, "Sector is not found.");

        public static readonly Error InvalidTradeDate                               = new Error(21, "Trade date value is invalid.");
        public static readonly Error InvalidEntryPrice                              = new Error(22, "Entry price is invalid.");

        public static readonly Error DuplicateIdsInList                             = new Error(23, "List of IDs contains duplicates");
        public static readonly Error ListOfIdsIsEmpty                               = new Error(24, "List of IDs is emtpy");
        public static readonly Error NoPositionsInPortfolios                        = new Error(25, "NoPositionsInPortfolio");

        public static readonly Error InvalidFromDate                                = new Error(30, "Invalid 'From' date");
        public static readonly Error InvalidToDate                                  = new Error(31, "Invalid 'To' date");
        public static readonly Error FromDateMustBeBeforeToDate                     = new Error(32, "'From' date must be before 'To' date");

        public static readonly Error GridSearchIsEmpty                              = new Error(50, "Search input can not be null.");
        public static readonly Error GridSearchInvalidStart                         = new Error(51, "Start parameter value is invalid.");
        public static readonly Error GridSearchInvalidLimit                         = new Error(52, "Limit parameter value is invalid.");
        public static readonly Error SearchValueAndFieldMustBeSetTogether           = new Error(53, "If SearchValue or SearchField is set, another value must be set also");
        public static readonly Error OrderFieldAndTypeMustBeSetTogether             = new Error(54, "If OrderField or OrderType is set, another value must be set also");
        public static readonly Error SecondaryEmailCannotBeUsedForOrdering          = new Error(55, "SecondaryEmail field cannot be used for ordering");

        public static readonly Error InvalidToken                                   = new Error(60, "Json Web Token is invalid.");
        public static readonly Error InvalidTokenIsExpired                          = new Error(61, "Json Web Token is expired.");
        public static readonly Error InvalidTokenUser                               = new Error(62, "Json Web Token user info is invalid.");
        public static readonly Error InvalidTokenAgent                              = new Error(63, "Json Web Token agent info is invalid.");
        public static readonly Error InvalidTokenTradeSmithUser                     = new Error(64, "Json Web Token used to create context is invalid.");

        public static readonly Error IntradayIsForCrypto                            = new Error(70, "Intraday prices are not available.");

        public static readonly Error InvalidEnumValue                               = new Error(80, "Enum value is not defined."); // enum name and value will be added to error message

        public static readonly Error LicenseKeyMustBePassedExplicitly               = new Error(91, "LicenseKey header must be set for requested endpoint.");
        public static readonly Error InvalidOrganizationIdForEndpoint               = new Error(92, "OrganizationId is not valid for requested endpoint.");

        // User

        public static readonly Error InvalidUserPersonalInfo                        = new Error(100, "User personal info is invalid.");
        public static readonly Error UserPersonalInfoIsInvalidFirstName             = new Error(101, "First Name value is invalid.");
        public static readonly Error UserPersonalInfoIsInvalidLastName              = new Error(102, "Last Name value is invalid.");
        public static readonly Error UserPersonalInfoIsInvalidCountry               = new Error(103, "Country value is invalid.");
        public static readonly Error UserIdInvalidError                             = new Error(104, "UserId value is invalid.");
        public static readonly Error UserNotFound                                   = new Error(105, "User not found.");
        public static readonly Error UserEmailIsNotUnique                           = new Error(106, "User email must be unique."); // email is added dynamically in code.
        public static readonly Error UserEmailIsNotValid                            = new Error(107, "User email is not valid.");
        public static readonly Error UserPasswordIsNotValid                         = new Error(108, "User password is not valid.");
        public static readonly Error UserExpirationDateIsNotValid                   = new Error(109, "User expiration date is not valid.");
        public static readonly Error SearchUserIsEmpty                              = new Error(110, "Search user input can not be empty.");
        public static readonly Error ProductSubscriptionIdIsNotValid                = new Error(111, "Product Subscription ID is not valid.");
        public static readonly Error UserProductSubscriptionNotFound                = new Error(112, "User product subscription was not found.");
        public static readonly Error UserProductSubscriptionAlreadyExists           = new Error(113, "User product subscription already exists.");

        public static readonly Error UpdatePostalInfoErrorInAgora                   = new Error(120, "Error while updating password in Agora."); // error message set dynamically in code

        // Login

        public static readonly Error LoginInvalidUsernameLength                     = new Error(150, "Invalid username length");
        public static readonly Error LoginInvalidPasswordLength                     = new Error(151, "Invalid password length");
        public static readonly Error LoginUserNotFound                              = new Error(154, "User not found");
        public static readonly Error LoginUserWrongPassword                         = new Error(155, "Wrong password");

        // Mobile

        public static readonly Error MobileDeviceNotFoundByRemoteDeviceId           = new Error(180, "Mobile Device is not found in the database by provided device identifier.");
        public static readonly Error MobileDeviceInvalidOwner                       = new Error(181, "This Mobile Devide doesn't belong to the provided user identity.");

        // Portfolio

        public static readonly Error PortfolioInvalidOwner                          = new Error(200, "This portfolio doesn't belong to the provided user identity.");
        public static readonly Error PortfolioNotFound                              = new Error(201, "Portfolio was not found.");
        public static readonly Error PortfolioIsEmpty                               = new Error(202, "Portfolio can not be empty.");
        public static readonly Error PortfolioNameIsEmpty                           = new Error(203, "Portfolio name can not be empty.");
        public static readonly Error PortfolioInvalidCurrency                       = new Error(204, "Currency value is invalid.");
        public static readonly Error PortfolioMaximumNumberReached                  = new Error(205, "The maximum number of portfolios for your account has been reached.");
        public static readonly Error PortfolioNameAlreadyExists                     = new Error(206, "Portfolio with the same name already exists.");
        public static readonly Error PortfolioIdentifierIsEmpty                     = new Error(208, "Portfolio identifier is empty");
        public static readonly Error PortfolioInvalidCash                           = new Error(209, "Portfolio cash is invalid");
        public static readonly Error PortfolioInvalidRiskPerPosition                = new Error(210, "Risk per position is invalid");
        public static readonly Error ReEntryPortfolioAlreadyCreated                 = new Error(211, "Re-Entry Watchlist already created");
        public static readonly Error PortfolioIdentifierListIsEmpty                 = new Error(212, "Portfolios identifier list can not be empty.");

        // Position

        public static readonly Error PositionInvalidOwner                           = new Error(300, "This position doesn't belong to the provided user identity.");
        public static readonly Error PositionNotFound                               = new Error(301, "Position was not found.");
        public static readonly Error PositionIsEmpty                                = new Error(302, "Position can not be empty.");
        public static readonly Error PositionInvalidSymbol                          = new Error(303, "SymbolId value is invalid.");
        public static readonly Error PositionInvalidClosePrice                      = new Error(305, "Close price value is invalid.");
        public static readonly Error PositionInvalidCloseDate                       = new Error(304, "Close date value is invalid.");
        public static readonly Error PositionInvalidSharesAdjustmentType            = new Error(306, "Shares adjustment type should be provided.");
        public static readonly Error PositionInvalidInitialShares                   = new Error(307, "Initial shares value is invalid.");
        public static readonly Error PositionInvalidPriceAdjustmentType             = new Error(308, "Price adjustment type should be provided.");
        public static readonly Error PositionInvalidPurchasePrice                   = new Error(309, "Purchase price value is invalid.");
        public static readonly Error PositionInvalidPurchaseDate                    = new Error(310, "Purchase date value is invalid.");
        public static readonly Error PositionInvalidAdjustmentForShort              = new Error(311, "IgnoreDividend must be set to true for positions with Trade Type = Short.");
        public static readonly Error PairTradePositionInvalidNumberOfSubtrades      = new Error(330, "Pairs trade position should have atleast two subtrades.");
        public static readonly Error PairTradePositionNotPairTradeType              = new Error(331, "Provided position is not a pairs trade.");
        public static readonly Error PairTradePositionInvalidSubtradeWeight         = new Error(332, "Subtrade weight value is invalid.");
        public static readonly Error PositionListIsEmpty                            = new Error(333, "Positions list can not be empty.");
        public static readonly Error ClosePositionInvalidSharesToSell               = new Error(334, "SharesToSell value is not valid.");
        public static readonly Error PairTradeClosePriceMustBeEmpty                 = new Error(335, "ClosePrice value for Pairs trade must be empty.");
        public static readonly Error PositionIdentifierListIsEmpty                  = new Error(336, "Positions identifier list can not be empty.");
        public static readonly Error PositionAlreadyInPortfolio                     = new Error(337, "Position portfolio and target portfolio are the same.");
        public static readonly Error PartialCloseOnlyForRegularPositions            = new Error(338, "Partial close works only for regular positions.");
        public static readonly Error SynchronizedPositionCanNotBeMoved              = new Error(339, "Synchronized position can't be moved.");
        public static readonly Error TopItemsCountShouldBeMoreThanZero              = new Error(340, "'Top items count' value should be more than zero.");

        // PositionsGrid && AlertsGrid

        public static readonly Error PortfolioIdAndTypeIsEmpty                      = new Error(360, "PortfolioId or CompositePortfolioType should be provided.");
        public static readonly Error GetClosedPositionsGridDataContractIsEmpty      = new Error(361, "Input can not be empty.");
        public static readonly Error GetClosedPositionsGridDataInvalidFromDate      = new Error(362, "FromDate value is invalid.");
        public static readonly Error GetClosedPositionsGridDataInvalidToDate        = new Error(363, "ToDate value is invalid.");
        public static readonly Error GetAlertsGridDataContractIsEmpty               = new Error(364, "Input can not be empty.");

        // Views

        public static readonly Error UserViewNotFound                               = new Error(380, "UserView not found.");
        public static readonly Error ColumnNotFound                                 = new Error(382, "Column not found.");
        public static readonly Error InvalidViewColumnWidth                         = new Error(383, "Width value is not valid.");
        public static readonly Error CanNotDeleteLastViewInType                     = new Error(384, "Can not delete last view in type.");
        public static readonly Error InvalidUserViewOwner                           = new Error(386, "This view doesn't belong to the provided user identity.");
        public static readonly Error InvalidUserViewType                            = new Error(387, "Invalid ViewType value for UserView.");

        public static readonly Error OrganizationViewNotFound                       = new Error(390, "Organization View not found.");
        public static readonly Error OrganizationViewColumnNotFound                 = new Error(391, "Organization View Column not found.");
        public static readonly Error InvalidOrganizationViewOwner                   = new Error(392, "This view doesn't belong to the provided user identity.");
        public static readonly Error InvalidOrganizationViewType                    = new Error(393, "Invalid ViewType value for OrganizationView.");

        public static readonly Error PortfolioViewNotFound                          = new Error(395, "Portfolio View not found.");
        public static readonly Error PortfolioViewColumnNotFound                    = new Error(396, "Portfolio View Column not found.");
        public static readonly Error InvalidPortfolioViewOwner                      = new Error(397, "This view doesn't belong to the provided user identity.");

        // Tag

        public static readonly Error InvalidTag                                     = new Error(400, "Tag can not be empty.");
        public static readonly Error TagNotFound                                    = new Error(401, "Tag was not found.");
        public static readonly Error InvalidTagOwner                                = new Error(402, "This tag doesn't belong to the provided user identity.");
        public static readonly Error TagAlreadyExists                               = new Error(403, "Tag with the same value already exists.");
        public static readonly Error EmptyTagsList                                  = new Error(404, "Tags list can not be empty.");

        // Newsletters

        public static readonly Error NewslettersPortfolioNotFound                   = new Error(450, "Newsletters portfolio is not found.");

        // Research

        public static readonly Error CombilnedVqIsEmpty                             = new Error(500, "Combilned VQ contract can not be empty.");
        public static readonly Error CombilnedVqInvalidCurrency                     = new Error(501, "DefaultCurrencyId value is invalid.");
        public static readonly Error CombilnedVqInvalidNumberOfPositions            = new Error(502, "To calculate combined VQ atleast two positons are required.");
        public static readonly Error CombilnedVqInvalidSymbol                       = new Error(503, "SymbolId value is invalid.");
        public static readonly Error CombilnedVqInvalidShares                       = new Error(504, "Shares value is invalid.");
        public static readonly Error CombilnedVqInvalidTradeDate                    = new Error(505, "TradeDate value is invalid.");

        public static readonly Error RiskRebalancePositionsIsEmpty                  = new Error(509, "Input value can not be empty");
        public static readonly Error InvalidCurrency                                = new Error(510, "DefaultCurrencyId value is invalid.");
        public static readonly Error RiskRebalancePositionsInvalidNumberOfPositions = new Error(511, "To run risk rebalance atleast two positions are required.");
        public static readonly Error RiskRebalancePositionsInvalidSymbol            = new Error(512, "SymbolId value is invalid.");
        public static readonly Error RiskRebalancePositionsInvalidShares            = new Error(513, "Shares value is invalid.");
        public static readonly Error RiskRebalancePortfolioIsEmpty                  = new Error(514, "Input value can not be empty");
        public static readonly Error InvalidThresholdValue                          = new Error(515, "ThresholdValue can not be null if StopType is TsPercent or FixedStopPrice");
        public static readonly Error InvalidInvestmentAmount                        = new Error(516, "Investment amount value is not valid");
        public static readonly Error InvalidRiskAmount                              = new Error(517, "Risk amount value is not valid");
        public static readonly Error UnavailableSsi                                 = new Error(518, "Ssi is not available for this symbol with specified TradeType");        

        public static readonly Error QuantToolInputIsEmpty                          = new Error(519, "Quant Tool input contract can not be empty.");
        public static readonly Error QuantToolPositionsListIsEmpty                  = new Error(520, "Quant Tool requires at least one position to run.");
        public static readonly Error QuantToolInvalidInvestmentAmount               = new Error(521, "InvestmentAmount value is invalid.");
        public static readonly Error QuantToolInvalidNumberOfPositions              = new Error(522, "NumberOfPositions value is invalid.");
        public static readonly Error QuantToolInvalidAverageVqThreshold             = new Error(523, "AverageVqThreshold value is invalid.");
        public static readonly Error InvalidStopType                                = new Error(524, "Invalid StopType value.");

        public static readonly Error InstrumentRatingsInvalidMaxResults             = new Error(525, "MaxResults parameter value is invalid.");
        public static readonly Error QuantToolInvalidAverageVolume                  = new Error(526, "AverageVolume is less than 0.");
        public static readonly Error QuantToolEmptySources                          = new Error(527, "All sources of positions are empty.");
        public static readonly Error QuantToolStockFinderSearchNotFound             = new Error(528, "Stock finder search source is not found.");
        public static readonly Error PureQuantResultsInvalidLatestPrice             = new Error(529, "Pure Quant position Latest Price value is invalid.");
        public static readonly Error PureQuantResultsInvalidPositionRisk            = new Error(530, "Pure Quant Position Risk value is invalid.");
        public static readonly Error PureQuantResultsInvalidPositionSize            = new Error(531, "Pure Quant Position Size value is invalid.");
        public static readonly Error PureQuantResultsInvalidPositionSizePercent     = new Error(532, "Pure Quant Position Size Percent value is invalid.");
        public static readonly Error PureQuantResultsInvalidShares                  = new Error(533, "Pure Quant position Shares value is invalid.");
        public static readonly Error PureQuantResultsInvalidRank                    = new Error(534, "Pure Quant position Smith Rank value is invalid.");
        public static readonly Error PureQuantResultsNotFound                       = new Error(535, "Pure Quant result is not found.");

        public static readonly Error MarketOutlookInputIsEmpty                      = new Error(540, "GroupId or MarketOutlookTypes param must be provided.");

        // Backtester

        public static readonly Error BacktesterTaskNotFound                         = new Error(550, "Backtester Task is not found.");
        public static readonly Error BacktesterTaskInvalidOwner                     = new Error(551, "User don't have access to the requested task.");
        public static readonly Error BacktesterEmptyListOfStrategies                = new Error(552, "List of strategies cannot be empty.");
        public static readonly Error BacktesterEmptyPositionSources                 = new Error(553, "All sources of positions are empty.");
        public static readonly Error BacktesterStrategyNotFound                     = new Error(554, "Backtester Strategy is not found.");
        public static readonly Error BacktesterStrategyInvalidOwner                 = new Error(555, "User don't have access to the requested strategy.");
        public static readonly Error BacktesterStrategyNameIsEmpty                  = new Error(556, "The Name field for Backtester Strategy can't be empty.");
        public static readonly Error BacktesterEntryStrategyParametersAreMissing    = new Error(557, "Entry parameters for Backtester Strategy are missing.");
        public static readonly Error BacktesterExistStrategyParametersAreMissing    = new Error(558, "Exit parameters for Backtester Strategy are missing.");
        public static readonly Error BacktesterPositionSizeParametersAreMissing     = new Error(559, "Position Size parameters for Backtester Strategy are missing.");

        // Chart

        public static readonly Error ChartsGetVqLineInvalidStartDate                = new Error(601, "StartDate value is invalid.");
        public static readonly Error ChartsGetVqLineInvalidEndDate                  = new Error(602, "EndDate value is invalid.");

        public static readonly Error PredictedPriceChartProbabilityLevelsAreEmpty   = new Error(605, "Probability levels list is empty.");
        public static readonly Error PredictedPriceChartPriceLineFromDateInvalid    = new Error(606, "PriceLineFromDate should be in the past and less that PredictionToDate.");
        public static readonly Error PredictedPriceChartPredictionToDateInvalid     = new Error(607, "PredictionToDate should be in the future and greater than PriceLineFromDate.");

        // Admin
        public static readonly Error BrokerPillIdInvalid                            = new Error(610, "BrokerPillId value is invalid.");
        public static readonly Error FinancialInstitutionsListInvalid               = new Error(611, "FinancialInstitutionIds value is invalid.");
        public static readonly Error InvalidFinancialInstitution                    = new Error(612, "FinancialInstitutionId value is invalid.");
        public static readonly Error FinancialInstitutionVendorIdIsInvalid          = new Error(613, "FinancialInstitution VendorId value is invalid.");
        public static readonly Error UsersListInvalid                               = new Error(614, "UserIds value is invalid.");
        public static readonly Error UserHasNoAccessToFinancialInstitution          = new Error(615, "The provided user doesn't have access to the specified financial institution.");

        public static readonly Error YodleeErrorCodeInvalid                         = new Error(620, "Yodlee error code invalid.");
        public static readonly Error YodleeUserContextIsNull                        = new Error(621, "Yodlee user context is null.");
        public static readonly Error YodleeUserContextIsInvalid                     = new Error(622, "Yodlee user context is invalid.");
        public static readonly Error YodleeUserContextCreationDateIsInvalid         = new Error(623, "Yodlee user context creation date is invalid.");
        public static readonly Error YodleeUserContextLastTouchDateIsInvalid        = new Error(624, "Yodlee user context last touch conversation credentials date is invalid.");

        public static readonly Error ImportProcessVendorAccountIdIsInvalid          = new Error(640, "Import process VendorAccountId is invalid.");
        public static readonly Error ImportProcessFinancialInstitutionIdIsInvalid   = new Error(641, "Import process FinancialInstitutionId is invalid.");
        public static readonly Error InvalidImportProcessOwner                      = new Error(642, "This import process doesn't belong to the provided user identity.");
        public static readonly Error ImportProcessImportDateIsInvalid               = new Error(643, "Import process ImportDate is invalid.");
        public static readonly Error ImportProcessProgressIsInvalid                 = new Error(644, "Import process Progress is invalid.");
        public static readonly Error ImportProcessNotFound                          = new Error(645, "Import process not found.");
        public static readonly Error ImportProcessesIdsListIsInvalid                = new Error(646, "Import processes IDs list is invalid.");
        public static readonly Error ImportProcessIsNull                            = new Error(647, "Import process is null.");

        public static readonly Error SnaidIsEmpty                                   = new Error(670, "Snaid value can not be empty");
        public static readonly Error AgoraCustomerNumberIsEmpty                     = new Error(671, "Agora customer number can not be empty");

        // Unconfirmed positions

        public static readonly Error UnconfirmedPositionNotFound                    = new Error(701, "Unconfirmed Position was not found.");
        public static readonly Error UnconfirmedPositionInvalidOwner                = new Error(703, "This position doesn't belong to the provided user identity.");

        public static readonly Error InvalidVendorAccountId                         = new Error(710, "Sync portfolio VendorAccountId is invalid.");
        public static readonly Error InvalidVendorPortfolioId                       = new Error(711, "Sync portfolio VendorPortfolioId is invalid.");

        public static readonly Error InvalidSyncPositionHoldingId                   = new Error(720, "Sync position HoldingId is invalid.");
        public static readonly Error InvalidSyncPositionHoldingSymbol               = new Error(721, "Sync position Holding Symbol is invalid.");
        public static readonly Error InvalidSyncPositionShares                      = new Error(722, "Sync position Shares should be equal or greater than zero.");
        public static readonly Error InvalidSyncPositionPurchasePrice               = new Error(723, "Sync position Purchase Price should be equal or greater than zero.");
        public static readonly Error InvalidSyncPositionStrikePrice                 = new Error(724, "Sync position Strike Price should be equal or greater than zero.");
        public static readonly Error InvalidSyncPositionPurchaseDate                = new Error(725, "Sync position Purchase Date is invalid.");
        public static readonly Error InvalidSyncPositionExpirationDate              = new Error(726, "Sync position Expiration Date is invalid.");

        public static readonly Error IncompleteOptionsListIsEmpty                   = new Error(750, "Incomplete options list can not be empty.");
        public static readonly Error IncompleteOptionIsEmpty                        = new Error(751, "Incomplete option can not be empty.");
        public static readonly Error InvalidIncompleteOptionStrikePrice             = new Error(752, "Incomplete option Strike Price should be equal or greater than zero.");
        public static readonly Error InvalidIncompleteOptionFinancialInstitutionId  = new Error(753, "Incomplete option FinancialInstitutionId is invalid.");
        public static readonly Error InvalidIncompleteOptionVendorHoldingId         = new Error(754, "Incomplete option VendorHoldingId should be greater than zero.");
        public static readonly Error InvalidIncompleteOptionExpirationDate          = new Error(755, "Incomplete option Expiration date is invalid.");
        public static readonly Error InvalidIncompleteOptionVendorPortfolioId       = new Error(756, "Incomplete option VendorPortfolioId should be greater than zero.");
        public static readonly Error InvalidIncompleteOptionVendorAccountId         = new Error(757, "Incomplete option VendorAccountId should be greater than zero.");

        // PositionTriggers

        public static readonly Error PositionTriggerIsEmpty                         = new Error(800, "Input can not be empty.");
        public static readonly Error PositionTriggerNoSsiStatesToTrack              = new Error(801, "Specify at least one SSI state to track.");
        public static readonly Error PositionTriggerInvalidThresholdValue           = new Error(802, "ThresholdValue value is invalid.");
        public static readonly Error PositionTriggerInvalidPeriodValue              = new Error(803, "Period value is invalid.");
        public static readonly Error PositionTriggerInvalidThresholdDate            = new Error(804, "ThresholdDate value is invalid.");
        public static readonly Error PositionTriggerInvalidOwner                    = new Error(805, "This position trigger doesn't belong to the provided user identity.");
        public static readonly Error PositionTriggerMaximumNumberOfAlerts           = new Error(806, "The maximum number of alerts for your account has been reached.");
        public static readonly Error PositionTriggerAvailableOnlyForShortCalls      = new Error(807, "This type of alert is available only for Short Calls.");
        public static readonly Error PositionTriggerAvailableOnlyForShortPuts       = new Error(808, "This type of alert is available only for Short Puts.");
        public static readonly Error PositionTriggerInvalidStockPurchasePriceValue  = new Error(809, "StockPurchasePrice param is invalid.");
        public static readonly Error PositionTriggerAvailableOnlyForOptions         = new Error(810, "This type of alert is available only for Options.");
        public static readonly Error PositionTriggerNotFound                        = new Error(811, "Position Trigger was not found.");
        public static readonly Error PositionTriggerInvalidStartDate                = new Error(812, "StartDate value is invalid.");
        public static readonly Error PositionTriggerDuplicateTrigger                = new Error(813, "Position trigger with the same parameters has already been created for this position.");
        public static readonly Error PositionTriggerInvalidStartDateForPairTrade    = new Error(814, "Start Date cannot be prior Entry Date.");
        public static readonly Error PositionTriggerAvailableOnlyStocks             = new Error(815, "This type of alert is available only for Stocks.");
        public static readonly Error PositionTriggerPositionIdsIsEmpty              = new Error(816, "Input can not be empty.");
        public static readonly Error PositionTriggerIsNotAvailableForOptions        = new Error(817, "Trigger of this type is not available for options");
        public static readonly Error PositionTriggerIsNotAvailableForPairTrades     = new Error(818, "Trigger of this type is not available for pair trades");
        public static readonly Error PositionTriggerPriceTypeNotValidForPairTrades  = new Error(819, "Price type, specified for this trigger is not valid for pair trades");
        public static readonly Error PositionTriggerPositionWithoutFundamentalData  = new Error(820, "Trigger of this type is available only for positions with fundamental data");
        public static readonly Error PositionTriggerPositionWithoutPurchaseDate     = new Error(821, "Trigger of this type is available only for positions with purchase date");
        public static readonly Error PositionTriggerPositionWithoutPurchasePrice    = new Error(822, "Trigger of this type is available only for positions with purchase price");
        public static readonly Error PositionTriggerNotEnoughHistoricalDataForSsi   = new Error(823, "Not enough historical data to create SSI trigger");
        public static readonly Error PositionTriggerNotAvailableForChosenCurrency   = new Error(824, "Trigger not available for chosen currency");
        public static readonly Error PositionTriggerNotAvailableForShorts           = new Error(825, "Trigger not available for shorts");
        public static readonly Error PositionTriggerIntradayNotAvailable            = new Error(826, "Intraday is not available for this alert type.");

        // AlertTemplates

        public static readonly Error AlertTemplateNotFound                          = new Error(851, "Alert template was not found.");
        public static readonly Error InvalidAlertTemplateOwner                      = new Error(852, "This alert doesn't belong to the provided user identity.");
        public static readonly Error AlertTemplateAlreadyDefault                    = new Error(853, "Alert template already marked as default.");
        public static readonly Error DefaultAlertTemplateCanNotBeDeleted            = new Error(854, "Default alert template can't be deleted.");
        public static readonly Error AlertTemplateTriggerNotFound                   = new Error(855, "Alert template trigger was not found.");
        public static readonly Error LastAlertTemplateTriggerCanNotBeDeleted        = new Error(856, "Last alert template trigger can't be deleted.");
        public static readonly Error AlertTemplateMustHaveAtLeastOneTrigger         = new Error(857, "Alert template must have at least one trigger.");
        public static readonly Error AlertTemplateDuplicateName                     = new Error(858, "Alert template with this name already exists.");
        public static readonly Error AlertTemplateTriggerDuplicate                  = new Error(859, "Alert template trigger with the same parameters has already been created.");

        // Reset Password

        public static readonly Error ResetPasswordIsEmpty                           = new Error(900, "Input can not be empty.");
        public static readonly Error ResetPasswordEmailNotFound                     = new Error(901, "User with the provided Email was not found.");
        public static readonly Error ResetPasswordInvalidToken                      = new Error(902, "Reset password token is invalid.");
        public static readonly Error ResetPasswordInvalidNewPassword                = new Error(903, "NewPassword value is invalid.");
        public static readonly Error ResetPasswordInvalidConfirmPassword            = new Error(904, "ConfirmPassword value is invalid.");
        public static readonly Error ResetPasswordPasswordsNotMatch                 = new Error(905, "NewPassword and ConfirmPassword values doesn't match.");
        public static readonly Error ResetPasswordInvalidPasswordLength             = new Error(906, "Password length should be 6-72 symbols.");

        // Portfolio Analyzer

        public static readonly Error PortfolioAnalyzerSnaidIsEmpty                  = new Error(1000, "Snaid value can not be empty");
        public static readonly Error PortfolioAnalyzerActionTakenIsEmpty            = new Error(1001, "ActionTaken value can not be empty");
        public static readonly Error PortfolioAnalyzerPageUrlIsEmpty                = new Error(1002, "PageUrl value can not be empty");
        public static readonly Error PortfolioAnalyzerIdentifierIsEmpty             = new Error(1003, "MemItemId and PortfolioId values can not be empty");

        //Vendor Sync Logs

        public static readonly Error VendorPortfolioLogIsEmpty                      = new Error(1010, "Input can not be empty.");
        public static readonly Error InvalidVendorPortfolioLogVendorSyncLogId       = new Error(1012, "VendorSyncLogId should be greater than zero.");
        public static readonly Error InvalidVendorPortfolioLogVendorPortfolioLogId  = new Error(1013, "VendorPortfolioLogId should be greater than zero.");
        public static readonly Error InvalidVendorPortfolioLogFilePath              = new Error(1014, "FilePath should not be null or empty");
        public static readonly Error InvalidVendorPortfolioLogPositionsAmount       = new Error(1015, "PositionsAmount should be equal or greater than zero.");
        public static readonly Error InvalidVendorPortfolioLogTransactionsAmount    = new Error(1016, "TransactionsAmount should be equal or greater than zero.");
        public static readonly Error InvalidTransactionsPeriodFrom                  = new Error(1017, "TransactionsPeriodFrom date is invalid.");
        public static readonly Error InvalidTransactionsPeriodTo                    = new Error(1018, "TransactionsPeriodTo date is invalid.");
        public static readonly Error VendorSyncLogIsEmpty                           = new Error(1020, "VendorSyncLog create contract can not be empty.");
        public static readonly Error VendorSyncLogInvalidVendorAccountId            = new Error(1022, "VendorAccountId should be greater than zero.");
        public static readonly Error VendorSyncLogInvalidUserId                     = new Error(1023, "UserId should be greater than zero.");
        public static readonly Error VendorSyncLogInvalidFinancialInstitutionName   = new Error(1024, "FinancialInstitutionName should not be null or empty.");
        public static readonly Error VendorSyncLogInvalidUserEmail                  = new Error(1025, "UserEmail should not be null or empty.");
        public static readonly Error VendorSyncLogInvalidVendorSyncLogId            = new Error(1026, "VendorSyncLogId is invalid.");
        public static readonly Error VendorSyncLogInvalidErrorCode                  = new Error(1027, "Error code and Vendor type both have to be set.");
        public static readonly Error VendorPortfolioLogInvalidId                    = new Error(1028, "VendorPortfolioLogId is invalid.");
        public static readonly Error VendorUsernameExists                           = new Error(1029, "VendorUserName already exists. It cannot be change.");

        //Deleted sync positions

        public static readonly Error DeletedSyncPositionNotFound                    = new Error(1050, "Deleted Sync Position was not found.");
        public static readonly Error DeletedSyncPositionInvalidOwner                = new Error(1051, "This not updated position doesn't belong to the provided user identity.");
        public static readonly Error DeletedSyncPositionInvalidHoldingId            = new Error(1052, "Deleted Sync Position holding ID should be greater than zero.");

        // System Events

        public static readonly Error SystemEventsInvalidOwner                       = new Error(1100, "This system event doesn't belong to the provided user identity");
        public static readonly Error SystemEventsInvalidStartDate                   = new Error(1101, "Start date is not valid");
        public static readonly Error SystemEventsInvalidEndDate                     = new Error(1102, "End date is not valid");

        // Stock Finder

        public static readonly Error StockFinderSearchNotFound                      = new Error(1110, "This stock finder search doesn't exist.");
        public static readonly Error StockFinderSearchAlreadyExists                 = new Error(1111, "This stock finder search already exists.");
        public static readonly Error StockFinderSearchNameMatches                   = new Error(1112, "This stock finder search name already exists.");
        public static readonly Error StockFinderFiltersNotFound                     = new Error(1113, "There is no stock finder search filters to save.");
        public static readonly Error StockFinderFilterParamsNotFound                = new Error(1114, "Stock finder search filter parameters doesn't exists.");
        public static readonly Error PairedStockFinderFilter                        = new Error(1115, "Paired stock finder filter doesn't contain parent parameter.");

        // Vendor sync error messages

        public static readonly Error VendorSyncErrorMessageIdInvalid                = new Error(1120, "VendorSyncErrorMessageId is invalid.");

        // Vendor Usernames

        public static readonly Error VendorUsernameIsEmpty                          = new Error(1125, "Vendor username can not be empty.");

        // TradeIt users and user contexts

        public static readonly Error TradeItLinkUserIdIsEmpty                       = new Error(1130, "LinkUserId value can not be empty.");
        public static readonly Error TradeItBrokerNameIsEmpty                       = new Error(1131, "BrokerName value can not be empty.");
        public static readonly Error TradeItUserInvalidOwner                        = new Error(1132, "This TradeIt user doesn't belong to the provided user identity.");
        public static readonly Error TradeItSessionTokenIsEmpty                     = new Error(1133, "SessionToken value can not be empty.");
        public static readonly Error TradeItUserIdInvalid                           = new Error(1134, "TradeIt user id should be greater than zero.");
        public static readonly Error TradeItUserNotFound                            = new Error(1135, "TradeIt user was not found.");
        public static readonly Error TradeItUserContextNotFound                     = new Error(1136, "TradeIt user context was not found.");

        // Sync vendor account tasks

        public static readonly Error SyncVendorAccountTaskNotFound                  = new Error(1140, "Sync vendor account task not found.");
        public static readonly Error SyncVendorAccountTaskInvalidOwner              = new Error(1141, "This sync vendor account task doesn't belong to the provided user identity.");
        public static readonly Error SyncVendorAccountTaskInvalidVendorAccountId    = new Error(1142, "Vendor account ID should not be null or empty.");
        public static readonly Error SyncVendorAccountTaskInvalidErrorMessageId     = new Error(1143, "Error message ID is invalid.");
        public static readonly Error SyncVendorAccountTaskNoActivePortfolios        = new Error(1144, "There are no active portfolios to be refreshed for provided Vendor Account ID and vendor type.");
        public static readonly Error SyncVendorAccountTaskInvalidVendorPortfolioId  = new Error(1145, "There is no portfolio with such vendor portfolio ID among portfolios from this account.");
        public static readonly Error SyncVendorAccountTaskAlreadyInitiated          = new Error(1146, "Refresh task for this vendor account was already initiated.");

        // Notification Events

        public static readonly Error NotificationEventEntityIdInvalidValue          = new Error(1150, "Notification event EntityId must have value greater than 0.");
        public static readonly Error NotificationEventNotFound                      = new Error(1151, "Notification event is not found.");
        public static readonly Error NotificationEventInvalidOwner                  = new Error(1152, "User don't have access to the requested notification event.");

        //News

        public static readonly Error NewsMetadataNotFound                           = new Error(1200, "News metadata not found.");
        public static readonly Error NewsMetadataWrongLimit                         = new Error(1201, "Limit field (number of items to take) must have value greater than 0.");

        // Financial institution rules
        public static readonly Error FinancialInstitutionRuleRestrictionsEmpty      = new Error(1210, "Financial institution rules restriction flags can not be empty.");
        public static readonly Error FinancialInstitutionRuleExists                 = new Error(1211, "Financial institution rule for the specified financial institution already exists.");
        public static readonly Error FinancialInstitutionRuleWarningMessage         = new Error(1212, "Financial institution rule warning message description can not be empty.");
        public static readonly Error FinancialInstitutionRuleWarningMessageTitle    = new Error(1213, "Financial institution rule warning message title can not be empty for popups.");
        public static readonly Error FinancialInstitutionRuleNotFound               = new Error(1214, "Financial institution rule not found.");

        //Email

        public static readonly Error EmailSenderIsNotValid                          = new Error(1250, "Email sender is not valid.");
        public static readonly Error EmailSubjectIsNotValid                         = new Error(1251, "Email subject is not valid.");
        public static readonly Error EmailBodyIsNotValid                            = new Error(1252, "Email body is not valid.");
        public static readonly Error SenderNameIsNotValid                           = new Error(1253, "Sender name is not valid.");
        public static readonly Error SenderAddressIsNotValid                        = new Error(1254, "Sender address is not valid.");
        public static readonly Error RecipientAddressIsNotValid                     = new Error(1255, "Recipient address is not valid.");
        public static readonly Error EmailCategoryIsNotValid                        = new Error(1256, "Email category is not valid.");

        //Publishers

        public static readonly Error PublishersPubCodeIsEmpty                       = new Error(1300, "Pub code can not be empty.");
        public static readonly Error PublishersOwnOrgIsEmpty                        = new Error(1301, "Own org can not be empty.");
        public static readonly Error PublishersOwnOrgNotAvailable                   = new Error(1302, "Own org is not available for customer.");
        public static readonly Error PublishersPubCodeIsDuplicate                   = new Error(1303, "Pub code is a duplicate.");
        public static readonly Error PublishersPubCodeDoesNotExist                  = new Error(1304, "Pub code does not exist.");
        public static readonly Error PublishersBusinessUnitExists                   = new Error(1305, "Business unit already exists for this customer.");
        public static readonly Error PublishersBusinessUnitIdIsNotValid             = new Error(1306, "Business unit id is not valid for this customer.");
        public static readonly Error PublishersCustomerDoesNotExist                 = new Error(1307, "Customer does not exist.");
        public static readonly Error PublishersCustomSymbolIsNotUnique              = new Error(1308, "Custom Symbol must be unique.");
        public static readonly Error PublishersCustomSymbolDoesNotExist             = new Error(1309, "Custom symbol does not exist.");
        public static readonly Error PublishersCustomSymbolNotAvailable             = new Error(1310, "Custom symbol is not available for customer.");
        public static readonly Error PublishersCustomSymbolNotFound                 = new Error(1311, "Custom symbol was not found.");
        public static readonly Error PublishersCustomSymbolNameIsEmpty              = new Error(1312, "Custom symbol name can not be empty.");
        public static readonly Error PublishersCustomPriceIsNotUnique               = new Error(1313, "Custom Price must be unique.");
        public static readonly Error PublishersCustomPriceValueInvalid              = new Error(1314, "Custom Price value must be greater or equal to 0 and less than or equal to 10000000.");
        public static readonly Error PublishersCustomPriceNotFound                  = new Error(1315, "Custom price was not found.");
        public static readonly Error PublishersInvalidCustomSymbolOwner             = new Error(1316, "Custom symbol doesn't belong to the provided customer identity.");
        public static readonly Error PublishersCustomPriceListIsEmpty               = new Error(1317, "Custom prices list can not be empty.");
        public static readonly Error PublishersTradeGroupIsNotUnique                = new Error(1318, "Trade Group must be unique.");
        public static readonly Error PublishersTradeGroupNotFound                   = new Error(1319, "Trade Group was not found.");
        public static readonly Error PublishersTradeGroupNotAvailable               = new Error(1320, "Trade group is not available for customer.");
        public static readonly Error PublishersTradeGroupCanNotChangePosition       = new Error(1321, "Trade group can not change position.");
        public static readonly Error PublishersPortfolioNotAvailable                = new Error(1322, "Portfolio is not available for customer.");
        public static readonly Error PublishersBusinessUnitNotFound                 = new Error(1323, "Business unit is not found.");
        public static readonly Error PublishersCustomDividendNotFound               = new Error(1324, "Custom dividend was not found.");
        public static readonly Error PublishersCustomDividendValueInvalid           = new Error(1325, "Dividend value must be greater or equal to 0 and less than or equal to 10000000.");
        public static readonly Error PublishersPortfolioNotFound                    = new Error(1326, "Portfolio was not found.");
        public static readonly Error PublishersPortfolioInvalidOwner                = new Error(1327, "This portfolio doesn't belong to the provided customer identity.");
        public static readonly Error PublishersPortfolioIdentifierListIsEmpty       = new Error(1328, "Portfolios identifier list can not be empty.");
        public static readonly Error PublishersCustomSymbolsIdentifierListIsEmpty   = new Error(1329, "Custom symbols identifier list can not be empty.");
        public static readonly Error PublishersSubtradeNotFound                     = new Error(1330, "Subtrade was not found.");
        public static readonly Error PublishersInvalidDividendTradeDate             = new Error(1331, "Dividend Trade Date should be greater than subtrade open date.");
        public static readonly Error PublishersInvalidDividendPayDate               = new Error(1332, "Pay Date should be greater than trade date.");
        public static readonly Error PublishersDividendNotFound                     = new Error(1333, "Dividend was not found.");
        public static readonly Error PublishersDividendInvalidOwner                 = new Error(1334, "This dividend doesn't belong to the provided customer identity.");
        public static readonly Error PublishersSubtradeNotAvailable                 = new Error(1335, "Subtrade is not available for customer.");
        public static readonly Error PublishersDividendCanNotBeUpdated              = new Error(1336, "Regular dividend can not be updated.");
        public static readonly Error PublishersDividendCanNotBeDeleted              = new Error(1337, "Regular dividend can not be deleted.");
        public static readonly Error PublishersViewNotFound                         = new Error(1338, "View was not found.");
        public static readonly Error PublishersDefaultViewCanNotBeDeleted           = new Error(1339, "Default view can't be deleted.");
        public static readonly Error PublishersViewTemplateInvalidOwner             = new Error(1340, "This view template doesn't belong to the provided customer identity.");
        public static readonly Error PublishersViewTemplateNotFound                 = new Error(1341, "View template was not found.");
        public static readonly Error PublishersViewTemplateCanNotBeChanged          = new Error(1342, "Default or legal view template can not be updated or deleted.");

        //Sync Flow module token validation
        public static readonly Error InvalidSyncFlowToken                           = new Error(1400, "Sync Flow token is invalid");

        // Publications
        public static readonly Error PublicationSubscriptionsNotFound               = new Error(1500, "There is no subscription for the requested publication type.");
        public static readonly Error PublicationNotFound                            = new Error(1501, "Publication was not found.");
        public static readonly Error PublicationCategoryNotFound                    = new Error(1502, "Publication category was not found.");

        // Timings
        public static readonly Error SymbolAndTimingIdentifiersListIsNotNull        = new Error(1600, "List of symbol and timing identifiers cannot have values at the same time.");

        // Portfolio Tracker
        public static readonly Error PortfolioTrackerPortfolioNameIsEmpty           = new Error(1701, "Portfolio name can not be empty.");
        public static readonly Error PortfolioTrackerPortfolioNameAlreadyExists     = new Error(1702, "Portfolio with the same name already exists.");
        public static readonly Error PortfolioTrackerPortfolioInvalidOwner          = new Error(1703, "This portfolio doesn't belong to the provided user identity.");
        public static readonly Error PortfolioTrackerPortfolioNotFound              = new Error(1704, "Portfolio was not found.");
        public static readonly Error PortfolioTrackerPortfolioInvalidOrganization   = new Error(1705, "This user doent's have access to the provided organization.");

        public static readonly Error PortfolioTrackerPortfolioGroupNameIsEmpty       = new Error(1711, "Portfolio Group name can not be empty.");
        public static readonly Error PortfolioTrackerPortfolioGroupNameAlreadyExists = new Error(1712, "Portfolio Group with the same name already exists.");
        public static readonly Error PortfolioTrackerPortfolioGroupInvalidOwner      = new Error(1713, "Portfolio Group doesn't belong to the provided user identity.");
        public static readonly Error PortfolioTrackerPortfolioGroupNotFound          = new Error(1714, "Portfolio Group was not found.");

        public static readonly Error PortfolioTrackerNotSubscribedToOrganization     = new Error(1715, "User doesn't have access to any Organization.");        
        public static readonly Error PortfolioTrackerOrganizationNotSelected         = new Error(1716, "User has access to multiple Organizations, so OrganizationId must be set explicitly.");        
        public static readonly Error PortfolioTrackerMixedOrganizations              = new Error(1717, "Cross-organization operations are not allowed.");
        public static readonly Error PortfolioTrackerTransactionsListIsEmpty         = new Error(1718, "It's not possible to create subtrade without transactions.");
        public static readonly Error PortfolioTrackerSubtradesListIsEmpty            = new Error(1719, "It's not possible to create position without transactions.");
        public static readonly Error PortfolioTrackerRegularPositionsOnly            = new Error(1720, "Combined and positions with multiple subtrades are not supported yet.");
        public static readonly Error PortfolioTrackerInvalidEntryTransaction         = new Error(1721, "Entry transaction must always happen before exit transaction.");
        public static readonly Error PortfolioTrackerInvalidTotalQuantity            = new Error(1722, "Total quantity of all transaction is invalid.");

        public static readonly Error PortfolioTrackerPositionNotFound                = new Error(1730, "Position was not found.");
        public static readonly Error PortfolioTrackerInvalidTradeDate                = new Error(1731, "Trade date value is invalid.");
        public static readonly Error PortfolioTrackerInvalidQuantity                 = new Error(1732, "Quantity value is invalid.");
        public static readonly Error PortfolioTrackerInvalidPrice                    = new Error(1733, "Price value is invalid.");
        public static readonly Error PortfolioTrackerInvalidQuantityAdjustmentType   = new Error(1734, "Quantity adjustment type must be provided.");
        public static readonly Error PortfolioTrackerInvalidPriceAdjustmentType      = new Error(1735, "Price adjustment type must be provided.");
        public static readonly Error PortfolioTrackerSubtradeNotFound                = new Error(1736, "Subtrade was not found.");

        public static readonly Error PtWidgetNotFound                                = new Error(1740, "Widget not found.");

        // Portfolio Lite
        public static readonly Error InvalidPortfolioLitePartnerKey                  = new Error(1800, "Invalid Portfolio Lite partner key.");
        public static readonly Error InvalidPortfolioLitePartnerId                   = new Error(1801, "Invalid Portfolio Lite partner ID");
        public static readonly Error PortfolioLiteTokenNotFound                      = new Error(1802, "Portfolio Lite token is not found in the database.");
        public static readonly Error PortfolioLiteExpiredToken                       = new Error(1803, "Portfolio Lite token cannot be used twice.");
        public static readonly Error PortfolioLiteStyleAlreadyExist                  = new Error(1804, "Portfolio Lite style already exist");

        // Platform Tasks
        public static readonly Error PlatformTaskNotFound                            = new Error(1810, "Platform Task is not found.");
        public static readonly Error PlatformTaskInvalidOwner                        = new Error(1811, "User don't have access to the requested platform task.");
        public static readonly Error PlatformTaskInvalidType                         = new Error(1812, "Platform Task Id doesn't belong to the requested task type.");
        public static readonly Error PlatformTaskInvalidProgressPercent              = new Error(1813, "Platform Task progress percent value should be between 0 and 100.");

        // Checklists
        public static readonly Error ChecklistNotFound                               = new Error(1820, "This checklist doesn't exist.");
        public static readonly Error ChecklistIsNotApplicableOptionType              = new Error(1821, "This checklist is not applicable for provided Option Type.");
        public static readonly Error ChecklistCannotDeletePredefined                 = new Error(1822, "Predefined checklist cannot be deleted.");
        public static readonly Error ChecklistIsNotApplicableAssetType               = new Error(1823, "This checklist is not applicable for provided Asset Type.");
        public static readonly Error ChecklistMustHaveAssetTypeFilter                = new Error(1824, "Checklist must have Asset Type filter.");

        // Baskets
        public static readonly Error BasketInvalidOwner                              = new Error(1830, "User don't have access to the requested basket.");
        public static readonly Error BasketIsNotFound                                = new Error(1831, "This basket doesn't exist.");
        public static readonly Error UserHasNoAccessToBasket                         = new Error(1832, "User doesn't have access to basket.");
        public static readonly Error BasketCannotEditSystem                          = new Error(1833, "System basket cannot be edited.");
        public static readonly Error BasketCannotBeDeleted                           = new Error(1834, "This type of basket cannot be deleted.");
        public static readonly Error BasketAlreadyExists                             = new Error(1835, "Basket with the same name already exists.");
        public static readonly Error BasketCannotGetNonSystem                        = new Error(1836, "Cannot get non-system basket.");
    }
}