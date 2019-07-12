using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Model.UserFinancingManagement
{
    /// <summary>
    /// 融资方提现记录
    /// </summary>
    public class Fer_Withdraw
    {
        public Fer_Withdraw()
        { }
        #region Model
        private int _fer_id = 0;
        private int _type = 0;
        private DateTime _c_time;
        private decimal _amount = 0.00000000M;
        private int _status = 0;
        private string _bankcard = "0";
        private string _user_withdraw_id;
        private int _id;
        private DateTime _m_time;
        private int _creator_id;
        private int _version;
        private string _remark;
        /// <summary>
        /// 用户账户ID
        /// </summary>
        public int fer_id
        {
            set { _fer_id = value; }
            get { return _fer_id; }
        }
        /// <summary>
        /// 提现方式（0易宝提现）
        /// </summary>
        public int type
        {
            set { _type = value; }
            get { return _type; }
        }
        /// <summary>
        /// 提现时间
        /// </summary>
        public DateTime c_time
        {
            set { _c_time = value; }
            get { return _c_time; }
        }
        /// <summary>
        /// 提现金额
        /// </summary>
        public decimal amount
        {
            set { _amount = value; }
            get { return _amount; }
        }
        /// <summary>
        /// 提现状态（0待审核,1审核不通过,2（审核通过）成功，3失败，）
        /// </summary>
        public int status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 银行卡号
        /// </summary>
        public string bankcard
        {
            set { _bankcard = value; }
            get { return _bankcard; }
        }
        /// <summary>
        /// 提现订单号
        /// </summary>
        public string user_withdraw_id
        {
            set { _user_withdraw_id = value; }
            get { return _user_withdraw_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int id
        {
            set { _id = value; }
            get { return _id; }
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
        /// 创建者id
        /// </summary>
        public int creator_id
        {
            set { _creator_id = value; }
            get { return _creator_id; }
        }
        /// <summary>
        /// 版本号(乐观锁)
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
