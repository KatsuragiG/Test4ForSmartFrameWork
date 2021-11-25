namespace TradeStops.Common.Enums
{
    // consider to rename PartnerKeys to [Api]Partners 
    // todo: PartnerKeyTypes to [Api]Partner[Key]Groups
    // todo: add ApiOperationGroups and simplify ApiOperationsMerge script
    public enum PartnerKeyTypes
    {
        Development = 1,
        Website = 2,
        ConsoleApp = 3,
        MobileApp = 4,
        PortfolioTracker = 5,
        Other = 10,
        ThirdParty = 20,
        Obsolete = 30,
    }
}
