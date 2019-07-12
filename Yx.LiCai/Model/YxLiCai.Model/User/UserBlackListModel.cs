using System;
namespace Maticsoft.Model
{
    /// <summary>
    /// user_blacklist:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class UserBlackListModel
    {
        public UserBlackListModel()
        { }
        #region Model
        private int _id;
        private long _user_id;
        private string _login_name;
        private string _my_realname;
        private string _phone;
        private DateTime _create_time =DateTime.Now;
        private int _oprate_man_id;
        private string _oprate_man_name;
        private string _remark;
        private int _is_delete;
        private DateTime _update_time = DateTime.Now;
        private int _update_man_id;
        /// <summary>
        /// auto_increment
        /// </summary>
        public int id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public long user_id
        {
            set { _user_id = value; }
            get { return _user_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string login_name
        {
            set { _login_name = value; }
            get { return _login_name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string my_realname
        {
            set { _my_realname = value; }
            get { return _my_realname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string phone
        {
            set { _phone = value; }
            get { return _phone; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime create_time
        {
            set { _create_time = value; }
            get { return _create_time; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int oprate_man_id
        {
            set { _oprate_man_id = value; }
            get { return _oprate_man_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string oprate_man_name
        {
            set { _oprate_man_name = value; }
            get { return _oprate_man_name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        /// <summary>
        /// auto_increment
        /// </summary>
        public int is_delete
        {
            set { _is_delete = value; }
            get { return _is_delete; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime update_time
        {
            set { _update_time = value; }
            get { return _update_time; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int update_man_id
        {
            set { _update_man_id = value; }
            get { return _update_man_id; }
        }
        #endregion Model

    }
}

