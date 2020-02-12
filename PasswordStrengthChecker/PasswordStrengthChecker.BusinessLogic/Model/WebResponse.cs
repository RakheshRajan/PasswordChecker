using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordStrengthChecker.BusinessLogic.Model
{
    public struct WebResponse
    {
        public string StatusCode { get; set; }
        public string Body { get; set; }
        public string HttpException { get; set; }
    }
}
