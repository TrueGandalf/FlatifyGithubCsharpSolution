# FlatifyGithubCsharpSolution

This project is designed to concatenate `.cs` files from a specified GitHub repository into a single file. 

## Prerequisites

Before you begin, ensure you have met the following requirements:

### Install Necessary NuGet Packages

You need to install the following NuGet packages:

- `Microsoft.Extensions.Configuration`
- `Microsoft.Extensions.Configuration.FileExtensions`
- `Microsoft.Extensions.Configuration.Json`

You can install these packages via the NuGet Package Manager or using the .NET CLI:

```bash
dotnet add package Microsoft.Extensions.Configuration
dotnet add package Microsoft.Extensions.Configuration.FileExtensions
dotnet add package Microsoft.Extensions.Configuration.Json
```

### Configuration File

The file `FlatifyGithubCsharpSolution/FlatifyGithubCsharpSolution/appsettings.json` is added to `.gitignore`. You need to create this file in your local setup. Use the following example file as a reference for creating `appsettings.json`:

- `FlatifyGithubCsharpSolution/FlatifyGithubCsharpSolution/appsettingsExample.json`

Ensure you replace the placeholder values in `appsettings.json` with your actual configuration values.
```