using Halcyon.Api.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Halcyon.Api.Services.Providers
{
    public class GoogleProvider : IProvider
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly GoogleSettings _googleSettings;

        private const string BaseUrl = "https://www.googleapis.com/oauth2/v3/tokeninfo";

        public GoogleProvider(
            IHttpClientFactory httpClientFactory,
            IOptions<GoogleSettings> googleSettings)
        {
            _httpClientFactory = httpClientFactory;
            _googleSettings = googleSettings.Value;
        }

        public async Task<ProviderUserModel> GetUser(string accessToken)
        {
            var client = _httpClientFactory.CreateClient();
            var url = $"{BaseUrl}?access_token={accessToken}";

            var result = await client.GetAsync(url);
            if (!result.IsSuccessStatusCode)
            {
                return null;
            }

            var content = await result.Content.ReadAsStringAsync();
            var profile = JObject.Parse(content);

            if (profile["aud"].ToString() != _googleSettings.ClientId)
            {
                return null;
            }

            return new ProviderUserModel
            {
                UserId = profile["sub"].ToString()
            };
        }
    }
}