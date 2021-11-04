using System.Runtime.InteropServices;

namespace Win32Api
{
    public static partial class Kernel32
    {
        #region Public Methods

        [DllImport(AssemblyName, SetLastError = true)]
        public static extern ES SetThreadExecutionState(ES esFlags);

        #endregion
    }
}
