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

                //driver.FindElement(By.XPath($"/html/body/div/form/table/tbody/tr/td[3]/input")).Click();
                driver.Navigate().GoToUrl("http://lotto.arclink.com.tw/Lotto39List.html");
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                log.WriteLog("001");

                int IssueCount = driver.FindElements(By.XPath($"/html/body/table[3]/tbody/tr")).Count;

                log.WriteLog("002");

                if (IssueCount == 0)
                {
                    log.WriteLog("003");
                    Thread.Sleep(5000);
                }
                log.WriteLog("004");
                IssueCount = IssueCount - 2;
                log.WriteLog("005");
                string finalIssue = driver.FindElement(By.XPath($"/html/body/table[3]/tbody/tr[{IssueCount}]/td[1]")).Text;
                log.WriteLog("006");
                string dowpdownStartName = "periods1";
                log.WriteLog("007");
                IWebElement dowpdownStartDate = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id(dowpdownStartName)));
                log.WriteLog("111");
                //dowpdownStartDate.Click();

                //找出所有g-menu-item
                //var itemCount = driver.FindElements(By.XPath($".//*[@id='{dowpdownStartName}']/option"));
                var selectElement = new SelectElement(dowpdownStartDate);
                log.WriteLog("222");
                try
                {
                    selectElement.SelectByText(startDate);
                    log.WriteLog("333");
                }
                catch(Exception ex)
                {
                    startDate = DateTime.Now.AddDays(dayRange-1).ToString("yyyy-MM-dd");
                    log.WriteLog("444");
                    selectElement.SelectByText(startDate);
                    log.WriteLog("555");
                }
                //driver.FindElement(By.XPath($"//*[@id='periods1']/option[1]")).Text
                //driver.FindElement(By.Name("Submit")).Click();
                log.WriteLog("666");
                //經常沒辦法改時間，改用while持續找到指定日期為止
                while (selectElement.SelectedOption.Text!= startDate)
                {
                    log.WriteLog("777");
                    selectElement.SelectByText(startDate);
                }
                log.WriteLog("888");
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
