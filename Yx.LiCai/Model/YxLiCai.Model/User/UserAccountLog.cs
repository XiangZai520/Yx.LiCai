using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Model.User
{
    /// <summary>
    /// 用户账户日志
    /// </summary>
    public class UserAccountLog
    {
        public UserAccountLog()
		{}
		#region Model
		private int _id;
		private DateTime _c_time;
		private int? _creator_id;
		private DateTime? _m_time;
		private int _user_id;
		private int _type;
		private decimal _before_amount;
		private decimal _after_amount;
		private decimal _change_amount;
		private int _account_source_id;
		private int? _from_id;
		private int? _pjt_id;
		private int? _ord_id;
		private int? _rfd_id;
		private int? _version;
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
		public int? creator_id
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
		public int user_id
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
		/// <summary>
		/// 
		/// </summary>
		public int? pjt_id
		{
			set{ _pjt_id=value;}
			get{return _pjt_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? ord_id
		{
			set{ _ord_id=value;}
			get{return _ord_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? rfd_id
		{
			set{ _rfd_id=value;}
			get{return _rfd_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? version
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
