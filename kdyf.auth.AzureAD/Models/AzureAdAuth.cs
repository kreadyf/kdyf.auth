using System;

namespace kdyf.auth.AzureAD.Models
{
    public class AzureAdAuth
    {
        public string Authority { get; set; }
        public string[] ValidAudiences { get; set; }
        public string[] ValidIssuers { get; set; }
    }
}
