using System;
using System.Net.Http;
using TradeStops.Common.Enums;
using TradeStops.Contracts;

namespace TradeStops.WebApi.Client.Version2
{
    internal class RequestData
    {
        /// <summary>
        /// (required) Request URL without base URL (without domain)
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// (required) HTTP method
        /// </summary>
        public HttpMethod Method { get; set; }

        /////// <summary>
        /////// (optional) Header: API LicenseKey. Normally LicenseKey never changes for client, so it is set as DefaultHeader for HttpClient
        /////// </summary>
        ////public string LicenseKey { get; set; }

        /// <summary>
        /// (optional) Header: User Context is used to authenticate user for user-specific operations
        /// </summary>
        public UserContextContract UserContext { get; set; }

        /// <summary>
        /// (optional) Header: UserId is used to indicate which User must be used to load the data from the database.
        /// Requires ApiOperation.Admin to be available for PartnerKey.
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// (optional) Header: UserGuid is used to indicate which User must be used to load the data from the database.
        /// Requires ApiOperation.Admin to be available for PartnerKey.
        /// </summary>
        public Guid? UserGuid { get; set; }

        /// <summary>
        /// (optional) Header: OrganizationId is used to indicate what Organization must be used to load the data from the database.
        /// Requires ApiOperation.Admin to be available for PartnerKey.
        /// Requires UserId or UserContext for user who is subscribed to corresponding Organization subscription.
        /// </summary>
        public PortfolioTrackerOrganizations? OrganizationId { get; set; }

        /// <summary>
        /// (required) Header: Mime-type for 'content-type' header
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// (optional) Request body (content).
        /// </summary>
        public object Body { get; set; }

        /// <summary>
        /// (optional) Request body type. Required if 'Body' field is specified
        /// </summary>
        public Type BodyType { get; set; }

        public void SetBody<T>(T body)
        {
            Body = body;
            BodyType = typeof(T);
        }
    }
}
