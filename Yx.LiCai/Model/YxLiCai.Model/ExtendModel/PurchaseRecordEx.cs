using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Model.ExtendModel
{
    /// <summary>
    /// 投资记录
    /// </summary>
    public class PurchaseRecordEx
    {
        public long OrderInfoID { get; set; }
        public string LoginName { get; set; }
        public int ProductType { get; set; }
        public decimal PurchaseMoney { get; set; }
        public decimal YearRate { get; set; }
        public decimal InterestRateCoupon { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
