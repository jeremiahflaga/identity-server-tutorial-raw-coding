using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IdentityModel;
using IdentityModel.Client;

namespace ApiTwo.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        
        [Route("/")]
        public async Task<IActionResult> Index()
        {
            // 1. Retrieve access token
            var serverClient = httpClientFactory.CreateClient();

            // [added the IdentityModel package]
            // https://localhost:44375/ is the same as the config.Authority of Startup class in ApiOne
            // the discovery document is the /.well-known/ endpoint
            var discoveryDocument = await serverClient.GetDiscoveryDocumentAsync("https://localhost:44375/");

            var tokenResponse = await serverClient.RequestClientCredentialsTokenAsync(
                new ClientCredentialsTokenRequest
                {
                    Address = discoveryDocument.TokenEndpoint,

                    ClientId = "client_id",

                    // [episode-9 ep.9 27"00] the client does not know anything about how to hash the secret
                    ClientSecret = "client_secret",

                    Scope = "ApiOne",
                });

            // 2. Retrieve secret data
            var apiClient = httpClientFactory.CreateClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);
            var response = await apiClient.GetAsync("https://localhost:44353/secret");
            var content = await response.Content.ReadAsStringAsync();

            return Ok(new
            {
                access_token = tokenResponse.AccessToken,
                message = content,
            });
        }
    }
}
