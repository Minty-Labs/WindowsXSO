using System.Text.Json;
using System.Text.Json.Serialization;
using Serilog;

namespace WindowsXSO; 

public class Configuration {
    [JsonPropertyName("Toggle Comment")] public string? ToggleComment { get; set; }
    [JsonPropertyName("Enable Whitelist")] public bool EnableWhitelist { get; set; }
    [JsonPropertyName("Target Application Names")] public List<string>? TargetApplicationNames { get; set; }
    [JsonPropertyName("Auto-Close with SteamVR")] public bool AutoCloseWithSteamVr { get; set; }
}

public static class Config {
    public static Configuration? Configuration { get; private set; }
    private static DirectoryInfo ConfigDirectory { get; set; } = new (Environment.CurrentDirectory);
    
    public static void LoadConfig() {
        if (!File.Exists(ConfigDirectory + "/Config.json")) {
            var defaultConfig = new Configuration {
                ToggleComment = "True - Enable Whitelist (Only allow specified), False - Enable Blacklist (Block specified)",
                EnableWhitelist = true,
                TargetApplicationNames = new List<string> { "discord" },
                AutoCloseWithSteamVr = false
            };
            File.WriteAllText(ConfigDirectory + "/Config.json", JsonSerializer.Serialize(defaultConfig, new JsonSerializerOptions { WriteIndented = true }));
            Log.Information("[{0}] Created Config.json", "JSON");
            return;
        }
        Configuration = JsonSerializer.Deserialize<Configuration>(File.ReadAllText(ConfigDirectory + "/Config.json"))!;
        Log.Information("[{0}] Loaded Config.json", "JSON");
    }
    
    public static void Save() => File.WriteAllText(ConfigDirectory + "/Config.json", JsonSerializer.Serialize(Configuration, new JsonSerializerOptions { WriteIndented = true }));
}