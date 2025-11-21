using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace Lottery539
{
    public class UpdateChromeDriver
    {
        private static string ChromeDriverExe = "chromedriver.exe";
        private static string ChromeDriverFolderName = "chromedriver-win64"; // Google 新版的資料夾名稱
        private static string LatestVersionApi = "https://googlechromelabs.github.io/chrome-for-testing/last-known-good-versions-with-downloads.json";

        public void UpdateChromDriverFun()
        {
            string exeDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string chromeVersion = GetInstalledChromeVersion();
            string driverVersion = GetInstalledDriverVersion(exeDir);

            if (driverVersion != chromeVersion)
            {
                Console.WriteLine($"更新 ChromeDriver： {driverVersion} -> {chromeVersion}");
                KillAllChromeDriverProcesses();

                string downloadUrl = GetLatestDriverDownloadUrl(chromeVersion);
                if (downloadUrl == null)
                {
                    Console.WriteLine("找不到對應版本，下載最新 LKG（Last Known Good）版本");
                    downloadUrl = GetLastKnownGoodDriverUrl();
                }

                DownloadAndExtractDriver(downloadUrl, exeDir);
                Console.WriteLine("ChromeDriver 更新完成！");
            }
            else
            {
                Console.WriteLine("ChromeDriver 已為最新版本");
            }
        }

        private string GetInstalledChromeVersion()
        {
            try
            {
                string chromePath = @"C:\Program Files\Google\Chrome\Application\chrome.exe";
                if (!File.Exists(chromePath))
                    chromePath = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe";

                var version = FileVersionInfo.GetVersionInfo(chromePath).FileVersion;
                return version.Split('.')[0]; // 大版本號即可，如 120
            }
            catch
            {
                throw new Exception("無法取得已安裝的 Chrome 版本");
            }
        }

        private string GetInstalledDriverVersion(string path)
        {
            string exePath = Path.Combine(path, ChromeDriverExe);
            if (!File.Exists(exePath))
                return "";

            try
            {
                ProcessStartInfo psi = new ProcessStartInfo(exePath, "--version")
                {
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                var p = Process.Start(psi);
                string output = p.StandardOutput.ReadToEnd();

                return output.Split(' ')[1].Split('.')[0]; // 大版本號，如 120
            }
            catch
            {
                return "";
            }
        }

        private string GetLatestDriverDownloadUrl(string chromeMainVersion)
        {
            string json = HttpGet(LatestVersionApi);
            dynamic obj = JsonConvert.DeserializeObject(json);

            foreach (var item in obj.channels.Stable.downloads.chromedriver)
            {
                if (item.platform.ToString() == "win64")
                {
                    string version = obj.channels.Stable.version;
                    if (version.ToString().StartsWith(chromeMainVersion))
                    {
                        return item.url;
                    }
                }
            }
            return null;
        }

        private string GetLastKnownGoodDriverUrl()
        {
            string json = HttpGet(LatestVersionApi);
            dynamic obj = JsonConvert.DeserializeObject(json);

            foreach (var item in obj.channels.Stable.downloads.chromedriver)
            {
                if (item.platform.ToString() == "win64")
                {
                    return item.url;
                }
            }
            return null;
        }

        private void DownloadAndExtractDriver(string url, string targetPath)
        {
            string zipPath = Path.Combine(targetPath, "chromedriver.zip");

            using (var wc = new WebClient())
            {
                if (File.Exists(zipPath)) File.Delete(zipPath);
                wc.DownloadFile(url, zipPath);
            }

            // 解壓縮
            System.IO.Compression.ZipFile.ExtractToDirectory(zipPath, targetPath);

            string sourceExe = Path.Combine(targetPath, ChromeDriverFolderName, ChromeDriverExe);
            string destExe = Path.Combine(targetPath, ChromeDriverExe);

            if (File.Exists(destExe))
                File.Delete(destExe);

            File.Move(sourceExe, destExe);

            Directory.Delete(Path.Combine(targetPath, ChromeDriverFolderName), true);
            File.Delete(zipPath);
        }

        private string HttpGet(string url)
        {
            using (var wc = new WebClient())
            {
                wc.Headers.Add("User-Agent", "Mozilla/5.0");
                return wc.DownloadString(url);
            }
        }

        private void KillAllChromeDriverProcesses()
        {
            foreach (var p in Process.GetProcessesByName("chromedriver"))
            {
                try { p.Kill(); } catch { }
            }
        }
    }
}
