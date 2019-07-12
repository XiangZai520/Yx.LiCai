using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Yx.LiCai.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        /// <summary>
        /// 产品类型
        /// </summary>
        public int Category { get; set; }
        /// <summary>
        /// month;year
        /// </summary>
        public string CategoryName { get; set; }
        /// <summary>
        /// 产品规则
        /// </summary>
        public string Rule { get; set; }
        /// <summary>
        /// 起始年化收益率
        /// </summary>
        public string YieldBase { get; set; }
        /// <summary>
        /// 递增年华收益率
        /// </summary>
        public string YieldIncrease { get; set; }
        /// <summary>
        /// 最高年化收益率
        /// </summary>
        public string YieldTop { get; set; }
        /// <summary>
        /// 已募金额百分比
        /// </summary>
        public string RaisedRate { get; set; }
        /// <summary>
        /// 已购买该产品人数
        /// </summary>
        public string PurchasedMemberSum { get; set; }
        /// <summary>
        /// 流动性描述
        /// </summary>
        public string Fluidness { get; set; }
        /// <summary>
        /// 可投金额
        /// </summary>
        public decimal AvailableAmount { get; set; }
        /// <summary>
        /// 产品状态
        /// </summary>
        public int ProductStatus { get; set; }
        /// <summary>
        /// 上线日期
        /// </summary>
        public DateTime EnableDate { get; set; }
        /// <summary>
        /// 产品详情链接
        /// </summary>
        public string DetailLink { get; set; }
        /// <summary>
        /// 安全保障链接
        /// </summary>
        public string SafetyLink { get; set; }
    }
}