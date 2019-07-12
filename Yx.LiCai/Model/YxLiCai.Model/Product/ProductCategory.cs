using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Model.Product
{
    public partial class ProductCategory
    {
        public ProductCategory()
        { }
        #region Model
        private int _id;
        private string _categoryname;
        private decimal? _yiedbase;
        private decimal? _yiedincrease;
        private decimal _yieldtop;
        private decimal? _withdrawfee = 0.0000M;
        private decimal? _alertset;
        private DateTime? _createdate;
        private DateTime? _auditdate;
        private int? _creatorid;
        private int? _auditorid;
        private int? _status = 0;
        /// <summary>
        /// 
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CategoryName
        {
            set { _categoryname = value; }
            get { return _categoryname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? YiedBase
        {
            set { _yiedbase = value; }
            get { return _yiedbase; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? YiedIncrease
        {
            set { _yiedincrease = value; }
            get { return _yiedincrease; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal YieldTop
        {
            set { _yieldtop = value; }
            get { return _yieldtop; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? WithdrawFee
        {
            set { _withdrawfee = value; }
            get { return _withdrawfee; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? AlertSet
        {
            set { _alertset = value; }
            get { return _alertset; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreateDate
        {
            set { _createdate = value; }
            get { return _createdate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? AuditDate
        {
            set { _auditdate = value; }
            get { return _auditdate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? CreatorID
        {
            set { _creatorid = value; }
            get { return _creatorid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? AuditorID
        {
            set { _auditorid = value; }
            get { return _auditorid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Status
        {
            set { _status = value; }
            get { return _status; }
        }
        #endregion Model

    }
}
