using System;

namespace kdyf.auth.AzureAD.Interfaces
{
    public interface IHttpPolicyService
    {
        bool HasPolicy(string policy);
    }
}
