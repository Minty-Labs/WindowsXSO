using System.Text.Json;
using System.Text.Json.Serialization;

namespace Updater; 

public class Configuration {
    [JsonPropertyName("Developer Variables (Please never change these values, unless told otherwise for debugging)")] public DeveloperVars DeveloperVars { get; set; }
    [JsonPropertyName("Toggle Comment")] public string? ToggleComment { get; set; }
    [JsonPropertyName("Enable Whitelist")] public bool EnableWhitelist { get; set; }
    [JsonPropertyName("Target Application Names")] public List<string>? TargetApplicationNames { get; set; }
    [JsonPropertyName("Auto-Close with SteamVR")] public bool AutoCloseWithSteamVr { get; set; }
    [JsonPropertyName("Auto Update")] public bool AutoUpdate { get; set; }
}

public class DeveloperVars {
    [JsonPropertyName("Config Version")] public int ConfigVersion { get; set; }
    [JsonPropertyName("Was it updated?")] public bool WasItUpdated { get; set; }
    [JsonPropertyName("Use Shell Execute")] public bool UseShellExecute { get; set; }
}

public static class Config {
    public static Configuration? Configuration { get; private set; } = Load();
    
    private static Configuration Load() => Configuration = JsonSerializer.Deserialize<Configuration>(File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Config.json")))!;
    
    public static void Save() => File.WriteAllText(Path.Combine(Environment.CurrentDirectory, "Config.json"), JsonSerializer.Serialize(Configuration, new JsonSerializerOptions { WriteIndented = true }));
}