using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YxLiCai.Model.UserAccumulatedEarnings;


namespace Yx.LiCai.Models.UserMoney
{
    public class UserYearViewModel
    {
        public UserYearViewModel(UserCountYear_AccumulatedEarnings_Model orderinfo)
        {
            this.CreateTime = orderinfo.create_time;
            this.EarningsAmount = orderinfo.earningsamount;
     
        }
        public DateTime CreateTime { get; set; }
        public decimal EarningsAmount { get; set; }    
 
    }
}