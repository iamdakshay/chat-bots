using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Common
{
    public static class Helper
    {
        public static async Task<ClaimsPrincipal> Validate(string accessToken)
        {
            string stsDiscoveryEndpoint = "https://login.microsoftonline.com/AZURE_TENANT_ID/.well-known/openid-configuration";

            ConfigurationManager<OpenIdConnectConfiguration> configManager = new Microsoft.IdentityModel.Protocols.ConfigurationManager<OpenIdConnectConfiguration>(stsDiscoveryEndpoint);

            OpenIdConnectConfiguration config = await configManager.GetConfigurationAsync();

            System.IdentityModel.Tokens.TokenValidationParameters validationParameters = new System.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidIssuer = config.Issuer,
                ValidateAudience = false,
                IssuerSigningTokens = config.SigningTokens,
                CertificateValidator = X509CertificateValidator.None
            };

            JwtSecurityTokenHandler tokendHandler = new JwtSecurityTokenHandler();

            System.IdentityModel.Tokens.SecurityToken jwt = new JwtSecurityToken();

            ClaimsPrincipal result = tokendHandler.ValidateToken(accessToken, validationParameters, out jwt);

            return result;
        }
    }
}
