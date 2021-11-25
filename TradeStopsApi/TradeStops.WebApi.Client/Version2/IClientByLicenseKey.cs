using System;
using TradeStops.Common.Enums;
using TradeStops.Contracts;

namespace TradeStops.WebApi.Client.Version2
{
    /// <summary>
    /// Interface is inherited by classes that contain endpoints that are available by LicenseKey-only.
    /// To get access to more endpoints, UserContext, UserId or OrganizationId must be provided.
    /// </summary>
    /// <typeparam name="TIClientByLicenseKey"></typeparam>
    /// <typeparam name="TIClientByContext"></typeparam>
    /// <typeparam name="TIClientByOrganization"></typeparam>
    public interface IClientByLicenseKey<out TIClientByLicenseKey, out TIClientByContext, out TIClientByOrganization>
        : IClientByLicenseKeyUserMethods<TIClientByContext>, 
          IClientByLicenseKeyOrganizationMethods<TIClientByOrganization>,
          IClientByLicenseKeyLicenseKeyMethods<TIClientByLicenseKey>
    {
    }

    /// <summary>
    /// Set of methods that return TIClientByLicenseKey.
    /// </summary>
    /// <typeparam name="TIClientByLicenseKey">The Client with the methods that are available when no additional parameters like UserId or OrganizationId are set.</typeparam>
    public interface IClientByLicenseKeyLicenseKeyMethods<out TIClientByLicenseKey>
    {
        TIClientByLicenseKey Json();
    }

    /// <summary>
    /// Set of methods that return TIClientByOrganization.
    /// </summary>
    /// <typeparam name="TIClientByOrganization">The Client with the methods that are available when OrganizationId is set.</typeparam>
    public interface IClientByLicenseKeyOrganizationMethods<out TIClientByOrganization>
    {
        TIClientByOrganization Organization(PortfolioTrackerOrganizations organizationId);
    }

    /// <summary>
    /// Set of methods that return TIClientByContext.
    /// Separate interfaces is used to fix the issue with generic method:
    /// Services.Sync.Tests.Extensions\FixtureExtensions.cs - FreezeClient[TIClient, TIClientForUser].
    /// We work only with IClientForUser in Services.Sync code
    /// and we want to avoid passing unnecessary IClientForOrganization type.
    /// </summary>
    /// <typeparam name="TIClientByContext">The Client with the methods that are available when UserId is set.</typeparam>
    public interface IClientByLicenseKeyUserMethods<out TIClientByContext>
    {
        TIClientByContext Context(Guid contextKey);

        TIClientByContext Context(UserContextContract context);

        TIClientByContext User(int userId);
     
        TIClientByContext User(Guid userGuid);
    }
}