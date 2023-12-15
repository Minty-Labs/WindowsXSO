using System.Runtime.InteropServices;

namespace WindowsXSO;

public static class Minimizer {
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
    
    [DllImport("kernel32.dll")]
    private static extern IntPtr GetConsoleWindow();
    
    public static async void Minimize() {
        await Task.Delay(TimeSpan.FromSeconds(10));
        var handle = GetConsoleWindow();
        ShowWindow(handle, 0);
    }
}