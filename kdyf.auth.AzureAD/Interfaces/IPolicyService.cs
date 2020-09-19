using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace kdyf.auth.AzureAD.Interfaces
{
    public interface IPolicyService
    {
        Dictionary<string, string> All { get; }
        List<string> GetUserPolicies(IEnumerable<Claim> claims);
    }
}
