using Activator.NET.Properties;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Win32Api;

namespace Activator.NET
{
    public partial class FormMain : Form
    {
        #region Private Fields

        private uint hibernateTimeout = uint.MaxValue;
        private uint screenSaverTimeout = uint.MaxValue;
        private uint standbyTimeout = uint.MaxValue;
        private readonly bool supportsVideoDim = false;
        private uint videoDimTimeout = uint.MaxValue;
        private uint videoPowerDownTimeout = uint.MaxValue;

        #endregion

        #region Public Methods

        public FormMain()
        {
            InitializeComponent();
            Version version = Environment.OSVersion.Version;

            if ((version.Major > 6) || ((version.Major == 6) && (version.Minor > 2)))
            {
                supportsVideoDim = true;
            }

            SetEnabled(true);
        }

        #endregion

        #region Private Methods

        private uint GetMinimum(uint value, params uint[] values)
        {
            uint result = value;

            foreach (uint value_ in values)
            {
                result = Math.Min(value, value_);
            }

            return result;
        }

        private bool GetMinimumTimeout()
        {
            GetPowerTimeouts();
            screenSaverTimeout = GetScreenSaverTime();
            uint interval = GetMinimum(screenSaverTimeout, hibernateTimeout, standbyTimeout, videoDimTimeout, videoPowerDownTimeout);

            if (interval == uint.MaxValue)
            {
                return false;
            }

            timer.Interval = (int)(interval * 1000 / 2);
            return true;
        }

        private bool GetPowerTimeout(Guid subGroupOfPowerSettingsGuid, Guid powerSettingGuid, out uint result)
        {
            IntPtr guid = IntPtr.Zero;
            result = uint.MaxValue;

            if (PowrProf.PowerGetActiveScheme(IntPtr.Zero, ref guid) != ERROR.SUCCESS)
            {
                return false;
            }

            Guid activePolicyGuid;

            try
            {
                activePolicyGuid = Marshal.PtrToStructure<Guid>(guid);
                uint value = uint.MinValue;
                bool passed = (PowrProf.PowerReadACValueIndex(IntPtr.Zero, ref activePolicyGuid, ref subGroupOfPowerSettingsGuid, ref powerSettingGuid, ref value) == ERROR.SUCCESS);

                if (passed)
                {
                    result = ((value > 0) ? value : uint.MaxValue);
                }

                return passed;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (guid != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(guid);
                }
            }
        }

        private void GetPowerTimeouts()
        {
            GetPowerTimeout(GUID.SLEEP_SUBGROUP, GUID.HIBERNATE_TIMEOUT, out hibernateTimeout);
            GetPowerTimeout(GUID.SLEEP_SUBGROUP, GUID.STANDBY_TIMEOUT, out standbyTimeout);

            if (supportsVideoDim)
            {
                GetPowerTimeout(GUID.VIDEO_SUBGROUP, GUID.VIDEO_DIM_TIMEOUT, out videoDimTimeout);
            }

            GetPowerTimeout(GUID.VIDEO_SUBGROUP, GUID.VIDEO_POWERDOWN_TIMEOUT, out videoPowerDownTimeout);
        }

        private uint GetScreenSaverTime()
        {
            uint maxValue = uint.MaxValue;

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("Control Panel\\Desktop"))
            {
                if (key.GetValue("SCRNSAVE.EXE") == null)
                {
                    return maxValue;
                }
            }

            IntPtr param = Marshal.AllocHGlobal(Marshal.SizeOf<uint>());
            uint result;

            try
            {
                if (!User32.SystemParametersInfo(SPI.GETSCREENSAVETIMEOUT, 0, param, 0))
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }

                result = Marshal.PtrToStructure<uint>(param);
            }
            catch
            {
                return maxValue;
            }
            finally
            {
                Marshal.FreeHGlobal(param);
            }

            return result;
        }

        private void SetEnabled(bool enabled)
        {
            if (enabled)
            {
                SetTimer();
                notifyIcon.Icon = Resources.Small;
            }
            else
            {
                timer.Stop();
                notifyIcon.Icon = Resources.Gray;
            }
        }

        private void SetThreadExecutionState()
        {
            Kernel32.SetThreadExecutionState(ES.SYSTEM_REQUIRED | ES.DISPLAY_REQUIRED);
        }

        private void SetTimer()
        {
            SetThreadExecutionState();

            if (!GetMinimumTimeout())
            {
                timer.Interval = 60000;
            }

            timer.Start();
        }

        #endregion

        // Designer's Methods

        private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            toolStripMenuItemEnabled.Checked = timer.Enabled;
        }

        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                toolStripMenuItemEnabled.PerformClick();
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            SetTimer();
        }

        private void toolStripMenuItemEnabled_Click(object sender, EventArgs e)
        {
            SetEnabled(!timer.Enabled);
        }

        private void toolStripMenuItemQuit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void visibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                Hide();
            }
        }
    }
}
