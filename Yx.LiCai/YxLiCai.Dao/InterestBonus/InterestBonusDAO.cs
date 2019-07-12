using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using YxLiCai.DataHelper;
using YxLiCai.Model.InterestBonus;

namespace YxLiCai.Dao.InterestBonus
{
    public class InterestBonusDAO
    {
        #region Insert
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(InterestBonusCategoryModel model)
        {
            var sqlArray = new List<string>();
            var paramArray = new List<DbParameter[]>();
            var strSql = new StringBuilder();

            strSql.Append("insert into bonus_category(");
            strSql.Append("CategoryName,Amount,ActiveDuration,CreatorID,CreateDate,Remark,Status)");
            strSql.Append(" values (");
            strSql.Append("@CategoryName,@Amount,@ActiveDuration,@CreatorID,@CreateDate,@Remark,@Status)");
            //sql array
            sqlArray.Add(strSql.ToString());

            MySqlParameter[] parameters = {
					new MySqlParameter("@CategoryName", MySqlDbType.VarChar,100),
					new MySqlParameter("@Amount", MySqlDbType.Decimal,10),
					new MySqlParameter("@ActiveDuration", MySqlDbType.Int32,11),
					new MySqlParameter("@CreatorID", MySqlDbType.Int32,11),
					new MySqlParameter("@CreateDate", MySqlDbType.DateTime),
					new MySqlParameter("@Remark", MySqlDbType.VarChar,500),
					new MySqlParameter("@Status", MySqlDbType.Int16,4)};
            parameters[0].Value = model.CategoryName;
            parameters[1].Value = model.Amount;
            parameters[2].Value = model.ActiveDuration;
            parameters[3].Value = model.CreatorID;
            parameters[4].Value = model.CreateDate;
            parameters[5].Value = model.Remark;
            parameters[6].Value = model.Status;
            //parameter Array
            paramArray.Add(parameters);

            return DataBaseOperator.MoneyWriteDbHelper.ExecutMultiSQLwithTransaction(sqlArray, paramArray);
        }
        public bool AddBonus(InterestBonusModel model)
        {

            return false;
        }
        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(InterestBonusCategoryModel model)
        {
            var sqlArray = new List<string>();
            var paramArray = new List<DbParameter[]>();
            var strSql = new StringBuilder();

            strSql.Append("update bonus_category set ");
            strSql.Append("CategoryName=@CategoryName,");
            strSql.Append("Amount=@Amount,");
            strSql.Append("ActiveDuration=@ActiveDuration,");
            strSql.Append("Remark=@Remark,");
            strSql.Append("Status=@Status");
            strSql.Append(" where Id=@Id");
            //sql array
            sqlArray.Add(strSql.ToString());

            MySqlParameter[] parameters = {
					new MySqlParameter("@CategoryName", MySqlDbType.VarChar,100),
					new MySqlParameter("@Amount", MySqlDbType.Decimal,10),
					new MySqlParameter("@ActiveDuration", MySqlDbType.Int32,11),	
					new MySqlParameter("@Remark", MySqlDbType.VarChar,500),
					new MySqlParameter("@Status", MySqlDbType.Int16,4),
					new MySqlParameter("@Id", MySqlDbType.Int32,11)};
            parameters[0].Value = model.CategoryName;
            parameters[1].Value = model.Amount;
            parameters[2].Value = model.ActiveDuration;
            parameters[3].Value = model.Remark;
            parameters[4].Value = model.Status;
            parameters[5].Value = model.Id;
            //parameter Array
            paramArray.Add(parameters);

            return DataBaseOperator.MoneyWriteDbHelper.ExecutMultiSQLwithTransaction(sqlArray, paramArray);
        }
        #endregion

        #region Select
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public InterestBonusCategoryModel GetModel(int Id)
        {
            var strSql = new StringBuilder();
            strSql.Append("select Id,CategoryName,Amount,ActiveDuration,CreatorID,CreateDate,Remark,Status from bonus_category ");
            strSql.Append(" where Id=@Id");
            MySqlParameter[] parameters = {
					new MySqlParameter("@Id", MySqlDbType.Int32)
			};
            parameters[0].Value = Id;
            var ds = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql.ToString(), CommandType.Text, parameters);

            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public InterestBonusCategoryModel DataRowToModel(DataRow row)
        {
            var model = new InterestBonusCategoryModel();
            if (row != null)
            {
                if (row["Id"] != null && row["Id"].ToString() != "")
                {
                    model.Id = int.Parse(row["Id"].ToString());
                }
                if (row["CategoryName"] != null)
                {
                    model.CategoryName = row["CategoryName"].ToString();
                }
                if (row["Amount"] != null && row["Amount"].ToString() != "")
                {
                    model.Amount = decimal.Parse(row["Amount"].ToString());
                }
                if (row["ActiveDuration"] != null && row["ActiveDuration"].ToString() != "")
                {
                    model.ActiveDuration = int.Parse(row["ActiveDuration"].ToString());
                }
                if (row["CreatorID"] != null && row["CreatorID"].ToString() != "")
                {
                    model.CreatorID = int.Parse(row["CreatorID"].ToString());
                }
                if (row["CreateDate"] != null && row["CreateDate"].ToString() != "")
                {
                    model.CreateDate = DateTime.Parse(row["CreateDate"].ToString());
                }
                if (row["Remark"] != null)
                {
                    model.Remark = row["Remark"].ToString();
                }
                if (row["Status"] != null && row["Status"].ToString() != "")
                {
                    model.Status = int.Parse(row["Status"].ToString());
                }
            }
            return model;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            var strSql = new StringBuilder();
            strSql.Append("select Id,CategoryName,Amount,ActiveDuration,CreatorID,CreateDate,Remark,Status ");
            strSql.Append(" FROM bonus_category ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql.ToString());
        }
        #endregion

        #region Delete
        #endregion
    }
}
