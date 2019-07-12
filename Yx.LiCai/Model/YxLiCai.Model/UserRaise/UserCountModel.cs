using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Model.UserRaise
{ 
    /// <summary>
	/// UserCount:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class UserCountModel
	{
		public UserCountModel()
		{}
		#region Model
		private int _id;
		private long _userid=0;
		private decimal _mymoney=0.00M;
		private decimal _raisemoney=0.00M;
		private decimal _yesterdayinterest=0.00M;
		private decimal _allinterest=0.00M;
		private decimal _lockmoney=0.00M;
		private decimal _myblance=0.00M;
		private decimal _haveinterest=0.00M;
		private decimal _notinterest=0.00M;
        private decimal _principal = 0.00m;
        private decimal _principalfz = 0.00m;
        private decimal _t_interest = 0.00m;
		/// <summary>
		/// auto_increment
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 用户id
		/// </summary>
		public Int64 UserId
		{
			set{ _userid=value;}
			get{return _userid;}
		}
		/// <summary>
		/// 总资产
		/// </summary>
		public decimal MyMoney
		{
			set{ _mymoney=value;}
			get{return _mymoney;}
		}
		/// <summary>
		/// 总投资本金
		/// </summary>
		public decimal RaiseMoney
		{
			set{ _raisemoney=value;}
			get{return _raisemoney;}
		}
		/// <summary>
		/// 昨日收益
		/// </summary>
		public decimal YesterdayInterest
		{
			set{ _yesterdayinterest=value;}
			get{return _yesterdayinterest;}
		}
		/// <summary>
		/// 累计收益
		/// </summary>
		public decimal AllInterest
		{
			set{ _allinterest=value;}
			get{return _allinterest;}
		}
		/// <summary>
		/// 冻结金额
		/// </summary>
		public decimal  LockMoney
		{
			set{ _lockmoney=value;}
			get{return _lockmoney;}
		}
		/// <summary>
		/// 我的余额
		/// </summary>
		public decimal MyBlance
		{
			set{ _myblance=value;}
			get{return _myblance;}
		}
		/// <summary>
		/// 已收利息总和
		/// </summary>
		public decimal HaveInterest
		{
			set{ _haveinterest=value;}
			get{return _haveinterest;}
		}
		/// <summary>
		/// 待收利息总和
		/// </summary>
		public decimal NotInterest
		{
			set{ _notinterest=value;}
			get{return _notinterest;}
		}
        /// <summary>
        /// 本金总额
        /// </summary>
        public decimal principal
        {
            set { _principal = value; }
            get { return _principal; }
        }
        /// <summary>
        /// 本金冻结总额
        /// </summary>
        public decimal principal_fz
        {
            set { _principalfz = value; }
            get { return _principalfz; }
        }
         /// <summary>
        /// 特享金利息
        /// </summary>
        public decimal Tinterest
        {
            set { _t_interest = value; }
            get { return _t_interest; }
        }
        
		#endregion Model

	}
}
