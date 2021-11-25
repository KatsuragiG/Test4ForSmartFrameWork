using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading.Tasks;
using TradeStops.Common.Constants;
using TradeStops.Common.Exceptions;
using TradeStops.Contracts;
using TradeStops.Serialization;
using TradeStops.WebApi.Client.Version2;

namespace TradeStops.WebApi.Client.Helpers
{
    internal static class PerformRequestHelper
    {
        internal static TResult PerformRequest<TResult>(HttpClient client, RequestData data)
        {
            try
            {
                var request = CreateRequest(data);
                return SendRequest<TResult>(client, request);
            }
            catch (InternalServerErrorException ex1)
            {
                data.ContentType = MimeTypes.Json;
                var request = CreateRequest(data);
                try
                {
                    return SendRequest<TResult>(client, request);
                }
                catch (Exception ex2)
                {
                    throw new AggregateException(ex1, ex2);
                }
            }
        }

        internal static async Task<TResult> PerformRequestAsync<TResult>(HttpClient client, RequestData data)
        {
            try
            {
                var request = CreateRequest(data);
                return await SendRequestAsync<TResult>(client, request);
            }
            catch (InternalServerErrorException ex1)
            {
                data.ContentType = MimeTypes.Json;
                var request = CreateRequest(data);

                try
                {
                    return await SendRequestAsync<TResult>(client, request);
                }
                catch (Exception ex2)
                {
                    throw new AggregateException(ex1, ex2);
                }
            }
        }

        internal static void PerformRequest(HttpClient client, RequestData data)
        {
            try
            {
                var request = CreateRequest(data);
                SendRequest(client, request);
            }
            catch (InternalServerErrorException ex1)
            {
                data.ContentType = MimeTypes.Json;
                var request = CreateRequest(data);

                try
                {
                    SendRequest(client, request);
                }
                catch (Exception ex2)
                {
                    throw new AggregateException(ex1, ex2);
                }
            }
        }

        internal static async Task PerformRequestAsync(HttpClient client, RequestData data)
        {
            try
            {
                var request = CreateRequest(data);
                await SendRequestAsync(client, request);
            }
            catch (InternalServerErrorException ex1)
            {
                data.ContentType = MimeTypes.Json;
                var request = CreateRequest(data);

                try
                {
                    await SendRequestAsync(client, request);
                }
                catch (Exception ex2)
                {
                    throw new AggregateException(ex1, ex2);
                }
            }
        }

        internal static async Task<T> SendRequestAsync<T>(HttpClient client, HttpRequestMessage request)
        {
            using (var response = await client.SendAsync(request))
            {
                EnsureSuccessResponse(response);

                var converter = GetConverter(response.Content.Headers.ContentType.MediaType);

                var result = converter.DeserializeResponseContent<T>(response.Content);

                return result;
            }
        }

        internal static T SendRequest<T>(HttpClient client, HttpRequestMessage request)
        {
            using (var response = Task.Run(() => client.SendAsync(request)).Result)
            {
                EnsureSuccessResponse(response);

                var converter = GetConverter(response.Content.Headers.ContentType.MediaType);

                var result = converter.DeserializeResponseContent<T>(response.Content);

                return result;
            }
        }

        internal static async Task SendRequestAsync(HttpClient client, HttpRequestMessage request)
        {
            using (var response = await client.SendAsync(request))
            {
                EnsureSuccessResponse(response);
            }
        }

        internal static void SendRequest(HttpClient client, HttpRequestMessage request)
        {
            using (var response = Task.Run(() => client.SendAsync(request)).Result)
            {
                EnsureSuccessResponse(response);
            }
        }

        [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "TODO: It's a good idea to fix this suppression.")]
        internal static void EnsureSuccessResponse(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return;
            }

            ErrorContract error;
            try
            {
                var converter = GetConverter(response.Content.Headers.ContentType.MediaType);

                error = converter.DeserializeResponseContent<ErrorContract>(response.Content);
            }
            catch
            {
                error = null; // API can return empty response or error page with html instead of ErrorContract
            }

            if (error == null || error.ErrorCode == 0 && error.ErrorMessage == null)
            {
                throw new InternalServerErrorException($"Unsuccessful request to API: Error {(int)response.StatusCode} - {response.ReasonPhrase}");
            }

            if (error.ErrorCode == Errors.InternalServerError.Code && error.ErrorMessage != null)
            {
                throw new InternalServerErrorException(error.ErrorMessage);
            }

            throw new BadRequestException(error.ErrorCode, error.ErrorMessage);
        }

        // static dependencies
        private static HttpRequestMessage CreateRequest(RequestData data) => CreateRequestHelper.CreateRequest(data);
        private static IContentConverter GetConverter(string mimeType) => ConvertersHelper.GetConverter(mimeType);
    }
}