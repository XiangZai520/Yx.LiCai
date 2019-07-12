using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Model.InterestBonus
{
    /// <summary>
    /// 加息券表
    /// </summary>
    public class InterestBonusModel
    {

    }
    /// <summary>
    /// 加息券类型 Model
    /// </summary>
    public class InterestBonusCategoryModel
    {
        public InterestBonusCategoryModel()
		{}
		#region Model
		private int _id;
		private string _categoryname;
		private decimal? _amount=0.0000M;
		private int? _activeduration=0;
		private int? _creatorid=-1;
		private DateTime? _createdate;
		private string _remark;
		private int? _status=0;
		/// <summary>
		/// auto_increment
		/// </summary>
		public int Id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CategoryName
		{
			set{ _categoryname=value;}
			get{return _categoryname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? Amount
		{
			set{ _amount=value;}
			get{return _amount;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? ActiveDuration
		{
			set{ _activeduration=value;}
			get{return _activeduration;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? CreatorID
		{
			set{ _creatorid=value;}
			get{return _creatorid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? CreateDate
		{
			set{ _createdate=value;}
			get{return _createdate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Remark
		{
			set{ _remark=value;}
			get{return _remark;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Status
		{
			set{ _status=value;}
			get{return _status;}
		}
		#endregion Model
    }
    /// <summary>
    /// 加息券记录表
    /// </summary>
    public partial class InterestBonus
    {
        public InterestBonus()
        { }
        #region Model
        private long _id;
        private string _serialnum;
        private string _encryptednum;
        private DateTime? _generateddate;
        private int? _status = 0;
        /// <summary>
        /// auto_increment
        /// </summary>
        public long Id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SerialNum
        {
            set { _serialnum = value; }
            get { return _serialnum; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string EncryptedNum
        {
            set { _encryptednum = value; }
            get { return _encryptednum; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? GeneratedDate
        {
            set { _generateddate = value; }
            get { return _generateddate; }
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
    /// <summary>
    /// 用户与加息券关联表
    /// </summary>
    public partial class User_InterestBonus
    {
        public User_InterestBonus()
        { }
        #region Model
        private long _id;
        private int _userid;
        private string _serialnum;
        private decimal? _amount;
        private int? _activeduration = 0;
        private DateTime? _receiveddate;
        private DateTime? _useddate;
        private DateTime? _expireddate;
        private int? _status = 0;
        /// <summary>
        /// auto_increment
        /// </summary>
        public long Id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int UserID
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SerialNum
        {
            set { _serialnum = value; }
            get { return _serialnum; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? Amount
        {
            set { _amount = value; }
            get { return _amount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ActiveDuration
        {
            set { _activeduration = value; }
            get { return _activeduration; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? ReceivedDate
        {
            set { _receiveddate = value; }
            get { return _receiveddate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? UsedDate
        {
            set { _useddate = value; }
            get { return _useddate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? ExpiredDate
        {
            set { _expireddate = value; }
            get { return _expireddate; }
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
