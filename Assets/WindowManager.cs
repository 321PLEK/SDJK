using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    static float x = 0;
    static float y = 0;
    static IntPtr handle;
    #if UNITY_STANDALONE_WIN
    [DllImport("user32.dll", SetLastError = true)]
    static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);

    private const int SWP_NOSIZE = 0x0001;
    private const int SWP_NOMOVE = 0x0002;
    private const int SWP_NOZORDER = 0x0004;
    private const int SWP_SHOWWINDOW = 0x0040;

    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left;        // x position of upper-left corner
        public int Top;         // y position of upper-left corner
        public int Right;       // x position of lower-right corner
        public int Bottom;      // y position of lower-right corner
    }

    [DllImport("user32.dll", EntryPoint = "FindWindow")]
    static extern IntPtr FindWindow(string LpClassName, string lpWindowName);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

    public static void GetWindowResolution(out int width, out int height)
    {
        RECT rect;
        IntPtr temp = GetWindowHandle();
        if (temp != IntPtr.Zero)
            handle = temp;

        GetWindowRect(handle, out rect);
        width = rect.Right - rect.Left;
        height = rect.Bottom - rect.Top;
    }

    public static IntPtr GetWindowHandle() => GetActiveWindow();

    public static Vector2 GetWindowSize()
    {
#if UNITY_STANDALONE_WIN
        IntPtr temp = GetWindowHandle();
        if (temp != IntPtr.Zero)
            handle = temp;

        GetWindowRect(handle, out RECT rect);
        return new Vector2(rect.Right - rect.Left, rect.Bottom - rect.Top);
#else
            return Vector2.zero;
#endif
    }

    public static Vector2 GetClientSize()
    {
#if UNITY_STANDALONE_WIN
        IntPtr temp = GetWindowHandle();
        if (temp != IntPtr.Zero)
            handle = temp;

        GetClientRect(handle, out RECT rect);
        return new Vector2(rect.Right, rect.Bottom);
#else
            return Vector2.zero;
#endif
    }

    public static void SetWindowPosition(float x, float y, int width, int height, datumPoint windowDatumPoint = datumPoint.Center, datumPoint screenDatumPoint = datumPoint.Center, bool Lerp = false, float time = 0.05f)
    {
        IntPtr temp = GetWindowHandle();
        if (temp != IntPtr.Zero)
            handle = temp;

        Vector2 border = GetWindowSize() - GetClientSize();

        SetWindowPos(handle, IntPtr.Zero, 0, 0, Mathf.RoundToInt(width + border.x), Mathf.RoundToInt(height + border.y), SWP_NOZORDER | SWP_NOMOVE | SWP_SHOWWINDOW);
        GetWindowResolution(out int width2, out int height2);

        if (windowDatumPoint == datumPoint.Center)
        {
            x -= width2 * 0.5f;
            y -= height2 * 0.5f;
        }
        else if (windowDatumPoint == datumPoint.LeftBottom)
            y -= height2;
        else if (windowDatumPoint == datumPoint.LeftCenter)
            y -= height2 * 0.5f;
        else if (windowDatumPoint == datumPoint.RightBottom)
        {
            x -= width2;
            y -= height2;
        }
        else if (windowDatumPoint == datumPoint.RightCenter)
        {
            x -= width2;
            y -= height2 * 0.5f;
        }
        else if (windowDatumPoint == datumPoint.RightTop)
            x -= width2;
        else if (windowDatumPoint == datumPoint.CenterTop)
            x -= width2 * 0.5f;
        else if (windowDatumPoint == datumPoint.CenterBottom)
        {
            x -= width2 * 0.5f;
            y -= height2;
        }

        if (screenDatumPoint == datumPoint.Center)
        {
            x += Screen.currentResolution.width * 0.5f;
            y += Screen.currentResolution.height * 0.5f;
        }
        else if (screenDatumPoint == datumPoint.LeftBottom)
            y += Screen.currentResolution.height;
        else if (screenDatumPoint == datumPoint.LeftCenter)
            y += Screen.currentResolution.height * 0.5f;
        else if (screenDatumPoint == datumPoint.RightBottom)
        {
            x += Screen.currentResolution.width;
            y += Screen.currentResolution.height;
        }
        else if (screenDatumPoint == datumPoint.RightCenter)
        {
            x += Screen.currentResolution.width;
            y += Screen.currentResolution.height * 0.5f;
        }
        else if (screenDatumPoint == datumPoint.RightTop)
            x += Screen.currentResolution.width;
        else if (screenDatumPoint == datumPoint.CenterTop)
            x += Screen.currentResolution.width * 0.5f;
        else if (screenDatumPoint == datumPoint.CenterBottom)
        {
            x += Screen.currentResolution.width * 0.5f;
            y += Screen.currentResolution.height;
        }

        if (!Lerp)
        {
            WindowManager.x = x;
            WindowManager.y = y;
        }
        else
        {
            WindowManager.x = Mathf.Lerp(WindowManager.x, x, time * 60 * Time.deltaTime);
            WindowManager.y = Mathf.Lerp(WindowManager.y, y, time * 60 * Time.deltaTime);
        }

        if (!Lerp)
            SetWindowPos(handle, IntPtr.Zero, Mathf.RoundToInt(x), Mathf.RoundToInt(y), 0, 0, SWP_NOZORDER | SWP_NOSIZE | SWP_SHOWWINDOW);
        else
            SetWindowPos(handle, IntPtr.Zero, Mathf.RoundToInt(WindowManager.x), Mathf.RoundToInt(WindowManager.y), 0, 0, SWP_NOZORDER | SWP_NOSIZE | SWP_SHOWWINDOW);
    }
    #endif

    public enum datumPoint
    {
        LeftTop,
        RightTop,
        LeftCenter,
        RightCenter,
        LeftBottom,
        RightBottom,
        CenterTop,
        CenterBottom,
        Center
    }
}