/*
 * 银行卡实体类
 * 作者：侯裕祥
 * 时间：2015年6月16日 09:53:53
 * 版本：1.0.0.0
 * 
 */
using System;

namespace YxLiCai.Model.User
{
    /// <summary>
    /// 银行卡实体类
    /// </summary>
    [Serializable]
    public class UserBankCardModel
    {
        public UserBankCardModel()
        { }
        #region Model
        private int _id;
        private long _userid = 0;
        private int _bank = 0;
        private string _bankname = "";
        private string _bankcard = "0";
        private string _bankregion = "";
        private string _bankaddress = "";
        private int? _status = 0;

        private string _bankcode;
        private string _firstnum;
        private string _lastnum;
        private string _bankphone;
        private string _requestid;
        private string _no_agree;
        /// <summary>
        /// 主键
        /// </summary>
        public int Id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 银行ID
        /// </summary>
        public int Bank
        {
            set { _bank = value; }
            get { return _bank; }
        }
        /// <summary>
        /// 银行名称
        /// </summary>
        public string BankName
        {
            set { _bankname = value; }
            get { return _bankname; }
        }
        /// <summary>
        /// 银行卡号
        /// </summary>
        public string BankCard
        {
            set { _bankcard = value; }
            get { return _bankcard; }
        }
        /// <summary>
        /// 支行
        /// </summary>
        public string BankRegion
        {
            set { _bankregion = value; }
            get { return _bankregion; }
        }
        /// <summary>
        /// 开户地址
        /// </summary>
        public string BankAddress
        {
            set { _bankaddress = value; }
            get { return _bankaddress; }
        }
        /// <summary>
        /// 状态
        /// </summary>
        public int? Status
        {
            set { _status = value; }
            get { return _status; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string BankCode
        {
            set { _bankcode = value; }
            get { return _bankcode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FirstNum
        {
            set { _firstnum = value; }
            get { return _firstnum; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string LastNum
        {
            set { _lastnum = value; }
            get { return _lastnum; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BankPhone
        {
            set { _bankphone = value; }
            get { return _bankphone; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Requestid
        {
            set { _requestid = value; }
            get { return _requestid; }
        }

        public string no_agree
        {
            set { _no_agree = value; }
            get { return _no_agree; }
        }

        #endregion Model
    }
}
