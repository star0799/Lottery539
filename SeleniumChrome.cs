using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Lottery539
{
    class SeleniumChrome
    {
        private readonly IWebDriver driver;
        private WebDriverWait wait;
        private readonly ReadFile readFile = new ReadFile();
        private readonly log log = new log();
        private List<LotteryData> clientData = new List<LotteryData>();
        private List<LotteryData> lotteryDataList = new List<LotteryData>();
        private long clientMaxIssue = 0;
        private DateTime? clientMaxDate;

        public SeleniumChrome()
        {
            string exeDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            driver = StartChrome(exeDir);
        }

        private IWebDriver StartChrome(string exeDir)
        {
            for (int attempt = 1; attempt <= 2; attempt++)
            {
                ChromeDriverService service = ChromeDriverService.CreateDefaultService(exeDir);
                service.HideCommandPromptWindow = true;

                try
                {
                    ChromeOptions options = new ChromeOptions
                    {
                        PageLoadStrategy = PageLoadStrategy.Eager
                    };
                    return new ChromeDriver(service, options, TimeSpan.FromSeconds(20));
                }
                catch (WebDriverException ex)
                {
                    service.Dispose();
                    log.WriteLog($"Chrome 啟動失敗（第 {attempt} 次）: {ex.Message}");

                    if (attempt == 2)
                        throw new InvalidOperationException("Chrome 無法啟動，已重試一次。原因: " + ex.Message, ex);

                    System.Threading.Thread.Sleep(1000);
                }
            }

            throw new InvalidOperationException("Chrome 無法啟動。");
        }

        public void LoadData()
        {
            try
            {
                clientData = readFile.ReadTxtFile();
                if (clientData.Count > 0)
                {
                    LotteryData latestData = clientData.OrderByDescending(f => Convert.ToInt64(f.Issue)).First();
                    clientMaxIssue = Convert.ToInt64(latestData.Issue);

                    DateTime parsedDate;
                    if (!DateTime.TryParseExact(latestData.LotteryDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
                        throw new InvalidOperationException("Lottery.txt 最新一期的日期格式錯誤: " + latestData.LotteryDate);

                    clientMaxDate = parsedDate;
                }

                GetData();
                log.WriteLog($"更新{lotteryDataList.Count}筆");
                WriteFile writeFile = new WriteFile();
                writeFile.WriteData(lotteryDataList);
            }
            catch (Exception ex)
            {
                log.WriteLog(ex.Message);
                throw;
            }
            finally
            {
                try
                {
                    driver.Quit();
                }
                catch (Exception ex)
                {
                    log.WriteLog("Chrome 關閉失敗: " + ex.Message);
                }
            }
        }

        public void GetData()
        {
            try
            {
                List<LotteryData> recentData = GetRecentData();
                List<LotteryData> historyData = new List<LotteryData>();

                if (NeedsHistoryData(recentData))
                {
                    log.WriteLog("遺漏頁範圍不足，登入舊頁補資料。");
                    try
                    {
                        historyData = GetHistoryData();
                    }
                    catch (Exception ex)
                    {
                        log.WriteLog("舊頁資料取得失敗，改由遺漏頁繼續處理: " + ex.Message);
                    }
                }
                else
                {
                    log.WriteLog("遺漏頁資料已連續，不查詢舊頁。");
                }

                lotteryDataList = MergeData(recentData, historyData);
                int addedCount = lotteryDataList.Count(d => Convert.ToInt64(d.Issue) > clientMaxIssue);
                int correctedCount = lotteryDataList.Count - addedCount;
                log.WriteLog($"遺漏頁取得{recentData.Count}筆，舊頁取得{historyData.Count}筆，新增{addedCount}筆，校正{correctedCount}筆");
            }
            catch (WebDriverTimeoutException tex)
            {
                log.WriteLog("操作超時 : " + tex.Message);
                throw;
            }
            catch (Exception ex)
            {
                log.WriteLog("GetData錯誤 : " + ex.Message);
                throw;
            }
        }

        private bool NeedsHistoryData(List<LotteryData> recentData)
        {
            if (recentData.Count == 0)
                return false;

            HashSet<long> recentIssues = new HashSet<long>(recentData.Select(d => Convert.ToInt64(d.Issue)));
            long maxRecentIssue = recentIssues.Max();
            for (long issue = clientMaxIssue + 1; issue <= maxRecentIssue; issue++)
            {
                if (!recentIssues.Contains(issue))
                    return true;
            }

            return false;
        }

        private List<LotteryData> GetRecentData()
        {
            string recentUrl = Helpers.GetConfigValue("LotteryRecentUrl") ?? "https://lotto.arclink.com.tw/LottoYL.do?type=9";
            driver.Navigate().GoToUrl(recentUrl);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElements(By.XPath("//tr[count(td)=40]")).Count > 0);

            List<LotteryData> result = new List<LotteryData>();
            IReadOnlyList<IWebElement> lotteryRows = driver.FindElements(By.XPath("//tr[count(td)=40]"));

            foreach (IWebElement row in lotteryRows)
            {
                IReadOnlyList<IWebElement> columns = row.FindElements(By.TagName("td"));
                long issue;
                if (!long.TryParse(columns[0].Text.Trim(), out issue) || issue <= clientMaxIssue)
                    continue;

                List<string> numbers = new List<string>();
                for (int columnIndex = 1; columnIndex < columns.Count; columnIndex++)
                {
                    if (columns[columnIndex].FindElements(By.TagName("b")).Count > 0)
                        numbers.Add(columnIndex.ToString("00"));
                }

                if (numbers.Count != 5)
                    throw new InvalidOperationException($"遺漏頁期號 {issue} 的號碼解析失敗，預期 5 個但取得 {numbers.Count} 個。");

                result.Add(new LotteryData
                {
                    Issue = issue.ToString(),
                    Numbers = string.Join(",", numbers)
                });
            }

            return result.OrderBy(d => Convert.ToInt64(d.Issue)).ToList();
        }

        private List<LotteryData> GetHistoryData()
        {
            if (clientData.Count == 0)
                return new List<LotteryData>();

            LotteryData startRecord = clientData
                .OrderByDescending(d => Convert.ToInt64(d.Issue))
                .First();
            string startDate = startRecord.LotteryDate;
            string historyUrl = Helpers.GetConfigValue("LotteryHistoryUrl") ?? "https://lotto.arclink.com.tw/Lotto39List.html";

            LoginForHistory();
            driver.Navigate().GoToUrl(historyUrl);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            IWebElement startDropdown;
            IWebElement endDropdown;
            try
            {
                startDropdown = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("periods1")));
                endDropdown = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("periods2")));
            }
            catch (WebDriverTimeoutException ex)
            {
                throw new InvalidOperationException("舊頁登入後無法載入期間選擇器。", ex);
            }
            SelectElement startSelect = new SelectElement(startDropdown);
            SelectElement endSelect = new SelectElement(endDropdown);

            startSelect.SelectByText(startDate);
            endSelect.SelectByIndex(0);
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Name("Submit"))).Click();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            try
            {
                wait.Until(d => d.FindElements(By.XPath($"//tr[td[2][normalize-space()='{startDate}']]")).Count > 0);
            }
            catch (WebDriverTimeoutException ex)
            {
                throw new InvalidOperationException("舊頁會員區間查詢未回傳起始日期 " + startDate + "。", ex);
            }

            return ParseHistoryRows();
        }

        private List<LotteryData> ParseHistoryRows()
        {
            List<LotteryData> result = new List<LotteryData>();
            IReadOnlyList<IWebElement> rows = driver.FindElements(By.XPath("//tr[count(td)>=3]"));
            foreach (IWebElement row in rows)
            {
                IReadOnlyList<IWebElement> columns = row.FindElements(By.TagName("td"));
                long issue;
                DateTime lotteryDate;
                if (!long.TryParse(columns[0].Text.Trim(), out issue) ||
                    !DateTime.TryParseExact(columns[1].Text.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out lotteryDate))
                    continue;

                string numbers = NormalizeNumbers(columns[2].Text);
                result.Add(new LotteryData
                {
                    Issue = issue.ToString(),
                    LotteryDate = lotteryDate.ToString("yyyy-MM-dd"),
                    Numbers = numbers
                });
            }

            return result
                .GroupBy(d => d.Issue)
                .Select(g => g.First())
                .OrderBy(d => Convert.ToInt64(d.Issue))
                .ToList();
        }

        private void LoginForHistory()
        {
            string userId = Helpers.GetConfigValue("UserId");
            string userPassword = Helpers.GetConfigValue("UserPwd");
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(userPassword))
                throw new InvalidOperationException("舊頁歷史補資料需要會員帳號，請設定 UserId 與 UserPwd。");

            string loginUrl = Helpers.GetConfigValue("LotteryLoginUrl") ?? "http://lotto.arclink.com.tw/";
            driver.Navigate().GoToUrl(loginUrl);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElements(By.Name("dynamicInfo")).Count > 0);
            driver.SwitchTo().Frame("dynamicInfo");

            try
            {
                IWebElement userName = wait.Until(ExpectedConditions.ElementIsVisible(By.Name("userName")));
                IWebElement password = driver.FindElement(By.Name("password"));
                userName.Clear();
                password.Clear();
                userName.SendKeys(userId);
                password.SendKeys(userPassword);
                IWebElement loginButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/div/form/table/tbody/tr/td[3]/input")));

                // The legacy form targets the top frame. A normal Click waits for every
                // resource on the destination page and can exceed ChromeDriver's command timeout.
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].form.submit();", loginButton);
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
                try
                {
                    wait.Until(d => d.Manage().Cookies.GetCookieNamed("memberCode") != null);
                }
                catch (WebDriverTimeoutException ex)
                {
                    throw new InvalidOperationException("舊頁登入未取得會員工作階段。", ex);
                }
            }
            finally
            {
                driver.SwitchTo().DefaultContent();
            }
        }

        private List<LotteryData> MergeData(List<LotteryData> recentData, List<LotteryData> historyData)
        {
            Dictionary<long, LotteryData> recentByIssue = recentData.ToDictionary(d => Convert.ToInt64(d.Issue));
            Dictionary<long, LotteryData> historyByIssue = historyData.ToDictionary(d => Convert.ToInt64(d.Issue));
            Dictionary<long, LotteryData> clientByIssue = clientData.ToDictionary(d => Convert.ToInt64(d.Issue));
            List<LotteryData> result = new List<LotteryData>();

            foreach (long issue in recentByIssue.Keys.Intersect(historyByIssue.Keys))
            {
                if (recentByIssue[issue].Numbers != historyByIssue[issue].Numbers)
                    throw new InvalidOperationException($"期號 {issue} 在遺漏頁與舊頁的號碼不一致，已停止更新。");
            }

            foreach (KeyValuePair<long, LotteryData> historyItem in historyByIssue.Where(item => item.Key <= clientMaxIssue))
            {
                LotteryData localData;
                if (!clientByIssue.TryGetValue(historyItem.Key, out localData))
                    continue;

                if (NormalizeNumbers(localData.Numbers) != historyItem.Value.Numbers)
                    throw new InvalidOperationException($"本機期號 {historyItem.Key} 與舊頁的號碼不一致，已停止更新。");

                if (localData.LotteryDate != historyItem.Value.LotteryDate)
                    result.Add(historyItem.Value);
            }

            long maxIssue = clientMaxIssue;
            if (recentByIssue.Count > 0)
                maxIssue = Math.Max(maxIssue, recentByIssue.Keys.Max());
            if (historyByIssue.Keys.Any(issue => issue > clientMaxIssue))
                maxIssue = Math.Max(maxIssue, historyByIssue.Keys.Where(issue => issue > clientMaxIssue).Max());

            if (maxIssue == clientMaxIssue)
                return result.OrderByDescending(d => Convert.ToInt64(d.Issue)).ToList();
            if (!clientMaxDate.HasValue)
                throw new InvalidOperationException("Lottery.txt 必須至少保留一期資料，才能合併新舊來源。");

            DateTime runningDate = clientMaxDate.Value;
            for (long issue = clientMaxIssue + 1; issue <= maxIssue; issue++)
            {
                LotteryData recentItem;
                LotteryData historyItem;
                bool hasRecent = recentByIssue.TryGetValue(issue, out recentItem);
                bool hasHistory = historyByIssue.TryGetValue(issue, out historyItem);

                if (!hasRecent && !hasHistory)
                    throw new InvalidOperationException($"資料期號不連續，缺少 {issue}，已停止更新。");

                if (hasRecent && hasHistory && recentItem.Numbers != historyItem.Numbers)
                    throw new InvalidOperationException($"期號 {issue} 在遺漏頁與舊頁的號碼不一致，已停止更新。");

                LotteryData mergedItem = new LotteryData
                {
                    Issue = issue.ToString(),
                    Numbers = hasRecent ? recentItem.Numbers : historyItem.Numbers
                };

                if (hasHistory)
                {
                    DateTime historyDate = DateTime.ParseExact(historyItem.LotteryDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    if (historyDate <= runningDate)
                        throw new InvalidOperationException($"舊頁期號 {issue} 的日期 {historyItem.LotteryDate} 不大於前一期日期，已停止更新。");

                    runningDate = historyDate;
                }
                else
                {
                    runningDate = GetNextLotteryDate(runningDate);
                }

                mergedItem.LotteryDate = runningDate.ToString("yyyy-MM-dd");
                result.Add(mergedItem);
            }

            return result.OrderByDescending(d => Convert.ToInt64(d.Issue)).ToList();
        }

        private static string NormalizeNumbers(string numbers)
        {
            string[] values = numbers
                .Split(',')
                .Select(value => value.Trim())
                .Where(value => !string.IsNullOrEmpty(value))
                .ToArray();

            if (values.Length != 5)
                throw new InvalidOperationException("開獎號碼格式錯誤: " + numbers);

            List<string> normalized = new List<string>();
            foreach (string value in values)
            {
                int number;
                if (!int.TryParse(value, out number) || number < 1 || number > 39)
                    throw new InvalidOperationException("開獎號碼格式錯誤: " + numbers);

                normalized.Add(number.ToString("00"));
            }

            return string.Join(",", normalized);
        }

        private static DateTime GetNextLotteryDate(DateTime currentDate)
        {
            DateTime nextDate = currentDate.AddDays(1);
            while (nextDate.DayOfWeek == DayOfWeek.Sunday)
                nextDate = nextDate.AddDays(1);

            return nextDate;
        }
    }
}
