using Halcyon.Api.Entities;
using Halcyon.Api.Extensions;
using Halcyon.Api.Filters;
using Halcyon.Api.Repositories;
using Halcyon.Api.Services.Email;
using Halcyon.Api.Services.Response;
using Halcyon.Api.Services.Security;
using Halcyon.Api.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Text;

namespace Halcyon.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLocalization(opts => { opts.ResourcesPath = "Resources"; });

            services
                .AddMvc(options =>
                {
                    options.Filters.Add(typeof(GlobalExceptionAttribute));
                    options.Filters.Add(typeof(ValidateModelStateAttribute));
                })
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Ignore;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ValidAudience = "HalcyonClient",
                        ValidIssuer = "HalcyonApi",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:SecurityKey"])),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();

                options.SwaggerDoc("v1", new Info
                {
                    Title = "Halcyon Api",
                    Version = "v1",
                    Description = "A .NET Core api project template."
                });

                options.AddSecurityDefinition("bearer", new ApiKeyScheme
                {
                    In = "header",
                    Name = "Authorization"
                });
            });

            services.AddOptions();
            services.AddCors();

            services.Configure<MongoDBSettings>(Configuration.GetSection("MongoDB"));
            services.Configure<SeedSettings>(Configuration.GetSection("Seed"));
            services.Configure<JwtSettings>(Configuration.GetSection("Jwt"));
            services.Configure<EmailSettings>(Configuration.GetSection("Email"));
            services.Configure<FacebookSettings>(Configuration.GetSection("Authentication:Facebook"));
            services.Configure<GoogleSettings>(Configuration.GetSection("Authentication:Google"));

            services.AddSingleton<HalcyonDbContext>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IPasswordService, PasswordService>();
            services.AddTransient<ITwoFactorService, TwoFactorService>();
            services.AddTransient<IJwtService, JwtService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IResponseService, ResponseService>();

            services.AddHttpClient();
            services.AddHandlerFactory();
            services.AddProviderFactory();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.DocumentTitle = "Halcyon Api";
                options.SwaggerEndpoint("../swagger/v1/swagger.json", "Halcyon Api v1");
            });
        }
    }
}