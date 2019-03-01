using Halcyon.Api.Services.Providers;

namespace Halcyon.Api.Models.User
{
    public class ExternalLoginModel
    {
        public ExternalLoginModel(Provider provider, string externalId)
        {
            Provider = provider;
            ExternalId = externalId;
        }

        public Provider Provider { get; set; }

        public string ExternalId { get; set; }
    }
}