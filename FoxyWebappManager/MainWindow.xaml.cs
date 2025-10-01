using System.Runtime.InteropServices;
using FoxyWebappManager.Helpers;

using Windows.UI.ViewManagement;
using WinRT.Interop;

namespace FoxyWebappManager;

public sealed partial class MainWindow : WindowEx
{

    private delegate IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

    private IntPtr _oldWndProc = IntPtr.Zero;
    private WndProc _newWndProc;

    const int GWLP_WNDPROC = -4;
    const int WM_NCLBUTTONDBLCLK = 0x00A3; // Doubleclick auf Titelleiste

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, WndProc newProc);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);



    private Microsoft.UI.Dispatching.DispatcherQueue dispatcherQueue;

    private UISettings settings;

    public MainWindow()
    {
        InitializeComponent();

        var hwnd = WindowNative.GetWindowHandle(this);

        // neue WindowProc setzen
        _newWndProc = CustomWndProc;
        _oldWndProc = SetWindowLongPtr(hwnd, GWLP_WNDPROC, _newWndProc);

        AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/WindowIcon.ico"));
        Content = null;
        Title = "AppDisplayName".GetLocalized();

        // Theme change code picked from https://github.com/microsoft/WinUI-Gallery/pull/1239
        dispatcherQueue = Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread();
        settings = new UISettings();
        settings.ColorValuesChanged += Settings_ColorValuesChanged; // cannot use FrameworkElement.ActualThemeChanged event
    }

    private nint CustomWndProc(nint hWnd, uint msg, nint wParam, nint lParam)
    {
        if (msg == WM_NCLBUTTONDBLCLK)
        {
            // Doppelklick auf Titelleiste ignorieren → kein Maximieren
            return IntPtr.Zero;
        }

        // alle anderen Nachrichten normal weiterleiten
        return CallWindowProc(_oldWndProc, hWnd, msg, wParam, lParam);
    }

    // this handles updating the caption button colors correctly when indows system theme is changed
    // while the app is open
    private void Settings_ColorValuesChanged(UISettings sender, object args)
    {
        // This calls comes off-thread, hence we will need to dispatch it to current app's thread
        dispatcherQueue.TryEnqueue(() =>
        {
            TitleBarHelper.ApplySystemThemeToCaptionButtons();
        });
    }
}
