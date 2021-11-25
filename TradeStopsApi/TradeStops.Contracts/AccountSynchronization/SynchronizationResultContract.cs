using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Contract with result from account synchronization process.
    /// </summary>
    public class SynchronizationResultContract
    {
        /// <summary>
        /// Default empty constructor
        /// </summary>
        public SynchronizationResultContract()
        {
        }

        /// <summary>
        /// Equals true when all the following conditions are true:
        /// 1) User in TradeSmith database was found and updated or created.
        /// 2) User account was found in remote CRM.
        /// 3) There are no exceptions during the synchronization.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Indicates whether remote CRM (either Agora or Stansberry) API failed during synchronization.
        /// </summary>
        public bool IsCrmApiFailed { get; set; }

        /// <summary>
        /// Indicates remote CRM that was used to synchronize user. It can be either Agora or Stansberry
        /// </summary>
        public SyncronizationSources SyncSource { get; set;  }

        /// <summary>
        /// TradeSmith user that was found/updated/created during synchronization.
        /// Equals null if there was an unexpected exception.
        /// </summary>
        public TradeSmithUserContract TradeSmithUser { get; set; }
    }
}