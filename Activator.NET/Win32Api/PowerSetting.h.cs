using System;
using System.Runtime.InteropServices;

namespace Win32Api
{
    public static partial class PowrProf
    {
        #region Public Methods

        [DllImport(AssemblyName)]
        public static extern uint PowerGetActiveScheme(IntPtr UserRootPowerKey, ref IntPtr ActivePolicyGuid);

        #endregion
    }
}
