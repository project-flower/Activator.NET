using System;
using System.Runtime.InteropServices;

namespace Win32Api
{
    public static class SPI
    {
        #region Public Fields

        public const uint GETSCREENSAVETIMEOUT = 0x000E;

        #endregion
    }

    public static partial class User32
    {
        #region Public Methods

        [DllImport(AssemblyName, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SystemParametersInfo(uint uiAction, uint uiParam, IntPtr pvParam, uint fWinIni);

        #endregion
    }
}
