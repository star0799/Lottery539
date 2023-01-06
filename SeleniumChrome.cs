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
        public void LoadData()
        {
            try
            {
               
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
        public void GetData(int year)
        {
            int TeamsCount = default;
            int dynamicIndex = 2;
            int Years, Level, TotalGames, WinGames, LoseGames, TieGames, WinBall, LoseBall;
            string TeamName;
            string SubtractBall;
            string XPath = ConfigurationManager.AppSettings["XPath"] ?? "//*[@id='liveresults-sports-immersive__league-fullpage']/div/div[2]/div[2]/div/div/div/div[3]/div/div/div/div[2]/div/div/div/div/div/div/div[2]/div";
            int Element = Convert.ToInt16(ConfigurationManager.AppSettings["Element"] ?? "2");
            try
            {
                //*[@id="liveresults-sports-immersive__league-fullpage"]/div/div[2]/div[2]/div/div/div/div[3]/div/div/div/div[2]/div/div/div/div/div/div/div[2]/div/div[2]/div/table/tbody/tr[1]
                wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(XPath + "/div[" + dynamicIndex + "]/div/table/tbody/tr[1]")));
                //TeamsCount = driver.FindElements(By.XPath($"//*[@id='liveresults-sports-immersive__league-fullpage']/div/div[2]/div[2]/div/div/div/div[3]/div/div/div/div[2]/div/div/div/div/div/div[" + dynamicIndex + "]/ div/table/tbody/tr")).Count;
                TeamsCount = driver.FindElements(By.XPath(XPath + "/div[" + dynamicIndex + "]/div/table/tbody/tr")).Count;
                if (TeamsCount == 0)
                {
                    dynamicIndex++;
                    TeamsCount = driver.FindElements(By.XPath($"//*[@id='liveresults-sports-immersive__league-fullpage']/div/div[2]/div[2]/div/div/div/div[3]/div/div/div/div[2]/div/div/div/div/div/div[" + dynamicIndex + "]/ div/table/tbody/tr")).Count;
                }
                for (int i = Element; i < TeamsCount + 1; i++)
                {
                    Years = year;
                    Level = Convert.ToInt32(driver.FindElement(By.XPath(XPath + "/div[" + dynamicIndex + "]/div/table/tbody/tr[" + i + "]/td[2]/div[2]")).Text);
                    TeamName = driver.FindElement(By.XPath(XPath + "/div[" + dynamicIndex + "]/div/table/tbody/tr[" + i + "]/td[3]/div/div/span")).Text;
                    TotalGames = Convert.ToInt32(driver.FindElement(By.XPath(XPath + "/div[" + dynamicIndex + "]/div/table/tbody/tr[" + i + "]/td[4]")).Text);
                    WinGames = Convert.ToInt32(driver.FindElement(By.XPath(XPath + "/div[" + dynamicIndex + "]/div/table/tbody/tr[" + i + "]/td[5]")).Text);
                    TieGames = Convert.ToInt32(driver.FindElement(By.XPath(XPath + "/div[" + dynamicIndex + "]/div/table/tbody/tr[" + i + "]/td[6]")).Text);
                    LoseGames = Convert.ToInt32(driver.FindElement(By.XPath(XPath + "/div[" + dynamicIndex + "]/div/table/tbody/tr[" + i + "]/td[7]")).Text);
                    WinBall = Convert.ToInt32(driver.FindElement(By.XPath(XPath + "/div[" + dynamicIndex + "]/div/table/tbody/tr[" + i + "]/td[8]")).Text);
                    LoseBall = Convert.ToInt32(driver.FindElement(By.XPath(XPath + "/div[" + dynamicIndex + "]/div/table/tbody/tr[" + i + "]/td[9]")).Text);
                    SubtractBall = driver.FindElement(By.XPath(XPath + "/div[" + dynamicIndex + "]/div/table/tbody/tr[" + i + "]/td[10]")).Text.ToString();
                    //ListLottery539Teams.Add(new Lottery539Teams { Years = Years, Level = Level, TeamName = TeamName, TotalGames = TotalGames, WinGames = WinGames, TieGames = TieGames, LoseGames = LoseGames, WinBall = WinBall, LoseBall = LoseBall, SubtractBall = SubtractBall });
                }
            }
            catch (Exception ex)
            {
                log.WriteLog("GetData錯誤 : " + ex.Message);
            }
        }
    }
}
