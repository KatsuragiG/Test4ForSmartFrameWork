using System;

namespace TradeStops.Common.Constants
{
    [Obsolete("Constants in this class are obsolete. Use TriggerTypes enum instead")]
    public static class TriggerTypeNames
    {
        public const string OptimalTrailingStop = "trailingStopsOptimal";
        public const string PercentTrailingStop = "trailingStopsPercent";
        public const string SmartTrailingStops2 = "smartTrailingStops2"; // todo : we don't have positiontriggers and alerttemplatettriggertypes with this alert (triggertypeid = 48)
    }
}
