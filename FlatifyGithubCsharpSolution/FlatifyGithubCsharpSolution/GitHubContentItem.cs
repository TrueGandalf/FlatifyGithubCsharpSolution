using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlatifyGithubCsharpSolution;

public class GitHubContentItem
{
    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("path")]
    public string Path { get; set; }

    [JsonProperty("download_url")]
    public string DownloadUrl { get; set; }

    // Add other properties as needed
}
