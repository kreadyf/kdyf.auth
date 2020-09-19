using kdyf.auth.AzureAD.Interfaces;
using kdyf.auth.AzureAD.Models;
using kdyf.auth.AzureAD.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace kdyf.auth.AzureAD.Extensions
{
    public static class AzureAdExtension
    {
        #region Authentication
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="events"></param>
        public static void AddAzureAdAuthentication(this IServiceCollection services, IConfiguration configuration, JwtBearerEvents events = null)
        {
            string name = "AzureAdAuth";

            AddAzureAdAuthentication(services, name, configuration, events);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="name"></param>
        /// <param name="configuration"></param>
        /// <param name="events"></param>
        public static void AddAzureAdAuthentication(this IServiceCollection services, string name, IConfiguration configuration, JwtBearerEvents events = null)
        {
            var azureAdAuth = new AzureAdAuth();
            configuration.Bind(name, azureAdAuth);

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
        /// <param name="configuration"></param>
        public static void AddAzureAdAuthorization(this IServiceCollection services, IConfiguration configuration)
        {
            string name = "AzureAdGroups";

            AddAzureAdAuthorization(services, name, configuration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="name"></param>
        /// <param name="configuration"></param>
        public static void AddAzureAdAuthorization(this IServiceCollection services, string name, IConfiguration configuration)
        {
            var securityGroups = new List<SecurityGroup>();
            configuration.Bind(name, securityGroups);

            var policies = new PolicyService(securityGroups);
            services.AddScoped<IPolicyService, PolicyService>(s => policies);
            services.AddScoped<IHttpPolicyService, HttpPolicyService>();

            services.AddAuthorization(options =>
            {
                foreach (var policy in policies.All)
                {
                    string[] policiesArray = policy.Key.Split(',');
                    foreach (var item in policiesArray)
                    {
                        options.AddPolicy(item, builder => builder.RequireAssertion(context => context.User.IsInRole(item)
                        || context.User.HasClaim("groups", policy.Value)));
                    }
                }
            });

        }
        #endregion
    }
}
