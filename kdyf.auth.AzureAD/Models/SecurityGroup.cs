using System;

namespace kdyf.auth.AzureAD.Models
{
    public class SecurityGroup
    {
        public string Id { get; set; }
        public string Group { get; set; }
        public string Policies { get; set; }
    }
}
