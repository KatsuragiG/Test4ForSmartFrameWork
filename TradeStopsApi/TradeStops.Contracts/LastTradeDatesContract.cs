using System;
using System.Collections.Generic;
using System.Text;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Information about latest available trade dates in the system
    /// </summary>
    public class LastTradeDatesContract
    {
        /// <summary>
        /// Date for USA close prices
        /// </summary>
        public DateTime UnitedStatesLastTradeDate { get; set; }

        /// <summary>
        /// Date for Canada close prices
        /// </summary>
        public DateTime CanadaLastTradeDate { get; set; }

        /// <summary>
        /// Date for London close prices
        /// </summary>
        public DateTime LondonLastTradeDate { get; set; }

        /// <summary>
        /// Date for Australian close prices
        /// </summary>
        public DateTime AustralianLastTradeDate { get; set; }

        /// <summary>
        /// Date for German close prices
        /// </summary>
        public DateTime GermanLastTradeDate { get; set; }

        /// <summary>
        /// Date for option close prices
        /// </summary>
        public DateTime OptionsLastTradeDate { get; set; }

        /// <summary>
        /// Date for Crypto-currencies close prices
        /// Note: Contains actual value only for CryptoStops API
        /// </summary>
        public DateTime CryptoCurrenciesLastTradeDate { get; set; }

        /// <summary>
        /// Date for Crypto-currencies intraday prices.
        /// Note: Contains actual value only for CryptoStops API
        /// </summary>
        public DateTime CryptoCurrenciesLastIntradayDate { get; set; }
    }
}
