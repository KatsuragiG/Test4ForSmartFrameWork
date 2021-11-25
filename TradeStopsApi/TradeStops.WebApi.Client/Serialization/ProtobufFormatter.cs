////using System;
////using System.Diagnostics;
////using System.IO;
////using System.Linq;
////using System.Net;
////using System.Net.Http;
////using System.Net.Http.Formatting;
////using System.Net.Http.Headers;
////using System.Reflection;
////using System.Threading.Tasks;
////using ProtoBuf;
////using ProtoBuf.Meta;
////using TradeStops.Contracts;
////using TradeStops.Logging;

////namespace TradeStops.WebApi.Client.JsonFormatting
////{
////    public class ProtobufFormatter : MediaTypeFormatter
////    {
////        private static readonly MediaTypeHeaderValue mediaType = new MediaTypeHeaderValue(MimeTypes.ApplicationProtobuf);
////        private static Lazy<RuntimeTypeModel> model = new Lazy<RuntimeTypeModel>(CreateTypeModel);

////        private ILogger _logger = LoggerFactory.GetLogger(typeof(ProtobufFormatter));

////        public ProtobufFormatter()
////        {
////            SupportedMediaTypes.Add(mediaType);
////        }

////        public static RuntimeTypeModel Model
////        {
////            get { return model.Value; }
////        }

////        public static MediaTypeHeaderValue DefaultMediaType
////        {
////            get { return mediaType; }
////        }

////        public override bool CanReadType(Type type)
////        {
////            return CanReadTypeCore(type);
////        }

////        public override bool CanWriteType(Type type)
////        {
////            return CanReadTypeCore(type);
////        }

////        public override Task<object> ReadFromStreamAsync(Type type, Stream stream, HttpContent content, IFormatterLogger formatterLogger)
////        {
////            var tcs = new TaskCompletionSource<object>();

////            try
////            {
////                var stopwatch = new Stopwatch();
////                stopwatch.Start();
////                object result = Model.Deserialize(stream, null, type);
////                stopwatch.Stop();
////                _logger.Info($"protobuf deserialization elapsed ms: {stopwatch.ElapsedMilliseconds}");
////                _logger.Info($"protobuf stream length: {stream.Length}");
////                tcs.SetResult(result);
////            }
////            catch (Exception ex)
////            {
////                tcs.SetException(ex);
////            }

////            return tcs.Task;
////        }

////        public override Task WriteToStreamAsync(Type type, object value, Stream stream, HttpContent content, TransportContext transportContext)
////        {
////            var tcs = new TaskCompletionSource<object>();

////            try
////            {
////                var stopwatch = new Stopwatch();
////                stopwatch.Start();
////                Model.Serialize(stream, value);
////                stopwatch.Stop();
////                _logger.Info($"protobuf serialization elapsed ms: {stopwatch.ElapsedMilliseconds}");
////                _logger.Info($"protobuf stream length: {stream.Length}");
////                tcs.SetResult(null);
////            }
////            catch (Exception ex)
////            {
////                tcs.SetException(ex);
////            }

////            return tcs.Task;
////        }

////        private static RuntimeTypeModel CreateTypeModel()
////        {
////            var typeModel = TypeModel.Create();
////            typeModel.AutoAddMissingTypes = true;

////            var allContracts = typeof(SymbolContract).Assembly.GetExportedTypes();

////            foreach (var contract in allContracts)
////            {
////                typeModel.Add(contract, true);
////                var properties = contract.GetProperties().Select(x => x.Name).ToArray();
////                typeModel[contract].Add(properties);
////            }

////            return typeModel;
////        }

////        private static bool CanReadTypeCore(Type type)
////        {
////            return true;
////            ////return type.GetCustomAttributes(typeof(ProtoContractAttribute)).Any();
////        }
////    }
////}