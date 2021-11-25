using TradeStops.Common.Enums;

namespace TradeStops.WebApi.Client.Version2
{
    /// <summary>
    /// Interface is inherited by classes that contain endpoints that are available by UserContext or by UserId.
    /// To get access to Organization data, Call Organization
    /// </summary>
    /// <typeparam name="TIClientByContext"></typeparam>
    /// <typeparam name="TIClientByOrganization"></typeparam>
    public interface IClientByContext<out TIClientByContext, out TIClientByOrganization>
    {
        //TIClientByLicenseKey Admin(int adminId); // it doesn't make sense to pass admin this way if we pass UserContext

        TIClientByContext User(int userId); // this method is mostly necessary for admin area where we pass admin's context + userId

        TIClientByOrganization Organization(PortfolioTrackerOrganizations organizationId);

        ////TIClientByContext Json(); // Json can be always called on IClientByLicenseKey, so this method is not really necessary here
    }
}