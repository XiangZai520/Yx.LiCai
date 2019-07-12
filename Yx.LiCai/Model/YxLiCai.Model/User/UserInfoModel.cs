
/*
 * 用户账户信息实体类
 * 作者：侯裕祥
 * 时间：2015年6月16日 09:53:53
 * 版本：1.0.0.0
 * 
 */
using System;

namespace YxLiCai.Model.User
{
    /// <summary>
    /// UserInfo:实体类(用户账户信息实体类)
    /// </summary>
    [Serializable]
    public partial class UserInfoModel
    {
        public UserInfoModel()
        { }
        #region Model
        private long _id;
        private string _loginname = "";
        private string _phone = "0";
        private string _password = "";
        private string _sallpassword = "";
        private string _myrealname = "";
        private string _myrealcard = "";
        private int? _isrealcard = 0;
        private string _mycode = "";
        private DateTime? _regtime;
        private int? _status = 0;
        private int _IsBindBank = 0;
        private string _bankname = "";
        private string _lastbanknum = "";
        private string _ip = "";
        private string _no_agree = "";
        private string _bankCode = "";

        /// <summary>
        /// 用户id（主键）
        /// </summary>
        public long id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 登录名
        /// </summary>
        public string LoginName
        {
            set { _loginname = value; }
            get { return _loginname; }
        }
        /// <summary>
        /// 手机
        /// </summary>
        public string Phone
        {
            set { _phone = value; }
            get { return _phone; }
        }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password
        {
            set { _password = value; }
            get { return _password; }
        }
        /// <summary>
        /// 支付密码
        /// </summary>
        public string Sallpassword
        {
            set { _sallpassword = value; }
            get { return _sallpassword; }
        }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string MyRealName
        {
            set { _myrealname = value; }
            get { return _myrealname; }
        }
        /// <summary>
        /// 我的身份证
        /// </summary>
        public string MyRealCard
        {
            set { _myrealcard = value; }
            get { return _myrealcard; }
        }
        /// <summary>
        /// 身份证是否验证是否实名认证(0未1认证)
        /// </summary>
        public int? IsRealCard
        {
            set { _isrealcard = value; }
            get { return _isrealcard; }
        }
        /// <summary>
        /// 我的邀请码
        /// </summary>
        public string MyCode
        {
            set { _mycode = value; }
            get { return _mycode; }
        }
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime? RegTime
        {
            set { _regtime = value; }
            get { return _regtime; }
        }
        /// <summary>
        /// 账户状态(1可用 0 冻结)
        /// </summary>
        public int? Status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 银行卡是否绑定
        /// </summary>
        public int IsBindBank
        {
            set { _IsBindBank = value; }
            get { return _IsBindBank; }
        }

        /// <summary>
        /// 银行卡code
        /// </summary>
        public string BankCode 
        {
            set { _bankCode = value; }
            get { return _bankCode; }
        }

        /// <summary>
        /// 银行卡名称
        /// </summary>
        public string BankName
        {
            set { _bankname = value; }
            get { return _bankname; }
        }

        /// <summary>
        /// 银行卡尾号
        /// </summary>
        public string LastBankNum 
        {
            set { _lastbanknum = value; }
            get { return _lastbanknum; }
        }
        /// <summary>
        /// 用户IP
        /// </summary>
        public string IP
        {
            set { _ip = value; }
            get { return _ip; }
        }
        /// <summary>
        /// 签约协议号
        /// </summary>
        public string no_agree
        {
            set { _no_agree = value; }
            get { return _no_agree; }
        }
        #endregion Model

    }
}
