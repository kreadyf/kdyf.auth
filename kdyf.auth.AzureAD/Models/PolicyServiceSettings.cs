using System;
using System.Collections.Generic;
using System.Text;
using kdyf.auth.AzureAD.Models;

namespace kdyf.auth.AzureAd.Core2.Models
{
    public class PolicyServiceSettings
    {
        public IList<SecurityGroup> SecurityGroups { get; set; } = new List<SecurityGroup>();
    }
}
