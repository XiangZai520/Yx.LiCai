/*
 * 融资方用户账户统计表
 * 作者：侯裕祥
 * 时间：2015年6月16日 09:53:53
 * 版本：1.0.0.0
 * 
 */

using System;
namespace YxLiCai.Model.UserFinancingManagement
{
    /// <summary>
    /// 融资方用户账户统计表
    /// </summary>
    public class Fina_User_Count_Model
    {
        public Fina_User_Count_Model()
        { }
        #region Model
        private int _id;
        private int _fer_id;
        private DateTime _c_time;
        private int _creator_id;
        private DateTime _m_time;
        private decimal _amount = 0.00000000M;
        private decimal _amount_user = 0.00000000M;
        private decimal _amount_user_fz = 0.00000000M;
        private decimal _amount_repay = 0.00000000M;
        private decimal _amount_repay_fz = 0.00000000M;
        private decimal _amount_paid = 0.00000000M;
        private decimal _amount_borrow = 0.00000000M;
        private decimal _interest_payable = 0.00000000M;
        private decimal _interest_paid = 0.00000000M;
        private int _version;
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
        /// 融资人id
        /// </summary>
        public int fer_id
        {
            set { _fer_id = value; }
            get { return _fer_id; }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime c_time
        {
            set { _c_time = value; }
            get { return _c_time; }
        }
        /// <summary>
        /// 创建账户id
        /// </summary>
        public int creator_id
        {
            set { _creator_id = value; }
            get { return _creator_id; }
        }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime m_time
        {
            set { _m_time = value; }
            get { return _m_time; }
        }
        /// <summary>
        /// 账户金额(总金额)
        /// </summary>
        public decimal amount
        {
            set { _amount = value; }
            get { return _amount; }
        }
        /// <summary>
        /// 用户投资、提现使用金额
        /// </summary>
        public decimal amount_user
        {
            set { _amount_user = value; }
            get { return _amount_user; }
        }
        /// <summary>
        /// 用户投资、提现使用金额(冻结金额)
        /// </summary>
        public decimal amount_user_fz
        {
            set { _amount_user_fz = value; }
            get { return _amount_user_fz; }
        }
        /// <summary>
        /// 融资方还款金额
        /// </summary>
        public decimal amount_repay
        {
            set { _amount_repay = value; }
            get { return _amount_repay; }
        }
        /// <summary>
        /// 融资方还款金额(冻结金额)
        /// </summary>
        public decimal amount_repay_fz
        {
            set { _amount_repay_fz = value; }
            get { return _amount_repay_fz; }
        }
        /// <summary>
        /// 已还本金
        /// </summary>
        public decimal amount_paid
        {
            set { _amount_paid = value; }
            get { return _amount_paid; }
        }
        /// <summary>
        /// 借款资金
        /// </summary>
        public decimal amount_borrow
        {
            set { _amount_borrow = value; }
            get { return _amount_borrow; }
        }
        /// <summary>
        /// 应付利息
        /// </summary>
        public decimal interest_payable
        {
            set { _interest_payable = value; }
            get { return _interest_payable; }
        }
        /// <summary>
        /// 已付利息
        /// </summary>
        public decimal interest_paid
        {
            set { _interest_paid = value; }
            get { return _interest_paid; }
        }
        /// <summary>
        /// 版本号
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
        #endregion Model
    }
}
