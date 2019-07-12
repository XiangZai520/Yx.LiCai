/*
 * 融资方用户账户充值记录统计日志（帮助赵亮添加）
 * 作者：侯裕祥
 * 时间：2015年6月16日 09:53:53
 * 版本：1.0.0.0
 * 
 */
using System;

namespace YxLiCai.Model.UserFinancingManagement
{
    /// <summary>
    /// 融资方用户账户充值记录统计日志（帮助赵亮添加）
    /// </summary>
    public class Financier_account_log
    {
        public Financier_account_log()
        { }
        #region Model
        private int _id;
        private DateTime _c_time;
        private int _creator_id;
        private DateTime _m_time;
        private int _fer_id;
        private int _fer_account_id;
        private int _type;
        private decimal _amount_before;
        private decimal _amount_after;
        private decimal _change_amount;
        private int _account_source_id;
        private int _from_id;
        private int _pjt_id;
        private int _user_id;
        private int _ord_id;
        private int _rfd_id;
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
        /// 创建时间(必填)
        /// </summary>
        public DateTime c_time
        {
            set { _c_time = value; }
            get { return _c_time; }
        }
        /// <summary>
        /// 创建者id
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
        /// 融资方id(必填)
        /// </summary>
        public int fer_id
        {
            set { _fer_id = value; }
            get { return _fer_id; }
        }
        /// <summary>
        /// 融资方账户id
        /// </summary>
        public int fer_account_id
        {
            set { _fer_account_id = value; }
            get { return _fer_account_id; }
        }
        /// <summary>
        /// 类型(0提取1存入)（必填）
        /// </summary>
        public int type
        {
            set { _type = value; }
            get { return _type; }
        }
        /// <summary>
        /// 变化前金额
        /// </summary>
        public decimal amount_before
        {
            set { _amount_before = value; }
            get { return _amount_before; }
        }
        /// <summary>
        /// 变化后金额
        /// </summary>
        public decimal amount_after
        {
            set { _amount_after = value; }
            get { return _amount_after; }
        }
        /// <summary>
        /// 变化金额（必填）
        /// </summary>
        public decimal change_amount
        {
            set { _change_amount = value; }
            get { return _change_amount; }
        }
        /// <summary>
        /// 资金渠道(0项目还款1用户购买2项目放款)-（必填）
        /// </summary>
        public int account_source_id
        {
            set { _account_source_id = value; }
            get { return _account_source_id; }
        }
        /// <summary>
        /// 出款账户id
        /// </summary>
        public int from_id
        {
            set { _from_id = value; }
            get { return _from_id; }
        }
        /// <summary>
        /// 项目id
        /// </summary>
        public int pjt_id
        {
            set { _pjt_id = value; }
            get { return _pjt_id; }
        }
        /// <summary>
        /// 用户id
        /// </summary>
        public int user_id
        {
            set { _user_id = value; }
            get { return _user_id; }
        }
        /// <summary>
        /// 订单id
        /// </summary>
        public int ord_id
        {
            set { _ord_id = value; }
            get { return _ord_id; }
        }
        /// <summary>
        /// 退款单id
        /// </summary>
        public int rfd_id
        {
            set { _rfd_id = value; }
            get { return _rfd_id; }
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
