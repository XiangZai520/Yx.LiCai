using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Model.User
{
    public class UserAccountLogModel
    {
        public UserAccountLogModel()
		{}
		#region Model
		private int _id;
		private DateTime _c_time= DateTime.Now;
		private int _creator_id=0;
		private DateTime? _m_time= DateTime.Now;
        private long _user_id = 0;
		private int _type=0;
		private decimal _before_amount=0.00000000M;
		private decimal _after_amount=0.00000000M;
		private decimal _change_amount=0.00000000M;
		private int _account_source_id=0;
		private int? _from_id=0;
		//private int? _pjt_id=0;
		private int? _user_ord_id=0;
		private int? _user_rfd_id=0;
		private int _version=0;
		private string _remark;
		/// <summary>
		/// auto_increment
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime c_time
		{
			set{ _c_time=value;}
			get{return _c_time;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int creator_id
		{
			set{ _creator_id=value;}
			get{return _creator_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? m_time
		{
			set{ _m_time=value;}
			get{return _m_time;}
		}
		/// <summary>
		/// 
		/// </summary>
		public long user_id
		{
			set{ _user_id=value;}
			get{return _user_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int type
		{
			set{ _type=value;}
			get{return _type;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal before_amount
		{
			set{ _before_amount=value;}
			get{return _before_amount;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal after_amount
		{
			set{ _after_amount=value;}
			get{return _after_amount;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal change_amount
		{
			set{ _change_amount=value;}
			get{return _change_amount;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int account_source_id
		{
			set{ _account_source_id=value;}
			get{return _account_source_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? from_id
		{
			set{ _from_id=value;}
			get{return _from_id;}
		}
        ///// <summary>
        ///// 
        ///// </summary>
        //public int? pjt_id
        //{
        //    set{ _pjt_id=value;}
        //    get{return _pjt_id;}
        //}
		/// <summary>
		/// 
		/// </summary>
		public int? user_ord_id
		{
			set{ _user_ord_id=value;}
			get{return _user_ord_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? user_rfd_id
		{
			set{ _user_rfd_id=value;}
			get{return _user_rfd_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int version
		{
			set{ _version=value;}
			get{return _version;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string remark
		{
			set{ _remark=value;}
			get{return _remark;}
		}
		#endregion Model 
    }
}
