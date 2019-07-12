using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Model.TransactionRecords
{
    /// <summary>
    /// 用户购买记录
    /// </summary>
    public class Ord_infoModel
    {
        public Ord_infoModel()
        { }
        #region Model
        private int _id;
        private DateTime _c_time;
        private int _creator_id;
        private DateTime _m_time;
        private int _user_id;
        private decimal _amount = 0.00000000M;
        private decimal _amount_withdraw = 0.00000000M;
        private decimal _amount_violate = 0.00000000M;
        private int _pdt_id = 0;
        private int _pdt_type = 0;
        private int _ord_type = 0;
        private int _ord_status = 0;
        private int _rfd_status = 0;
        private int _interest_pay_type;
        private decimal _interest_rate = 0.00000000M;
        private decimal _interest_rate_coupon;
        private decimal _interest_rate_added = 0.00000000M;
        private decimal _interest_added = 0.00000000M;
        private decimal _interest_paid = 0.00000000M;
        private DateTime _invest_b_time;
        private DateTime _invest_e_time;
        private DateTime _expiration_time;
        private int _atone_status = 0;
        private int _version = 0;
        private string _remark;
        private string _mark;
        /// <summary>
        /// auto_increment
        /// </summary>
        public int id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime c_time
        {
            set { _c_time = value; }
            get { return _c_time; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int creator_id
        {
            set { _creator_id = value; }
            get { return _creator_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime m_time
        {
            set { _m_time = value; }
            get { return _m_time; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int user_id
        {
            set { _user_id = value; }
            get { return _user_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal amount
        {
            set { _amount = value; }
            get { return _amount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal amount_withdraw
        {
            set { _amount_withdraw = value; }
            get { return _amount_withdraw; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal amount_violate
        {
            set { _amount_violate = value; }
            get { return _amount_violate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int pdt_id
        {
            set { _pdt_id = value; }
            get { return _pdt_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int pdt_type
        {
            set { _pdt_type = value; }
            get { return _pdt_type; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int ord_type
        {
            set { _ord_type = value; }
            get { return _ord_type; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int ord_status
        {
            set { _ord_status = value; }
            get { return _ord_status; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int rfd_status
        {
            set { _rfd_status = value; }
            get { return _rfd_status; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int interest_pay_type
        {
            set { _interest_pay_type = value; }
            get { return _interest_pay_type; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal interest_rate
        {
            set { _interest_rate = value; }
            get { return _interest_rate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal interest_rate_coupon
        {
            set { _interest_rate_coupon = value; }
            get { return _interest_rate_coupon; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal interest_rate_added
        {
            set { _interest_rate_added = value; }
            get { return _interest_rate_added; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal interest_added
        {
            set { _interest_added = value; }
            get { return _interest_added; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal interest_paid
        {
            set { _interest_paid = value; }
            get { return _interest_paid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime invest_b_time
        {
            set { _invest_b_time = value; }
            get { return _invest_b_time; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime invest_e_time
        {
            set { _invest_e_time = value; }
            get { return _invest_e_time; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime expiration_time
        {
            set { _expiration_time = value; }
            get { return _expiration_time; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int atone_status
        {
            set { _atone_status = value; }
            get { return _atone_status; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int version
        {
            set { _version = value; }
            get { return _version; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string mark
        {
            set { _mark = value; }
            get { return _mark; }
        }

        /// <summary>
        /// 订单号
        /// </summary>
        public string Order { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public int ProductName { get; set; }
        /// <summary>
        /// 年化利率
        /// </summary>
        public string YerRate { get; set; }
        /// <summary>
        /// 加息券
        /// </summary>
        public string InterestRate { get; set; }
        /// <summary>
        /// 投资金额
        /// </summary>
        public decimal Amount { get; set; }
        #endregion Model

    }
}
