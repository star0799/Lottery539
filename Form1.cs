
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
            int lotteryType = cbLotteryType.SelectedIndex;
            Dictionary<string, int> groupNum = new Dictionary<string, int>();
            int MaxVal =Convert.ToInt16(tbMax.Text);
            int MinVal = Convert.ToInt16(tbMin.Text);
           
            if (lotteryType == 1)
                groupNum = GetGroupNum(57, datas);
            else
            {
                groupNum = GetGroupNum(48, datas);
                datas = datas.Take(48).ToList();
            }
            int[] intNumsCount = groupNum.Values.ToArray();
            string[] numsCount = intNumsCount.Select(x => x.ToString()).ToArray();

            lvStatistics.Font = new Font(lvStatistics.Font, FontStyle.Bold);
            foreach (var nums in groupNum)
            lvStatistics.Columns.Add(nums.Key,30);
            lvStatistics.Items.Add(new ListViewItem(numsCount));
            lvStatistics.Items[0].Font = new Font("新細明體", 12f, FontStyle.Regular);



            List<string> hotNums = groupNum.Where(g => g.Value >= MaxVal).Select(s => s.Key).ToList();
            List<string> coldNums = groupNum.Where(g => g.Value <= MinVal).Select(s => s.Key).ToList();
            string horNumString = string.Empty;
            string coldNumString = string.Empty;
            string hotPointNumString = string.Empty;
            horNumString = helpers.GetNumString(hotNums);
            coldNumString = helpers.GetNumString(coldNums);

            foreach (var d in datas)
            {
                hotPointNumString = GetHotPointNum(hotNums, d.Numbers);
                lvShow.Items.Add(new ListViewItem(new string[] { d.Issue,d.LotteryDate,d.Numbers, hotPointNumString}));
            }
            lvNumResult.Items.Add(new ListViewItem(new string[] { horNumString,coldNumString }));
        }
        private Dictionary<string, int> GetGroupNum(int groupCount, List<LotteryData> datas)
        {
            List<int> intlist = new List<int>();
            Dictionary<string, int> keyValues = new Dictionary<string, int>();
            string tmpString = string.Empty;

            datas = datas.Take(groupCount).ToList();
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
                    key =x.Key;
                keyValues.Add(key, x.Value);
            }
            Dictionary<string, int> result = keyValues.OrderBy(o =>Convert.ToInt32(o.Key)).ToDictionary(o => o.Key, p => p.Value);
            return result;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cbLotteryType.Items.Add("48期");
            cbLotteryType.Items.Add("57期");
            cbLotteryType.SelectedIndex = 0;
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

            lvStatistics.View = View.Details;
            lvStatistics.GridLines = true;
            lvStatistics.FullRowSelect = true;

            lvNumResult.View = View.Details;
            lvNumResult.GridLines = true;
            lvNumResult.FullRowSelect = true;
            lvNumResult.Columns.Add("熱門號", 600);
            lvNumResult.Columns.Add("冷門號", 600);
        }
        public string GetHotPointNum(List<string> hotNums, string num)
        {
            string result = string.Empty;
            var numList=helpers.GetNumList(num);
            foreach(var n in numList)
            {
                if (hotNums.Contains(n))
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

        private void cbLotteryType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbLotteryType.SelectedIndex == 1)
            {
                tbMax.Text = "9";
                tbMin.Text = "5";
            }
            else
            {
                tbMax.Text = "8";
                tbMin.Text = "4";
            }
        }
    }
}
