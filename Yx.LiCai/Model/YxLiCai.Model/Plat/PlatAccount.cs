using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Model.Plat
{
    public class PlatAccount
    {
        public PlatAccount()
		{}
		#region Model
		private int _id;
		private DateTime _c_time;
		private int _type;
		private string _name;
		private decimal _amount;
		private decimal _amount_recharge=0.00000000M;
		private decimal _amount_paid=0.00000000M;
		private decimal? _base_amount;
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
		public int type
		{
			set{ _type=value;}
			get{return _type;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string name
		{
			set{ _name=value;}
			get{return _name;}
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
		public decimal amount_recharge
		{
			set{ _amount_recharge=value;}
			get{return _amount_recharge;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal amount_paid
		{
			set{ _amount_paid=value;}
			get{return _amount_paid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? base_amount
		{
			set{ _base_amount=value;}
			get{return _base_amount;}
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
