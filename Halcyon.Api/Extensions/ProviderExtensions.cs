using Halcyon.Api.Services.Providers;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Halcyon.Api.Extensions
{
    public static class ProviderExtensions
    {
        public static IServiceCollection AddProviderFactory(this IServiceCollection services)
        {
            services.AddHttpClient("Providers");
            services.AddTransient<FacebookProvider>();
            services.AddTransient<GoogleProvider>();

            services.AddTransient<Func<Provider, IProvider>>(serviceProvider => provider =>
            {
                switch (provider)
                {
                    case Provider.Facebook:
                        return serviceProvider.GetService<FacebookProvider>();

                    case Provider.Google:
                        return serviceProvider.GetService<GoogleProvider>();

                    default:
                        return null;

                }
            });

            return services;
        }
    }
}
