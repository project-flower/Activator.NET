using System;

namespace Win32Api
{
    public enum ES : uint
    {
        SYSTEM_REQUIRED = 0x00000001,
        DISPLAY_REQUIRED = 0x00000002,
        USER_PRESENT = 0x00000004,
        AWAYMODE_REQUIRED = 0x00000040,
        CONTINUOUS = 0x80000000
    }

    public static partial class GUID
    {
        #region Public Fields

        public static readonly Guid HIBERNATE_TIMEOUT = new Guid("9d7815a6-7ee4-497e-8888-515a05f02364");
        public static readonly Guid SLEEP_SUBGROUP = new Guid("238c9fa8-0aad-41ed-83f4-97be242c8f20");
        public static readonly Guid STANDBY_TIMEOUT = new Guid("29f6c1db-86da-48c5-9fdb-f2b67b1f44da");
        public static readonly Guid VIDEO_DIM_TIMEOUT = new Guid("17aaa29b-8b43-4b94-aafe-35f64daaf1ee");
        public static readonly Guid VIDEO_POWERDOWN_TIMEOUT = new Guid("3c0bc021-c8a8-4e07-a973-6b14cbcb2b7e");
        public static readonly Guid VIDEO_SUBGROUP = new Guid("7516b95f-f776-4464-8c53-06167f40cc99");

        #endregion
    }
}
