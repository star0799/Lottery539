using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lottery539
{
    static class Program
    {
        static string filePath = Application.StartupPath;
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Check for updates (you need to implement this logic)
            string downloadUrl;
            bool updateAvailable = UpdateLottery539.IsUpdate(out downloadUrl);

            if (updateAvailable)
            {
                UpdateLottery539.DownloadFileAsync(downloadUrl, filePath);
                RunUpdater();
                return;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        static void RunUpdater()
        {
            // Specify the path to your updater executable
            string updaterPath = Path.Combine(filePath, "Lottery539.exe");

            // Start the updater as a separate process
            Process.Start(updaterPath);

            // Exit the main application
            Environment.Exit(0);
        }
    }
}
