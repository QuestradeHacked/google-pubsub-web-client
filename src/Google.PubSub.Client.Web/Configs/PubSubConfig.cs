using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Google.PubSub.Client.Web.Configs
{
    public class PubSubConfig
    {
        public PubSubConfig(IConfiguration config)
        {
            EmulatorHost = config.GetSection("PubSub:EmulatorHost").Value;
            Projects = config.GetSection("PubSub:Projects").Get<List<string>>();
        }
        
        public string EmulatorHost { get; set; }
        
        public IReadOnlyCollection<string> Projects { get; set; }
    }
}