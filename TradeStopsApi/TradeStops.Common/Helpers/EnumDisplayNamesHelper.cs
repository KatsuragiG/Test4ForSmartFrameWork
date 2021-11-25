using System;
using TradeStops.Common.Enums;

namespace TradeStops.Common.Helpers
{
    // Note: Using not localized text or DisplayAttribute on enum values is not welcomed. Consider to use localized values in your project.
    // In future when we decide to localize this file it will be enough to put all strings to LocalizableStrings.cs instead of using magic LocalizableEnums resource file or whatewer
    public static class EnumDisplayNamesHelper
    {
        public static string Get(SearchUserFields item)
        {
            switch (item)
            {
                case SearchUserFields.PrimaryEmail:
                    return "Primary Email";
                case SearchUserFields.SecondaryEmail:
                    return "Secondary Email";
                case SearchUserFields.Snaid:
                    return "SNAID";
                case SearchUserFields.AgoraCustomerNumber:
                    return "Agora Customer Number";
                case SearchUserFields.LastName:
                    return "Last Name";
                case SearchUserFields.PhoneNumber:
                    return "Phone Number";
                default:
                    throw new ArgumentOutOfRangeException(nameof(item), item, null);
            }
        }

        public static string Get(SyncProcessType item)
        {
            switch (item)
            {
                case SyncProcessType.All:
                    return "All";
                case SyncProcessType.Service:
                    return "Service";
                case SyncProcessType.Manual:
                    return "Website";
                case SyncProcessType.Restore:
                    return "Restore Portfolios";
                case SyncProcessType.Missing:
                    return "Missing Portfolios";
                case SyncProcessType.Credentials:
                    return "Credentials";
                case SyncProcessType.PortfolioAnalyzer:
                    return "Portfolio Analyzer";
                default:
                    throw new ArgumentOutOfRangeException(nameof(item), item, null);
            }
        }

        public static string Get(PeriodTypes item)
        {
            switch (item)
            {
                case PeriodTypes.Day:
                    return "Day";
                case PeriodTypes.Week:
                    return "Week";
                case PeriodTypes.Month:
                    return "Month";
                default:
                    throw new ArgumentOutOfRangeException(nameof(item), item, null);
            }
        }

        public static string Get(PortfolioTypes item)
        {
            switch (item)
            {
                case PortfolioTypes.Investment:
                    return "Investment";
                case PortfolioTypes.WatchOnly:
                    return "Watch Only";
                default:
                    throw new ArgumentOutOfRangeException(nameof(item), item, null);
            }
        }

        public static string Get(PriceTypes item)
        {
            switch (item)
            {
                case PriceTypes.Open:
                    return "Open";
                case PriceTypes.High:
                    return "High";
                case PriceTypes.Low:
                    return "Low";
                case PriceTypes.Close:
                    return "Close";
                default:
                    throw new ArgumentOutOfRangeException(nameof(item), item, null);
            }
        }

        public static string Get(SystemEventCategories item)
        {
            switch (item)
            {
                case SystemEventCategories.PurchaseOpen:
                    return "Purchase (open)";
                case SystemEventCategories.SellClose:
                    return "Sell (close)";
                case SystemEventCategories.AlertTriggered:
                    return "Alert triggered";
                case SystemEventCategories.AlertCancelled:
                    return "Alert cancelled";
                case SystemEventCategories.SplitIssued:
                    return "Split issued";
                case SystemEventCategories.DividendIssued:
                    return "Dividend issued";
                case SystemEventCategories.PartialSellClose:
                    return "Partial sell (close)";
                case SystemEventCategories.DeletePosition:
                    return "Delete position";
                case SystemEventCategories.AlertCreated:
                    return "Alert created";
                case SystemEventCategories.AlertEdited:
                    return "Alert edited";
                case SystemEventCategories.PortfolioCreated:
                    return "Portfolio Created";
                case SystemEventCategories.PortfolioDeleted:
                    return "Portfolio Deleted";
                case SystemEventCategories.SpinoffIssued:
                    return "Spinoff issued";
                case SystemEventCategories.StockDistributionIssued:
                    return "Stock distribution issued";
                case SystemEventCategories.MovePosition:
                    return "Move position";
                case SystemEventCategories.PortfolioEdited:
                    return "Portfolio Edited";
                case SystemEventCategories.CopyPosition:
                    return "Copy position";
                case SystemEventCategories.PositionEdited:
                    return "Position Edited";
                default:
                    throw new ArgumentOutOfRangeException(nameof(item), item, null);
            }
        }

        public static string Get(TargetColumnNames item)
        {
            switch (item)
            {
                case TargetColumnNames.MarketCap:
                    return "Market Cap";
                case TargetColumnNames.EnterpriseValueAndFQ:
                    return "Enterprise Value";
                case TargetColumnNames.EnterpriseValueToRevenue:
                    return "Enterprise Value/Revenue";
                case TargetColumnNames.EnterpriseValueEBITDAndTTM:
                    return "Enterprise Value/EBITDA";
                case TargetColumnNames.PriceBookAndFQ:
                    return "Price/Book";
                case TargetColumnNames.PriceEarningsAndTTM:
                    return "Price/Earnings";
                case TargetColumnNames.PriceEarningsToGrowthAndTTM:
                    return "PEG";
                default:
                    throw new ArgumentOutOfRangeException(nameof(item), item, null);
            }
        }

        public static string Get(TriggerOperationTypes item)
        {
            // not sure what is correct text to display
            return item.ToString();

            ////switch (item)
            ////{
            ////    case TriggerOperationTypes.None:
            ////        return "";
            ////    case TriggerOperationTypes.Less:
            ////        return "";
            ////    case TriggerOperationTypes.LessOrEqual:
            ////        return "";
            ////    case TriggerOperationTypes.Equal:
            ////        return "";
            ////    case TriggerOperationTypes.Greater:
            ////        return "";
            ////    case TriggerOperationTypes.GreaterOrEqual:
            ////        return "";
            ////    case TriggerOperationTypes.ModuleDeletion:
            ////        return "";
            ////    default:
            ////        throw new ArgumentOutOfRangeException(nameof(item), item, null);
            ////}
        }

        public static string Get(PublisherTypes item)
        {
            switch (item)
            {
                case PublisherTypes.Stansberry: return "Stansberry Research";
                case PublisherTypes.CommonSensePublishing: return "Palm Beach Research Group";
                case PublisherTypes.OxfordClub: return "The Oxford Club";
                case PublisherTypes.BanyanHill: return "Banyan Hill";
                case PublisherTypes.BonnerAndPartners: return "Rogue Economics";
                case PublisherTypes.Southbank: return "Southbank Investment Research";
                case PublisherTypes.TradeStopsBillionairePortfolios: return "Billionaires Club";
                case PublisherTypes.Casey: return "Casey Research";
                case PublisherTypes.JeffClark: return "Jeff Clark";
                case PublisherTypes.InvestorPlaceMedia: return "InvestorPlace Media";
                case PublisherTypes.TradeStopsLikeFolio: return "LikeFolio";
                case PublisherTypes.EmpireFinancialResearch: return "Empire Financial Research";
                case PublisherTypes.PortPhillipPublishing: return "Port Phillip Publishing";
                case PublisherTypes.FatTailMedia: return "Fat Tail Media";
                case PublisherTypes.BrownstoneResearch: return "Brownstone Research";
                case PublisherTypes.Decoder: return "Decoder";
                case PublisherTypes.Pt2TradeSmith: return "TradeSmith";
                case PublisherTypes.Pt2Stansberry: return "Stansberry";
                case PublisherTypes.ParadigmPress: return "Paradigm Press";
                case PublisherTypes.ThreeFounders: return "Three Founders Publishing";
                case PublisherTypes.StPaulResearch: return "St. Paul Research";
                case PublisherTypes.Pt2TsOptions: return "TS Options";
                case PublisherTypes.Pt2TsPlatinum: return "TS Platinum";
                case PublisherTypes.Pt2Test: return "Test Organization";
                case PublisherTypes.TestPublisher: return "Test Publisher";

                default:
                    throw new ArgumentOutOfRangeException(nameof(item), item, null);
            }
        }

        public static string Get(NewslettersPublisherSources item)
        {
            switch (item)
            {
                case NewslettersPublisherSources.TradeStops: return "TradeSmith";
                case NewslettersPublisherSources.PortfolioTracker: return "Newsletters";
                case NewslettersPublisherSources.PortfolioTracker2: return "Newsletters";

                default:
                    throw new ArgumentOutOfRangeException(nameof(item), item, null);
            }
        }

        public static string Get(PublishersStopPriorityTypes item)
        {
            switch (item)
            {
                case PublishersStopPriorityTypes.TrailingStop:
                    return "% Trailing Stop";
                case PublishersStopPriorityTypes.CloseAboveStop:
                    return "Close Above";
                case PublishersStopPriorityTypes.CloseBelowStop:
                    return "Close Below";
                case PublishersStopPriorityTypes.HardStop:
                    return "% Hard Stop";
                case PublishersStopPriorityTypes.SmartTrailingStop:
                    return "% VQ";
                case PublishersStopPriorityTypes.TSMinusDividendStop:
                    return "% TS Minus Dividend";
                default:
                    throw new ArgumentOutOfRangeException(nameof(item), item, null);
            }
        }
    }
}
