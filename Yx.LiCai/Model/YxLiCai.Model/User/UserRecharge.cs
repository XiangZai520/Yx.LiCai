using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Model.User
{
    public class UserRecharge
    {
        public UserRecharge()
		{}
		#region Model
		private int _id;
		private DateTime _c_time;
		private long _user_id;
		private int _type;
		private decimal _amount=0.00000000M;
		private int _status=0;
		private string _mer_ord_id;
		private int _version=0;
		private string _remark;
		private DateTime? _pay_time;
		private string _third_ord_id;
		private decimal _user_poundage=0.00000000M;
		private decimal _mer_poundage=0.00000000M;
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
		public decimal amount
		{
			set{ _amount=value;}
			get{return _amount;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int status
		{
			set{ _status=value;}
			get{return _status;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string mer_ord_id
		{
			set{ _mer_ord_id=value;}
			get{return _mer_ord_id;}
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
		/// <summary>
		/// 
		/// </summary>
		public DateTime? pay_time
		{
			set{ _pay_time=value;}
			get{return _pay_time;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string third_ord_id
		{
			set{ _third_ord_id=value;}
			get{return _third_ord_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal user_poundage
		{
			set{ _user_poundage=value;}
			get{return _user_poundage;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal mer_poundage
		{
			set{ _mer_poundage=value;}
			get{return _mer_poundage;}
		}
		#endregion Model
    }
}
