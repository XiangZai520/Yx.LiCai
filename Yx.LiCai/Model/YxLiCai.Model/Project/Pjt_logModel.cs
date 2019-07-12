using System;

namespace YxLiCai.Model.Project
{
    /// <summary>
    /// 项目日志表
    /// </summary>
    public class Pjt_logModel
    {
        public Pjt_logModel()
        { }
        #region Model
        private int _id;
        private DateTime _c_time;
        private int _pjt_id;
        private decimal _amount_sold;
        private decimal _amount_able;
        private decimal _amount_able_fz;
        private int _version;
        private string _remark;
        private int _creator_id;
        private DateTime _m_time;
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
        /// 项目id
        /// </summary>
        public int pjt_id
        {
            set { _pjt_id = value; }
            get { return _pjt_id; }
        }
        /// <summary>
        /// 已售金额
        /// </summary>
        public decimal amount_sold
        {
            set { _amount_sold = value; }
            get { return _amount_sold; }
        }
        /// <summary>
        /// 可售金额
        /// </summary>
        public decimal amount_able
        {
            set { _amount_able = value; }
            get { return _amount_able; }
        }
        /// <summary>
        /// 冻结可售金额
        /// </summary>
        public decimal amount_able_fz
        {
            set { _amount_able_fz = value; }
            get { return _amount_able_fz; }
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
        /// <summary>
        /// 创建人ID
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
        #endregion Model

    }
}
