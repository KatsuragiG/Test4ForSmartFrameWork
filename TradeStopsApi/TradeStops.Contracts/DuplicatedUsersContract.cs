using System.Collections.Generic;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Indicates the pair of users with duplication by SNAID, ACN or Email/Username
    /// </summary>
    public class DuplicatedUsersContract
    {
        /// <summary>
        /// First user with duplication
        /// </summary>
        public TradeSmithUserContract First { get; set; }

        /// <summary>
        /// Second user with duplication
        /// </summary>
        public TradeSmithUserContract Second { get; set; }
    }
}
