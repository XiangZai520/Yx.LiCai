
using System;
namespace YxLiCai.Model.Refund
{
    /// <summary>
    /// 还款相关业务实体类    
    /// </summary>
    public class RefundModel
    {
        public RefundModel()
        { }
        #region Model
        private long _id;
        private int _merchantid = 0;
        private int _pjid;
        private decimal _amount = 0M;
        private DateTime _ctime;
        private DateTime? _rtime;
        private int _status = 0;
        /// <summary>
        /// auto_increment
        /// </summary>
        public long id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 融资方id
        /// </summary>
        public int merchantID
        {
            set { _merchantid = value; }
            get { return _merchantid; }
        }
        /// <summary>
        /// 项目id
        /// </summary>
        public int pjid
        {
            set { _pjid = value; }
            get { return _pjid; }
        }
        /// <summary>
        /// 还款金额
        /// </summary>
        public decimal amount
        {
            set { _amount = value; }
            get { return _amount; }
        }
        /// <summary>
        /// 记录创建日期
        /// </summary>
        public DateTime ctime
        {
            set { _ctime = value; }
            get { return _ctime; }
        }
        /// <summary>
        /// 还款时间
        /// </summary>
        public DateTime? rtime
        {
            set { _rtime = value; }
            get { return _rtime; }
        }
        /// <summary>
        /// 状态：0待处理；1已还款；2通知充值
        /// </summary>
        public int status
        {
            set { _status = value; }
            get { return _status; }
        }         
        #endregion Model
    }
    /// <summary>
    /// 还款相关业务实体类 扩展
    /// </summary>
    public class RefundModelExtend : RefundModel
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 项目金额
        /// </summary>
        public decimal ProjectAmount { get; set; }
        /// <summary>
        /// 项目ID
        /// </summary>
        public int pjt_id { get; set; }
        /// <summary>
        /// 项目期限
        /// </summary>
        public int LoanPeriod { get; set; }
        /// <summary>
        /// 融资方名称
        /// </summary>
        public string MerchantName { get; set; }
        /// <summary>
        /// 融资方账户余额
        /// </summary>
        public decimal MerchantBalance { get; set; }
        /// <summary>
        /// 申请还款本金金额
        /// </summary>
        public decimal RepayAmount { get; set; }
        /// <summary>
        /// 还款类型
        /// </summary>
        public string Type { get; set; }
        public decimal interest_paid { get; set; }
    }
}
