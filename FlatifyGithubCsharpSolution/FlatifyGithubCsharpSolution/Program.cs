using System.Text;

namespace FlatifyGithubCsharpSolution;

class Program
{
    static readonly HttpClient client = new HttpClient();
    static readonly string baseUrl = "https://api.github.com/repos/{owner}/{repo}/contents/";

    static async Task Main(string[] args)
    {
        string owner = "ownerName"; // Replace with the repo owner's name
        string repo = "repoName"; // Replace with the repository name
        string path = ""; // Start path. Leave empty to start from the repo root

        try
        {
            StringBuilder allCode = new StringBuilder();
            await FetchAndConcatenateFiles(owner, repo, path, allCode);
            System.IO.File.WriteAllText(@"path\to\your\output\file.cs", allCode.ToString());
            Console.WriteLine("All .cs files have been concatenated.");
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("\nException Caught!");
            Console.WriteLine("Message :{0} ", e.Message);
        }
    }

    static async Task FetchAndConcatenateFiles(string owner, string repo, string path, StringBuilder allCode)
    {
        // Update the request URI with the current path
        var requestUri = new Uri($"{baseUrl}{path}?ref=master");

        // Set up the request
        client.DefaultRequestHeaders.UserAgent.TryParseAdd("request");

        var response = await client.GetAsync(requestUri);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();

        // Parse the JSON response body here and iterate over each item
        // This pseudocode assumes you parse the JSON into an appropriate object
        // Check if the item is a file and ends with .cs, then download and append its content
        // If it's a directory, call this method recursively with the new path

        // Example:
        // if (item.Type == "file" && item.Path.EndsWith(".cs"))
        // {
        //     string fileContent = await DownloadFileContent(item.DownloadUrl);
        //     allCode.AppendLine("// " + item.Path);
        //     allCode.AppendLine(fileContent);
        //     allCode.AppendLine();
        // }
        // else if (item.Type == "dir")
        // {
        //     await FetchAndConcatenateFiles(owner, repo, item.Path, allCode);
        // }
    }

    static async Task<string> DownloadFileContent(string downloadUrl)
    {
        var response = await client.GetAsync(downloadUrl);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}