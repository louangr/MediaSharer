using MediaSharer.Core;
using Microsoft.UI.Xaml;
using System;
using WinRT;

namespace MediaSharer.Utils
{
    public static class WinApi
    {
        public static void LoadIcon(Window window, string iconName)
        {
            var hwnd = window.As<IWindowNative>().WindowHandle;
            IntPtr hIcon = PInvoke.User32.LoadImage(IntPtr.Zero, iconName, PInvoke.User32.ImageType.IMAGE_ICON, 16, 16, PInvoke.User32.LoadImageFlags.LR_LOADFROMFILE);
            PInvoke.User32.SendMessage(hwnd, PInvoke.User32.WindowMessage.WM_SETICON, (IntPtr)0, hIcon);
        }
    }
}
