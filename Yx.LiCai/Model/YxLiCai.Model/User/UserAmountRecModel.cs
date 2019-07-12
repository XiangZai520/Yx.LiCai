using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Model.User
{
    /// <summary>
    /// 用户金额记录表
    /// </summary>
    public class UserAmountRecModel
    {
        public UserAmountRecModel()
		{}
		#region Model
		private int _id;
		private long _user_id=0;
		private int _type=0;
        private int _prodtype = 0;
		private decimal _amount=0.00M;
		private DateTime _c_time;
		private int _creator_id=0;
		private int _version=0;
		private string _remark= "0";
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
        /// 0充值1提现2赎回3充值失败
		/// </summary>
		public int type
		{
			set{ _type=value;}
			get{return _type;}
		}
		/// <summary>
		/// 金额
		/// </summary>
		public decimal amount
		{
			set{ _amount=value;}
			get{return _amount;}
		}
		/// <summary>
		/// 时间
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
        /// 1月月2三个月 3六个月4年
        /// </summary>
        public int Prodtype
        {
            set { _prodtype = value; }
            get { return _prodtype; }
        }
		#endregion Model
    }
}
