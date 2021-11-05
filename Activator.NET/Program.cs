using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Activator.NET
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            string guid = Assembly.GetExecutingAssembly().GetCustomAttribute<GuidAttribute>().Value;
            var mutex = new Mutex(false, guid);
            bool hasHandle = false;

            try
            {
                Application.EnableVisualStyles();

                try
                {
                    if (!mutex.WaitOne(0, false))
                    {
                        MessageBox.Show("多重起動はできません。", "Activator.NET", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                catch (AbandonedMutexException)
                {
                }

                hasHandle = true;
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FormMain());
            }
            finally
            {
                if (hasHandle)
                {
                    mutex.ReleaseMutex();
                }

                mutex.Close();
            }
        }
    }
}
