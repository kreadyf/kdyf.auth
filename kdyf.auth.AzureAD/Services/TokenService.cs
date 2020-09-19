using kdyf.auth.AzureAD.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace kdyf.auth.AzureAD.Services
{
    public class TokenService : ITokenService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="formContent"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public async Task<IActionResult> GetTokenByHttpContent(FormUrlEncodedContent formContent, string tenantId)
        {
            using (var myHttpClient = new HttpClient())
            {
                var response = await myHttpClient.PostAsync($"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/token", formContent);

                return (!string.IsNullOrWhiteSpace(tenantId))
                    ? (ActionResult)new OkObjectResult(await response.Content.ReadAsStringAsync())
                    : new BadRequestObjectResult("Tenant missing");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public async Task<IActionResult> GetTokenByRequest(HttpRequest request, string tenantId)
        {
            var formContent = new FormUrlEncodedContent(request.Form.Select(s => new KeyValuePair<string, string>(s.Key, s.Value)));

            return await GetTokenByHttpContent(formContent, tenantId);
        }
    }
}
