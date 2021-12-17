using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace Grid2Visualizer
{
    internal static class WindowExtensions
    {
        public static void SetOwner(this Window window, IntPtr hwndParent)
        {
            new WindowInteropHelper(window).Owner = hwndParent;
        }

        public static void RemoveIcon(this Window window)
        {
            // Get this window's handle
            IntPtr hwnd = GetWindowHandle(window);

            WS_EX extendedStyle = GetWindowStyleEx(hwnd);
            Debug.Assert(0 != extendedStyle, "Could not get extended window style");

            if (0 != extendedStyle)
            {
                // Change the extended window style to not show a window icon
                SetWindowStyleEx(hwnd, extendedStyle | WS_EX.DLGMODALFRAME);

                // Reset the large and small icon setting
                SetIcon(hwnd, ICON.SMALL);
                SetIcon(hwnd, ICON.BIG);

                // Update the window's non-client area to reflect the changes
                RefreshNonClientArea(hwnd);
            }
        }

        public static void RemoveMinButton(this Window window)
        {
            // Get this window's handle
            IntPtr hwnd = GetWindowHandle(window);

            WS style = GetWindowStyle(hwnd);
            Debug.Assert(0 != style, "Could not get window style");

            if (0 != style)
            {
                // Remove the minimize button
                SetWindowStyle(hwnd, style & ~WS.MINIMIZEBOX);

                // Update the window's non-client area to reflect the changes
                RefreshNonClientArea(hwnd);
            }
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hwnd, GWL index);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int SetWindowLong(IntPtr hwnd, GWL index, int newStyle);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter, int x, int y, int width, int height, SWP flags);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SendMessage(IntPtr hwnd, WM msg, IntPtr wParam, IntPtr lParam);

        private static WS GetWindowStyle(IntPtr hwnd) => (WS)GetWindowLong(hwnd, GWL.STYLE);

        private static void SetWindowStyle(IntPtr hwnd, WS style) => SetWindowLong(hwnd, GWL.STYLE, (int)style);

        private static WS_EX GetWindowStyleEx(IntPtr hwnd) => (WS_EX)GetWindowLong(hwnd, GWL.EXSTYLE);

        private static void SetWindowStyleEx(IntPtr hwnd, WS_EX style) => SetWindowLong(hwnd, GWL.EXSTYLE, (int)style);

        private static void SetIcon(IntPtr hwnd, ICON icon) => SendMessage(hwnd, WM.SETICON, (IntPtr)icon, IntPtr.Zero);

        private static void RefreshNonClientArea(IntPtr hwnd) => SetWindowPos(hwnd, IntPtr.Zero, 0, 0, 0, 0, SWP.NOMOVE | SWP.NOSIZE | SWP.NOZORDER | SWP.FRAMECHANGED);

        private static IntPtr GetWindowHandle(Window window) => new WindowInteropHelper(window).EnsureHandle();

        private enum WM : uint
        {
            SETICON = 0x0080,
        }

        private enum ICON : int
        {
            SMALL = 0,
            BIG = 1,
        }

        private enum GWL
        {
            STYLE = -16,
            EXSTYLE = -20,
        }

        [Flags]
        private enum WS : int
        {
            MAXIMIZEBOX = 0x00010000,
            MINIMIZEBOX = 0x00020000,
            SYSMENU = 0x00080000,
        }

        [Flags]
        private enum WS_EX : int
        {
            DLGMODALFRAME = 0x0001,
        }

        [Flags]
        private enum SWP : uint
        {
            FRAMECHANGED = 0x0020,
            NOMOVE = 0x0002,
            NOSIZE = 0x0001,
            NOZORDER = 0x0004,
        }
    }
}
