using kdyf.auth.AzureAD.Interfaces;
using kdyf.auth.AzureAD.Models;
using kdyf.auth.AzureAD.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using kdyf.auth.AzureAd.Core2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace kdyf.auth.AzureAD.Extensions
{
    public static class AzureAdExtension
    {
        #region Authentication

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="name"></param>
        /// <param name="configuration"></param>
        /// <param name="events"></param>
        public static void AddAzureAdAuthentication(this IServiceCollection services, IConfiguration configuration, JwtBearerEvents events = null, string sectionName = "AzureAdAuth")
        {
            var azureAdAuth = new AzureAdAuth();
            configuration.Bind(sectionName, azureAdAuth);

            services.AddAuthentication(sharedoptions =>
            {
                sharedoptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = azureAdAuth.Authority;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidAudiences = azureAdAuth.ValidAudiences,
                    ValidIssuers = azureAdAuth.ValidIssuers
                };

                if (events != null)
                    options.Events = events;
            });

            services.AddScoped<ITokenService, TokenService>();
        }
        #endregion

        #region Authorization

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="name"></param>
        /// <param name="configuration"></param>
        public static void ConfigureAzureAdAuthorization(this IServiceCollection services, IConfiguration configuration, string sectionName = "AzureAdGroups")
        {
            services.Configure<PolicyServiceSettings>(settings => configuration.Bind(sectionName, settings.SecurityGroups));

            services.AddSingleton<IPolicyService, PolicyService>();
            services.AddScoped<IHttpPolicyService, HttpPolicyService>();
        }
        #endregion

        public static void ConfigurePolicies(this AuthorizationOptions options, IServiceCollection services)
        {
            var policyService = services.BuildServiceProvider().GetService<IPolicyService>();

            foreach (var policy in policyService.All)
            {
                string[] policiesArray = policy.Key.Split(',');
                foreach (var item in policiesArray)
                {
                    options.AddPolicy(item, builder => builder.RequireAssertion(context => context.User.IsInRole(item)
                        || context.User.HasClaim("groups", policy.Value)));
                }
            }
        }
    }
}
