using System.Diagnostics;
using System.Text.Json;

namespace Updater;

public class Program {
    public static async Task Main(string[]? args = null) {
        Console.Title = "WindowsXSO Updater";
        
        var mainFile = Path.Combine(Environment.CurrentDirectory, "WindowsXSO.exe");
        var tempFile = Path.Combine(Environment.CurrentDirectory, "WindowsXSO.exe.temp");
        var oldFile = Path.Combine(Environment.CurrentDirectory, "WindowsXSO.exe.old");
        var httpClient = new HttpClient();
        const string gitHubApiUrl = "https://api.github.com/repos/Minty-Labs/WindowsXSO/releases/latest";

        httpClient.DefaultRequestHeaders.Add("User-Agent", "Minty-Labs/WindowsXSO-Updater 1.0.0");
        var apiResponse = await httpClient.GetStringAsync(gitHubApiUrl);
        var apiResponseJson = JsonSerializer.Deserialize<Api>(apiResponse);
        if (apiResponseJson == null) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Failed to deserialize GitHub API response.");
            Console.ResetColor();
            Console.ReadKey();
            httpClient.Dispose();
            return;
        }
        
        // Copy existing EXE to .old
        if (File.Exists(mainFile)) {
            if (File.Exists(oldFile)) 
                File.Delete(oldFile);
            Console.WriteLine("Copying current exe to .old");
            File.Copy(mainFile, oldFile);
        }

        Console.WriteLine("Downloading new exe");
        var newExeUrl = apiResponseJson.assets[0].browser_download_url;
        var newExeBytes = await httpClient.GetByteArrayAsync(newExeUrl);
        await File.WriteAllBytesAsync(tempFile, newExeBytes);

        Console.WriteLine("Checking main file");
        if (!File.Exists(mainFile)) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("WindowsXSO.exe does not exist, cannot replace with new file.");
            Console.ResetColor();
            Console.ReadKey();
            httpClient.Dispose();
            return;
        }

        Console.WriteLine("Checking old file");
        if (!File.Exists(oldFile)) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("WindowsXSO.exe.old does not exist. How does this even happen? This file is only created when updating.");
            Console.ResetColor();
            Console.ReadKey();
            httpClient.Dispose();
            return;
        }

        Console.WriteLine("Checking temp file");
        if (!File.Exists(tempFile)) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("WindowsXSO.temp.exe does not exist. Do not prematurely delete this file.");
            Console.ResetColor();
            Console.ReadKey();
            httpClient.Dispose();
        }
        
        // File.Move(mainFile, oldFile);
        Console.WriteLine("Moving temp file to main file");
        File.Move(tempFile, mainFile, true);
        try {
            Console.WriteLine("Deleting old file");
            File.Delete(oldFile);
        }
        catch { /*ignored*/ }
        
        httpClient.Dispose();
        try {
            Config.Configuration!.DeveloperVars.WasItUpdated = false;
            Config.Save();
        }
        catch { /*ignored*/ }
        Console.WriteLine("Starting WindowsXSO.exe");
        var processInfo = new ProcessStartInfo {
            Arguments = string.Join(' ', args ?? Array.Empty<string>()),
            FileName = "WindowsXSO.exe",
            WorkingDirectory = Environment.CurrentDirectory,
            UseShellExecute = Config.Configuration!.DeveloperVars.UseShellExecute,
            CreateNoWindow = false
        };
        Process.Start(processInfo);
        Console.WriteLine("Closing updater");
        Process.GetCurrentProcess().Kill();
    }
}