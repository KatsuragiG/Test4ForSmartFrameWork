using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MessagePack;
using TradeStops.Common.Constants;
using TradeStops.Serialization.MessagePackCustomizations;

// ReSharper disable once CheckNamespace - different namespace is intended to simplify copy-pasting between WebApi.Client and WebApi.Server assemblies
namespace TradeStops.Serialization
{
    // https://github.com/sketch7/MessagePack.MediaTypeFormatter
    public class MessagePackFormatter : MediaTypeFormatter
    {
        private readonly IFormatterResolver _resolver = CustomCompositeFormatterResolver.Instance;

        public MessagePackFormatter()
        {
            var mediaType = new MediaTypeHeaderValue(MimeTypes.MessagePack);

            SupportedMediaTypes.Add(mediaType);
        }

        public override bool CanReadType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return IsAllowedType(type);
        }

        public override bool CanWriteType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return IsAllowedType(type);
        }

        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            var result = MessagePackSerializer.NonGeneric.Deserialize(type, readStream, _resolver);
            return Task.FromResult(result);
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext)
        {
            MessagePackSerializer.NonGeneric.Serialize(type, writeStream, value, _resolver);
            return Task.CompletedTask;
        }

        private static bool IsAllowedType(Type t)
        {
            return t != null;

            ////if (t != null && !t.IsAbstract && !t.IsInterface && !t.IsNotPublic)
            ////{
            ////    return true;
            ////}

            ////if (typeof(IEnumerable<>).IsAssignableFrom(t))
            ////{
            ////    return true;
            ////}

            ////return false;
        }
    }
}