using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YxLiCai.Model.Order;

namespace Yx.LiCai.Models
{
    public class UserAssetsViewModel
    {
        public UserAssetsViewModel(YxLiCai.Model.ExtendModel.UserAssetsEx orderinfo)
        {
            this.CreateTime = orderinfo.create_time;
            this.TotalRate = 100*orderinfo.interest_rate;
            this.CurrentRate =100* (orderinfo.interest_rate - orderinfo.interest_rate_added);
            this.ExRate = 100*orderinfo.interest_rate_added;
            this.ActualMoney = orderinfo.order_investment;
            this.Income = orderinfo.interest_added;
            this.OrderInfoID = orderinfo.id;
            this.EndTime = orderinfo.expiration_time.AddDays(-1);
        }
        public long OrderInfoID { get; set; }
        public DateTime CreateTime { get; set; }
        public decimal TotalRate { get; set; }
        public decimal CurrentRate { get; set; }
        public decimal ExRate { get; set; }
        public decimal ActualMoney { get; set; }
        public decimal Income { get; set; }
        public DateTime EndTime { get; set; }
    }
}