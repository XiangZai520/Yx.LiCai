using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Model.UserRaise
{
    /// <summary>
    /// user_purchase_records:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class UserRaiseProductModel
    {
        public UserRaiseProductModel()
        { }
        #region Model
        private long _id;
        private DateTime _create_time;
        private DateTime _e_time;
        private long _user_id;
        private decimal _purchase_amount = 0.00000000M;
        private decimal _purchase_amount_balance = 0.00000000M;
        private decimal _purchase_amount_bank = 0.00000000M;
        private int _status = 0;
        private int _op_status = 0;
        private decimal _interest_rate = 0.00000000M;
        private decimal _interest_rate_added = 0.00000000M;
        private int _product_id = 0;
        private int _product_type = 0;
        private string _interest_rate_coupons;
        private string _third_order_id;
        private long? _records_summary_id;
        private int _version = 0;
        private string _remark;
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
        public DateTime e_time
        {
            set { _e_time = value; }
            get { return _e_time; }
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
        public decimal purchase_amount
        {
            set { _purchase_amount = value; }
            get { return _purchase_amount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal purchase_amount_balance
        {
            set { _purchase_amount_balance = value; }
            get { return _purchase_amount_balance; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal purchase_amount_bank
        {
            set { _purchase_amount_bank = value; }
            get { return _purchase_amount_bank; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int op_status
        {
            set { _op_status = value; }
            get { return _op_status; }
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
        public string interest_rate_coupons
        {
            set { _interest_rate_coupons = value; }
            get { return _interest_rate_coupons; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string third_order_id
        {
            set { _third_order_id = value; }
            get { return _third_order_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public long? records_summary_id
        {
            set { _records_summary_id = value; }
            get { return _records_summary_id; }
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
        #endregion Model

    }
 
}
