using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Server
{
    /// <summary>
    /// 最新分页请求实体
    /// </summary>
    public class PagingResult
    {
        private int _pageIndex;

        public PagingResult()
        {

        }
        public PagingResult(int pageIndex, int pageSize)
        {
            this._pageIndex = pageIndex;
            this.PageSize = pageSize;
        }

        public int PageIndex
        {
            get
            {
                return _pageIndex == 0 ? 1 : _pageIndex;
            }
            set { _pageIndex = value; }
        }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }
        public int RecordCount { get; set; }

        public int TotalPages
        {
            get
            {
                var val = (float)TotalCount / PageSize;
                var count = (int)Math.Ceiling(val);
                return count;
            }
        }

        public bool HasPreviousPage
        {
            get { return (PageIndex > 1); }
        }

        public bool HasNextPage
        {
            get { return (PageIndex < TotalPages); }
        }
    } 
}
