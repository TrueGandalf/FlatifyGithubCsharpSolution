using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace FlatifyGithubCsharpSolution;

class Program
{
    static readonly HttpClient client = new HttpClient();
    static readonly string baseUrl = "https://api.github.com/repos/";
    static IConfiguration _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

    static async Task Main(string[] args)
    {
        string owner = _configuration["ownerName"]!; // Replace with the repo owner's name
        string repo = _configuration["repoName"]!; // Replace with the repository name
        string pathToRepoFolder = _configuration["pathToRepoFolder"]!; // Start path. Leave empty to start from the repo root
        string pathToResultFile = _configuration["pathToResultFile"]!; // Start path. Leave empty to start from the repo root

        try
        {
            StringBuilder allCode = new StringBuilder();
            await FetchAndConcatenateFiles(owner, repo, pathToRepoFolder, allCode);
            System.IO.File.WriteAllText(pathToResultFile, allCode.ToString());
            Console.WriteLine("All .cs files have been concatenated.");
            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("\nException Caught!");
            Console.WriteLine("Message :{0} ", e.Message);
        }
    }

    static async Task FetchAndConcatenateFiles(string owner, string repo, string path, StringBuilder allCode)
    {
        string githubToken = _configuration["GitHubToken"]!;

        // Update the request URI with the current path
        var requestUri = new Uri($"{baseUrl}{owner}/{repo}/contents/{path}?ref=main");//contents/{path}?ref=master

        // Set up the request
        client.DefaultRequestHeaders.UserAgent.TryParseAdd("request");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", githubToken);

        var response = await client.GetAsync(requestUri);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();

        var items = JsonConvert.DeserializeObject<List<GitHubContentItem>>(responseBody);

        foreach (var item in items)
        {
            if (item.Type == "file" && item.Path.EndsWith(".cs"))
            {
                string fileContent = await DownloadFileContent(item.DownloadUrl);
                allCode.AppendLine("// start of content of " + item.Path);
                allCode.AppendLine(fileContent);
                allCode.AppendLine("// end of content of " + item.Path);
                allCode.AppendLine();

                Console.WriteLine($"Added: {item.Path}");
            }
            else if (item.Type == "dir")
            {
                await FetchAndConcatenateFiles(owner, repo, item.Path, allCode);
            }
            else
            {
                Console.WriteLine($"Ignored: {item.Path}");
            }
        }
    }

    static async Task<string> DownloadFileContent(string downloadUrl)
    {
        var response = await client.GetAsync(downloadUrl);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}