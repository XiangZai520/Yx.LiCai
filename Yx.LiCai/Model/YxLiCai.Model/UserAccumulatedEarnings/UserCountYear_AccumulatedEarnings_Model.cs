/*
 * 年账户累计收益实体类
 * 作者：侯裕祥
 * 时间：2015年6月16日 09:53:53
 * 版本：1.0.0.0
 * 
 */
using System;

namespace YxLiCai.Model.UserAccumulatedEarnings
{
    /// <summary>
    /// 年账户累计收益Model实体类
    /// </summary>
    [Serializable]
    public partial class UserCountYear_AccumulatedEarnings_Model
    {
        public UserCountYear_AccumulatedEarnings_Model()
		{}
		#region Model
		private int _id;
		private long _user_id;
		private decimal _earningsamount=0.00000000M;
		private DateTime _create_time;
		/// <summary>
		/// 主键
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 用户
		/// </summary>
		public long user_id
		{
			set{ _user_id=value;}
			get{return _user_id;}
		}
		/// <summary>
        /// 产生的收益金额（产生的利息）
		/// </summary>
		public decimal earningsamount
		{
			set{ _earningsamount=value;}
			get{return _earningsamount;}
		}
		/// <summary>
        /// 计息时间
		/// </summary>
		public DateTime create_time
		{
			set{ _create_time=value;}
			get{return _create_time;}
		}
		#endregion Model
    }
}
