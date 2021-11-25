using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Jil;
using TradeStops.Common.Constants;

// ReSharper disable once CheckNamespace - different namespace is intended to simplify copy-pasting between WebApi.Client and WebApi.Server assemblies
namespace TradeStops.Serialization
{
    public class JilJsonFormatter : MediaTypeFormatter
    {
        private readonly Options _jilOptions;

        public JilJsonFormatter()
        {
            _jilOptions = new Options(dateFormat: DateTimeFormat.ISO8601, unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC, includeInherited: true);
            SupportedEncodings.Add(new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true));
            SupportedEncodings.Add(new UnicodeEncoding(bigEndian: false, byteOrderMark: true, throwOnInvalidBytes: true));

            SupportedMediaTypes.Add(new MediaTypeHeaderValue(MimeTypes.Json));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(MimeTypes.TextJson));
        }

        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            var effectiveEncoding = SelectCharacterEncoding(content?.Headers);
            object result = null;

            using (var reader = new StreamReader(readStream, effectiveEncoding))
            {
                result = JSON.Deserialize(reader, type, _jilOptions);
            }

            return Task.FromResult(result);
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext)
        {
            var effectiveEncoding = SelectCharacterEncoding(content?.Headers);

            using (var streamWriter = new StreamWriter(writeStream, effectiveEncoding, 1024, true))
            {
                JSON.Serialize(value, streamWriter, _jilOptions);
                streamWriter.Flush();
            }

            return Task.FromResult(writeStream);
        }

        public override bool CanReadType(Type type)
        {
            return type != null;
        }

        public override bool CanWriteType(Type type)
        {
            return type != null;
        }
    }
}