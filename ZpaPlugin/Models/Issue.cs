using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZpaPlugin.Models
{
    public class GenericIssueData
    {
        [JsonProperty("issues")]
        public List<Issue> Issues { get; set; }
    }

    public class Issue
    {
        [JsonProperty("severity")]
        public string Severity { get; set; }
    }
}
