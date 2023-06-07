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
        public void WriteData(List<LotteryData> data)
        {
            string FileName = Path.Combine(path, fileName + ".txt");
            List<string> existingLines = File.ReadAllLines(FileName).ToList();
            List<string> newDataLines = FormatDataFile(data);
            existingLines.InsertRange(0, newDataLines);
            File.WriteAllLines(FileName, existingLines);
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
