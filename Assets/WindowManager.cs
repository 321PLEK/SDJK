using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using UnityEngine;
//using UnityEngine;

public class WindowManager : MonoBehaviour
{
    static float x = 0;
    static float y = 0;
    static IntPtr handle;
    #if UNITY_STANDALONE_WIN
    [DllImport("user32.dll", SetLastError = true)]
    static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);

    private const int SWP_NOSIZE = 0x0001;
    private const int SWP_NOZORDER = 0x0004;
    private const int SWP_SHOWWINDOW = 0x0040;

    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool GetWindowRect(IntPtr hWnd, out Rectangle lpRect);

    [DllImport("user32.dll", EntryPoint = "FindWindow")]
    static extern IntPtr FindWindow(string LpClassName, string lpWindowName);

    public static void GetWindowResolution(out int width, out int height)
    {
        Rectangle rectangle;
        IntPtr temp = GetWindowHandle();
        if (temp != IntPtr.Zero)
            handle = temp;

        GetWindowRect(handle, out rectangle);
        width = rectangle.Width - rectangle.X;
        height = rectangle.Height - rectangle.Y;
    }

    public static IntPtr GetWindowHandle() => GetActiveWindow();

    public static void SetWindowPosition(float x2, float y2, datumPoint windowDatumPoint = datumPoint.Center, datumPoint screenDatumPoint = datumPoint.Center, bool Lerp = false, float time = 0.05f)
    {
        IntPtr temp = GetWindowHandle();
        if (temp != IntPtr.Zero)
            handle = temp;

        GetWindowResolution(out int width, out int height);

        if (windowDatumPoint == datumPoint.Center)
        {
            x2 -= width * 0.5f;
            y2 -= height * 0.5f;
        }
        else if (windowDatumPoint == datumPoint.LeftBottom)
            y2 -= height;
        else if (windowDatumPoint == datumPoint.LeftCenter)
            y2 -= height * 0.5f;
        else if (windowDatumPoint == datumPoint.RightBottom)
        {
            x2 -= width;
            y2 -= height;
        }
        else if (windowDatumPoint == datumPoint.RightCenter)
        {
            x2 -= width;
            y2 -= height * 0.5f;
        }
        else if (windowDatumPoint == datumPoint.RightTop)
            x2 -= width;
        else if (windowDatumPoint == datumPoint.CenterTop)
            x2 -= width * 0.5f;
        else if (windowDatumPoint == datumPoint.CenterBottom)
        {
            x2 -= width * 0.5f;
            y2 -= height;
        }

        if (screenDatumPoint == datumPoint.Center)
        {
            x2 += Screen.currentResolution.width * 0.5f;
            y2 += Screen.currentResolution.height * 0.5f;
        }
        else if (screenDatumPoint == datumPoint.LeftBottom)
            y2 += Screen.currentResolution.height;
        else if (screenDatumPoint == datumPoint.LeftCenter)
            y2 += Screen.currentResolution.height * 0.5f;
        else if (screenDatumPoint == datumPoint.RightBottom)
        {
            x2 += Screen.currentResolution.width;
            y2 += Screen.currentResolution.height;
        }
        else if (screenDatumPoint == datumPoint.RightCenter)
        {
            x2 += Screen.currentResolution.width;
            y2 += Screen.currentResolution.height * 0.5f;
        }
        else if (screenDatumPoint == datumPoint.RightTop)
            x2 += Screen.currentResolution.width;
        else if (screenDatumPoint == datumPoint.CenterTop)
            x2 += Screen.currentResolution.width * 0.5f;
        else if (screenDatumPoint == datumPoint.CenterBottom)
        {
            x2 += Screen.currentResolution.width * 0.5f;
            y2 += Screen.currentResolution.height;
        }

        if (!Lerp)
        {
            x = x2;
            y = y2;
        }
        else
        {
            x = Mathf.Lerp(x, x2, time * 60 * Time.deltaTime);
            y = Mathf.Lerp(y, y2, time * 60 * Time.deltaTime);
        }

        if (!Lerp)
            SetWindowPos(handle, IntPtr.Zero, Mathf.RoundToInt(x2), Mathf.RoundToInt(y2), 0, 0, SWP_NOZORDER | SWP_NOSIZE | SWP_SHOWWINDOW);
        else
            SetWindowPos(handle, IntPtr.Zero, Mathf.RoundToInt(x), Mathf.RoundToInt(y), 0, 0, SWP_NOZORDER | SWP_NOSIZE | SWP_SHOWWINDOW);
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