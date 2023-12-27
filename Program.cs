using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lottery539
{
    static class Program
    {
        static string filePath = Application.StartupPath;
        //不異動的檔案
        static List<string> continueFiles = new List<string> { "Lottery.txt", "AutoLottery539.exe", "config.ini" };
        static log log = new log();
        [STAThread]
        static void Main()
        {
            try
            {
                string downloadUrl;
                bool updateAvailable = UpdateLottery539.IsUpdate(out downloadUrl);

                if (updateAvailable)
                {
                    // Move all files (excluding directories) from the original directory to the temporary directory
                    string[] filesToMove = Directory.GetFiles(filePath);
                    foreach (string fileToMove in filesToMove)
                    {
                        //資料檔不重製
                        if (continueFiles.Contains(Path.GetFileName(fileToMove)))
                            continue;
                        string fileName = Path.GetFileName(fileToMove);
                        string tempFilePathForFile = Path.Combine(Path.GetTempPath(), fileName);
                        if (File.Exists(tempFilePathForFile))
                        {
                            File.Delete(tempFilePathForFile);
                        }
                        File.Move(fileToMove, tempFilePathForFile);
                    }

                    UpdateLottery539.DownloadFileAsync(downloadUrl, filePath);

                    UpdateLottery539.MoveAndReplaceFiles();

                    // Schedule commands to be executed after a delay
                    string tempFilePath = Path.Combine(Path.GetTempPath(), "Lottery539_temp.exe");
                    ScheduleCommands(tempFilePath);
                    log.WriteLog("更新應用程式完成!");
                    // Exit the main application
                    Application.Exit();
                    return;
                }
            }
            catch (Exception ex)
            {
                log.WriteLog(ex.ToString());
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        static void ScheduleCommands(string tempFilePath)
        {
            Process P_sources = new Process();
            //設定一秒延遲,讓程式順利關閉才能刪除 
            P_sources.StartInfo = new ProcessStartInfo("cmd.exe", "/C choice /C Y /N /D Y /T 1 & Del " + "\"" + tempFilePath + "\"");
            P_sources.StartInfo.CreateNoWindow = true;
            P_sources.StartInfo.UseShellExecute = false;
            P_sources.Start();

            //執行新版程式自啟
            Process P_new = new Process();
            //設定一秒延遲,讓程式順利關閉才能刪除 
            P_new.StartInfo = new ProcessStartInfo("cmd.exe", "/C choice /C Y /N /D Y /T 1 & " + "\"" + Path.Combine(filePath, "Lottery539.exe") + "\"");
            P_new.StartInfo.CreateNoWindow = true;
            P_new.StartInfo.UseShellExecute = false;
            P_new.Start();
        }
    }
}