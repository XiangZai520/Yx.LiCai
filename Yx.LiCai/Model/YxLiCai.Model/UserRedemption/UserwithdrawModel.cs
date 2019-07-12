using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Model.UserRedemption
{
    /// <summary>
    /// 取现申请 实体类
    /// </summary>
    public class WithdrawModel
    {
        #region Model
        private long _id;
        private DateTime _create_time;
        private long _user_id;
        private decimal _amount;
        private decimal? _amount_balance;
        private decimal? _amount_principal;
        private int _status;
        private int _sys_status = 0;
        private int? _auditor_id;
        private DateTime? _audit_time;
        private long? _records_summary_id;
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
        public long user_id
        {
            set { _user_id = value; }
            get { return _user_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal amount
        {
            set { _amount = value; }
            get { return _amount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? amount_balance
        {
            set { _amount_balance = value; }
            get { return _amount_balance; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? amount_principal
        {
            set { _amount_principal = value; }
            get { return _amount_principal; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string sys_status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? auditor_id
        {
            set { _auditor_id = value; }
            get { return _auditor_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? audit_time
        {
            set { _audit_time = value; }
            get { return _audit_time; }
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
        public string remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        #endregion Model
    }
    /// <summary>
    /// 用户提现实体类
    /// </summary>
    public class UserWithdrawModel : WithdrawModel
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        public YxLiCai.Model.User.UserInfoModel UserInfo { get; set; }
        /// <summary>
        /// 银行卡信息
        /// </summary>
        public YxLiCai.Model.User.UserBankCardModel BankCardInfo { get; set; }
    }
    /// <summary>
    /// 用户提现扩展实体类
    /// </summary>
    public class WithdrawModelExtend
    {
        #region Model
        private int _id;
        private int _user_id;
        private string _l_name;
        private string _r_name;
        private string _u_phone;
        private string _bk_name;
        private string _bk_card;
        private DateTime _c_time;
        private decimal _amount;
        private decimal _amount_balance;
        private decimal _amount_principal;
        private int _status;
        private int _op_status = 0;
        private int? _auditor_id;
        private DateTime? _audit_time;
        private int? _rec_summary_id;
        private int? _rfd_balance_id;
        private int? _rfd_principal_id;
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
        /// 
        /// </summary>
        public int user_id
        {
            set { _user_id = value; }
            get { return _user_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string l_name
        {
            set { _l_name = value; }
            get { return _l_name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string r_name
        {
            set { _r_name = value; }
            get { return _r_name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string u_phone
        {
            set { _u_phone = value; }
            get { return _u_phone; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string bk_name
        {
            set { _bk_name = value; }
            get { return _bk_name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string bk_card
        {
            set { _bk_card = value; }
            get { return _bk_card; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime c_time
        {
            set { _c_time = value; }
            get { return _c_time; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal amount
        {
            set { _amount = value; }
            get { return _amount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal amount_balance
        {
            set { _amount_balance = value; }
            get { return _amount_balance; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal amount_principal
        {
            set { _amount_principal = value; }
            get { return _amount_principal; }
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
        public int? auditor_id
        {
            set { _auditor_id = value; }
            get { return _auditor_id; }
        }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime audit_time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? rec_summary_id
        {
            set { _rec_summary_id = value; }
            get { return _rec_summary_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? rfd_balance_id
        {
            set { _rfd_balance_id = value; }
            get { return _rfd_balance_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? rfd_principal_id
        {
            set { _rfd_principal_id = value; }
            get { return _rfd_principal_id; }
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
        /// <summary>
        /// 余额
        /// </summary>
        public decimal balance { get; set; }
        /// <summary>
        /// 本金
        /// </summary>
        public decimal principal { get; set; }
        /// <summary>
        /// 利息
        /// </summary>
        public decimal interest { get; set; }
        /// <summary>
        /// 违约金
        /// </summary>
        public decimal penalsum { get; set; }
        /// <summary>
        /// 实付金额
        /// </summary>
        public decimal amount_pay { get; set; }
        /// <summary>
        /// 用户操作：提现/赎回
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 订单id
        /// </summary>
        public string ord_id { get; set; }
    }
    /// <summary>
    /// 融资方提现记录实体类
    /// </summary>
    public class MerchantWithdrawModel
    {
        public int ID { get; set; }
        /// <summary>
        /// 融资方账户id
        /// </summary>
        public int AccountID { get; set; }
        public string LoginName { get; set; }
        public string RealName { get; set; }
        public string Bank { get; set; }
        public string BankCard { get; set; }
        public string Phone { get; set; }
        public decimal Amount { get; set; }
        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime ApplyTime { get; set; }
        public int AuditorID { get; set; }
        /// <summary>
        /// 审核人姓名
        /// </summary>
        public string AuditorName { get; set; }
        /// <summary>
        /// 放款时间
        /// </summary>
        public DateTime AuditTime { get; set; }
        /// <summary>
        /// 放款人id
        /// </summary>
        public int OperatorID { get; set; }
        /// <summary>
        /// 放款人姓名
        /// </summary>
        public string OperatorName { get; set; }
        /// <summary>
        /// 放款时间
        /// </summary>
        public DateTime PayTime { get; set; }
        /// <summary>
        /// 提现状态（0待审核,1待放款,2审核不通过,3提现成功,4提现失败）
        /// </summary>
        public int Status { get; set; }
        public string remark { get; set; }
    }
}
