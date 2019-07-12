using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Server.Common.ResponseModel
{
    /// <summary>
    /// 提现借口响应类
    /// </summary>
    public class WithdrawResponse
    {
        public string error_code { get; set; }
        public string error_msg { get; set; }

        public string merchantaccount { get; set; }
        public string requestid { get; set; }
        public string ybdrawflowid { get; set; }
        public int amount { get; set; }
        public string card_top { get; set; }
        public string card_last { get; set; }
        public string status { get; set; }
        public string sign { get; set; }
    }
}
