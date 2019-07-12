using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Model.UserFinancingManagement
{
    /// <summary>
    /// 项目还款记录
    /// </summary>
    public class Pjt_repayment_Model
    {
        public Pjt_repayment_Model()
        { }
        #region Model
        private int _id;
        private DateTime _c_time;
        private int _op_status = 0;
        private int _pjt_id;
        private decimal _rep_amount;
        private int _version = 0;
        private string _remark;
        private int _creator_id;
        private DateTime _m_time;
        private int _pjt_type = 0;
        /// <summary>
        /// auto_increment
        /// </summary>
        public int id
        {
            set { _id = value; }
            get { return _id; }
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
        /// 处理状态(0:未处理1:已处理2:异常)
        /// </summary>
        public int op_status
        {
            set { _op_status = value; }
            get { return _op_status; }
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
        /// 偿还金额（本金或者利息按照偿还类型走）
        /// </summary>
        public decimal rep_amount
        {
            set { _rep_amount = value; }
            get { return _rep_amount; }
        }
        /// <summary>
        /// 乐观锁
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
        /// 创建 人ID
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
        /// 偿还类型(0利息偿还、1本金偿还)
        /// </summary>
        public int pjt_type
        {
            set { _pjt_type = value; }
            get { return _pjt_type; }
        }
        /// <summary>
        /// 项目ID
        /// </summary>
        public int PID { get; set; }
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
