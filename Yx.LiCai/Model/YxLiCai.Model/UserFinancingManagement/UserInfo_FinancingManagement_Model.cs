/*
 * 融资方用户账户信息实体类
 * 作者：侯裕祥
 * 时间：2015年6月16日 09:53:53
 * 版本：1.0.0.0
 * 
 */
using System;

namespace YxLiCai.Model.UserFinancingManagement
{
    /// <summary>
    /// 融资用户管理实体类
    /// </summary>
    [Serializable]
    public partial class UserInfo_FinancingManagement_Model
    {
        public UserInfo_FinancingManagement_Model()
        { }
        #region Model
        private int _financier_id;
        private string _financier_name;
        private string _phone;
        private string _password;
        private string _company_address;
        private string _myreal_name;
        private string _myreal_card;
        private string _bank_name;
        private string _bank_card;
        private string _bank_code;
        private string _first_num;
        private string _last_num;
        private string _bank_phone;
        private int _isreal_card = 0;
        private int _isbank_card = 0;
        private string _requestid;
        private int _status = 1;
        private DateTime _reg_time;
        /// <summary>
        /// 账户ID
        /// </summary>
        public int financier_id
        {
            set { _financier_id = value; }
            get { return _financier_id; }
        }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string financier_name
        {
            set { _financier_name = value; }
            get { return _financier_name; }
        }
        /// <summary>
        /// 手机（唯一可做账号）
        /// </summary>
        public string phone
        {
            set { _phone = value; }
            get { return _phone; }
        }
        /// <summary>
        /// 支付密码
        /// </summary>
        public string spassword
        {
            set { _password = value; }
            get { return _password; }
        }
        /// <summary>
        /// 公司地址
        /// </summary>
        public string company_address
        {
            set { _company_address = value; }
            get { return _company_address; }
        }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string myreal_name
        {
            set { _myreal_name = value; }
            get { return _myreal_name; }
        }
        /// <summary>
        /// 身份证
        /// </summary>
        public string myreal_card
        {
            set { _myreal_card = value; }
            get { return _myreal_card; }
        }
        /// <summary>
        /// 银行名称
        /// </summary>
        public string bank_name
        {
            set { _bank_name = value; }
            get { return _bank_name; }
        }
        /// <summary>
        /// 银行卡号
        /// </summary>
        public string bank_card
        {
            set { _bank_card = value; }
            get { return _bank_card; }
        }
        /// <summary>
        /// 银行编码
        /// </summary>
        public string bank_code
        {
            set { _bank_code = value; }
            get { return _bank_code; }
        }
        /// <summary>
        /// 卡号前六位
        /// </summary>
        public string first_num
        {
            set { _first_num = value; }
            get { return _first_num; }
        }
        /// <summary>
        /// 卡号后4位
        /// </summary>
        public string last_num
        {
            set { _last_num = value; }
            get { return _last_num; }
        }
        /// <summary>
        /// 银行预留手机号
        /// </summary>
        public string bank_phone
        {
            set { _bank_phone = value; }
            get { return _bank_phone; }
        }
        /// <summary>
        /// 是否认证（0未认证1已认证）
        /// </summary>
        public int isreal_card
        {
            set { _isreal_card = value; }
            get { return _isreal_card; }
        }
        /// <summary>
        /// 是否绑定（0未绑定1已绑定）
        /// </summary>
        public int isbank_card
        {
            set { _isbank_card = value; }
            get { return _isbank_card; }
        }
        /// <summary>
        /// 唯一请求id
        /// </summary>
        public string requestid
        {
            set { _requestid = value; }
            get { return _requestid; }
        }
        /// <summary>
        /// 状态（1正常0冻结）
        /// </summary>
        public int status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime reg_time
        {
            set { _reg_time = value; }
            get { return _reg_time; }
        }
        /// <summary>
        /// 账户余额
        /// </summary>
        public decimal Money { get; set; }
        /// <summary>
        /// 登录名
        /// </summary>
        public string l_name { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        public string pwd { get; set; }
        #endregion Model


    }
}
