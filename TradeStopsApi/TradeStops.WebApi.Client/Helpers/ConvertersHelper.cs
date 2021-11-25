using System;
using TradeStops.Common.Constants;
using TradeStops.Serialization;

namespace TradeStops.WebApi.Client.Helpers
{
    internal static class ConvertersHelper
    {
        private static readonly IContentConverter MessagePackConverter = new MessagePackConverter();
        private static readonly IContentConverter JsonConverter = new JilJsonConverter();

        public static IContentConverter GetConverter(string mimeType)
        {
            if (string.Compare(mimeType, MimeTypes.MessagePack, StringComparison.OrdinalIgnoreCase) == 0)
            {
                return MessagePackConverter;
            }

            return JsonConverter;
        }
    }
}
