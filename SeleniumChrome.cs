using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium.Chrome;
using System.Threading;
using System.Configuration;

namespace Lottery539
{

    class SeleniumChrome
    {
        WebDriverWait wait;
        IWebDriver driver = new ChromeDriver();
        ReadFile readFile = new ReadFile();
        log log = new log();
        List<LotteryData> lotteryDataList = new List<LotteryData>();
        long clientMaxIssue = 0;
        public void LoadData()
        {
            try
            {
                var file = readFile.ReadTxtFile();
                if (file.Count > 0)
                clientMaxIssue =Convert.ToInt64(file.Max(f => f.Issue));
                GetData();
                WriteFile writeFile = new WriteFile();
                writeFile.WriteData(lotteryDataList);
            }
            catch (Exception ex)
            {
                log.WriteLog(ex.Message);
            }
            finally
            {
                driver.Quit();
            }
        }
        public void GetData()
        {           
            try
            {
                int dayRange = Convert.ToInt32(Helpers.GetConfigValue("DayRange"));
                string endDate = DateTime.Now.ToString("yyyy-MM-dd");
                string lotteryUrl = Helpers.GetConfigValue("LotteryUrl") ?? "http://lotto.arclink.com.tw/";

                DateTime date = DateTime.Now.AddDays(dayRange);
                if (date.DayOfWeek == DayOfWeek.Sunday)
                    date=date.AddDays(-1);
                string startDate = date.ToString("yyyy-MM-dd");

                driver.Navigate().GoToUrl(lotteryUrl);
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                driver.SwitchTo().Frame("dynamicInfo");
                driver.FindElement(By.Name("userName")).SendKeys(Helpers.GetConfigValue("UserId"));
                driver.FindElement(By.Name("password")).SendKeys(Helpers.GetConfigValue("UserPwd"));
                IWebElement loginBtn = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath($"/html/body/div/form/table/tbody/tr/td[3]/input")));
                loginBtn.Click();
                driver.SwitchTo().DefaultContent();
                driver.Navigate().GoToUrl("http://lotto.arclink.com.tw/Lotto39List.html");
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                string dowpdownStartName = "periods1";
                IWebElement dowpdownStartDate = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id(dowpdownStartName)));
                var selectElement = new SelectElement(dowpdownStartDate);
                selectElement.SelectByText(startDate);

                while (selectElement.SelectedOption.Text != startDate)
                {
                    selectElement.SelectByText(startDate);
                }

                wait.Until(ExpectedConditions.ElementToBeClickable(By.Name("Submit"))).Click();

                int IssueCount = driver.FindElements(By.XPath($"/html/body/table[3]/tbody/tr")).Count;
                if (IssueCount == 0)
                {
                    wait.Until(ExpectedConditions.ElementExists(By.XPath($"/html/body/table[3]/tbody/tr")));
                }

                lotteryDataList = new List<LotteryData>();
                string maxIssueXPath = $"//td[1][text() > '{clientMaxIssue}']/parent::tr";

                IReadOnlyList<IWebElement> lotteryRows = driver.FindElements(By.XPath(maxIssueXPath));

                foreach (IWebElement row in lotteryRows)
                {
                    IReadOnlyList<IWebElement> columns = row.FindElements(By.TagName("td"));
                    LotteryData lotteryData = new LotteryData
                    {
                        Issue = columns[0].Text,
                        LotteryDate = columns[1].Text,
                        Numbers = columns[2].Text
                    };
                    lotteryDataList.Add(lotteryData);
                }
            }
            catch (Exception ex)
            {
                log.WriteLog("GetData錯誤 : " + ex.Message);
            }
        }
    }
}
