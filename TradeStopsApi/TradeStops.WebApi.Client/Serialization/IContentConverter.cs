using System;
using System.Net.Http;

// ReSharper disable once CheckNamespace - different namespace is intended to simplify copy-pasting between WebApi.Client and WebApi.Server assemblies
namespace TradeStops.Serialization
{
    internal interface IContentConverter
    {
        T DeserializeResponseContent<T>(HttpContent responseContent);
        HttpContent SerializeRequestContent<T>(T obj);
        HttpContent SerializeRequestContent(Type type, object obj);
    }
}