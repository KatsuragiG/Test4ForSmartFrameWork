using System;
using System.Collections.Generic;
using System.Text;

namespace TradeStops.Contracts
{
    /// <summary>
    ///  Company's industry information. GICS - Global Industry Classification Standard.
    /// </summary>
    public class CompanyGicsContract
    {
        /// <summary>
        /// Company sub industry name
        /// </summary>
        public string SubIndustryName { get; set; }

        /// <summary>
        /// Company sub industry code
        /// </summary>
        public int SubIndustryCode { get; set; }

        /// <summary>
        /// Company industry name
        /// </summary>
        public string IndustryName { get; set; }

        /// <summary>
        /// Company industry code
        /// </summary>
        public int IndustryCode { get; set; }

        /// <summary>
        /// Company industry group name
        /// </summary>
        public string IndustryGroupName { get; set; }

        /// <summary>
        /// Company industry group code
        /// </summary>
        public int IndustryGroupCode { get; set; }

        /// <summary>
        /// Company sector name
        /// </summary>
        public string SectorName { get; set; }

        /// <summary>
        /// Company sector code
        /// </summary>
        public int SectorCode { get; set; }
    }
}
