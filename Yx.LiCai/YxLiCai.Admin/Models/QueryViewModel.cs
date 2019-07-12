using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YxLiCai.Admin.Models
{
    public class QueryViewModel
    {
        public List<SortDescription> Sorting { get; set; }
        public FilterContainer Filter { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
    }
    public class SortDescription
    {
        public string field { get; set; }
        public string dir { get; set; }
    }

    public class FilterContainer
    {
        public List<FilterDescription> filters { get; set; }
        public string logic { get; set; }
    }

    public class FilterDescription
    {
        public string @operator { get; set; }
        public string field { get; set; }
        public string value { get; set; }
    }
    public class ProductQueryViewModel : QueryViewModel
    {
        public string ProductName { get; set; }
        public int ProductCategory { get; set; }
        public int ProductStatus { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int UserID { get; set; }
    }
    public class ProjuctQueryViewModel : QueryViewModel
    {
        private int _ProjectStatus = -1;
        public string ProjectName { get; set; }
        public int ProjectStatus
        {
            set { _ProjectStatus = value; }
            get { return _ProjectStatus; }
        }
        public DateTime LaunchTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Weight { get; set; }
        public int EndWeight { get; set; }
    }
    public class UserFinancingQueryViewModel : QueryViewModel
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 还款期限
        /// </summary>
        public int LoanPeriod { get; set; }
        /// <summary>
        /// 项目金额-str
        /// </summary>
        public decimal recharge_moneyStar { get; set; }
        /// <summary>
        /// 项目金额-end
        /// </summary>
        public decimal recharge_moneyEnd { get; set; }
        /// <summary>
        /// 日期-str
        /// </summary>
        public DateTime recharge_timeStar { get; set; }
        /// <summary>
        /// 日期-end
        /// </summary>
        public DateTime recharge_timeEnd { get; set; }
        /// <summary>
        /// 待付利息
        /// </summary>
        public decimal interest_payable { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string Order { get; set; }
    }

}