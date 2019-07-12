using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YxLiCai.DataHelper;
using YxLiCai.Model.User;

namespace YxLiCai.Dao.User
{
    /// <summary>
    /// 用户消息数据操作类
    /// </summary>
    public class UserMessageDao
    { 
        /// <summary>
        /// 读取消息内容 by平扬 2015年7月2日
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="start"></param>
        /// <param name="pagesize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<UserMessageModel> GetUserMessage(long user_id,int start,int pagesize,ref int count)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT id,title,content,sendtime,isread FROM user_message WHERE user_id=?user_id order by sendtime desc LIMIT ?startpage,?pagesize;");
            strSql.Append(" SELECT count(*) FROM user_message WHERE user_id=?user_id ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("?user_id", MySqlDbType.Int32,11),
                    new MySqlParameter("?startpage", MySqlDbType.Int32,11),
                    new MySqlParameter("?pagesize", MySqlDbType.Int32,11)};
            parameters[0].Value = user_id;
            parameters[1].Value = start;
            parameters[2].Value = pagesize;
            List<UserMessageModel> list = new List<UserMessageModel>();
            var ds = DataBaseOperator.YxLiCalUserWrite.ExecuteDataSet(strSql.ToString(), "读取消息内容,by平扬", parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    UserMessageModel model = new UserMessageModel();
                    if (row["id"] != null && row["id"].ToString() != "")
                    {
                        model.id = int.Parse(row["id"].ToString());
                    } 
                    if (row["title"] != null)
                    {
                        model.title = row["title"].ToString();
                    }
                    if (row["content"] != null)
                    {
                        model.content = row["content"].ToString();
                    }
                    if (row["sendtime"] != null && row["sendtime"].ToString() != "")
                    {
                        model.sendtime = DateTime.Parse(row["sendtime"].ToString());
                    }
                    if (row["isread"] != null && row["isread"].ToString() != "")
                    {
                        model.isread = int.Parse(row["isread"].ToString());
                    }
                    list.Add(model);
                }
            }

            if (ds.Tables[1].Rows.Count > 0)
            {
                count = Tools.Util.ParseHelper.ToInt(ds.Tables[1].Rows[0][0].ToString());
            }
            return list;
        }
        /// <summary>
        /// 未读消息数量
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public int GetUserMessageCount(long user_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select count(*) from  user_message WHERE user_id=?user_id and isread=0");
            MySqlParameter[] parameters = {
                    new MySqlParameter("?user_id", MySqlDbType.Int64,11)};
            parameters[0].Value = user_id;
            object obj = DataBaseOperator.YxLiCalUserWrite.ExecuteScalar(strSql.ToString(), "未读消息数量,by平扬", parameters);
            if (obj != null)
            {
                return Convert.ToInt32(obj.ToString());
            }
            return 0;
        }

      
        /// <summary>
        /// 更新消息状态
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool UpdateUserMessage(long user_id, int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" update user_message set isread=1 WHERE user_id=?user_id and id=?id");
            MySqlParameter[] parameters = {
                    new MySqlParameter("?user_id", MySqlDbType.Int64,11),
                    new MySqlParameter("?id", MySqlDbType.Int32,11)};
            parameters[0].Value = user_id;
            parameters[1].Value = id;
            List<UserMessageModel> list = new List<UserMessageModel>();
            return DataBaseOperator.YxLiCalUserWrite.ExecuteNonQuery(strSql.ToString(), "更新消息状态,by平扬", parameters); 
        }

        /// <summary>
        /// 插入用户消息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddUserMessage(UserMessageModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into user_message(");
            strSql.Append("user_id,title,content,sendtime,isread,cate_id,cate_name)");
            strSql.Append(" values (");
            strSql.Append("?user_id,?title,?content,?sendtime,?isread,?cate_id,?cate_name)");
            MySqlParameter[] parameters = {
					new MySqlParameter("?user_id", MySqlDbType.Int64,11),
					new MySqlParameter("?title", MySqlDbType.VarChar,150),
					new MySqlParameter("?content", MySqlDbType.VarChar,500),
					new MySqlParameter("?sendtime", MySqlDbType.DateTime),
					new MySqlParameter("?isread", MySqlDbType.Int16,3),
					new MySqlParameter("?cate_id", MySqlDbType.Int16,4),
					new MySqlParameter("?cate_name", MySqlDbType.VarChar,30)};
            parameters[0].Value = model.user_id;
            parameters[1].Value = model.title;
            parameters[2].Value = model.content;
            parameters[3].Value = model.sendtime;
            parameters[4].Value = model.isread;
            parameters[5].Value = model.cate_id;
            parameters[6].Value = model.cate_name;
            return DataBaseOperator.YxLiCalUserWrite.ExecuteNonQuery(strSql.ToString(), "插入用户消息,by平扬", parameters);
        }
    }
}
