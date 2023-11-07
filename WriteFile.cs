using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
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
            if (!File.Exists(FileName))
            {
                File.Create(FileName).Close();
            }
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
        public void InsertLottery(LotteryData data)
        {
            string FileName = Path.Combine(path, fileName + ".txt");
            if (!File.Exists(FileName))
            {
                File.Create(FileName).Close();
            }
            List<string> existingLines = File.ReadAllLines(FileName).ToList();
            List<string> newDataLines = FormatDataFile(new List<LotteryData> { data });

            // 将新数据的日期解析为 DateTime
            DateTime newDate = DateTime.ParseExact(data.LotteryDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            // 查找要插入的位置
            int insertIndex = 0;
            for (int i = 0; i < existingLines.Count; i++)
            {
                // 解析现有行的日期
                DateTime existingDate = DateTime.ParseExact(existingLines[i].Split(':')[1], "yyyy-MM-dd", CultureInfo.InvariantCulture);

                // 如果新数据日期早于或等于现有行的日期，则找到插入位置
                if (newDate > existingDate)
                {
                    insertIndex = i;
                    break;
                }
                if(insertIndex == 0 && i == existingLines.Count - 1)
                {
                    insertIndex = existingLines.Count;
                }
            }
            // 插入新数据
            existingLines.Insert(insertIndex, newDataLines[0]);

            // 保存按日期排序的数据
            File.WriteAllLines(FileName, existingLines);
        }
    }
}
