
using System.Collections.Generic;
using YxLiCai.Model.User;

namespace Yx.LiCai.Models
{
    /// <summary>
    /// 交易记录显示类
    /// </summary>
    public class UserAmountRecModelList
    {
        /// <summary>
        /// 当前页面
        /// </summary>
        public int CurrentPage { get; set; }
        /// <summary>
        /// 一共多少条
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 产品类型
        /// </summary>
        public int ProductType { get; set; }
        /// <summary>
        /// 显示数据
        /// </summary>
        public List<UserAmountRecModel> listModel { get; set; }
    }
}