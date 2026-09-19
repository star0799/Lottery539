using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;

namespace Lottery539
{
    public class UpdateChromeDriver
    {
        private static readonly string ChromeDriverExe = "chromedriver.exe";
        private static readonly string ChromeDriverFolderName = "chromedriver-win64";
        private static readonly string LatestReleaseApi = "https://googlechromelabs.github.io/chrome-for-testing/LATEST_RELEASE_";
        private static readonly string DownloadBaseUrl = "https://storage.googleapis.com/chrome-for-testing-public";

        public void UpdateChromDriverFun()
        {
            string exeDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string chromeVersion = GetInstalledChromeVersion();
            string driverVersion = GetInstalledDriverVersion(exeDir);

            // Chrome for Testing may use a different patch number for the same Chrome build.
            if (string.IsNullOrWhiteSpace(driverVersion) ||
                !string.Equals(GetBuildPrefix(driverVersion), GetBuildPrefix(chromeVersion), StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"更新 ChromeDriver：{driverVersion} -> {chromeVersion}");
                KillAllChromeDriverProcesses();

                string downloadUrl = GetMatchingDriverDownloadUrl(chromeVersion);
                if (string.IsNullOrWhiteSpace(downloadUrl))
                    throw new Exception($"找不到與 Chrome {chromeVersion} 相容的 ChromeDriver 下載網址");

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

                if (!File.Exists(chromePath))
                    throw new FileNotFoundException("找不到 Chrome 執行檔");

                return FileVersionInfo.GetVersionInfo(chromePath).FileVersion;
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
                return string.Empty;

            try
            {
                ProcessStartInfo psi = new ProcessStartInfo(exePath, "--version")
                {
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process p = Process.Start(psi))
                {
                    string output = p.StandardOutput.ReadToEnd();
                    p.WaitForExit();

                    string[] parts = output.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    return parts.Length > 1 ? parts[1].Trim() : string.Empty;
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        private string GetMatchingDriverDownloadUrl(string chromeVersion)
        {
            string buildPrefix = GetBuildPrefix(chromeVersion);
            string driverVersion = HttpGet(LatestReleaseApi + buildPrefix).Trim();
            Version parsedVersion;
            if (!Version.TryParse(driverVersion, out parsedVersion) ||
                !string.Equals(GetBuildPrefix(driverVersion), buildPrefix, StringComparison.OrdinalIgnoreCase))
                return null;

            return $"{DownloadBaseUrl}/{driverVersion}/win64/chromedriver-win64.zip";
        }

        private string GetBuildPrefix(string version)
        {
            if (string.IsNullOrWhiteSpace(version))
                return string.Empty;

            string[] parts = version.Split('.');
            if (parts.Length < 3)
                return version;

            return string.Join(".", parts.Take(3));
        }

        private void DownloadAndExtractDriver(string url, string targetPath)
        {
            string zipPath = Path.Combine(targetPath, "chromedriver.zip");
            string folderPath = Path.Combine(targetPath, ChromeDriverFolderName);

            if (Directory.Exists(folderPath))
                Directory.Delete(folderPath, true);

            using (var wc = new TimeoutWebClient())
            {
                if (File.Exists(zipPath))
                    File.Delete(zipPath);

                wc.DownloadFile(url, zipPath);
            }

            System.IO.Compression.ZipFile.ExtractToDirectory(zipPath, targetPath);

            string sourceExe = Path.Combine(folderPath, ChromeDriverExe);
            string destExe = Path.Combine(targetPath, ChromeDriverExe);

            if (File.Exists(destExe))
                File.Delete(destExe);

            File.Move(sourceExe, destExe);

            Directory.Delete(folderPath, true);
            File.Delete(zipPath);
        }

        private string HttpGet(string url)
        {
            using (var wc = new TimeoutWebClient())
            {
                wc.Headers.Add("User-Agent", "Mozilla/5.0");
                return wc.DownloadString(url);
            }
        }

        private sealed class TimeoutWebClient : WebClient
        {
            protected override WebRequest GetWebRequest(Uri address)
            {
                WebRequest request = base.GetWebRequest(address);
                request.Timeout = 15000;
                HttpWebRequest httpRequest = request as HttpWebRequest;
                if (httpRequest != null)
                    httpRequest.ReadWriteTimeout = 15000;
                return request;
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
