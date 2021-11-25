namespace TradeStops.Common.Enums
{
    // ex-PublishingSources enum
    // It is the source where subscription (pub-code) to newsletters portfolio exists.
    // It allows as to separate subscriptions when pub-code is the same in both systems.
    // It also determines in which CRM to look for the subscription.
    public enum NewslettersSubscriptionSources
    {
        Stansberry = 1,

        Agora = 2
    }
}
