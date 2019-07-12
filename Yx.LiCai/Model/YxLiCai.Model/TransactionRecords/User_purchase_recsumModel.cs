using System;

namespace YxLiCai.Model.TransactionRecords
{
    /// <summary>
    /// 用户购买记录
    /// </summary>
    public class User_purchase_recsumModel
    {
        public User_purchase_recsumModel()
        { }
        #region Model
        private int _id;
        private DateTime _c_time;
        private long _user_id;
        private int _ord_id;
        private decimal _purchase_amount;
        private int _status;
        private decimal _interest_rate;
        private decimal _interest_rate_added;
        private int _product_id;
        private int _pdt_type;
        private int _version = 0;
        private string _remark;
        /// <summary>
        /// auto_increment
        /// </summary>
        public int id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 投资时间
        /// </summary>
        public DateTime c_time
        {
            set { _c_time = value; }
            get { return _c_time; }
        }
        /// <summary>
        /// 用户id
        /// </summary>
        public long user_id
        {
            set { _user_id = value; }
            get { return _user_id; }
        }
        /// <summary>
        /// 生成订单的ID
        /// </summary>
        public int ord_id
        {
            set { _ord_id = value; }
            get { return _ord_id; }
        }
        /// <summary>
        /// 投资金额
        /// </summary>
        public decimal purchase_amount
        {
            set { _purchase_amount = value; }
            get { return _purchase_amount; }
        }
        /// <summary>
        /// 状态，0:未处理 1:已处理
        /// </summary>
        public int status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 总利率
        /// </summary>
        public decimal interest_rate
        {
            set { _interest_rate = value; }
            get { return _interest_rate; }
        }
        /// <summary>
        /// 已增利率
        /// </summary>
        public decimal interest_rate_added
        {
            set { _interest_rate_added = value; }
            get { return _interest_rate_added; }
        }
        /// <summary>
        /// 产品id
        /// </summary>
        public int product_id
        {
            set { _product_id = value; }
            get { return _product_id; }
        }
        /// <summary>
        /// 类型（1:Q1月，2:Q3,3:Q6，4:Q12）
        /// </summary>
        public int pdt_type
        {
            set { _pdt_type = value; }
            get { return _pdt_type; }
        }
        /// <summary>
        /// 乐观锁
        /// </summary>
        public int version
        {
            set { _version = value; }
            get { return _version; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string remark
        {
            set { _remark = value; }
            get { return _remark; }
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
        public string ProductName { get; set; }
        /// <summary>
        /// 年化利率
        /// </summary>
        public string YerRate { get; set; }
        /// <summary>
        /// 加息券
        /// </summary>
        public string InterestRate { get; set; }
        #endregion Model

    }
}
