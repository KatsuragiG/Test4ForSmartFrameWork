using System;
using System.Collections.Generic;
using System.Text;

namespace TradeStops.Common.Enums
{
    // We have to much different position types on the UI so it became difficult to find good name for this enum. PositionStatusTypes and PositionTypes are alreasy exist.
    public enum PositionStatusFilterTypes : byte 
    {
        ExpiringSoon = 1,
        Delisted = 2,
        Expired = 3,
        NewlySynched = 4
    }
}
