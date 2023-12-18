using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lottery539
{
    public class UpdateLottery539
    {
        private static readonly IFileSystem fileSystem = new FileSystem();
        static string owner = Helpers.GetConfigValue("GithubUser");
        static string repo = Helpers.GetConfigValue("GithubRepo");
        static log log = new log();
        public static bool IsUpdate(out string url)
        {
            string localVersion = ReadFile.ReadVersion();

            ReleaseInfo release = GetLatestRelease();

            if (release != null && IsNewVersion(release.tag_name.Trim(), localVersion.Trim()))
            {
                url = release.Assets[0].browser_download_url;
                log.WriteLog("當前版本: " + localVersion + " , 新版本: " + release.tag_name);
                return true;
            }
            else
            {
                url = string.Empty;
                return false;
            }
        }

        static ReleaseInfo GetLatestRelease()
        {
            string apiUrl = $"https://api.github.com/repos/{owner}/{repo}/releases/latest";
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
                HttpResponseMessage response = client.GetAsync(apiUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    string jsonContent = response.Content.ReadAsStringAsync().Result;
                    ReleaseInfo release = JsonConvert.DeserializeObject<ReleaseInfo>(jsonContent);
                    return release;
                }
                return null;
            }
        }

        public static void DownloadFileAsync(string url, string filePath)
        {
            filePath = Path.Combine(filePath, "Lottery539.zip");
            using (HttpClient client = new HttpClient())
            {
                // Download the file
                using (HttpResponseMessage response = client.GetAsync(url).Result)
                {
                    response.EnsureSuccessStatusCode();

                    using (Stream contentStream = response.Content.ReadAsStreamAsync().Result,
                                  stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
                    {
                        contentStream.CopyToAsync(stream);
                    }
                }
            }

            // Unzip the file
            string extractPath = Path.GetDirectoryName(filePath);
            //壓縮檔內有一層publish，先判斷有無存在先刪除否則解壓縮會異常
            string publishPath = Path.Combine(extractPath, "publish");
            if (Directory.Exists(publishPath))
            {
                Directory.Delete(publishPath, true);
            }
            ZipFile.ExtractToDirectory(filePath, extractPath);

            // Optionally, you can delete the original zip file if needed
            File.Delete(filePath);
        }

        public static void MoveAndReplaceFiles()
        {
            string sourceDirectory = Path.Combine(Application.StartupPath, "publish");
            string destinationDirectory = Application.StartupPath;

            // 检查源目录是否存在
            if (fileSystem.Directory.Exists(sourceDirectory))
            {
                // 复制源目录下的所有文件到目标目录，并允许覆盖现有文件
                foreach (string filePath in fileSystem.Directory.GetFiles(sourceDirectory))
                {
                    string fileName = fileSystem.Path.GetFileName(filePath);
                    string destinationPath = fileSystem.Path.Combine(destinationDirectory, fileName);

                    fileSystem.File.Copy(filePath, destinationPath, true);
                }

                // 删除源目录
                fileSystem.Directory.Delete(sourceDirectory, true);
            }
        }

        static bool IsNewVersion(string latestVersion, string currentVersion)
        {
            return string.Compare(latestVersion, currentVersion) > 0;
        }
    }

    public class ReleaseInfo
    {
        public string tag_name { get; set; }
        public Asset[] Assets { get; set; }
    }
    public class Asset
    {
        public string browser_download_url { get; set; }
    }
}
