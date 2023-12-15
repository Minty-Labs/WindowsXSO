using Serilog;

namespace WindowsXSO;

public static class Language {
    private static readonly ILogger Logger = Log.ForContext(typeof(Language));

    public static void Start() {
        switch (Config.Configuration!.Language.Language) {
            case 0: 
                Logger.Information("Language set to English");
                break;
            case 1:
                Logger.Information("Idioma establecido en Español");
                break;
            case 2:
                Logger.Information("Sprache auf Deutsch gesetzt");
                break;
            default:
                Logger.Warning("Language not set, defaulting to English");
                break;
        }
    }
    
    public static string NotificationsAllowed(this string text, int languageOption) {
        return languageOption switch {
            1 => "Se permiten notificaciones",
            2 => "Benachrichtigungen sind erlaubt",
            _ => text
        };
    }
    
    public static string NotificationsBlocked(this string text, int languageOption) {
        return languageOption switch {
            1 => "No se permiten notificaciones",
            2 => "Benachrichtigungen sind nicht zulässig",
            _ => text
        };
    }
    
    public static string GrantNotifications(this string text, int languageOption) {
        return languageOption switch {
            1 => "Por favor, concede acceso a las notificaciones.",
            2 => "Bitte gewähren Sie Zugriff auf Benachrichtigungen",
            _ => text
        };
    }
    
    public static string Settings(this string text, int languageOption) {
        return languageOption switch {
            1 => "Ajustes del sistema",
            2 => "Systemeinstellungen",
            _ => text
        };
    }
    
    public static string PrivacySecurity(this string text, int languageOption) {
        return languageOption switch {
            1 => "Privacidad y seguridad",
            2 => "Privatsphäre & Sicherheit",
            _ => text
        };
    }
    
    public static string NotificationSection(this string text, int languageOption) {
        return languageOption switch {
            1 => "Notificaciones (Sección)",
            2 => "Benachrichtigungen (Abschnitt)",
            _ => text
        };
    }
    
    public static string NotificationActions(this string text, int languageOption) {
        return languageOption switch {
            1 => "Notificaciones y acciones",
            2 => "Benachrichtigungen und Aktionen",
            _ => text
        };
    }
    
    public static string AllowApps(this string text, int languageOption) {
        return languageOption switch {
            1 => "Permitir que las aplicaciones accedan a las notificaciones",
            2 => "Erlauben Sie Apps, auf Benachrichtigungen zuzugreifen",
            _ => text
        };
    }
    
    public static string On(this string text, int languageOption) {
        return languageOption switch {
            1 => "ON (verdadero)",
            2 => "EIN (wahr)",
            _ => text
        };
    }
    
    public static string GetNotifFromOtherApp(this string text, int languageOption) {
        return languageOption switch {
            1 => "Recibe notificaciones de aplicaciones y otros remitentes",
            2 => "Erhalten Sie Benachrichtigungen von Apps und anderen Absendern",
            _ => text
        };
    }
    
    public static string Both(this string text, int languageOption) {
        return languageOption switch {
            1 => "AMBOS",
            2 => "BEIDE",
            _ => text
        };
    }
    
    public static string AppFocus(this string text, int languageOption) {
        return languageOption switch {
            1 => "Asegúrese de que Focus Assist esté APAGADO (falso)",
            2 => "Stellen Sie sicher, dass Focus Assist ausgeschaltet ist (falsch)",
            _ => text
        };
    }
    
    public static string Restart(this string text, int languageOption) {
        return languageOption switch {
            1 => "Una vez completado, reinicie este programa.",
            2 => "Wenn Sie fertig sind, starten Sie dieses Programm neu.",
            _ => text
        };
    }
    
    public static string PressAnyKey(this string text, int languageOption, string extra) {
        return languageOption switch {
            1 => "Presione cualquier tecla para salir de " + extra,
            2 => "Drücken Sie eine beliebige Taste, um " + extra + " zu verlassen",
            _ => text + " " + extra
        };
    }
    
    public static string Unspecified(this string text, int languageOption) {
        return languageOption switch {
            1 => "Acceso a notificaciones no especificado",
            2 => "Zugriff auf Benachrichtigungen nicht angegeben",
            _ => text
        };
    }
    
    public static string MayNotWork(this string text, int languageOption) {
        return languageOption switch {
            1 => "Es posible que las notificaciones no funcionen según lo previsto.",
            2 => "Benachrichtigungen funktionieren möglicherweise nicht wie vorgesehen.",
            _ => text
        };
    }
    
    public static string AttemptDetect(this string text, int languageOption) {
        return languageOption switch {
            1 => "Intentando detectar SteamVR...",
            2 => "Es wird versucht, SteamVR zu erkennen...",
            _ => text
        };
    }
    
    public static string SteamVrDetected(this string text, int languageOption, string extra) {
        return languageOption switch {
            1 => "SteamVR detectado. Este " + Vars.AppName + " se cerrará cuando se cierre SteamVR...",
            2 => "SteamVR erkannt. Dieses " + Vars.AppName + " wird geschlossen, wenn SteamVR geschlossen wird ...",
            _ => text
        };
    }
    
    public static string SteamVrNotDetected(this string text, int languageOption) {
        return languageOption switch {
            1 => "SteamVR no fue detectado. El cierre automático con SteamVR está deshabilitado.",
            2 => "SteamVR wurde nicht erkannt. Das automatische Schließen mit SteamVR ist deaktiviert.",
            _ => text
        };
    }
    
    public static string StartAfter(this string text, int languageOption) {
        return languageOption switch {
            1 => "Inicie este programa después de que SteamVR haya iniciado.",
            2 => "Bitte starten Sie dieses Programm, nachdem SteamVR gestartet wurde.",
            _ => text
        };
    }
    
    public static string Starting(this string text, int languageOption) {
        return languageOption switch {
            1 => "Iniciando escucha de notificaciones...",
            2 => "Benachrichtigungs-Listener wird gestartet...",
            _ => text
        };
    }
    
    public static string SentFrom(this string text, int languageOption) {
        return languageOption switch {
            1 => "Notificación enviada desde ",
            2 => "Benachrichtigung gesendet von ",
            _ => text
        };
    }
    
    public static string Exited(this string text, int languageOption) {
        return languageOption switch {
            1 => "SteamVR ha salido. Saliendo en 5 segundos...",
            2 => "SteamVR wurde beendet. Verlassen in 5 Sekunden...",
            _ => text
        };
    }
    
    public static string MinimizeIn(this string text, int languageOption) {
        return languageOption switch {
            1 => "Minimizando en 10 segundos...",
            2 => "Minimierung in 10 Sekunden...",
            _ => text
        };
    }
}