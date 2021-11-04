using System;
using System.Runtime.InteropServices;

namespace Win32Api
{
    public static partial class PowrProf
    {
        #region Public Methods

        [DllImport(AssemblyName)]
        public static extern uint PowerReadACValueIndex(IntPtr RootPowerKey, ref Guid SchemeGuid, ref Guid SubGroupOfPowerSettingsGuid, ref Guid PowerSettingGuid, ref uint AcValueIndex);

        #endregion
    }
}
