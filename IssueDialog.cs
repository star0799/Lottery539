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
    public partial class IssueDialog : Form
    {
        public IssueDialog()
        {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            string inputIssue = tbIssue.Text;
            string inputDate = dtDate.Text;
            string inputNumbers = tbNumbers.Text;
            if (string.IsNullOrEmpty(inputIssue) || string.IsNullOrEmpty(inputDate) || string.IsNullOrEmpty(inputNumbers))
            {
                MessageBox.Show("欄位不得為空");
                return;
            }

            char[] separators = { ',', '.',':','+','-','/','\\','`','，','。','?' };

            // 去掉开头和结尾的逗号和句点，然后按逗号分割字符串
            string[] arrNumbers = inputNumbers.Trim(separators).Split(separators);
            if (arrNumbers.Length != 5)
            {
                MessageBox.Show("格式錯誤!");
                return;
            }
            ReadFile readFile = new ReadFile();
            List<LotteryData> datas = readFile.ReadTxtFile();
            if (datas.Select(d => d.Issue).Contains(inputIssue))
            {
                MessageBox.Show("重複的期號!");
                return;
            }
            inputDate = Convert.ToDateTime(inputDate).ToString("yyyy-MM-dd");
            if (datas.Select(d => d.LotteryDate).Contains(inputDate))
            {
                MessageBox.Show("重複的日期!");
                return;
            }
            WriteFile writeFile = new WriteFile();
            writeFile.InsertLottery(new LotteryData { Issue = inputIssue, LotteryDate = inputDate, Numbers = inputNumbers });
            this.Close();
            MessageBox.Show("新增成功");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
