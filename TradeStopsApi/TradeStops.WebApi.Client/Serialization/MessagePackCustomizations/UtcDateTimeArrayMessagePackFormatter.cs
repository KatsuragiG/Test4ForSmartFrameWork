using System;
using MessagePack;
using MessagePack.Formatters;

// ReSharper disable once CheckNamespace - different namespace is intended to simplify copy-pasting between WebApi.Client and WebApi.Server assemblies
namespace TradeStops.Serialization.MessagePackCustomizations
{
    internal sealed class UtcDateTimeArrayMessagePackFormatter : IMessagePackFormatter<DateTime[]>
    {
        public static readonly UtcDateTimeArrayMessagePackFormatter Instance = new UtcDateTimeArrayMessagePackFormatter();

        public int Serialize(ref byte[] bytes, int offset, DateTime[] value, IFormatterResolver formatterResolver)
        {
            return NativeDateTimeArrayFormatter.Instance.Serialize(ref bytes, offset, value, formatterResolver);
        }

        public DateTime[] Deserialize(byte[] bytes, int offset, IFormatterResolver formatterResolver, out int readSize)
        {
            var result = NativeDateTimeArrayFormatter.Instance.Deserialize(bytes, offset, formatterResolver, out readSize);
            for (var i = 0; i < result.Length; i++)
            {
                if (result[i].Kind == DateTimeKind.Unspecified)
                {
                    result[i] = DateTime.SpecifyKind(result[i], DateTimeKind.Utc);
                }
            }

            return result;
        }
    }
}
