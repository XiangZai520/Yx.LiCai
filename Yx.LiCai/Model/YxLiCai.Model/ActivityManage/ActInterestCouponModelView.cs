using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Model.ActivityManage
{
    /// <summary>
    /// 加息券活动拓展视图类 by 张浩然 2015-6-30
    /// </summary>
    public class ActInterestCouponModelView:ActInterestCoupon
    {
        public int actid { get; set; }
        public string ad_content { get; set; }

        public int limit_num { get; set; }
        
    }
}
