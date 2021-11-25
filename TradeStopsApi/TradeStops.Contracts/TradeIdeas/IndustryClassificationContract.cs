namespace TradeStops.Contracts
{
    /// <summary>
    /// Industry classification
    /// </summary>
    public class IndustryClassificationContract
    {
        /// <summary>
        /// ID of the sector
        /// </summary>
        public int SectorId { get; set; }

        /// <summary>
        /// The name of Sector
        /// </summary>
        public string Sector { get; set; }

        /// <summary>
        /// ID of the industry
        /// </summary>
        public int IndustryId { get; set; }

        /// <summary>
        /// The name of industry
        /// </summary>
        public string Industry { get; set; }

        /// <summary>
        /// ID of the subsector
        /// </summary>
        public int SubSectorId { get; set; }

        /// <summary>
        /// The name of subsector
        /// </summary>
        public string SubSector { get; set; }

        /// <summary>
        /// ID of the subindustry
        /// </summary>
        public int SubIndustryId { get; set; }

        /// <summary>
        /// The name of subindustry
        /// </summary>
        public string SubIndustry { get; set; }
    }
}
