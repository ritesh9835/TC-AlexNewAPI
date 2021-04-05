using System;
using System.Collections.Generic;
using System.Text;

namespace TazzerClean.Util.Models
{
    public class Common
    {
        public string Name { get; set; }
        public string LogoUrl { get; set; } = "";
    }
    public class WelcomeEmail : Common
    {
        public string VerifyUrl { get; set; }
    }
}
