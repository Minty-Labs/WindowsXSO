using System.Diagnostics;
using System.Text.Json;
using Serilog;

namespace WindowsXSO; 

public class Updater {
    public Updater UpdaterClass() => this;
    private const string GitHubApiUrl = "https://api.github.com/repos/Minty-Labs/WindowsXSO/releases/latest";
    private string ApiResponse;
    private const string UpdaterDownloadUrl = "https://raw.githubusercontent.com/Minty-Labs/WindowsXSO/main/Resources/Updater.exe";

    public async Task Start(string[]? args = null) {
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("User-Agent", $"Minty-Labs/{Vars.AppName} {Vars.AppVersion}");
        ApiResponse = await httpClient.GetStringAsync(GitHubApiUrl);
        var apiResponseJson = JsonSerializer.Deserialize<GitHubApiResponseJson.Api>(ApiResponse);
        if (apiResponseJson == null) {
            Log.Error("Failed to deserialize GitHub API response.");
            httpClient.Dispose();
            return;
        }
        var latestVersion = apiResponseJson.tag_name;
        if (Vars.AppVersion == latestVersion) {
#if DEBUG
            Log.Debug("You are running the latest version of {0}.", Vars.AppName);
 #endif
            httpClient.Dispose();
            return;
        }
        
        Log.Information("{0} v{1} is available to download. (Current v{2})", Vars.AppName, latestVersion, Vars.AppVersion);
        if (!Config.Configuration!.AutoUpdate) {
#if DEBUG
            Log.Debug("Auto Update is disabled.");
#endif
            httpClient.Dispose();
            return;
        }
        Log.Information("Updating {0} to v{1}", Vars.AppName, latestVersion);
        
        var updaterFile = Path.Combine(Environment.CurrentDirectory, "Updater.exe");
        
        // Set variable for updater
        Config.Configuration.DeveloperVars.WasItUpdated = true;
        Config.Save();
        
        // Create Updater EXE
        if (!File.Exists(updaterFile)) {
            Log.Information("Downloading Updater.exe from {0}", UpdaterDownloadUrl);
            var updaterExeBytes = await httpClient.GetByteArrayAsync(UpdaterDownloadUrl);
            await File.WriteAllBytesAsync(updaterFile, updaterExeBytes);
        }
        
        httpClient.Dispose();
        var processInfo = new ProcessStartInfo {
            Arguments = string.Join(' ', args ?? Array.Empty<string>()),
            FileName = "Updater.exe",
            WorkingDirectory = Environment.CurrentDirectory,
            UseShellExecute = Config.Configuration.DeveloperVars.UseShellExecute,
            CreateNoWindow = false
        };
        Process.Start(processInfo);
        Process.GetCurrentProcess().Kill();
    }
}