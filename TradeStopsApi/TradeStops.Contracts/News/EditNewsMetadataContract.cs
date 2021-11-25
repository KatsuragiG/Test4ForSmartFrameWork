using System;
using System.Collections.Generic;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to edit news metadata
    /// </summary>
    public class EditNewsMetadataContract
    {
        /// <summary>
        /// News Id
        /// </summary>
        public int NewsId { get; set; }     

        /// <summary>
        /// Set visible/hidden news
        /// </summary>
        public bool IsVisible { get; set; }
    }
}
