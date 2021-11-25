namespace TradeStops.Contracts
{
    /// <summary>
    /// News content (full article)
    /// </summary>
    public class NewsContentContract
    {
        /// <summary>
        /// News ID
        /// </summary>
        public int NewsId { get; set; }

        /// <summary>
        /// Content of the article
        /// </summary>
        public string Content { get; set; }
    }
}
