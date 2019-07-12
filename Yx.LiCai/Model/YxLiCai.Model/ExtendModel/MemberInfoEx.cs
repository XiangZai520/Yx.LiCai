using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Model.ExtendModel
{
    /// <summary>
    /// 会员信息
    /// </summary>
    public class MemberInfoEx
    {
        //a.LoginName,a.MyRealName,a.Phone,a.RegTime,a.login_times,a.last_login_time,a.Status
        public long UserID { get; set; }
        public string LoginName { get; set; }
        public string MyRealName { get; set; }
        public string Phone { get; set; }
        public DateTime RegTime { get; set; }
        public int login_times { get; set; }
        public DateTime last_login_time { get; set; }
        public int Status { get; set; }
        public decimal TotalMoney { get; set; }

        public string RegTime_s { get; set; }
        public string last_login_time_s { get; set; }
    }
}
