using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaSoi_Api.Request
{
    public class ChangePass_Request
    {
        public string Tk { get; set; }
        public string PassOld { get; set; }
        public string PassNew { get; set; }
    }
}
