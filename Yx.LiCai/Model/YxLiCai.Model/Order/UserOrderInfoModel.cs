using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Model.Order
{ 
    /// <summary>
    /// order_info:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class order_info
    {
        public order_info()
        { }
        #region Model
        private long _id;
        private DateTime _create_time;
        private long _user_id;
        private decimal _order_investment = 0.00000000M;
        private decimal _order_withdrawal = 0.00000000M;
        private int _product_id;
        private int _product_type;
        private int _order_type;
        private int _order_status;
        private int _refund_status;
        private int _interest_pay_type;
        private decimal _interest_rate;
        private decimal _interest_rate_added;
        private decimal _interest_added;
        private decimal _interest_paid;
        private DateTime _invest_begin_time;
        private DateTime _invest_end_time;
        private DateTime? _expiration_time;
        private int _version = 0;
        private string _remark;
        private int _atone_status=0;
        /// <summary>
        /// auto_increment
        /// </summary>
        public long id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime create_time
        {
            set { _create_time = value; }
            get { return _create_time; }
        }
        /// <summary>
        /// 
        /// </summary>
        public long user_id
        {
            set { _user_id = value; }
            get { return _user_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal order_investment
        {
            set { _order_investment = value; }
            get { return _order_investment; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal order_withdrawal
        {
            set { _order_withdrawal = value; }
            get { return _order_withdrawal; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int product_id
        {
            set { _product_id = value; }
            get { return _product_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int product_type
        {
            set { _product_type = value; }
            get { return _product_type; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int order_type
        {
            set { _order_type = value; }
            get { return _order_type; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int order_status
        {
            set { _order_status = value; }
            get { return _order_status; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int refund_status
        {
            set { _refund_status = value; }
            get { return _refund_status; }
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
        public DateTime invest_begin_time
        {
            set { _invest_begin_time = value; }
            get { return _invest_begin_time; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime invest_end_time
        {
            set { _invest_end_time = value; }
            get { return _invest_end_time; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? expiration_time
        {
            set { _expiration_time = value; }
            get { return _expiration_time; }
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

        public int atone_status
        {
            set { _atone_status = value; }
            get { return _atone_status; }
        }
        #endregion Model

    }
}
