using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;


namespace kdyf.auth.AzureAD.Interfaces
{
    public interface ITokenService
    {
        Task<IActionResult> GetTokenByHttpContent(FormUrlEncodedContent formContent, string tenantId);
        Task<IActionResult> GetTokenByRequest(HttpRequest request, string tenantId);
    }
}
