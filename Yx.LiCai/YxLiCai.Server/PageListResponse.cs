using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Server
{
    /// <summary>
    /// 分页基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageListResponse<T>
    {
        /// <summary>
        /// 是否执行成功
        /// </summary>
        public bool Result
        {
            get;
            set;
        }
        /// <summary>
        /// 分页参数
        /// </summary>
        public PagingResult PagingResult     
        {
            get;
            set;
        }
        private List<T> _dataobject = new List<T>();
        /// <summary>
        /// 数据对象
        /// </summary>
        [Description("数据对象")]
        public List<T> DataObject
        {
            get { return this._dataobject; }
            set { this._dataobject = value; }
        }
    }
}
