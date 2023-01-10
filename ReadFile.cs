using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lottery539
{
    class ReadFile
    {
        string path = Path.Combine(System.Windows.Forms.Application.StartupPath);
        log log = new log();
        string fileName = "Lottery";
        //從txt讀檔轉成list
        public List<LotteryData> ReadTxtFile()
        {
            List<LotteryData> ListLotteryData = new List<LotteryData>();
            string TxtPath = Path.Combine(path, fileName + ".txt");
            string line = "";
            string[] Data = default;
            try
            {
                if (File.Exists(TxtPath))
                {
                    using (StreamReader sr = new StreamReader(TxtPath))
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            Data = line.Split(':');
                            ListLotteryData.Add(new LotteryData { Issue = Data[0], LotteryDate = Data[1], Numbers = Data[2]});
                        }
                    }
                }
                else
                {
                    log.WriteLog("找不到" + TxtPath);
                    return ListLotteryData;
                }
            }
            catch (Exception ex)
            {
                log.WriteLog("ReadFileToList錯誤:" + ex.Message);
            }
            return ListLotteryData;
        }
    }
}
