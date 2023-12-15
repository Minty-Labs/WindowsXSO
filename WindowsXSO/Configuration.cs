using System.Text.Json;
using System.Text.Json.Serialization;
using Serilog;

namespace WindowsXSO; 

public class Configuration {
    [JsonPropertyName("Developer Variables (Please never change these values, unless told otherwise for debugging)")] public DeveloperVars DeveloperVars { get; set; }
    [JsonPropertyName("Toggle Comment")] public string? ToggleComment { get; set; }
    [JsonPropertyName("Enable Whitelist")] public bool EnableWhitelist { get; set; }
    [JsonPropertyName("Target Application Names")] public List<string>? TargetApplicationNames { get; set; }
    [JsonPropertyName("Auto-Close with SteamVR")] public bool AutoCloseWithSteamVr { get; set; }
    [JsonPropertyName("Auto Update")] public bool AutoUpdate { get; set; }
    [JsonPropertyName("Language")] public LanguageConf Language { get; set; } = new ();
}

public class DeveloperVars {
    [JsonPropertyName("Config Version")] public int ConfigVersion { get; set; }
    [JsonPropertyName("Was it updated?")] public bool WasItUpdated { get; set; }
    [JsonPropertyName("Use Shell Execute")] public bool UseShellExecute { get; set; }
public class LanguageConf {
    [JsonPropertyName("Comment")] public string? Comment { get; set; } = "0=English, 1=Espanol, 2=Deutsch";
    [JsonPropertyName("Set")] public int Language { get; set; } = 0;
}

public static class Config {
    private static readonly ILogger Logger = Log.ForContext(typeof(Config));
    public static Configuration? Configuration { get; private set; }
    
    public static void LoadConfig() {
        if (!File.Exists(Path.Combine(Environment.CurrentDirectory, "Config.json"))) {
            var defaultConfig = new Configuration {
                ToggleComment = "True - Enable Whitelist (Only allow specified), False - Enable Blacklist (Block specified)",
                EnableWhitelist = true,
                TargetApplicationNames = new List<string> { "discord", "discord ptb", "discord canary" },
                AutoCloseWithSteamVr = true,
                AutoUpdate = true,
                DeveloperVars = new DeveloperVars {
                    ConfigVersion = 2,
                    WasItUpdated = false,
                    UseShellExecute = false
                }
            };
            File.WriteAllText(Path.Combine(Environment.CurrentDirectory, "Config.json"), JsonSerializer.Serialize(defaultConfig, new JsonSerializerOptions { WriteIndented = true }));
            Log.Information("[{0}] Created Config.json", "CONFIG");
            Configuration = defaultConfig;
            return;
        }
        Configuration = JsonSerializer.Deserialize<Configuration>(File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Config.json")))!;
        Log.Information("[{0}] Loaded from Config.json", "CONFIG");
        Save();
    }
    
    public static void Save() => File.WriteAllText(Path.Combine(Environment.CurrentDirectory, "Config.json"), JsonSerializer.Serialize(Configuration, new JsonSerializerOptions { WriteIndented = true }));
}