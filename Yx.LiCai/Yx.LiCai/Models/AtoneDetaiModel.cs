using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Yx.LiCai.Models
{
    public class AtoneDetaiModel
    {
        public AtoneDetaiModel(YxLiCai.Model.Order.order_info orderinfo)
        {
            this.ProductType = orderinfo.product_type;
            this.ProductTypeName = orderinfo.product_type == 4 ? "年年丰" : "季季享";
            if (orderinfo.product_type == 2)
            {
                this.ProductTypeName = "季季享3个月";
            }
            else if (orderinfo.product_type == 3)
            {
                this.ProductTypeName = "季季享6个月";
            }
            this.BuyMoney = YxLiCai.Tools.Const.SystemConst.MoenyConvert(orderinfo.order_investment);
            this.CreateTime = orderinfo.create_time;
            this.OrderInfoID = orderinfo.id;
            this.EndTime = (orderinfo.expiration_time ?? DateTime.Now).AddDays(-1);
        }
        public long OrderInfoID { get; set; }
        public int ProductType { get; set; }
        public string ProductTypeName { get; set; }
        public decimal BuyMoney { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}