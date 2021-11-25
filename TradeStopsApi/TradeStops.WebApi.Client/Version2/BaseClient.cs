using System;
using System.Threading.Tasks;
using TradeStops.Common.Constants;
using TradeStops.Common.Enums;
using TradeStops.Contracts;
using TradeStops.WebApi.Client.Helpers;

namespace TradeStops.WebApi.Client.Version2
{
    public abstract class BaseClient<TIClientByLicenseKey, TIClientByContext, TIClientByOrganization, TClient> :
        IClientByLicenseKey<TIClientByLicenseKey, TIClientByContext, TIClientByOrganization>,
        IClientByContext<TIClientByContext, TIClientByOrganization>
        where TClient : 
            BaseClient<TIClientByLicenseKey, TIClientByContext, TIClientByOrganization, TClient>, 
            TIClientByLicenseKey, 
            TIClientByContext, 
            TIClientByOrganization, 
            new()
    {
        private bool UseJson { get; set; } // false by default - we use MessagePack in our projects

        private UserContextContract UserContext { get; set; } // null by default

        private int? UserId { get; set; } // null by default

        private Guid? UserGuid { get; set; } // null by default

        private PortfolioTrackerOrganizations? OrganizationId { get; set; } // null by default

        // todo: consider to change 'UserContextContract UserContext' field to 'Guid? ContextKey'
        public TIClientByContext Context(Guid contextKey)
        {
            var copy = ShallowCopy();
            copy.UserContext = new UserContextContract { ContextKey = contextKey };
            return copy;
        }

        public TIClientByContext Context(UserContextContract context)
        {
            var copy = ShallowCopy();
            copy.UserContext = context;
            return copy;
        }

        public TIClientByContext User(int userId)
        {
            var copy = ShallowCopy();
            copy.UserId = userId;

            return copy;
        }

        public TIClientByContext User(Guid userGuid)
        {
            var copy = ShallowCopy();
            copy.UserGuid = userGuid;

            return copy;
        }

        public TIClientByOrganization Organization(PortfolioTrackerOrganizations organizationId)
        {
            var copy = ShallowCopy();
            copy.OrganizationId = organizationId;

            return copy;
        }

        public TClient Json()
        {
            var copy = ShallowCopy();
            copy.UseJson = true;
            return copy;
        }

        TIClientByLicenseKey IClientByLicenseKeyLicenseKeyMethods<TIClientByLicenseKey>.Json()
        {
            return Json();
        }

        ////TIClientByContext IClientByContext<TIClientByContext>.Json()
        ////{
        ////    return Json();
        ////}

        #region PerformRequest

        internal TResult PerformRequest<TResult>(RequestData data)
        {
            FillRequestHeaders(data);
            var client = HttpClientHelper.GetClient();
            return PerformRequestHelper.PerformRequest<TResult>(client, data);
        }

        internal async Task<TResult> PerformRequestAsync<TResult>(RequestData data)
        {
            FillRequestHeaders(data);
            var client = HttpClientHelper.GetClient();
            return await PerformRequestHelper.PerformRequestAsync<TResult>(client, data);
        }

        internal void PerformRequest(RequestData data)
        {
            FillRequestHeaders(data);
            var client = HttpClientHelper.GetClient();
            PerformRequestHelper.PerformRequest(client, data);
        }

        internal async Task PerformRequestAsync(RequestData data)
        {
            FillRequestHeaders(data);
            var client = HttpClientHelper.GetClient();
            await PerformRequestHelper.PerformRequestAsync(client, data);
        }

        #endregion

        private void FillRequestHeaders(RequestData data)
        {
            data.UserContext = UserContext;
            data.UserId = UserId;
            data.UserGuid = UserGuid;
            data.OrganizationId = OrganizationId;

            data.ContentType = UseJson ? MimeTypes.Json : MimeTypes.MessagePack;
        }

        private TClient ShallowCopy()
        {
            return (TClient)MemberwiseClone();
        }
    }
}
