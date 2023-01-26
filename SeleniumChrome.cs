using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Chrome;
using System.Threading;
using System.Configuration;
using Lottery539;

namespace Lottery539
{

    class SeleniumChrome
    {
        WebDriverWait wait;
        IWebDriver driver = new ChromeDriver();
        WriteFile writeFile = new WriteFile();
        log log = new log();
        List<LotteryData> lotteryDataList = new List<LotteryData>();
        public void LoadData()
        {
            try
            {                
                while (lotteryDataList.Count == 0)
                {
                    GetData();
                }
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
                int dayRange = Convert.ToInt32(GetConfigValue("DayRange"));
                string endDate = DateTime.Now.ToString("yyyy-MM-dd");
                string lotteryUrl = GetConfigValue("LotteryUrl") ?? "http://lotto.arclink.com.tw/";

                DateTime date = DateTime.Now.AddDays(dayRange);
                if (date.DayOfWeek == DayOfWeek.Sunday)
                    date=date.AddDays(-1);
                string startDate = date.ToString("yyyy-MM-dd");

                driver.Navigate().GoToUrl(lotteryUrl);
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                driver.SwitchTo().Frame("dynamicInfo");
                driver.FindElement(By.Name("userName")).SendKeys(GetConfigValue("UserId"));
                driver.FindElement(By.Name("password")).SendKeys(GetConfigValue("UserPwd"));
                IWebElement loginBtn = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath($"/html/body/div/form/table/tbody/tr/td[3]/input")));
                //這邊一定要等
                Thread.Sleep(2000);
                loginBtn.Click();
                Thread.Sleep(2000);
                driver.SwitchTo().DefaultContent();
                //driver.FindElement(By.XPath($"/html/body/div/form/table/tbody/tr/td[3]/input")).Click();
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
                var lotteryRows = driver.FindElements(By.XPath($"/html/body/table[3]/tbody/tr")).Skip(2);
                lotteryRows = lotteryRows.Take(lotteryRows.Count() - 1);
                foreach (var row in lotteryRows)
                {
                    var columns = row.FindElements(By.TagName("td"));
                    var lotteryData = new LotteryData
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
        private string GetConfigValue(string key) => ConfigurationManager.AppSettings[key]?.ToString();
    }
}
