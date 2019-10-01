using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FacilityManagement.IdentityServer
{
    public static class Config
    {
        // identity-related resources (scopes)
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource(
                    "position",
                    "Job title",
                     new List<string>() { "position" }),
            };
        }
        
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>()
            {
                new Client
                {
                    ClientName = "Facility Management Web",
                    ClientId = "facilitymanagementweb",
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    RequireConsent = false,
                    RedirectUris = new List<string>()
                    {
                        "https://localhost:44302/signin-oidc"
                    },
                    PostLogoutRedirectUris = new List<string>()
                    {
                        "https://localhost:44302/signout-callback-oidc"
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "position",
                        "facilitymanagementapi"
                    },
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    }
                }
             };
        }

        // api-related resources (scopes)
        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>
            {
                new ApiResource("facilitymanagementapi", "Facility Management API",
                new List<string>() {"position" } )
            };
        }
    }
}
