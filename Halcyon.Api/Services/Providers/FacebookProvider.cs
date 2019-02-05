using Halcyon.Api.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Halcyon.Api.Services.Providers
{
    public class FacebookProvider : IProvider
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly FacebookSettings _facebookSettings;

        private const string BaseUrl = "https://graph.facebook.com/v3.1/debug_token";

        public FacebookProvider(
            IHttpClientFactory httpClientFactory,
            IOptions<FacebookSettings> facebookSettings)
        {
            _httpClientFactory = httpClientFactory;
            _facebookSettings = facebookSettings.Value;
        }

        public async Task<ProviderUserModel> GetUser(string accessToken)
        {
            var client = _httpClientFactory.CreateClient("Providers");

            var url = $"{BaseUrl}?input_token={accessToken}&access_token={_facebookSettings.AppId}|{_facebookSettings.AppSecret}";
            var result = await client.GetAsync(url);
            if (!result.IsSuccessStatusCode)
            {
                return null;
            }

            var content = await result.Content.ReadAsStringAsync();
            var profile = JObject.Parse(content);

            return new ProviderUserModel
            {
                UserId = profile["data"]["user_id"].ToString()
            };
        }
    }
}