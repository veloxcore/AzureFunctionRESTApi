using JWT.Algorithms;
using JWT.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Rest.Security;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Rest.API
{
    public static class Authenticate
    {
        [FunctionName("Authenticate")]
        public static async Task<IActionResult> ValidateCredential(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "auth/login")] HttpRequestMessage req,
            ILogger log)
        {
            if (req.Headers.Authorization.Scheme.StartsWith("Basic"))
            {
                Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                string usernamePassword = encoding.GetString(Convert.FromBase64String(req.Headers.Authorization.Parameter));
                int seperatorIndex = usernamePassword.IndexOf(':');

                string username = usernamePassword.Substring(0, seperatorIndex);
                string password = usernamePassword.Substring(seperatorIndex + 1);

                if (username.Equals("admin", StringComparison.CurrentCultureIgnoreCase) && password.Equals("admin"))
                {
                    string token = Authentication.GenerateJWT();
                    return new OkObjectResult(new { access_token = token, token_type = "Bearer" });
                }
                else
                {
                    return new UnauthorizedResult();
                }
            } else {
                return new ForbidResult();
            }
        }
    }
}
