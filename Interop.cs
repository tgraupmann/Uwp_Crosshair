// Ref: https://www.travelneil.com/wndproc-in-uwp.html

using System;
using System.Runtime.InteropServices;
using System.Text;

public class Interop
{
    [ComImport, Guid("45D64A29-A63E-4CB6-B498-5781D298CB4F")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface ICoreWindowInterop
    {
        IntPtr WindowHandle { get; }
        bool MessageHandled { get; }
    }

    private static IntPtr GetCoreWindowHwnd()
    {
        dynamic coreWindow = Windows.UI.Core.CoreWindow.GetForCurrentThread();
        var interop = (ICoreWindowInterop)coreWindow;
        return interop.WindowHandle;
    }

    [DllImport("user32.dll", EntryPoint = "SetWindowLong")] //32-bit
    public static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

    [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")] // 64-bit
    public static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

    private const int GWLP_WNDPROC = -4;
    public delegate IntPtr WndProcDelegate(IntPtr hwnd, uint message, IntPtr wParam, IntPtr lParam);

    public static IntPtr SetWndProc(WndProcDelegate newProc)
    {
        dynamic coreWindow = Windows.UI.Core.CoreWindow.GetForCurrentThread();
        var interop = (ICoreWindowInterop)coreWindow;
        var hwnd = interop.WindowHandle;

        IntPtr functionPointer = Marshal.GetFunctionPointerForDelegate(newProc);

        if (IntPtr.Size == 8)
        {
            return SetWindowLongPtr(hwnd, GWLP_WNDPROC, functionPointer);
        }
        else
        {
            return SetWindowLong(hwnd, GWLP_WNDPROC, functionPointer);
        }
    }

    [DllImport("user32.dll")]
    public static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr DefWindowProc(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);


    [DllImport("user32.dll")]
    public static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

    public static string GetActiveWindowTitle()
    {
        const int nChars = 256;
        StringBuilder Buff = new StringBuilder(nChars);
        IntPtr handle = GetForegroundWindow();

        if (GetWindowText(handle, Buff, nChars) > 0)
        {
            return Buff.ToString();
        }
        return null;
    }
}
