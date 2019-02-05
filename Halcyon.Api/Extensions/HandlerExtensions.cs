using Halcyon.Api.Services.Handlers;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Halcyon.Api.Extensions
{
    public static class HandlerExtensions
    {
        public static IServiceCollection AddHandlerFactory(this IServiceCollection services)
        {
            services.AddTransient<PasswordHandler>();
            services.AddTransient<RefreshTokenHandler>();
            services.AddTransient<ExternalHandler>();
            services.AddTransient<TwoFactorHandler>();

            services.AddTransient<Func<GrantType, IHandler>>(serviceProvider => type =>
            {
                switch (type)
                {
                    case GrantType.Password:
                        return serviceProvider.GetService<PasswordHandler>();

                    case GrantType.RefreshToken:
                        return serviceProvider.GetService<RefreshTokenHandler>();

                    case GrantType.External:
                        return serviceProvider.GetService<ExternalHandler>();

                    case GrantType.TwoFactor:
                        return serviceProvider.GetService<TwoFactorHandler>();

                    default:
                        return null;
                }
            });

            return services;
        }
    }
}
