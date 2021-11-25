namespace TradeStops.Common.Constants
{
    // http://nhibernate.info/doc/nhibernate-reference/architecture.html#architecture-current-session
    public static class NhibernateSessionContextTypes
    {
        public const string Web = "web";

        public const string ThreadStatic = "thread_static";
    }
}
