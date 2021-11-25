using System;
using System.Net.Http;
using System.Net.Http.Headers;
using TradeStops.Common.Enums;
using TradeStops.Contracts;
using TradeStops.WebApi.Client.Version2;

namespace TradeStops.WebApi.Client.Helpers
{
    internal static class CreateRequestHelper
    {
        private const string LicenseKeyHeader = "LicenseKey";
        private const string UserContextHeader = "UserContext";
        private const string UserIdHeader = "UserId";
        private const string UserGuidHeader = "UserGuid";
        private const string OrganizationIdHeader = "OrganizationId";

        public static HttpRequestMessage CreateRequest(RequestData data)
        {
            var request = new HttpRequestMessage(data.Method, data.Url);

            AddAcceptTypeHeader(request, data.ContentType);

            ////// for new API clients LicenseKey is added to client.DefaultHeaders, but for compatibility with old clients, we set it there
            ////if (data.LicenseKey != null)
            ////{
            ////    AddLicenseKeyHeader(request, data.LicenseKey);
            ////}

            if (data.UserContext != null)
            {
                AddUserContextHeader(request, data.UserContext);
            }

            if (data.UserId != null)
            {
                AddUserIdHeader(request, data.UserId.Value);
            }

            if (data.UserGuid != null)
            {
                AddUserGuidHeader(request, data.UserGuid.Value);
            }

            if (data.OrganizationId != null)
            {
                AddOrganizationIdHeader(request, data.OrganizationId.Value);
            }

            if (data.Body != null)
            {
                AddRequestBody(request, data.Body, data.BodyType, data.ContentType);
            }

            return request;
        }

        private static void AddRequestBody(HttpRequestMessage request, object body, Type bodyType, string mimeType)
        {
            var converter = ConvertersHelper.GetConverter(mimeType);

            request.Content = converter.SerializeRequestContent(bodyType, body);
        }

        private static void AddAcceptTypeHeader(HttpRequestMessage request, string mimeType)
        {
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mimeType));
        }

        private static void AddLicenseKeyHeader(HttpRequestMessage request, string licenseKey)
        {
            request.Headers.Add(LicenseKeyHeader, licenseKey);
        }

        private static void AddUserContextHeader(HttpRequestMessage request, UserContextContract userContext)
        {
            request.Headers.Add(UserContextHeader, userContext.ContextKey.ToString());
        }

        private static void AddUserIdHeader(HttpRequestMessage request, int userId)
        {
            request.Headers.Add(UserIdHeader, userId.ToString());
        }

        private static void AddUserGuidHeader(HttpRequestMessage request, Guid userGuid)
        {
            request.Headers.Add(UserGuidHeader, userGuid.ToString());
        }

        private static void AddOrganizationIdHeader(HttpRequestMessage request, PortfolioTrackerOrganizations organizationId)
        {
            request.Headers.Add(OrganizationIdHeader, ((int)organizationId).ToString());
        }
    }
}
