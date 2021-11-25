using TradeStops.Common.Enums;

namespace TradeStops.Common.Helpers
{
    public static class SyncVendorAccountHelper
    {
        public static SyncProcessType GetServiceProcessType(SynchronizationActionTypes? synchronizationActionType, SyncProcessType defaultValue)
        {
            switch (synchronizationActionType)
            {
                case SynchronizationActionTypes.Refresh:
                case SynchronizationActionTypes.RefreshAll:
                    return SyncProcessType.Manual;
                case SynchronizationActionTypes.Restore:
                case SynchronizationActionTypes.RestoreAll:
                    return SyncProcessType.Restore;
                case SynchronizationActionTypes.UpdateCredentials:
                    return SyncProcessType.Credentials;
                case SynchronizationActionTypes.ImportMissing:
                    return SyncProcessType.Missing;
                default:
                    return defaultValue;
            }
        }
    }
}
