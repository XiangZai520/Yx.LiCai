using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YxLiCai.Model.UserAccumulatedEarnings;


namespace Yx.LiCai.Models.UserMoney
{
    public class UserMonthViewModel
    {
        public UserMonthViewModel(UserCountMonth_AccumulatedEarnings_Model orderinfo)
        {
            this.CreateTime = orderinfo.create_time;
            this.EarningsAmount = orderinfo.earningsamount;
            this.Type = orderinfo.type;
        }
        public int Type { get; set; }
        public DateTime CreateTime { get; set; }
        public decimal EarningsAmount { get; set; }    
 
    }
}