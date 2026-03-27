using Newtonsoft.Json.Linq;
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
        private static readonly string KnownGoodVersionsApi = "https://googlechromelabs.github.io/chrome-for-testing/known-good-versions-with-downloads.json";

        public void UpdateChromDriverFun()
        {
            string exeDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string chromeVersion = GetInstalledChromeVersion();
            string driverVersion = GetInstalledDriverVersion(exeDir);

            if (!string.Equals(driverVersion, chromeVersion, StringComparison.OrdinalIgnoreCase))
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
            string json = HttpGet(KnownGoodVersionsApi);
            JObject obj = JObject.Parse(json);
            JArray versions = obj["versions"] as JArray;
            if (versions == null)
                return null;

            JObject matchedVersion = versions
                .OfType<JObject>()
                .FirstOrDefault(v => string.Equals(v["version"]?.ToString(), chromeVersion, StringComparison.OrdinalIgnoreCase));

            if (matchedVersion == null)
            {
                string buildPrefix = GetBuildPrefix(chromeVersion);
                matchedVersion = versions
                    .OfType<JObject>()
                    .Where(v => v["version"] != null && v["version"].ToString().StartsWith(buildPrefix + ".", StringComparison.OrdinalIgnoreCase))
                    .OrderByDescending(v => ParseVersion(v["version"].ToString()))
                    .FirstOrDefault();
            }

            return GetPlatformDownloadUrl(matchedVersion, "win64");
        }

        private string GetPlatformDownloadUrl(JObject versionInfo, string platform)
        {
            if (versionInfo == null)
                return null;

            JArray downloads = versionInfo.SelectToken("downloads.chromedriver") as JArray;
            if (downloads == null)
                return null;

            JObject matchedDownload = downloads
                .OfType<JObject>()
                .FirstOrDefault(item => string.Equals(item["platform"]?.ToString(), platform, StringComparison.OrdinalIgnoreCase));

            return matchedDownload?["url"]?.ToString();
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

        private Version ParseVersion(string version)
        {
            Version parsedVersion;
            return Version.TryParse(version, out parsedVersion) ? parsedVersion : new Version(0, 0);
        }

        private void DownloadAndExtractDriver(string url, string targetPath)
        {
            string zipPath = Path.Combine(targetPath, "chromedriver.zip");
            string folderPath = Path.Combine(targetPath, ChromeDriverFolderName);

            if (Directory.Exists(folderPath))
                Directory.Delete(folderPath, true);

            using (var wc = new WebClient())
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
