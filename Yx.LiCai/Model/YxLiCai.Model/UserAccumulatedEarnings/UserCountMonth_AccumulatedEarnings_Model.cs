/*
 * 月账户累计收益实体类
 * 作者：侯裕祥
 * 时间：2015年6月16日 09:53:53
 * 版本：1.0.0.0
 * 
 */
using System;

namespace YxLiCai.Model.UserAccumulatedEarnings
{
    /// <summary>
    /// 用户月账户累计收益的实体Model类
    /// </summary>
    [Serializable]
    public class UserCountMonth_AccumulatedEarnings_Model
    {
        public UserCountMonth_AccumulatedEarnings_Model()
		{}
		#region Model
		private int _id;
		private long _user_id;
		private int _type;
		private decimal _earningsamount=0.00000000M;
		private DateTime _create_time;
		/// <summary>
		/// auto_increment
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
        ///类型 （0自有本金1特享金）
		/// </summary>
		public int type
		{
			set{ _type=value;}
			get{return _type;}
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
