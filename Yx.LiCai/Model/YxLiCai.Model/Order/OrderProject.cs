using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Model.Order
{
    /// <summary>
    /// order_project:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class OrderProject
    {
        public OrderProject()
        { }
        #region Model
        private long _order_id;
        private int _project_id;
        private DateTime _create_time;
        private long _user_id;
        private decimal _amount;
        private decimal _interest_rate;
        private decimal _interest_added;
        private decimal _interest_paid;
        private DateTime? _expiration_time;
        private DateTime _project_begin_time;
        private DateTime _project_end_time;
        private int _version = 0;
        private string _remark;
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
        public int project_id
        {
            set { _project_id = value; }
            get { return _project_id; }
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
        public decimal interest_rate
        {
            set { _interest_rate = value; }
            get { return _interest_rate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal interest_added
        {
            set { _interest_added = value; }
            get { return _interest_added; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal interest_paid
        {
            set { _interest_paid = value; }
            get { return _interest_paid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? expiration_time
        {
            set { _expiration_time = value; }
            get { return _expiration_time; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime project_begin_time
        {
            set { _project_begin_time = value; }
            get { return _project_begin_time; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime project_end_time
        {
            set { _project_end_time = value; }
            get { return _project_end_time; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int version
        {
            set { _version = value; }
            get { return _version; }
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
}
