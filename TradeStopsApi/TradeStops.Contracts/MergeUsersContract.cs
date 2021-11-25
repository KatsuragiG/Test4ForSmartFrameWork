namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to merge users
    /// </summary>
    public class MergeUsersContract
    {
        /// <summary>
        /// ID of the user to Keep in the database
        /// </summary>
        public int TradeSmithUserIdToKeep { get; set; }

        /// <summary>
        /// ID of the second user to merge. The older (min) ID will survive in result of merge
        /// </summary>
        public int TradeSmithUserIdToDelete { get; set; }

        /// <summary>
        /// Primary email that must survive in result of merge.
        /// </summary>
        public string PrimaryEmail { get; set; }

        /// <summary>
        /// The SNAID that must survive in result of merge. Can be null.
        /// </summary>
        public string Snaid { get; set; }

        /// <summary>
        /// The Agora customer number that must survive in result of merge. Can be null.
        /// </summary>
        public string AgoraCustomerNumber { get; set; }

        /// <summary>
        /// Indicates whether Portfolios data from the deleted user must be kept for survived user
        /// </summary>
        public bool KeepPortfoliosFromDeletedUser { get; set; }
    }
}
