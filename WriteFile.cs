using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lottery539
{
    class WriteFile
    {
        string path = Path.Combine(System.Windows.Forms.Application.StartupPath);
        string fileName = "Lottery";
        //判斷檔案和檔案內容是否存在
        //public bool IsExistData(string maxIssue, int year)
        //{
        //    bool result = true;
        //    string line = "";
        //    string DataPath = Path.Combine(path, "Lottery" + ".txt");
        //    if (File.Exists(DataPath))
        //    {
        //        StreamReader sr = new StreamReader(DataPath);
        //        while ((line = sr.ReadLine()) != null)
        //        {
        //            if (line.Contains(year.ToString()))
        //            {
        //                result = true;

        //                break;
        //            }
        //            else
        //                result = false;
        //        }
        //        sr.Close();
        //    }
        //    else
        //    {
        //        result = false;
        //    }
        //    return result;
        //}
        public void WriteData(List<LotteryData> data)
        {
            string FileName = Path.Combine(path, fileName + ".txt");
            if (File.Exists(FileName))
                 File.Delete(FileName);
            using (StreamWriter sw = new StreamWriter(FileName, true))
            {
                foreach (var d in FormatDataFile(data))
                    sw.WriteLine(d);
                sw.Close();
            }
        }
        public List<string> FormatDataFile(List<LotteryData> data)
        {
            List<string> result = new List<string>();
            foreach (var d in data)
            {
                result.Add(d.Issue+":"+d.LotteryDate+":"+ SplitEmpty(d.Numbers));
            }
            return result;
        }
        public string SplitEmpty(string Numbers)
        {
            string result = string.Empty;
            string[] splitList = Numbers.Split(',');
            foreach(var s in splitList)
            {
                result += s.Trim()+",";
            }
            result = result.Substring(0,result.Length-1);
            return result;
        }
    }
}
