using System.Collections.Generic;
using Newtonsoft.Json;

namespace ZpaPlugin.Models
{
    public class GenericIssueData
    {
        [JsonProperty("issues")]
        public List<Issue> Issues { get; set; }
    }

    public class Issue
    {
        [JsonProperty("ruleId")]
        public string RuleId { get; set; }

        [JsonProperty("severity")]
        public string Severity { get; set; }

        [JsonProperty("primaryLocation")]
        public PrimaryLocation PrimaryLocation { get; set; }
    }

    public class PrimaryLocation
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("textRange")]
        public TextRange TextRange { get; set; }
    }

    public class TextRange
    {
        [JsonProperty("startLine")]
        public int StartLine { get; set; }

        [JsonProperty("startColumn")]
        public int? StartColumn { get; set; }
    }
}
