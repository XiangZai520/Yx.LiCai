using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YxLiCai.Model.ActivityManage;

namespace YxLiCai.Admin.Models
{
    /// <summary>
    /// 修改活动outModel
    /// </summary>
    public class ModifyActivityOut
    {
        public ActPromotion actPromotion { get; set; }
        public List<ActPromotionItem> All_Item { get; set; }
        public List<ActPromotionItem> Choose_Item { get; set; }
    }
}