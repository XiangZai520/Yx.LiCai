using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YxLiCai.FinaAdmin.Models
{
    /// <summary>
    /// 分页返回数据model
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageViewModel<T>
    {
        /// <summary>
        /// 数据
        /// </summary>
        public List<T> DataResult { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalCount { get; set; }
    }
}