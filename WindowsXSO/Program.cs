using System.Diagnostics;
using Windows.UI.Notifications;
using Windows.UI.Notifications.Management;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Templates;
using Serilog.Templates.Themes;
using XSNotifications;
using XSNotifications.Enum;

namespace WindowsXSO;

public static class Vars {
    public const string AppName = "WindowsXSO";
    public const string WindowsTitle = "Windows to XSOverlay Notification Relay";
    public const string AppVersion = "1.3.1";
    public const int ConfigVersion = 3;
}

public class Program {
    private static readonly ILogger Logger = Log.ForContext("SourceContext", Vars.AppName);
    private static UserNotificationListener? _listener;
    private static readonly List<uint> KnownNotifications = new List<uint>();
    private static List<Process> _knownProcesses = new List<Process>();
    private static Process? _steamVrProcess;
    
    public static async Task Main(string[]? args = null) {
        var levelSwitch = new LoggingLevelSwitch {
#if DEBUG
            MinimumLevel = LogEventLevel.Debug
#else
            MinimumLevel = LogEventLevel.Information
#endif
        };
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.ControlledBy(levelSwitch)
            .WriteTo.Console(new ExpressionTemplate(
                template: "[{@t:HH:mm:ss} {@l:u3} {Coalesce(Substring(SourceContext, LastIndexOf(SourceContext, '.') + 1),'unset')}] {@m}\n{@x}",
                theme: TemplateTheme.Literate))
            .CreateLogger();
        
        Console.Title = Vars.WindowsTitle + " v" + Vars.AppVersion;

        Config.Load();
        await new Updater().Start(args);
        Language.Start();
        
        var lang = Config.Configuration!.Language.Language;
        _listener = UserNotificationListener.Current;
        var accessStatus = _listener.RequestAccessAsync().GetResults();

        var isInRestartMessage = false;
        switch (accessStatus) {
            case UserNotificationListenerAccessStatus.Allowed:
                Logger.Information("Notifications are allowed".NotificationsAllowed(Config.Configuration!.Language.Language));
                break;
            case UserNotificationListenerAccessStatus.Denied:
                Logger.Error("Notifications are not allowed".NotificationsBlocked(lang));
                isInRestartMessage = true;
                Logger.Warning("Please grant access to notifications".GrantNotifications(lang));
                Console.WriteLine("----------------------------------------");
                Logger.Warning("<[{0}]>", "Windows 11");
                Logger.Warning("System Settings".Settings(lang) + " > " + "Privacy & Security".PrivacySecurity(lang) + " > " + "Notifications (Section)".NotificationSection(lang) + 
                               " > " + "Allow apps to access notifications".AllowApps(lang) + " > " + "ON (true)".On(lang));
                Logger.Warning("<[{0}]>", "Windows 10");
                Logger.Warning("System Settings".Settings(lang) + " > " + "Notifications & actions".NotificationActions(lang) + " > " + "Get notifications from apps and other senders".GetNotifFromOtherApp(lang) + 
                               " > " + "ON (true)".On(lang));
                Logger.Warning("<[{0}]>", "BOTH".Both(lang));
                Logger.Warning("Make sure Focus Assist is OFF (false)".AppFocus(lang));
                Logger.Warning("Once complete, restart this program.".Restart(lang));
                Logger.Warning("Press any key to exit".PressAnyKey(lang, Vars.AppName));
                Console.ReadKey();
                break;
            case UserNotificationListenerAccessStatus.Unspecified:
                Logger.Warning("Notifications access unspecified".Unspecified(lang));
                Logger.Warning("Notifications may not work as intended.".MayNotWork(lang));
                break;
            default: throw new ArgumentOutOfRangeException();
        }

        if (isInRestartMessage)
            Process.GetCurrentProcess().Kill();

        var config = Config.Configuration!;
        _knownProcesses = Process.GetProcesses().ToList();
        
        try {
            if (config.AutoCloseWithSteamVr) {
                Logger.Information("Attempting to detect SteamVR...".AttemptDetect(lang));
                _steamVrProcess = _knownProcesses.FirstOrDefault(p => p.ProcessName.ToLower() == "vrserver");
                if (_steamVrProcess != null && _steamVrProcess.ProcessName.ToLower() == "vrserver") 
                    Logger.Information($"SteamVR detected. This {Vars.AppName} will close when SteamVR closes...".SteamVrDetected(lang, Vars.AppName));
                else
                    Logger.Warning("SteamVR was not detected. Auto-Close with SteamVR is disabled.".SteamVrNotDetected(lang) + " {0}", "Please start this program after SteamVR has started.".StartAfter(lang));
            }
        }
        catch {
            // ignored
        }

        Logger.Information($"{(config.EnableWhitelist ? "Whitelist" : "Blacklist")} enabled. {(config.EnableWhitelist ? "Allowing" : "Blocking")} target applications: " + "{0}", string.Join(", ", config.TargetApplicationNames!));
        
        if (config.AutoMinimize) {
            Logger.Information("Minimizing in 10 seconds...".MinimizeIn(lang));
            Minimizer.Minimize();
        }
        
        Logger.Information("Starting notification listener...".Starting(lang));
        while (true) { // Keep the program running
            
            // Check if SteamVR is still running
            if (_steamVrProcess is { HasExited: true }) 
                await ExitApplicationWithSteamVr(lang);
            
            IReadOnlyList<UserNotification> readOnlyListOfNotifications = _listener.GetNotificationsAsync(NotificationKinds.Toast).AsTask().Result;
            
            foreach (var userNotification in readOnlyListOfNotifications) {
                if (KnownNotifications.Contains(userNotification.Id)) continue;
                KnownNotifications.Add(userNotification.Id);

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
                    
                    if (text.Length > 150) {
                        height += 25f;
                        timeout = 7f;
                    }

                    if (text.Length > 275) {
                        height += 75f;
                        timeout = 8f;
                    }

                    if (text.Length > 400) {
                        height += 155f;
                        timeout = 10f;
                    }

                    var truncateText = false;
                    if (text.Length > 500) {
                        height += 200f;
                        timeout = 12f;
                        truncateText = true;
                    }

                    if (text.ToLower().ContainsMultiple(".png", ".jpg", ".jpeg", ".gif", ".webp", ".bmp", ".tiff", ".tif", ".svg")) {
                        text = "[" + "image".Image(lang) + $": {text}]";
                        height = 100f;
                        timeout = 3f;
                    }
                    
                    var xsNotification = new XSNotification {
                        Title = $"{appName} - {title}", // supports Rich Text Formatting
                        Content = truncateText ? text[..1500] : text, // supports Rich Text Formatting
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
                    Logger.Information("Notification sent from ".SentFrom(lang) + "{0}: \"{1} - {2}\"", appName, title, text);
#if DEBUG
                    Logger.Debug("JSON: {0}\n", xsNotification.AsJson());
#endif
                }
                catch (Exception e) {
                    Logger.Error(e, "Error sending notification.");
                }

                
            }
            await Task.Delay(TimeSpan.FromSeconds(1));
        }
        // ReSharper disable once FunctionNeverReturns
    }

    private static async Task ExitApplicationWithSteamVr(int languageSelection) {
        if (!Config.Configuration!.AutoCloseWithSteamVr) return;
        if (_steamVrProcess == null) return;
        Logger.Information("SteamVR has exited. Exiting in 5 seconds...".Exited(languageSelection));
        await Task.Delay(TimeSpan.FromSeconds(5));
        Process.GetCurrentProcess().Kill();
    }
}
// Many thanks to Katie for helping me get Windows Notifications. ♥
// Thanks for Natsumi for helping me with the Updater. ♥