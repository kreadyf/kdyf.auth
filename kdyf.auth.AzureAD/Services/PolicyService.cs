using kdyf.auth.AzureAD.Interfaces;
using kdyf.auth.AzureAD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using kdyf.auth.AzureAd.Core2.Models;
using Microsoft.Extensions.Options;

namespace kdyf.auth.AzureAD.Services
{
    public class PolicyService : IPolicyService
    {
        public const string Default = "default";
        private IEnumerable<SecurityGroup> _policies;

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, string> All { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="securityGroups"></param>
        public PolicyService(IOptions<PolicyServiceSettings> options)
        {
            _policies = options.Value.SecurityGroups;

            var result = new Dictionary<string, string>();

            foreach (var policyModel in _policies)
            {
                foreach (var policy in policyModel.Policies.Split('0'))
                {
                    result.Add(policy, policyModel.Id);
                }
            }

            All = result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        public List<string> GetUserPolicies(IEnumerable<Claim> claims)
        {
            var result = new List<string>();

            foreach (var group in claims.Where(c => c.Type == "groups").ToList())
            {
                if (_policies.Any(n => n.Id.Equals(@group.Value)))
                {
                    foreach (var policyModel in _policies.Where(n => n.Id.Equals(group.Value)))
                    {
                        result.AddRange(policyModel.Policies.Split(','));
                    }
                }
            }

            return result;
        }

    }
}
