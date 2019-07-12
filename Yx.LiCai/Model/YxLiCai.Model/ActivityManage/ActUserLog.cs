using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Model.ActivityManage
{
    public class ActUserLog
    {
        public ActUserLog()
		{}
		#region Model
		private int _id;
        private long _user_id;
        private int _act_id;
        private int _item_id;
		private DateTime _c_time=DateTime.Now;
		private int? _creator_id=0;
		private int? _version=0;
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
        /// 用户编号
        /// </summary>
        public long user_id
        {
            set { _user_id = value; }
            get { return _user_id; }
        }

        /// <summary>
        /// 活动编号
        /// </summary>
        public int act_id
        {
            set { _act_id = value; }
            get { return _act_id; }
        }

        /// <summary>
        /// 活动子项目编号
        /// </summary>
        public int item_id
        {
            set { _item_id = value; }
            get { return _item_id; }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
		public DateTime c_time
		{
			set{ _c_time=value;}
			get{return _c_time;}
		}
		/// <summary>
		/// 创建者编号
		/// </summary>
		public int? creator_id
		{
			set{ _creator_id=value;}
			get{return _creator_id;}
		}
		/// <summary>
		/// 版本号
		/// </summary>
		public int? version
		{
			set{ _version=value;}
			get{return _version;}
		}
		/// <summary>
		/// 备注
		/// </summary>
		public string remark
		{
			set{ _remark=value;}
			get{return _remark;}
		}
        
		#endregion Model
    }
}
