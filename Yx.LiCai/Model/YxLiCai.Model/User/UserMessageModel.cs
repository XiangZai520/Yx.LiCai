using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Model.User
{
    /// <summary>
	/// user_message:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class UserMessageModel
	{
		public UserMessageModel()
		{}
		#region Model
		private int _id;
		private long _user_id=0;
		private string _title;
		private string _content;
		private DateTime _sendtime;
		private int _isread=0;
		private int? _cate_id=0;
		private string _cate_name;
		/// <summary>
		/// auto_increment
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 用户id
		/// </summary>
		public long user_id
		{
			set{ _user_id=value;}
			get{return _user_id;}
		}
		/// <summary>
		/// 消息标题
		/// </summary>
		public string title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// 消息内容
		/// </summary>
		public string content
		{
			set{ _content=value;}
			get{return _content;}
		}
		/// <summary>
		/// 发送时间
		/// </summary>
		public DateTime sendtime
		{
			set{ _sendtime=value;}
			get{return _sendtime;}
		}
		/// <summary>
		/// 是否已读
		/// </summary>
		public int isread
		{
			set{ _isread=value;}
			get{return _isread;}
		}
		/// <summary>
		/// 类型id
		/// </summary>
		public int? cate_id
		{
			set{ _cate_id=value;}
			get{return _cate_id;}
		}
		/// <summary>
		/// 类型
		/// </summary>
		public string cate_name
		{
			set{ _cate_name=value;}
			get{return _cate_name;}
		}
		#endregion Model

	}
}
