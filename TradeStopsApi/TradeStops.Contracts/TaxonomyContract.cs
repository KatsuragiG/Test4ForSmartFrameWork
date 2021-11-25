using System;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Information about crypto symbol taxonomy.
    /// </summary>
    public class TaxonomyContract
    {
        /// <summary>
        /// Asset name.
        /// </summary>
        public string AssetName { get; set; }

        /// <summary>
        /// Asset description.
        /// </summary>
        public string AssetDescription { get; set; }

        /// <summary>
        /// Symbol ID.
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// One line description.
        /// </summary>
        public string Oneliner { get; set; }

        /// <summary>
        /// Symbol's ticker, like 'AAPL'.
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// Governing body.
        /// </summary>
        public string GoverningBody { get; set; }

        /// <summary>
        /// Parent chain.
        /// </summary>
        public string ParentChain { get; set; }

        /// <summary>
        /// Token standard.
        /// </summary>
        public string TokenStandard { get; set; }

        /// <summary>
        /// Codebase.
        /// </summary>
        public string Codebase { get; set; }

        /// <summary>
        /// Consensus model.
        /// </summary>
        public string ConsensusModel { get; set; }

        /// <summary>
        /// Hashing algorithm.
        /// </summary>
        public string HashingAlgorithm { get; set; }

        /// <summary>
        /// Transaction identity.
        /// </summary>
        public string TransactionIdentity { get; set; }

        /// <summary>
        /// Indicates masternode support.
        /// </summary>
        public bool? MasternodeSupport { get; set; }

        /// <summary>
        /// Contract address.
        /// </summary>
        public string ContractAddress { get; set; }

        /// <summary>
        /// Brave New Coin industry sector.
        /// </summary>
        public string BncIndustrySector { get; set; }

        /// <summary>
        /// Homepage url.
        /// </summary>
        public string HomeUrl { get; set; }

        /// <summary>
        /// Announcement url.
        /// </summary>
        public string Announcement { get; set; }

        /// <summary>
        /// Whitepaper url.
        /// </summary>
        public string Whitepaper { get; set; }

        /// <summary>
        /// Block explorer url.
        /// </summary>
        public string Explorer { get; set; }

        /// <summary>
        /// Github child repository.
        /// </summary>
        public string SourceCodeChildRepo { get; set; }

        /// <summary>
        /// Github main repository.
        /// </summary>
        public string SourceCode { get; set; }

        /// <summary>
        /// Twitter url.
        /// </summary>
        public string TwitterUrl { get; set; }

        /// <summary>
        /// Reddit url.
        /// </summary>
        public string RedditUrl { get; set; }

        /// <summary>
        /// Facebook url.
        /// </summary>
        public string FacebookUrl { get; set; }

        /// <summary>
        /// Forum url.
        /// </summary>
        public string ForumUrl { get; set; }

        /// <summary>
        /// Blog url.
        /// </summary>
        public string BlogUrl { get; set; }

        /// <summary>
        /// Telegram url.
        /// </summary>
        public string TelegramUrl { get; set; }

        /// <summary>
        /// Logo url.
        /// </summary>
        public string LogoUrl { get; set; }
    }
}
