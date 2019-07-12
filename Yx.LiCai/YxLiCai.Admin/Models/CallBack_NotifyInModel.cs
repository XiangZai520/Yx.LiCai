using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YxLiCai.Admin.Models
{
    public class CallBack_NotifyInModel
    {
        public string oid_partner { get; set; }
        public string sign_type { get; set; }
        public string sign { get; set; }
        public string dt_order { get; set; }
        public string no_order { get; set; }
        public string oid_paybill { get; set; }
        public string money_order { get; set; }
        public string result_pay { get; set; }
        public string settle_date { get; set; }
        public string info_order { get; set; }
        public string pay_type { get; set; }
        public string bank_code { get; set; }
        public string no_agree { get; set; }
        public string id_type { get; set; }
        public string id_no { get; set; }
        public string acct_name { get; set; }
    }
}