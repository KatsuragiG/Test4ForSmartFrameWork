using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;

// ReSharper disable once CheckNamespace - different namespace is intended to simplify copy-pasting between WebApi.Client and WebApi.Server assemblies
namespace TradeStops.Serialization
{
    internal class MessagePackConverter : IContentConverter
    {
        private readonly List<MediaTypeFormatter> _formatters;

        public MessagePackConverter()
        {
            _formatters = new List<MediaTypeFormatter> { new MessagePackFormatter() };
        }

        protected MediaTypeFormatter DefaultFormatter => _formatters[0];

        public T DeserializeResponseContent<T>(HttpContent responseContent)
        {
            return responseContent.ReadAsAsync<T>(_formatters).Result;
        }

        public HttpContent SerializeRequestContent<T>(T obj)
        {
            return new ObjectContent<T>(obj, DefaultFormatter);
        }

        public HttpContent SerializeRequestContent(Type type, object obj)
        {
            return new ObjectContent(type, obj, DefaultFormatter);
        }
    }
}
