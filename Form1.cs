
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lottery539
{
    public partial class Form1 : Form
    {
        log log = new log();
        Helpers helpers = new Helpers();
        public Form1()
        {
            InitializeComponent();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateChromDriver updateChromDriver = new UpdateChromDriver();
            updateChromDriver.UpdateChromDriverFun();
            log.WriteLog("爬蟲開始...");
            try
            {
                SeleniumChrome seleniumChrome = new SeleniumChrome();
                seleniumChrome.LoadData();
                log.WriteLog("爬蟲完成!");
                MessageBox.Show("更新完成");
            }
            catch (Exception ex)
            {
                log.WriteLog("爬蟲失敗，原因 : " + ex.Message);
                MessageBox.Show("更新失敗，原因 : " + ex.Message);
            }
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {      
            LoadDataToListView();                   
        }
        private void LoadDataToListView()
        {
            ReloadListView();
            log.WriteLog("匯入資料開始...");
            ReadFile readFile = new ReadFile();
            List<LotteryData> datas = readFile.ReadTxtFile();
            log.WriteLog("匯入資料完成...");
            //int IssueCount = Convert.ToInt16(tbLotteryType.Text);
            Dictionary<string, int> groupNum = new Dictionary<string, int>();
            int MaxVal =0;
            int MinVal = 0;
            if (!string.IsNullOrEmpty(tbMax.Text.Trim()))
            {
                MaxVal = Convert.ToInt16(tbMax.Text);
            }
            else
            {
                tbMax.Text = "0";
            }
            if (!string.IsNullOrEmpty(tbMin.Text.Trim()))
            {
                MinVal = Convert.ToInt16(tbMin.Text);
            }
            else
            {
                tbMin.Text = "0";
            }
            datas = QueryDate(datas);
            groupNum = GetGroupNum(datas);
            int[] intNumsCount = groupNum.Values.ToArray();
            string[] numsCount = intNumsCount.Select(x => x.ToString()).ToArray();

            lvStatistics.Font = new Font(lvStatistics.Font, FontStyle.Bold);
            foreach (var nums in groupNum)
            lvStatistics.Columns.Add(nums.Key,40);
            lvStatistics.Items.Add(new ListViewItem(numsCount));
            lvStatistics.Items[0].Font = new Font("新細明體", 12f, FontStyle.Regular);



            List<string> hotNums = groupNum.Where(g => g.Value >= MaxVal).Select(s => s.Key).ToList();
            List<string> coldNums = groupNum.Where(g => g.Value <= MinVal).Select(s => s.Key).ToList();
            string horNumString = string.Empty;
            string coldNumString = string.Empty;
            string hotPointNumString = string.Empty;
            string coldPointNumString = string.Empty;
            string hotPointCount = string.Empty;
            string coldPointCount = string.Empty;
            horNumString = helpers.GetNumString(hotNums);
            coldNumString = helpers.GetNumString(coldNums);

            foreach (var d in datas)
            {
                hotPointNumString = GetPointNum(hotNums, d.Numbers);
                hotPointCount = GetPointCount(hotPointNumString).ToString();
                coldPointNumString = GetPointNum(coldNums, d.Numbers);
                coldPointCount= GetPointCount(coldPointNumString).ToString();
                lvShow.Items.Add(new ListViewItem(new string[] { d.Issue,d.LotteryDate,d.Numbers, hotPointNumString, hotPointCount, coldPointNumString, coldPointCount }));
            }
            tbLotteryType.Text = datas.Count.ToString() ;
            lvNumResult.Items.Add(new ListViewItem(new string[] { horNumString,coldNumString }));
        }
        private Dictionary<string, int> GetGroupNum( List<LotteryData> datas)
        {
            List<int> intlist = new List<int>();
            Dictionary<string, int> keyValues = new Dictionary<string, int>();
            string tmpString = string.Empty;

            datas = datas.Take(datas.Count()).ToList();
            foreach (var d in datas)
            tmpString += d.Numbers + ",";
            tmpString = tmpString.Substring(0, tmpString.Length - 1);
            var numList = tmpString.Split(',');
            
            foreach (var n in numList)
            {
                intlist.Add(Convert.ToInt32(n));
            }
            var q =from p in intlist
                   group p by p.ToString() into g
                   select new
                   {
                       g.Key,
                       Value = g.Count()
                   };


            foreach (var x in q)
            {
                string key = "";
                if (Convert.ToInt32(x.Key) < 10)
                    key = "0" + x.Key;
                else
                    key = x.Key;
                keyValues.Add(key, x.Value);
            }

            for(int i = 1; i <= 39; i++)
            {
                string key = "";
                if (Convert.ToInt32(i) < 10)
                    key = "0" + i;
                else
                    key = i.ToString();
                if (!keyValues.ContainsKey(key))
                   keyValues.Add(key, 0);
            }
            Dictionary<string, int> result = keyValues.OrderBy(o =>Convert.ToInt32(o.Key)).ToDictionary(o => o.Key, p => p.Value);
            return result;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //tbLotteryType.Items.Add("48期");
            //tbLotteryType.Items.Add("57期");
            //tbLotteryType.SelectedIndex = 0;          
            dtStart.Value=GetDayByIssueCount(48, dtEnd.Value.Date);
            ReloadListView();
        }
        private void ReloadListView()
        {
            lvShow.Clear();
            lvStatistics.Clear();
            lvNumResult.Clear();
            lvShow.View = View.Details;
            lvShow.GridLines = true;
            lvShow.FullRowSelect = true;
            lvShow.Columns.Add("期號", 200);
            lvShow.Columns.Add("日期", 300);
            lvShow.Columns.Add("開獎號碼", 400);
            lvShow.Columns.Add("當期開出熱門號", 300);
            lvShow.Columns.Add("熱門號數量", 150);
            lvShow.Columns.Add("當期開出冷門號", 300);
            lvShow.Columns.Add("冷門號數量",150);

            lvStatistics.View = View.Details;
            lvStatistics.GridLines = true;
            lvStatistics.FullRowSelect = true;

            lvNumResult.View = View.Details;
            lvNumResult.GridLines = true;
            lvNumResult.FullRowSelect = true;
            lvNumResult.Columns.Add("熱門號", 600);
            lvNumResult.Columns.Add("冷門號", 600);
        }
        public string GetPointNum(List<string> nums, string num)
        {
            string result = string.Empty;
            var numList=helpers.GetNumList(num);
            foreach(var n in numList)
            {
                if (nums.Contains(n))
                    result += n + ",";
            }
            try
            {
                result = result.Substring(0, result.Length - 1);
            }
            catch(Exception ex)
            {
                result = string.Empty;
                log.WriteLog("GetHotPointNum: "+ex.Message);
            }
            return result;
        }

        private void tbMax_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private List<LotteryData> QueryDate(List<LotteryData> datas)
        {
            DateTime starDate = dtStart.Value.Date;
            DateTime endDate = dtEnd.Value.Date;
            datas = datas.Where(d => Convert.ToDateTime(d.LotteryDate).Date >= starDate && Convert.ToDateTime(d.LotteryDate).Date <= endDate).ToList();
            return datas;
        }
        private int GetPointCount(string num)
        {
            int result = 0;
            if(num.Trim().Length!=0)
            {
                result = num.Split(',').Count();
            }
            return result;
        }

        private void tbLotteryType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (tbLotteryType.SelectedIndex == 1)
            //{
            //    tbMax.Text = "9";
            //    tbMin.Text = "5";
            //}
            //else
            //{
            //    tbMax.Text = "8";
            //    tbMin.Text = "4";
            //}
        }
        private int GetIssueCountByDayRange(DateTime startDay, DateTime endDay)
        {
            int result = 0;
            if (startDay > endDay)
                return result;
            DateTime day = startDay;
            while (day != endDay)
            {
                if (day.DayOfWeek == DayOfWeek.Sunday)
                    continue;
                else
                    result++;
                day.AddDays(1);
            }
            return result;
        }
        private DateTime GetDayByIssueCount(int issues, DateTime endDay)
        {
            DateTime result = DateTime.Now.AddDays(-65);
            DateTime day = endDay;
            int count = 0;
            try
            {               
                while (count != issues)
                {                    
                    if (day.DayOfWeek != DayOfWeek.Sunday)
                        count++;                                    
                    result=day.AddDays(-1);
                    day = result;
                }
            }
            catch(Exception ex)
            {
                log.WriteLog(ex.Message);
            }
            return result;
        }

        private void dtStart_ValueChanged(object sender, EventArgs e)
        {
            tbLotteryType.Text = "";
        }

        private void dtEnd_ValueChanged(object sender, EventArgs e)
        {
            tbLotteryType.Text = "";
        }
    }
}
