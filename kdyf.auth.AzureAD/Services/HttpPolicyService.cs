using kdyf.auth.AzureAD.Interfaces;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;


namespace kdyf.auth.AzureAD.Services
{
    public class HttpPolicyService : IHttpPolicyService
    {
        private readonly HashSet<string> _security;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="principal"></param>
        public HttpPolicyService(IPolicyService policy, IPrincipal principal)
        {
            var policies = policy.GetUserPolicies((principal.Identity as ClaimsIdentity).Claims);

            _security = new HashSet<string>(policies);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="policy"></param>
        /// <returns></returns>
        public bool HasPolicy(string policy)
        {
            return _security.Contains(policy);
        }
    }
}
