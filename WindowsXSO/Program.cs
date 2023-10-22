using System.Diagnostics;
using Windows.UI.Notifications;
using Windows.UI.Notifications.Management;
using Serilog;
using XSNotifications;
using XSNotifications.Enum;

namespace WindowsXSO;

public static class Vars {
    public const string AppName = "WindowsXSO";
    public const string WindowsTitle = "Windows to XSOverlay Notification Relay";
    public const string AppVersion = "1.1.3";
}

public class Program {
    private static UserNotificationListener? _listener;
    private static List<uint> _knownNotifications = new List<uint>();
    private static List<Process> _knownProcesses = new List<Process>();
    
    public static void Main(string[]? args = null) {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .CreateLogger();
        
        Console.Title = Vars.WindowsTitle + " v" + Vars.AppVersion;

        _listener = UserNotificationListener.Current;
        var accessStatus = _listener.RequestAccessAsync().GetResults();

        var isInRestartMessage = false;
        switch (accessStatus) {
            case UserNotificationListenerAccessStatus.Allowed:
                Log.Information("Notifications {0}.", "access granted");
                break;
            case UserNotificationListenerAccessStatus.Denied:
                Log.Error("Notifications {0}.", "access denied");
                isInRestartMessage = true;
                Log.Warning("Please grant access to notifications.");
                Console.WriteLine("----------------------------------------");
                Log.Warning("<[{0}]>", "Windows 11");
                Log.Warning("(System) Settings > Privacy & Security > Notifications (Section) > Allow apps to access notifications > ON (true)");
                Log.Warning("<[{0}]>", "Windows 10");
                Log.Warning("(System) Settings > Notifications & actions > Get notifications from apps and other senders > ON (true)");
                Log.Warning("Once complete, restart this program.");
                Log.Warning($"Press any key to exit {Vars.AppName}.");
                Console.ReadKey();
                break;
            case UserNotificationListenerAccessStatus.Unspecified:
                Log.Warning("Notifications {0}.", "access unspecified");
                Log.Warning("Notifications may not work as intended.");
                break;
            default: throw new ArgumentOutOfRangeException();
        }

        if (isInRestartMessage)
            Process.GetCurrentProcess().Kill();

        Config.LoadConfig();
        var config = Config.Configuration!;
        _knownProcesses = Process.GetProcesses().ToList();
        
        try {
            if (config.AutoCloseWithSteamVr) {
                Log.Information("Attempting to detect SteamVR...");
                var getSteamVrProcess = _knownProcesses.FirstOrDefault(p => p.ProcessName.ToLower() == "vrserver");
                if (getSteamVrProcess != null && getSteamVrProcess.ProcessName.ToLower() == "vrserver") {
                    Log.Information($"SteamVR detected. This {Vars.AppName} will close when SteamVR closes...");
                    getSteamVrProcess.Exited += (sender, eventArgs) => {
                        Log.Information("SteamVR has exited. Exiting in 5 seconds...");
                        Task.Run(async () => {
                            await Task.Delay(TimeSpan.FromSeconds(5));
                            Process.GetCurrentProcess().Kill();
                        });
                    };
                }
                else Log.Warning("SteamVR was {0}. Auto-Close with SteamVR is disabled. {1}", "not detected", "Please start this program after SteamVR has started.");
            }
        }
        catch {
            // ignored
        }

        Log.Information($"{(config.EnableWhitelist ? "Whitelist" : "Blacklist")} enabled. {(config.EnableWhitelist ? "Allowing" : "Blocking")} target applications: " + "{0}", string.Join(", ", config.TargetApplicationNames!));

        Log.Information("Starting notification listener...");
        while (true) { // Keep the program running
            IReadOnlyList<UserNotification> readOnlyListOfNotifications = _listener.GetNotificationsAsync(NotificationKinds.Toast).AsTask().Result;
            
            foreach (var userNotification in readOnlyListOfNotifications) {
                if (_knownNotifications.Contains(userNotification.Id)) continue;
                _knownNotifications.Add(userNotification.Id);

                try {
                    Windows.ApplicationModel.AppInfo? appInfo = null;
                    try { appInfo = userNotification.AppInfo; } catch { /*ignored*/ }
                    
                    var appName = appInfo != null ? appInfo.DisplayInfo.DisplayName : "";
                    var textElements = userNotification.Notification.Visual.GetBinding(KnownNotificationBindings.ToastGeneric)?.GetTextElements();
                    var elementList = textElements?.Select(t => t.Text).ToArray();
                    if (elementList == null) continue;
                    var title = elementList?[0];
                    
                    switch (config.EnableWhitelist) {
                        case true when !config.TargetApplicationNames!.Contains(appName!.ToLower()):
                        case false when config.TargetApplicationNames!.Contains(appName!.ToLower()):
                            continue;
                    }

                    var text = elementList?.Length >= 2 ? string.Join("\n", elementList.Skip(1)) : "";
                    
                    if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(text)) continue;
                    
                    var height = 175f;
                    var timeout = 6f;
                    
                    if (text.Length > 100) {
                        height += 25f;
                        timeout = 7f;
                    }

                    if (text.Length > 200) {
                        height += 75f;
                        timeout = 8f;
                    }

                    if (text.Length > 300) {
                        height += 155f;
                        timeout = 10f;
                    }

                    if (text.ToLower().Contains("image.png") || text.ToLower().Contains("image.jpg") || text.ToLower().Contains("image.jpeg") || text.ToLower().Contains("unknown.png")) {
                        text = "Sent an image.";
                        height = 100f;
                        timeout = 3f;
                    }
                    
                    var xsNotification = new XSNotification {
                        Title = $"{appName} - {title}", // supports Rich Text Formatting
                        Content = text, // supports Rich Text Formatting
                        Timeout = timeout, // [float] seconds
                        SourceApp = Vars.AppName,
                        MessageType = XSMessageType.Notification,
                        UseBase64Icon = false,
                        // Base64 encoded image
                        Icon = "default", // Can also be "default", "error", or "warning"
                        Opacity = 0.8f, // [float] 0 to 1
                        Height = height,
                        Volume = 0f, // [float] 0 to 1
                    };
        
                    new XSNotifier().SendNotification(xsNotification);
                    Log.Information("Notification sent from {0}: \"{1} - {2}\"", appName, title, text);
#if DEBUG
                    Log.Information("JSON: {0}\n", xsNotification.AsJson());
#endif
                }
                catch (Exception e) {
                    Log.Error(e, "Error sending notification.");
                }
            }
            Task.Delay(TimeSpan.FromSeconds(1));
        }
        // ReSharper disable once FunctionNeverReturns
    }
}
// Many thanks to Katie for helping me get Windows Notifications. ♥