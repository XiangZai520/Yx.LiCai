using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Model.UserFinancingManagement
{
    /// <summary>
    /// 融资方本金偿还实体类
    /// </summary>
    public class Project_refund_principal
    {
        public Project_refund_principal()
        { }
        #region Model
        private int _id;
        private DateTime _c_time;
        private int _creator_id;
        private DateTime _m_time;
        private int _fer_id;
        private int _fer_account_id = -1;
        private int _pjt_id = 0;
        private decimal _amount;
        private int _status = 0;
        private int _op_status;
        private string _third_ord_id;
        private int _version;
        private string _remark;
        /// <summary>
        /// 
        /// </summary>
        public int id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime c_time
        {
            set { _c_time = value; }
            get { return _c_time; }
        }
        /// <summary>
        /// 创建人ID
        /// </summary>
        public int creator_id
        {
            set { _creator_id = value; }
            get { return _creator_id; }
        }
        /// <summary>
        /// 操作日期（修改时间 ）
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
        /// 项目id
        /// </summary>
        public int pjt_id
        {
            set { _pjt_id = value; }
            get { return _pjt_id; }
        }
        /// <summary>
        /// 还款金额
        /// </summary>
        public decimal amount
        {
            set { _amount = value; }
            get { return _amount; }
        }
        /// <summary>
        /// 状态：0待处理；1申请还款；2全部还款3部分还款4通知充值
        /// </summary>
        public int status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 处理状态(0未处理1已处理2处理失败)
        /// </summary>
        public int op_status
        {
            set { _op_status = value; }
            get { return _op_status; }
        }
        /// <summary>
        /// 第三方支付订单id
        /// </summary>
        public string third_ord_id
        {
            set { _third_ord_id = value; }
            get { return _third_ord_id; }
        }
        /// <summary>
        /// 版本
        /// </summary>
        public int version
        {
            set { _version = value; }
            get { return _version; }
        }
        /// <summary>
        ///  备注
        /// </summary>
        public string remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string Pname { get; set; }
        /// <summary>
        /// 项目金额
        /// </summary>
        public decimal Pmoney { get; set; }
        /// <summary>
        /// 还款期限
        /// </summary>
        public int LoanPeriod { get; set; }
        #endregion Model
    }
}
