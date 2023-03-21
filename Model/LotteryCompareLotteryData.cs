using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lottery539.Model
{
    internal class LotteryCompareLotteryData
    {
        public string NextIssue { get; set; }
        public string NextLotteryDate { get; set; }
        public string NextNumbers { get; set; }
        public string BenchmarkIssue{ get; set; }
        public string BenchmarkDate { get; set; }
        public string BenchmarkNumbers { get; set; }
        public List<LotteryData> Datas { get; set; } = new List<LotteryData>();
    }
}
