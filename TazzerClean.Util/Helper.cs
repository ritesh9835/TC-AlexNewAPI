using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace TazzerClean.Util
{
    public class Helper
    {
        private static Random random = new Random();
        public string CodeGenerator(int length)
        {
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public string PasswordGenerator(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$&";
            StringBuilder res = new StringBuilder();
           
            while (0 < length--)
            {
                res.Append(valid[random.Next(valid.Length)]);
            }
            return res.ToString();
        }
        public string GenerateUserCode()
        {
            int _min = 0001;
            int _max = 9999;

            return random.Next(_min, _max).ToString();
        }
    }
}
