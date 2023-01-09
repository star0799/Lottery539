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
                GetData();
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
        //重新到google頁面搜尋
        public void ReSearch(string country)
        {
            try
            {
                IWebElement GetSearchBar;
                IWebElement SearchSubmit;
                IWebElement RecordSubmit;
                string GoogleUrl = ConfigurationManager.AppSettings["GoogleUrl"].ToString() ?? "http://google.com";
                driver.Navigate().GoToUrl(GoogleUrl);
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                GetSearchBar = wait.Until(ExpectedConditions.ElementToBeClickable(By.Name("q")));
                GetSearchBar.SendKeys(country.ToString());
                SearchSubmit = wait.Until(ExpectedConditions.ElementToBeClickable(By.Name("btnK")));
                SearchSubmit.Click();
                //按下戰績
                RecordSubmit = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath($"//*[@id='sports-app']/div/div[2]/div/div/div/ol/li[3]")));
                RecordSubmit.Click();
            }
            catch (Exception ex)
            {
                log.WriteLog("ReSearch，重新搜尋錯誤。" + ex.Message);
            }
        }
        public void GetData()
        {           
            try
            {
                string userId = ConfigurationManager.AppSettings["UserId"].ToString();
                string userPwd = ConfigurationManager.AppSettings["UserPwd"].ToString();
                int dayRange = Convert.ToInt32(ConfigurationManager.AppSettings["DayRange"]);
                int Issue1 = Convert.ToInt32(ConfigurationManager.AppSettings["Issue1"]);
                string startDate = DateTime.Now.AddDays(dayRange).ToString("yyyy-MM-dd");
                string endDate = DateTime.Now.ToString("yyyy-MM-dd");
                string lotteryUrl = ConfigurationManager.AppSettings["LotteryUrl"].ToString() ?? "http://lotto.arclink.com.tw/";
                driver.Navigate().GoToUrl(lotteryUrl);
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                driver.SwitchTo().Frame("dynamicInfo");
                driver.FindElement(By.Name("userName")).SendKeys(userId);
                driver.FindElement(By.Name("password")).SendKeys(userPwd);
                IWebElement loginBtn = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath($"/html/body/div/form/table/tbody/tr/td[3]/input")));
                Thread.Sleep(2000);
                loginBtn.Click();

                //IWebElement b = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath($"//*[@id='mt']/div[7]/a")));
                //b.Click();


                //driver.FindElement(By.XPath($"/html/body/div/form/table/tbody/tr/td[3]/input")).Click();
                driver.Navigate().GoToUrl("http://lotto.arclink.com.tw/Lotto39List.html");
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                ////*[@id="mt"]/div[7]/a
                int IssueCount = driver.FindElements(By.XPath($"/html/body/table[3]/tbody/tr")).Count;

                if (IssueCount == 0)
                {
                    Thread.Sleep(5000);
                }
                IssueCount = IssueCount - 2;
                string finalIssue = driver.FindElement(By.XPath($"/html/body/table[3]/tbody/tr[{IssueCount}]/td[1]")).Text;
                string dowpdownStartName = "periods1";
                IWebElement dowpdownStartDate = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id(dowpdownStartName)));
                //dowpdownStartDate.Click();

                //找出所有g-menu-item
                //var itemCount = driver.FindElements(By.XPath($".//*[@id='{dowpdownStartName}']/option"));
                var selectElement = new SelectElement(dowpdownStartDate);
                try
                {
                    selectElement.SelectByText(startDate);
                }
                catch(Exception ex)
                {
                    startDate = DateTime.Now.AddDays(dayRange-1).ToString("yyyy-MM-dd");
                    selectElement.SelectByText(startDate);
                }
                //driver.FindElement(By.XPath($"//*[@id='periods1']/option[1]")).Text
                //driver.FindElement(By.Name("Submit")).Click();
                //經常沒辦法改時間，改用while持續找到指定日期為止
                while (selectElement.SelectedOption.Text!= startDate)
                {
                    selectElement.SelectByText(startDate);
                }
                wait.Until(ExpectedConditions.ElementToBeClickable(By.Name("Submit"))).Click();

                for (int i= 0; i <Issue1; i++)
                {
                    LotteryData lotteryData = new LotteryData();
                    lotteryData.Issue = driver.FindElement(By.XPath($"/html/body/table[3]/tbody/tr[{i+3}]/td[1]")).Text;
                    lotteryData.LotteryDate = driver.FindElement(By.XPath($"/html/body/table[3]/tbody/tr[{i + 3}]/td[2]")).Text;
                    lotteryData.Numbers = driver.FindElement(By.XPath($"/html/body/table[3]/tbody/tr[{i + 3}]/td[3]")).Text;
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
