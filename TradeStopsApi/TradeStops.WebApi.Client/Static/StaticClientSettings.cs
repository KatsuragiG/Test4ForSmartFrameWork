////#if NET47
////using System;
////using System.Configuration;
////using TradeStops.WebApi.Client.Configuration;

////namespace TradeStops.WebApi.Client.Static
////{
////    public static class StaticClientSettings
////    {
////        private static readonly Lazy<IClientSettings> Lazy = new Lazy<IClientSettings>
////            (() => ConfigurationManager.GetSection(nameof(ClientSettings)) as ClientSettings);

////        private static IClientSettings LazyInitialized => Lazy.Value;

////        private static IClientSettings _manuallyInitialized = null;

////        /// <summary>
////        /// By default, API ClientSettings instance is lazy-initialized with config section 'ClientSettings' in application config file.
////        /// To use other instance of clientSettings, you can manually initialize it in InitSettings method
////        /// </summary>
////        public static IClientSettings Instance = _manuallyInitialized ?? LazyInitialized;

////        public static void InitSettings(IClientSettings settings)
////        {
////            _manuallyInitialized = settings;
////        }
////    }
////}
////#endif