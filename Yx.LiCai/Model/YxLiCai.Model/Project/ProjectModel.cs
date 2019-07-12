using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Model.Project
{
    /// <summary>
    /// Project:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class ProjectModel
    {
        public ProjectModel()
        { }
        #region Model
        private int _id = 1;
        private string _projectname = "‘’";
        private string _ordernumber = "";
        private decimal? _amount = 0.00M;
        private int? _projectstatus = 0;
        private int? _verifystatus = 0;
        private int? _projecttype = 0;
        private string _borrower = "";
        private int? _weight = 0;
        private decimal? _availableamount = 0.00M;
        private decimal? _amountsold = 0.00M;
        private DateTime? _addtime;
        private DateTime _launchtime;
        private DateTime _endtime;
        private decimal? _borrowingrate = 0.0000M;
        private string _remark = "";
        private bool _isdeleted = false;
        private string _projectstatuss = "";
        private decimal? _quantity = 0.0000M;
        private decimal? _singleday = 0.0000M;
        private decimal? _fullscaledays = 0.0000M;
        private string _repaymentmode;
        private int? _loanperiod = 0;
        private string _checkstatuss = "";
        private int chushoudays = 0;
        /// <summary>
        /// 主键ID
        /// </summary>
        public int Id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName
        {
            set { _projectname = value; }
            get { return _projectname; }
        }
        /// <summary>
        /// 合同编号
        /// </summary>
        public string OrderNumber
        {
            set { _ordernumber = value; }
            get { return _ordernumber; }
        }
        /// <summary>
        /// 项目金额
        /// </summary>
        public decimal? Amount
        {
            set { _amount = value; }
            get { return _amount; }
        }
        /// <summary>
        /// 项目状态(数字)
        /// </summary>
        public int? ProjectStatus
        {
            set { _projectstatus = value; }
            get { return _projectstatus; }
        }
        /// <summary>
        /// 审核状态
        /// </summary>
        public int? VerifyStatus
        {
            set { _verifystatus = value; }
            get { return _verifystatus; }
        }
        /// <summary>
        /// 项目类型
        /// </summary>
        public int? ProjectType
        {
            set { _projecttype = value; }
            get { return _projecttype; }
        }
        /// <summary>
        /// 借款人
        /// </summary>
        public string Borrower
        {
            set { _borrower = value; }
            get { return _borrower; }
        }
        /// <summary>
        /// 权值
        /// </summary>
        public int? Weight
        {
            set { _weight = value; }
            get { return _weight; }
        }
        /// <summary>
        /// 已售金额
        /// </summary>
        public decimal? AmountSold
        {
            set { _amountsold = value; }
            get { return _amountsold; }
        }
        /// <summary>
        /// 可售金额
        /// </summary>
        public decimal? AvailableAmount
        {
            set { _availableamount = value; }
            get { return _availableamount; }
        }
        /// <summary>
        /// 项目冻结金额
        /// </summary>
        public decimal Amount_able_fz { get; set; }
        /// <summary>
        /// 添加时间(项目创建时间)
        /// </summary>
        public DateTime? AddTime
        {
            set { _addtime = value; }
            get { return _addtime; }
        }
        /// <summary>
        /// 发起时间（项目开始时间）
        /// </summary>
        public DateTime LaunchTime
        {
            set { _launchtime = value; }
            get { return _launchtime; }
        }
        /// <summary>
        /// 截止时间（项目结束时间）
        /// </summary>
        public DateTime EndTime
        {
            set { _endtime = value; }
            get { return _endtime; }
        }
        /// <summary>
        /// 借款利率
        /// </summary>
        public decimal? BorrowingRate
        {
            set { _borrowingrate = value; }
            get { return _borrowingrate; }
        }
        /// <summary>
        /// 备注（通过未通过原因）
        /// </summary>
        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDeleted
        {
            set { _isdeleted = value; }
            get { return _isdeleted; }
        }
        /// <summary>
        /// 项目状态（文本）
        /// </summary>
        public string ProjectStatuss
        {
            set { _projectstatuss = value; }
            get { return _projectstatuss; }
        }
        /// <summary>
        /// 审核状态（文本）
        /// </summary>
        public string Checkstatuss
        {
            set { _checkstatuss = value; }
            get { return _checkstatuss; }
        }

        /// <summary>
        /// 出售量
        /// </summary>
        public decimal? Quantity
        {
            set { _quantity = value; }
            get { return _quantity; }
        }
        /// <summary>
        /// 单日出售量
        /// </summary>
        public decimal? Singleday
        {
            set { _singleday = value; }
            get { return _singleday; }
        }
        /// <summary>
        /// 满标天数
        /// </summary>
        public decimal? Fullscaledays
        {
            set { _fullscaledays = value; }
            get { return _fullscaledays; }
        }
        /// <summary>
        /// 还款方式
        /// </summary>
        public string RepaymentMode
        {
            set { _repaymentmode = value; }
            get { return _repaymentmode; }
        }
        /// <summary>
        /// 借款期限月
        /// </summary>
        public int? LoanPeriod
        {
            set { _loanperiod = value; }
            get { return _loanperiod; }

        }
        /// <summary>
        /// 出售天数
        /// </summary>
        public int Chushoudays
        {
            set { chushoudays = value; }
            get { return chushoudays; }
        }
        /// <summary>
        /// 已放款金额
        /// </summary>
        public decimal amount_lent { get; set; }
        /// <summary>
        /// 融资方ID
        /// </summary>
        public int account_id { get; set; }
        /// <summary>
        /// 融资方名称
        /// </summary>
        public string account_id_name { get; set; }
        /// <summary>
        /// 已还金额
        /// </summary>
        public decimal Amount_paid { get; set; }
        /// <summary>
        /// 融资方可用金额
        /// </summary>
        public decimal amount_repay { get; set; }
        /// <summary>
        /// 融资方冻结金额
        /// </summary>
        public decimal amount_repay_fz { get; set; }
        /// <summary>
        /// 还款状态(0还款中,1审核中,2,已还清)
        /// </summary>
        public int rep_status { get; set; }
        /// <summary>
        /// 融资方应付利息
        /// </summary>
        public decimal interest_payable { get; set; }

        /// <summary>
        /// 跳转URl
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 所选产品类型ID
        /// </summary>
        public string pdt_collection { get; set; }
        #endregion Model

    }
}

