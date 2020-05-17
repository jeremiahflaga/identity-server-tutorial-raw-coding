using IdentityModel;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer
{
    public static class Configuration
    {
        // [episode-9 06:40] this is how IdentityServer4 lets you identify your API
        public static IEnumerable<ApiResource> GetApis() =>
            new List<ApiResource>() { 
                new ApiResource("ApiOne")
            };

        // we need a client that will consume this API
        public static IEnumerable<Client> GetClients() =>
            new List<Client>() {
                new Client
                {
                    ClientId = "client_id",
                    ClientSecrets = { new Secret("client_secret".ToSha256()) }, // Sha256 is the algorithm that we will be using

                    // now, how is it going to retrieve the access token? 
                    // we will have to specify the grant
                    // we want client credentials flow
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    // now we want to specify the scopes that this client is allowed to get
                    // scopes means "what can this access token be used for?"
                    AllowedScopes = { "ApiOne" }
                }
            };
    }
}
