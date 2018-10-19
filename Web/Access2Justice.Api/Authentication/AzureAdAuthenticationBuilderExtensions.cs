using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;

namespace Access2Justice.Api.Authentication
{
    public static class AzureAdServiceCollectionExtensions
    {
        public static AuthenticationBuilder AddAzureAdBearer(this AuthenticationBuilder builder)
            => builder.AddAzureAdBearer(_ => { });

        public static AuthenticationBuilder AddAzureAdBearer(this AuthenticationBuilder builder, Action<AzureAdOptions> configureOptions)
        {
            builder.Services.Configure(configureOptions);
            builder.Services.AddSingleton<IConfigureOptions<JwtBearerOptions>, ConfigureAzureOptions>();
            builder.AddJwtBearer();
            return builder;
        }

        private class ConfigureAzureOptions : IConfigureNamedOptions<JwtBearerOptions>
        {
            private readonly AzureAdOptions azureOptions;

            public ConfigureAzureOptions(IOptions<AzureAdOptions> azureOptions)
            {
                this.azureOptions = azureOptions.Value;
            }

            public void Configure(string name, JwtBearerOptions options)
            {
                options.Audience = azureOptions.ClientId;
                options.Authority = $"{azureOptions.Instance}{azureOptions.TenantId}/{azureOptions.TokenVersion}/";
                options.TokenValidationParameters.ValidateIssuer = true;
                options.TokenValidationParameters.IssuerValidator = ValidateIssuer;
            }

            /// <summary>
            /// Validate the issuer. 
            /// </summary>
            /// <param name="issuer">Issuer to validate (will be tenanted)</param>
            /// <param name="securityToken">Received Security Token</param>
            /// <param name="validationParameters">Token Validation parameters</param>
            /// <remarks>The issuer is considered as valid if it has the same http scheme and authority as the
            /// authority from the configuration file, has a tenant Id, and optionally v2.0 (this web api
            /// accepts both V1 and V2 tokens)</remarks>
            /// <returns>The <c>issuer</c> if it's valid, or otherwise <c>null</c></returns>
            private string ValidateIssuer(string issuer, SecurityToken securityToken, TokenValidationParameters validationParameters)
            {
                Uri uri = new Uri(issuer);
                Uri authorityUri = new Uri(azureOptions.Instance);
                string[] parts = uri.AbsolutePath.Split('/');
                if (parts.Length >= 2)
                {
                    if (uri.Scheme != authorityUri.Scheme || uri.Authority != authorityUri.Authority)
                    {
                        throw new SecurityTokenInvalidIssuerException("Issuer has wrong authority");
                    }
                    if (!Guid.TryParse(parts[1], out Guid tenantId))
                    {
                        throw new SecurityTokenInvalidIssuerException("Cannot find the tenant GUID for the issuer");
                    }
                    if (parts.Length > 2 && parts[2] != azureOptions.TokenVersion)
                    {
                        throw new SecurityTokenInvalidIssuerException("Only accepted protocol versions are AAD v1.0 or V2.0");
                    }
                    return issuer;
                }
                else
                {
                    throw new SecurityTokenInvalidIssuerException("Unknown issuer");
                }
            }

            public void Configure(JwtBearerOptions options)
            {
                Configure(Options.DefaultName, options);
            }
        }
    }
}
