
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
            log.WriteLog("匯入資料開始...");
            ReadFile readFile = new ReadFile();
            List<LotteryData> datas = readFile.ReadTxtFile();
            log.WriteLog("匯入資料完成...");
            Dictionary<string, int> groupNum57 = GetGroupNum(57, datas);
            Dictionary<string, int> groupNum48 = GetGroupNum(48, datas);
            List<string> hotNums =groupNum57.Where(g => g.Value > 10).Select(s => s.Key).ToList();
            string horNumString = helpers.GetNumString(hotNums);
            foreach (var d in datas)
            {
                string hotPointNumString = GetHotPointNum(hotNums,d.Numbers);               
                lvShow.Items.Add(new ListViewItem(new string[] { d.Issue,d.LotteryDate,"","",d.Numbers, horNumString, hotPointNumString,"" }));
            }         
            var a = "";
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
            ReloadListView();
        }
        private void ReloadListView()
        {
            lvShow.Clear();
            lvStatistics.Clear();
            lvShow.View = View.Details;
            lvShow.GridLines = true;
            lvShow.FullRowSelect = true;
            lvShow.Columns.Add("期號", 80);
            lvShow.Columns.Add("日期", 150);
            lvShow.Columns.Add("48期", 80);
            lvShow.Columns.Add("57期", 80);
            lvShow.Columns.Add("開獎號碼", 200);
            lvShow.Columns.Add("熱門號", 200);
            lvShow.Columns.Add("當期開出熱門號", 150);
            lvShow.Columns.Add("均值", 70);

            lvStatistics.View = View.Details;
            lvStatistics.GridLines = true;
            lvStatistics.FullRowSelect = true;
            lvStatistics.Columns.Add("期號", 80);
            lvStatistics.Columns.Add("日期", 80);
            lvStatistics.Columns.Add("48期", 80);
            lvStatistics.Columns.Add("57期", 80);
            lvStatistics.Columns.Add("開獎號碼", 120);
            lvStatistics.Columns.Add("熱門號", 120);
            lvStatistics.Columns.Add("當期開出熱門號", 120);
            lvStatistics.Columns.Add("均值", 70);
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
            if(result.Length!=0)
               result = result.Substring(0, result.Length - 1);
            return result;
        }
    }
}
