
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
            ReadFile readFile = new ReadFile();
            List<LotteryData> datas = readFile.ReadTxtFile();
            GetGroupNum(57,datas);            
        }
        private void GetGroupNum(int groupCount, List<LotteryData> datas)
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
            //aaa
        }
    }
}
