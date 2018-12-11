using JWT;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Serializers;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Rest.Security
{
    /// <summary>
    /// Authentication to processes related to user authentications
    /// </summary>
    public static class Authentication
    {
        private const string SECRET = "RESTApiAudienceSecret";

        /// <summary>
        /// Generate Token
        /// </summary>
        /// <returns></returns>
        public static string GenerateJWT()
        {
            return new JwtBuilder()
                            .WithAlgorithm(new HMACSHA256Algorithm())
                            .WithSecret(SECRET)
                            .AddClaim("exp", DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds())
                            .AddClaim("book-read", "read-write-delete")
                            .Build();
        }

        /// <summary>
        /// Validate Token
        /// </summary>
        /// <param name="value">Authentication Header</param>
        /// <returns>Token Is valid or not</returns>
        public static bool ValidateToken(AuthenticationHeaderValue value)
        {
            if (value?.Scheme != "Bearer")
            {
                return false;
            }

            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);

                var json = decoder.Decode(value.Parameter, SECRET, verify: true);
                return true;
            }
            catch (TokenExpiredException)
            {
                return false;
            }
            catch (SignatureVerificationException)
            {
                return false;
            }
        }
    }
}
