////using System.Collections.Generic;
////using System.Net.Http;
////using System.Net.Http.Formatting;

////namespace TradeStops.WebApi.Client.JsonFormatting
////{
////    internal class ProtobufConverter : IDefaultJsonConverter
////    {
////        private readonly List<MediaTypeFormatter> _formatters;

////        protected MediaTypeFormatter DefaultFormatter => _formatters[0];

////        public ProtobufConverter()
////        {
////            _formatters = new List<MediaTypeFormatter> { new ProtobufFormatter() };
////        }

////        public T DeserializeResponseContent<T>(HttpContent responseContent)
////        {
////            return responseContent.ReadAsAsync<T>(_formatters).Result;
////        }

////        public HttpContent SerializeRequestContent<T>(T obj)
////        {
////            return new ObjectContent<T>(obj, DefaultFormatter);
////        }
////    }
////}