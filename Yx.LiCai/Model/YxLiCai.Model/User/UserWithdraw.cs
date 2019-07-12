using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Model.User
{
    public class UserWithdraw
    {
        public UserWithdraw()
		{}
		#region Model
		private int _id;
		private long _user_id;
		private string _l_name;
		private string _r_name;
		private string _u_phone;
		private string _bk_name;
		private string _bk_card;
		private DateTime _c_time;
		private decimal _amount;
		private decimal _amount_balance;
		private decimal _amount_principal;
		private int _status;
		private int _op_status=0;
		private int? _auditor_id;
		private DateTime? _audit_time;
		private int? _rec_summary_id;
		private int? _rfd_balance_id;
		private int? _rfd_principal_id;
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
		public long user_id
		{
			set{ _user_id=value;}
			get{return _user_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string l_name
		{
			set{ _l_name=value;}
			get{return _l_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string r_name
		{
			set{ _r_name=value;}
			get{return _r_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string u_phone
		{
			set{ _u_phone=value;}
			get{return _u_phone;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string bk_name
		{
			set{ _bk_name=value;}
			get{return _bk_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string bk_card
		{
			set{ _bk_card=value;}
			get{return _bk_card;}
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
		public decimal amount
		{
			set{ _amount=value;}
			get{return _amount;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal amount_balance
		{
			set{ _amount_balance=value;}
			get{return _amount_balance;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal amount_principal
		{
			set{ _amount_principal=value;}
			get{return _amount_principal;}
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
		public int op_status
		{
			set{ _op_status=value;}
			get{return _op_status;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? auditor_id
		{
			set{ _auditor_id=value;}
			get{return _auditor_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? audit_time
		{
			set{ _audit_time=value;}
			get{return _audit_time;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? rec_summary_id
		{
			set{ _rec_summary_id=value;}
			get{return _rec_summary_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? rfd_balance_id
		{
			set{ _rfd_balance_id=value;}
			get{return _rfd_balance_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? rfd_principal_id
		{
			set{ _rfd_principal_id=value;}
			get{return _rfd_principal_id;}
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
