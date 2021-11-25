using MessagePack;
using MessagePack.Formatters;
using MessagePack.Resolvers;

// ReSharper disable once CheckNamespace - different namespace is intended to simplify copy-pasting between WebApi.Client and WebApi.Server assemblies
namespace TradeStops.Serialization.MessagePackCustomizations
{
    // source: https://github.com/neuecc/MessagePack-CSharp/blob/master/README.md
    // original code modified to meet codeanalysis rules
    internal class CustomCompositeFormatterResolver : IFormatterResolver
    {
        private static readonly IFormatterResolver _instance = new CustomCompositeFormatterResolver();

        private static readonly IFormatterResolver[] _resolvers = new[]
        {
            UtcDateTimeFormatterResolver.Instance,
            ContractlessStandardResolver.Instance,
        };

        private CustomCompositeFormatterResolver()
        {
        }

        public static IFormatterResolver Instance => _instance;

        public IMessagePackFormatter<T> GetFormatter<T>()
        {
            return FormatterCache<T>.Formatter;
        }

        private static class FormatterCache<T>
        {
            public static readonly IMessagePackFormatter<T> Formatter = InitFormatter();

            // static ctor was changed to static method to satisfy CA1810 rule
            private static IMessagePackFormatter<T> InitFormatter()
            {
                foreach (var item in _resolvers)
                {
                    var f = item.GetFormatter<T>();
                    if (f != null)
                    {
                        return f;
                    }
                }

                return null;
            }
        }
    }
}
