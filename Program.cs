using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lottery539
{
    static class Program
    {
        static string filePath = Application.StartupPath;

        [STAThread]
        static void Main()
        {
            // Check for updates
            string downloadUrl;
            bool updateAvailable = UpdateLottery539.IsUpdate(out downloadUrl);

            if (updateAvailable)
            {
                // Move all files (excluding directories) from the original directory to the temporary directory
                string[] filesToMove = Directory.GetFiles(filePath);
                foreach (string fileToMove in filesToMove)
                {
                    string fileName = Path.GetFileName(fileToMove);
                    string tempFilePathForFile = Path.Combine(Path.GetTempPath(), fileName);
                    if (File.Exists(tempFilePathForFile))
                    {
                        File.Delete(tempFilePathForFile);
                    }
                    File.Move(fileToMove, tempFilePathForFile);
                }

                // Download the update
                UpdateLottery539.DownloadFileAsync(downloadUrl, filePath);

                UpdateLottery539.MoveAndReplaceFiles();

                // Schedule commands to be executed after a delay
                string tempFilePath = Path.Combine(Path.GetTempPath(), "Lottery539_temp.exe");
                ScheduleCommands(tempFilePath);

                // Exit the main application
                Application.Exit();
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        static void ScheduleCommands(string tempFilePath)
        {
            //// Specify the paths for the commands
            //string deleteCommand = $"cmd.exe /c timeout 5 && del \"{tempFilePath}\"";
            //string startCommand = $"cmd.exe /c timeout 5 && start \"\" \"{Path.Combine(filePath, "Lottery539.exe")}\"";

            //// Start processes to execute the commands
            //Task.Run(() => ExecuteCommand(deleteCommand));
            //Task.Run(() => ExecuteCommand(startCommand));

            //清除垃圾舊檔
            Process P_sources = new Process();
            //設定一秒延遲,讓程式順利關閉才能刪除 
            P_sources.StartInfo = new ProcessStartInfo("cmd.exe", "/C choice /C Y /N /D Y /T 1 & Del " + "\"" + tempFilePath + "\"");
            P_sources.StartInfo.CreateNoWindow = true;
            P_sources.StartInfo.UseShellExecute = false;
            P_sources.Start();

            //執行新版程式自啟
            Process P_new = new Process();
            //設定一秒延遲,讓程式順利關閉才能刪除 
            P_new.StartInfo = new ProcessStartInfo("cmd.exe", "/C choice /C Y /N /D Y /T 1 & " + "\"" +  Path.Combine(filePath, "Lottery539.exe")  + "\"");
            P_new.StartInfo.CreateNoWindow = true;
            P_new.StartInfo.UseShellExecute = false;
            P_new.Start();
        }

        static void ExecuteCommand(string command)
        {
            Process process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    RedirectStandardInput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
            StreamWriter sw = process.StandardInput;
            sw.WriteLine(command);
            sw.Close();
            process.WaitForExit();
        }
    }
}