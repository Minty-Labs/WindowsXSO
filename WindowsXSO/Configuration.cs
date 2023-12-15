using System.Text.Json;
using System.Text.Json.Serialization;
using Serilog;

namespace WindowsXSO; 

public class Configuration {
    [JsonPropertyName("Developer Variables (Please never change these values, unless told otherwise for debugging)")] public DeveloperVars DeveloperVars { get; set; } = new ();
    [JsonPropertyName("Toggle Comment")] public string? ToggleComment { get; set; } = "True - Enable Whitelist (Only allow specified), False - Enable Blacklist (Block specified)";
    [JsonPropertyName("Enable Whitelist")] public bool EnableWhitelist { get; set; } = true;
    [JsonPropertyName("Target Application Names")] public List<string>? TargetApplicationNames { get; set; } = new () { "discord", "discord ptb", "discord canary" };
    [JsonPropertyName("Auto-Close with SteamVR")] public bool AutoCloseWithSteamVr { get; set; } = true;
    [JsonPropertyName("Auto Minimize Window")] public bool AutoMinimize { get; set; } = true;
    [JsonPropertyName("Auto Update")] public bool AutoUpdate { get; set; } = true;
    [JsonPropertyName("Language")] public LanguageConf Language { get; set; } = new ();
}

public class DeveloperVars {
    [JsonPropertyName("Config Version")] public int ConfigVersion { get; set; } = Vars.ConfigVersion;
    [JsonPropertyName("Was it updated?")] public bool WasItUpdated { get; set; } = false;
    [JsonPropertyName("Use Shell Execute")] public bool UseShellExecute { get; set; } = false;
}

public class LanguageConf {
    [JsonPropertyName("Comment")] public string? Comment { get; set; } = "0=English, 1=Espanol, 2=Deutsch";
    [JsonPropertyName("Set")] public int Language { get; set; } = 0;
}

public static class Config {
    private static readonly ILogger Logger = Log.ForContext(typeof(Config));
    public static Configuration? Configuration { get; private set; }
    
    public static void LoadConfig() {
        var hasFile = File.Exists(Path.Combine(Environment.CurrentDirectory, "Config.json"));
        
        var defaultConfig = new Configuration {
            ToggleComment = "True - Enable Whitelist (Only allow specified), False - Enable Blacklist (Block specified)",
            EnableWhitelist = true,
            TargetApplicationNames = new List<string> { "discord", "discord ptb", "discord canary" },
            AutoCloseWithSteamVr = true,
            AutoUpdate = true,
            AutoMinimize = true,
            DeveloperVars = new DeveloperVars {
                ConfigVersion = Vars.ConfigVersion,
                WasItUpdated = false,
                UseShellExecute = false
            },
            Language = new LanguageConf {
                Comment = "0=English, 1=Espanol, 2=Deutsch",
                Language = 0
            }
        };

        bool update;
        Configuration config = null;
        if (hasFile) {
            var oldJson = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Config.json"));
            config = JsonSerializer.Deserialize<Configuration>(oldJson);
            if (config?.DeveloperVars.ConfigVersion == Vars.ConfigVersion) {
                Configuration = config;
                update = false;
            } else {
                update = true;
                config!.DeveloperVars.ConfigVersion = Vars.ConfigVersion;
            }
        } else {
            update = true;
        }
        
        var json = JsonSerializer.Serialize(config ?? defaultConfig, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(Path.Combine(Environment.CurrentDirectory, "Config.json"), json);
        Logger.Information("{0} Config.json", update ? "Updated" : hasFile ? "Loaded" : "Created");
        Configuration = config ?? defaultConfig;
    }
    
    public static void Save() => File.WriteAllText(Path.Combine(Environment.CurrentDirectory, "Config.json"), JsonSerializer.Serialize(Configuration, new JsonSerializerOptions { WriteIndented = true }));
}