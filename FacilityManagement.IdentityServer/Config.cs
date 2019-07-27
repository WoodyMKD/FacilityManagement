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
                    RedirectUris = new List<string>()
                    {
                        "https://localhost:44302/signin-oidc"
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    },
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    }
                }
             };
        }
    }
}
