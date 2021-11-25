using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using TradeStops.Common.Constants;

// ReSharper disable once CheckNamespace - different namespace is intended to simplify copy-pasting between WebApi.Client and WebApi.Server assemblies
namespace TradeStops.Serialization
{
    internal class NewtonsoftJsonConverter : IContentConverter
    {
        public NewtonsoftJsonConverter()
        {
        }

        public T DeserializeResponseContent<T>(HttpContent responseContent)
        {
            var responseContentString = responseContent.ReadAsStringAsync().Result;

            return  DeserializeJson<T>(responseContentString);
        }

        public HttpContent SerializeRequestContent<T>(T obj)
        {
            return new StringContent(JsonConvert.SerializeObject(obj, new StringEnumConverter()), Encoding.UTF8, MimeTypes.Json);
        }

        public HttpContent SerializeRequestContent(Type type, object obj)
        {
            return new StringContent(JsonConvert.SerializeObject(obj, new StringEnumConverter()), Encoding.UTF8, MimeTypes.Json);
        }

        private T DeserializeJson<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, new StringEnumConverter());
        }
    }
}