using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Model.ActivityManage
{
    /// <summary>
    /// 活动子表
    /// </summary>
    public class ActPromotionItem
    {
        public ActPromotionItem()
		{}
		#region Model
		private int _id;
		private int _act_id=0;
		private int _item_type=0;
		private int _item_id=0;
        private string _item_name = "";
		private int _item_qty=1;
		private DateTime _c_time=DateTime.Now;
		private int _creator_id=0;
		private DateTime? _m_time;
		private int _version=0;
		private string _remark;
        private decimal _amount;
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
		public int act_id
		{
			set{ _act_id=value;}
			get{return _act_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int item_type
		{
			set{ _item_type=value;}
			get{return _item_type;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int item_id
		{
			set{ _item_id=value;}
			get{return _item_id;}
		}
        /// <summary>
        /// 
        /// </summary>
        public string item_name
        {
            set { _item_name = value; }
            get { return _item_name; }
        }
		/// <summary>
		/// 
		/// </summary>
		public int item_qty
		{
			set{ _item_qty=value;}
			get{return _item_qty;}
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

        public decimal Amount
        {
            set { _amount = value; }
            get { return _amount; }
        }

        #endregion Model
    }
}
