
using Lottery539.Model;
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
        List<LotteryData> datas = new List<LotteryData>();
        List<LotteryCompareLotteryData> lotteryCompareLotteryDatas = new List<LotteryCompareLotteryData>();
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
            datas = readFile.ReadTxtFile();
            log.WriteLog("匯入資料完成...");
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
            LotteryData nextIssue =GetNextIssue(datas);
            LotteryData previousIssue = GetPreviousIssue(datas);
            var queryDatas = QueryDate(datas);
            groupNum = GetGroupNum(queryDatas);

            List<LotteryData> allData = new List<LotteryData>(queryDatas);
            allData.Add(previousIssue);
            var noFinalIssue= GetGroupNum(allData.Skip(1).ToList());
            int[] intNumsCount = groupNum.Values.ToArray();
            string[] numsCount = intNumsCount.Select(x => x.ToString()).ToArray();

            lvStatistics.Font = new Font(lvStatistics.Font, FontStyle.Bold);
            foreach (var nums in groupNum)
            lvStatistics.Columns.Add(nums.Key,40);
            lvStatistics.Items.Add(new ListViewItem(numsCount));
            lvStatistics.Items[0].Font = new Font("新細明體", 12f, FontStyle.Regular);



            List<string> hotNums = groupNum.Where(g => g.Value >= MaxVal).Select(s => s.Key).ToList();
            List<string> coldNums = groupNum.Where(g => g.Value <= MinVal).Select(s => s.Key).ToList();

            List<string> noFinalIssuehotNums = noFinalIssue.Where(g => g.Value >= MaxVal).Select(s => s.Key).ToList();
            List<string> noFinalIssuecoldNums = noFinalIssue.Where(g => g.Value <= MinVal).Select(s => s.Key).ToList();

            string horNumString = string.Empty;
            string coldNumString = string.Empty;
            string hotPointNumString = string.Empty;
            string coldPointNumString = string.Empty;
            horNumString = helpers.GetNumString(hotNums);
            coldNumString = helpers.GetNumString(coldNums);
            foreach (var d in queryDatas)
            {
                if (queryDatas.IndexOf(d) - 1 == -1)
                {
                    if (nextIssue == null)
                    {
                        hotPointNumString = "";
                        coldPointNumString = "";
                    }
                    else
                    {
                        hotPointNumString = GetPointNum(queryDatas.Count, d.LotteryDate, nextIssue.Numbers, true);
                        coldPointNumString = GetPointNum(queryDatas.Count, d.LotteryDate, nextIssue.Numbers, false);
                    }
                }
                else
                {
                    hotPointNumString = GetPointNum(queryDatas.Count,d.LotteryDate, queryDatas[queryDatas.IndexOf(d) -1].Numbers,true);
                    coldPointNumString = GetPointNum(queryDatas.Count, d.LotteryDate, queryDatas[queryDatas.IndexOf(d) -1].Numbers,false);
                }
                //coldPointCount= GetPointCount(coldPointNumString).ToString();
                lvShow.Items.Add(new ListViewItem(new string[] { d.Issue,d.LotteryDate,d.Numbers, hotPointNumString,coldPointNumString }));
            }
            tbLotteryType.Text = queryDatas.Count.ToString() ;
            lvNumResult.Items.Add(new ListViewItem(new string[] { horNumString, hotNums.Count.ToString(), coldNumString, coldNums.Count.ToString() }));
        }
        private Dictionary<string, int> GetGroupNum(List<LotteryData> datas)
        {
            var intlist = datas.SelectMany(d => d.Numbers.Split(',').Select(int.Parse));
            var keyValues = intlist.GroupBy(n => n, new NumberEqualityComparer())
                                   .ToDictionary(g => g.Key.ToString("D2"), g => g.Count()).OrderBy(o => Convert.ToInt32(o.Key)).ToDictionary(o => o.Key, p => p.Value);
            return keyValues;
        }

        private class NumberEqualityComparer : IEqualityComparer<int>
        {
            public bool Equals(int x, int y) => x == y;
            public int GetHashCode(int obj) => obj.GetHashCode();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
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
            lvShow.Columns.Add("當期開出冷門號", 300);

            lvStatistics.View = View.Details;
            lvStatistics.GridLines = true;
            lvStatistics.FullRowSelect = true;

            lvNumResult.View = View.Details;
            lvNumResult.GridLines = true;
            lvNumResult.FullRowSelect = true;
            lvNumResult.Columns.Add("熱門號", 600);
            lvNumResult.Columns.Add("支數", 80);
            lvNumResult.Columns.Add("冷門號", 600);
            lvNumResult.Columns.Add("支數", 80);
        }
        public string GetPointNum(int queryCount,string nowDay, string num,bool IsMax)
        {
            string result = string.Empty;
            List<string> numList=helpers.GetNumList(num);
            List<LotteryData> rangeData = datas.Where(q => Convert.ToDateTime(q.LotteryDate) <= Convert.ToDateTime(nowDay)).Take(queryCount).ToList();
            List<KeyValuePair<string, int>> groupDic = new List<KeyValuePair<string, int>>();
            
            if(IsMax)
                groupDic = GetGroupNum(rangeData).Where(g => g.Value >= Convert.ToInt16(tbMax.Text)).ToList();
            else
                groupDic = GetGroupNum(rangeData).Where(g => g.Value <= Convert.ToInt16(tbMin.Text)).ToList();

            foreach (var g in groupDic)
            {
                foreach(var n in numList)
                if (g.Key==n)
                {
                        result += n + ",";
                        break;
                }
            }
            try
            {
                result = result.Substring(0, result.Length - 1);
            }
            catch (Exception ex)
            {
                result = string.Empty;
                log.WriteLog("GetHotPointNum: " + ex.Message);
            }
            if (string.IsNullOrEmpty(result))
                result = "X";
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
                {
                    day = day.AddDays(1);
                    continue;
                }
                else
                {
                    result++;
                    day = day.AddDays(1);
                }
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
        private List<string> DeductNums(List<string> nums, Dictionary<string, int> groupNum)
        {
            List<string> result = new List<string>();
            foreach(var d in nums)
            {
                var val=groupNum[d];
                groupNum[d] = val - 1;
            }
            return result;
        }
        private LotteryData GetNextIssue(List<LotteryData> datas)
        {
            LotteryData result = new LotteryData();
            DateTime date = dtEnd.Value.Date;
            if (!IsLotteryDay(date))
                date=date.AddDays(1);
            result = datas.Where(d => Convert.ToDateTime(d.LotteryDate).Date > date).LastOrDefault();
            return result;
        }
        private LotteryData GetPreviousIssue(List<LotteryData> datas)
        {
            LotteryData result = new LotteryData();
            DateTime date = dtStart.Value.Date;
            if (!IsLotteryDay(date))
                date = date.AddDays(1);
            result = datas.Where(d => Convert.ToDateTime(d.LotteryDate).Date < date).FirstOrDefault();
            return result;
        }

        private bool IsLotteryDay(DateTime day)
        {
            if (day.DayOfWeek == DayOfWeek.Sunday)
            {
                return false;
            }
            else 
            {
                return true;
            }
        }

        private void dtStart_ValueChanged(object sender, EventArgs e)
        {
            tbLotteryType.Text = "";
        }

        private void dtEnd_ValueChanged(object sender, EventArgs e)
        {
            tbLotteryType.Text = "";
        }

        private void btnQuery2_Click(object sender, EventArgs e)
        {
            LoadDataToListView2();
        }
        private void LoadDataToListView2()
        {
            ReloadListView2();
            log.WriteLog("匯入資料2開始...");
            ReadFile readFile = new ReadFile();
            datas = readFile.ReadTxtFile();
            log.WriteLog("匯入資料2完成...");
            var queryDatas = datas.Where(d => Convert.ToDateTime(d.LotteryDate).Date >= dtStart2.Value.Date && Convert.ToDateTime(d.LotteryDate).Date <= dtEnd2.Value.Date).ToList();
            tbPeriod2.Text=queryDatas.Count.ToString();
            foreach (var queryData in queryDatas)
                tbResult2.Text += queryData.Numbers + Environment.NewLine;
            if (queryDatas.Count > 0)
            {
                var result = CompareLotteryData(datas, queryDatas);
                lotteryCompareLotteryDatas = result;
                lvResult2.Items.AddRange(result.Select(d => new ListViewItem(new[] { d.NextIssue, d.NextLotteryDate, d.NextNumbers })).ToArray());
            }
            else
            {
                MessageBox.Show("選擇的日期區間沒有資料");
            }
        }
        private List<LotteryCompareLotteryData> CompareLotteryData(List<LotteryData> AllDatas, List<LotteryData> BenchMark)
        {
            List<LotteryCompareLotteryData> result=new List<LotteryCompareLotteryData>();
            
            List<LotteryData> tempList = new List<LotteryData>();
            // 比對 A 和 B 的資料
            for (int i = 0; i <= AllDatas.Count - BenchMark.Count; i++)
            {
                if (AllDatas[i].Issue == BenchMark[0].Issue)
                {
                    continue; // Issue 相同，直接跳過
                }
                bool isMatch = true;            
                for (int j = 0; j < BenchMark.Count; j++)
                {
                    var getMatchedNumbers = string.Empty;
                        if (!IsNumberMatch(AllDatas[i + j].Numbers, BenchMark[j].Numbers, out getMatchedNumbers))
                        {
                            isMatch = false;
                            break; // 一旦有不符合的數字就跳出迴圈
                        }
                    LotteryData tempLotteryData = new LotteryData()
                    {
                        Issue = AllDatas[i + j].Issue,
                        Numbers = AllDatas[i + j].Numbers,
                        LotteryDate = AllDatas[i + j].LotteryDate,
                        PointNumbers = getMatchedNumbers
                    };
                    tempList.Add(tempLotteryData);                    
                }

                if (isMatch)
                {
                    if (i == 0)
                    {
                        LotteryCompareLotteryData data = new LotteryCompareLotteryData()
                        {
                            Datas = tempList.ToList(),
                            NextLotteryDate = "未開獎",
                            NextIssue = "未開獎",
                            NextNumbers = "未開獎"
                        };
                        result.Add(data);
                    }
                    else
                    {
                        LotteryCompareLotteryData data = new LotteryCompareLotteryData()
                        {
                            Datas = tempList.ToList(),
                            NextLotteryDate = AllDatas[i - 1].LotteryDate,
                            NextIssue = AllDatas[i - 1].Issue,
                            NextNumbers = AllDatas[i - 1].Numbers
                        };
                        result.Add(data);
                    }                        
                }
                tempList.Clear();
            }
            return result;
        }
        private static bool IsNumberMatch(string numbersA, string numbersB, out string matchedNumbers)
        {
            var numListA = numbersA.Split(',');
            var numListB = numbersB.Split(',');
            var matchedNumList = numListA.Intersect(numListB);

            if (matchedNumList.Any())
            {
                matchedNumbers = string.Join(",", matchedNumList);
                return true;
            }
            else
            {
                matchedNumbers = string.Empty;
                return false;
            }
        }

        private void ReloadListView2()
        {
            lvResult2.Clear();
            lvDetail2.Clear();
            tbResult2.Clear();
            lvResult2.View = View.Details;
            lvResult2.GridLines = true;
            lvResult2.FullRowSelect = true;
            lvResult2.Columns.Add("期號", 200);
            lvResult2.Columns.Add("日期", 300);
            lvResult2.Columns.Add("號碼", 300);

            lvDetail2.View = View.Details;
            lvDetail2.GridLines = true;
            lvDetail2.FullRowSelect = true;
            lvDetail2.Columns.Add("期號", 200);
            lvDetail2.Columns.Add("日期", 300);           
            lvDetail2.Columns.Add("", 40);
            lvDetail2.Columns.Add("", 40);
            lvDetail2.Columns.Add("", 40);
            lvDetail2.Columns.Add("", 40);
            lvDetail2.Columns.Add("", 40);
        }

        private void dtStart2_ValueChanged(object sender, EventArgs e)
        {
            //tbPeriod2GetIssueCount();
        }
        private void dtEnd2_ValueChanged(object sender, EventArgs e)
        {
            //tbPeriod2GetIssueCount();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabControl1.TabPages["tabPage1"])//your specific tabname
            {
                ReloadListView();
            }
            else if (tabControl1.SelectedTab == tabControl1.TabPages["tabPage2"])//your specific tabname
            {
                ReloadListView2();
                dtStart2.Value  = GetDayByIssueCount(5, dtEnd.Value.Date); // 設定起始日期
                dtEnd2.Value = DateTime.Now.Date;              
                //tbPeriod2GetIssueCount();
            }
        }

        private void lvResult2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvResult2.SelectedItems.Count > 0)
            {
                // 獲取總表選中的項目 
                var selectedItem = lvResult2.SelectedItems[0];
                var selectedID = selectedItem.SubItems[0].Text;

                // 清空明細表中的項目 
                lvDetail2.Items.Clear();
                var data = lotteryCompareLotteryDatas.Where(w => w.NextIssue == selectedID).Select(x => x.Datas).FirstOrDefault();
                foreach (var d in data)
                {
                    string[] numbers = d.Numbers.Split(',');
                    string[] pointNumbers = d.PointNumbers.Split(',');

                    // 產生明細資料
                    var listViewItem = new ListViewItem(new string[] { d.Issue, d.LotteryDate });
                    listViewItem.UseItemStyleForSubItems = false;
                    for (int i = 0; i < 5; i++)
                    {
                        var number = numbers[i];
                        var isPointNumber = pointNumbers.Contains(number);

                        if (isPointNumber)
                        {
                            // 將對應欄位的文字設為紅色
                            listViewItem.SubItems.Add(new ListViewItem.ListViewSubItem(listViewItem, number, Color.Red, Color.Empty, lvDetail2.Font) { Font = new Font(lvDetail2.Font, FontStyle.Bold) });
                        }
                        else
                        {
                            // 將對應欄位的文字設為黑色
                            listViewItem.SubItems.Add(new ListViewItem.ListViewSubItem(listViewItem, number));
                        }
                    }
                    lvDetail2.Items.Add(listViewItem);
                }
            }
        }
    }
}
