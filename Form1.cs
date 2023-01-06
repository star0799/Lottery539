
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
    }
}
