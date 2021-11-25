using System;
using MessagePack;
using MessagePack.Formatters;

// ReSharper disable once CheckNamespace - different namespace is intended to simplify copy-pasting between WebApi.Client and WebApi.Server assemblies
namespace TradeStops.Serialization.MessagePackCustomizations
{
    internal class UtcDateTimeMessagePackFormatter : IMessagePackFormatter<DateTime>
    {
        public static readonly UtcDateTimeMessagePackFormatter Instance = new UtcDateTimeMessagePackFormatter();

        public int Serialize(ref byte[] bytes, int offset, DateTime value, IFormatterResolver formatterResolver)
        {
            return NativeDateTimeFormatter.Instance.Serialize(ref bytes, offset, value, formatterResolver);
        }

        public DateTime Deserialize(byte[] bytes, int offset, IFormatterResolver formatterResolver, out int readSize)
        {
            var result = NativeDateTimeFormatter.Instance.Deserialize(bytes, offset, formatterResolver, out readSize);

            if (result.Kind == DateTimeKind.Unspecified)
            {
                result = DateTime.SpecifyKind(result, DateTimeKind.Utc);
            }

            return result;
        }
    }
}
