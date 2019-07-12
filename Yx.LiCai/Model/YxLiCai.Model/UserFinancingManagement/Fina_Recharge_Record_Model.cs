/*
 * 融资方用户充值记录
 * 作者：侯裕祥
 * 时间：2015年6月16日 09:53:53
 * 版本：1.0.0.0
 * 
 */
using System;

namespace YxLiCai.Model.UserFinancingManagement
{
    /// <summary>
    /// 融资方用户充值记录
    /// </summary>
    public class Fina_Recharge_Record_Model
    {
        public Fina_Recharge_Record_Model()
        { }
        #region Model
        private int _id;
        private DateTime _c_time;
        private int _creator_id;
        private decimal _amount = 0.00000000M;
        private int _status = 0;
        private string _bankcard;
        private string _third_order;
        private int _repay_type = 0;
        private int _version;
        private string _remark;
        private DateTime _m_time;
        private int _fer_id = 0;
        private int _fer_account_id;
        private int _pay_type = 0;
        /// <summary>
        /// 主键
        /// </summary>
        public int id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 充值时间
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
        /// 充值金额
        /// </summary>
        public decimal amount
        {
            set { _amount = value; }
            get { return _amount; }
        }
        /// <summary>
        /// 充值状态（0失败，1成功）
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
        /// 充值订单号(第三方单号)
        /// </summary>
        public string third_order
        {
            set { _third_order = value; }
            get { return _third_order; }
        }
        /// <summary>
        /// 还款方式(0本金偿还，1利息偿还)
        /// </summary>
        public int repay_type
        {
            set { _repay_type = value; }
            get { return _repay_type; }
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
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime m_time
        {
            set { _m_time = value; }
            get { return _m_time; }
        }
        /// <summary>
        /// 融资方id
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
        /// 充值方式（0易宝充值）
        /// </summary>
        public int pay_type
        {
            set { _pay_type = value; }
            get { return _pay_type; }
        }

        private string _mer_ord_id = "0";
        private decimal _user_poundage = 0.00000000M;
        private decimal _mer_poundage = 0.00000000M;
        /// <summary>
        /// 商户订单号
        /// </summary>
        public string mer_ord_id
        {
            set { _mer_ord_id = value; }
            get { return _mer_ord_id; }
        }
        /// <summary>
        /// 用户手续费
        /// </summary>
        public decimal user_poundage
        {
            set { _user_poundage = value; }
            get { return _user_poundage; }
        }
        /// <summary>
        /// 商户手续费
        /// </summary>
        public decimal mer_poundage
        {
            set { _mer_poundage = value; }
            get { return _mer_poundage; }
        }

        #endregion Model

    }
}
