using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lottery539
{
    public class UpdateLottery539
    {
        static string owner = Helpers.GetConfigValue("GithubUser");
        static string repo = Helpers.GetConfigValue("GithubRepo");
        public static bool IsUpdate(out string url)
        {
            string localVersion = ReadFile.ReadVersion(); 
            string executablePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            ReleaseInfo release = GetLatestRelease();

            if (release != null && IsNewVersion(release.tag_name, localVersion))
            {
                url = release.Assets[0].browser_download_url;
                return true;
            }
            else
            {
                url=string.Empty;
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
            ZipFile.ExtractToDirectory(filePath, extractPath);

            // Optionally, you can delete the original zip file if needed
            File.Delete(filePath);
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
