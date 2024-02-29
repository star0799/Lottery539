
using Ionic.Zip;
using Microsoft.Win32;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lottery539
{
    class UpdateChromDriver
    {
        static log _log = new log();
        static string zipName = "chromedriver.zip";
        static string exeName = "chromedriver.exe";
        public void UpdateChromDriverFun()
        {
            string ChromeDriverPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string ChromDriverVersion = GetChromDriverVersion(ChromeDriverPath);
            string ChromeWebVersion = GetWebChromeVersion();
            if (ChromDriverVersion != ChromeWebVersion)
            {
                var urlToDownload = GetURLToDownload(ChromeWebVersion);
                KillAllChromeDriverProcesses();
                DownloadNewVersionOfChrome(urlToDownload, ChromeDriverPath);
                string extract = ExtructZip(ChromeDriverPath);
                MoveChromeDriver(ChromeDriverPath);
            }
        }
        static string GetChromDriverVersion(string ChromeDriverePath)
        {
            string driverversion = "";
            try
            {
                if (File.Exists(ChromeDriverePath + "\\"+ exeName))
                {
                    IWebDriver driver = new ChromeDriver(ChromeDriverePath);
                    ICapabilities capabilities = ((ChromeDriver)driver).Capabilities;
                    driverversion = ((capabilities.GetCapability("chrome") as Dictionary<string, object>)["chromedriverVersion"]).ToString().Split(' ').First();
                    driver.Dispose();
                }
                else
                {
                    Console.WriteLine("ChromeDriver.exe missing !!");
                }
            }
            catch (Exception ex)
            {
                _log.WriteLog(ex.ToString());
            }
            return driverversion;
        }
        static string GetChromeWebPath()
        {
            //Path originates from here: https://chromedriver.chromium.org/downloads/version-selection            
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\App Paths\\chrome.exe"))
            {
                if (key != null)
                {
                    Object o = key.GetValue("");
                    if (!String.IsNullOrEmpty(o.ToString()))
                    {
                        return o.ToString();
                    }
                    else
                    {
                        throw new ArgumentException("Unable to get version because chrome registry value was null");
                    }
                }
                else
                {
                    throw new ArgumentException("Unable to get version because chrome registry path was null");
                }
            }
        }
        static string GetWebChromeVersion()
        {
            string productVersionPath = GetChromeWebPath();
            if (String.IsNullOrEmpty(productVersionPath))
            {
                throw new ArgumentException("Unable to get version because path is empty");
            }

            if (!File.Exists(productVersionPath))
            {
                throw new FileNotFoundException("Unable to get version because path specifies a file that does not exists");
            }

            var versionInfo = FileVersionInfo.GetVersionInfo(productVersionPath);
            if (versionInfo != null && !String.IsNullOrEmpty(versionInfo.FileVersion))
            {
                return versionInfo.FileVersion;
            }
            else
            {
                throw new ArgumentException("Unable to get version from path because the version is either null or empty: " + productVersionPath);
            }
        }
        static string GetURLToDownload(string version)
        {
            if (string.IsNullOrEmpty(version))
            {
                throw new ArgumentException("Unable to get url because version is empty");
            }

            string completeVersion = string.Empty;
            string latestReleaseUrl = Helpers.GetConfigValue("ChromeLatestReleaseUrl");
            //urlToPathLocation為大版本
            string urlToPathLocation = latestReleaseUrl + String.Join(".", version.Split('.').Take(3));

            //去找實際完整版號
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlToPathLocation);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                completeVersion = reader.ReadToEnd();
            }

            if (String.IsNullOrEmpty(completeVersion))
            {
                throw new WebException("Unable to get version path from website");
            }
            string downloadUrl = Helpers.GetConfigValue("ChromeDownloadUrl");
            string downloadFile = Helpers.GetConfigValue("ChromeDownloadFile");
            return downloadUrl + completeVersion + downloadFile;
        }
        static void KillAllChromeDriverProcesses()
        {
            var processes = Process.GetProcessesByName("chromedriver");
            foreach (var process in processes)
            {
                try
                {
                    process.Kill();
                }
                catch
                {

                }
            }
        }
        static void DownloadNewVersionOfChrome(string urlToDownload, string ChromDriverPath)
        {
            if (String.IsNullOrEmpty(urlToDownload))
            {
                throw new ArgumentException("Unable to get url because urlToDownload is empty");
            }
            using (var client = new WebClient())
            {
                if (File.Exists(ChromDriverPath + "\\"+ zipName))
                {
                    File.Delete(ChromDriverPath + "\\"+ zipName);
                }

                client.DownloadFile(urlToDownload, zipName);

                if (File.Exists(ChromDriverPath + "\\"+ zipName) && File.Exists(ChromDriverPath + "\\"+ exeName))
                {
                    File.Delete(ChromDriverPath + "\\"+ exeName);
                }

            }
        }
        static string ExtructZip(string ChromeDriverPath)
        {
            string errorMsg = "";
            string zipFileName = "";
            try
            {
                zipFileName = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\"+zipName;
                string orgFileName = "";
                using (Ionic.Zip.ZipFile zip = Ionic.Zip.ZipFile.Read(zipFileName))
                {
                    foreach (ZipEntry e in zip)
                    {
                        e.Extract(ChromeDriverPath, ExtractExistingFileAction.OverwriteSilently);
                        orgFileName = e.FileName;
                    }
                }
                File.Delete(zipFileName);
                errorMsg = "Done";
            }
            catch (Exception ex)
            {
                errorMsg = "ExtructZip  file " + zipFileName.Split('\\').Last() + " - " + ex.Message;
            }
            return errorMsg;
        }
        public void MoveChromeDriver(string ChromeDriverPath)
        {
            // 构建chromedriver.exe的完整路径
            string chromedriverExePath = Path.Combine(ChromeDriverPath, "chromedriver-win64", exeName);

            try
            {
                // 移动chromedriver.exe到目标路径
                File.Move(chromedriverExePath, Path.Combine(ChromeDriverPath, exeName));

                // 删除chromedriver-win64文件夹
                Directory.Delete(Path.Combine(ChromeDriverPath, "chromedriver-win64"), true);
            }
            catch (Exception ex)
            {
                // 处理错误，例如文件不存在或无法删除文件夹
                Console.WriteLine("发生错误：" + ex.Message);
            }
        }
    }
}
