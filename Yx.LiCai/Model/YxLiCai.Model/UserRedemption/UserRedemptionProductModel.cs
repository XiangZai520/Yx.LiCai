using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Model.UserRedemption
{ 
    /// <summary>
    /// UserRedemptionProject:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class UserRedemptionProductModel
    {
        public UserRedemptionProductModel()
        { }
        #region Model
        private long _id;
        private DateTime _create_time;
        private long _user_id = 0;
        private long _order_id;
        private int _stauts = 0;
        private int _sys_status = 0;
        private decimal _amount;
        private int? _auditor_id;
        private DateTime? _audit_time;
        private string _remark;
        private int? _version = 0;

        public decimal atone_benjin { get; set; }
        public decimal atone_lixi { get; set; }
        public decimal atone_weiyurjin_rate { get; set; }
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
        public long order_id
        {
            set { _order_id = value; }
            get { return _order_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int stauts
        {
            set { _stauts = value; }
            get { return _stauts; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int sys_status
        {
            set { _sys_status = value; }
            get { return _sys_status; }
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
        public string remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? version
        {
            set { _version = value; }
            get { return _version; }
        }
        #endregion Model
    }
    public class RedemptionModel: UserRedemptionProductModel
    {
        /// <summary>
        /// 违约金
        /// </summary>
        public decimal PenalSum { get; set; }
        /// <summary>
        /// 用户信息
        /// </summary>
        public YxLiCai.Model.User.UserInfoModel UserInfo { get; set; }
        /// <summary>
        /// 银行卡信息
        /// </summary>
        public YxLiCai.Model.User.UserBankCardModel BankCardInfo { get; set; }
    }
}
