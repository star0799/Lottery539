using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lottery539
{
    class Helpers
    {
        log log = new log();
        public string GetNumString(List<string> nums)
        {
            string result = string.Empty;
           
            foreach (var s in nums)
            {
                result += s.Trim() + ",";
            }
            try
            {
                result = result.Substring(0, result.Length - 1);
            }
            catch(Exception ex)
            {
                result = string.Empty;
                log.WriteLog(ex.Message);
            }
            return result;
        }
        public string GetNumString(List<int> nums)
        {
            string result = string.Empty;
            foreach (var s in nums)
            {
                result += s.ToString().Trim() + ",";
            }
            try
            {
                result = result.Substring(0, result.Length - 1);
            }
            catch (Exception ex)
            {
                result = string.Empty;
                log.WriteLog(ex.Message);
            }
            return result;
        }

        public List<string> GetNumList(string num)
        {
            List<string> result =new  List<string>();
            result = num.Split(',').ToList();
            return result;
        }
    }
}
