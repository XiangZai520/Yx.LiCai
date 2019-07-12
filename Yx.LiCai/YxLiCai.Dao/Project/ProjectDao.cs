/*
 * 项目管理数据访问
 * 作者：侯裕祥
 * 时间：2015年6月16日 09:53:53
 * 版本：1.0.0.0
 * 
 */
using System.Text;
using YxLiCai.Model.Project;
using MySql.Data.MySqlClient;
using YxLiCai.DataHelper;
using System.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using YxLiCai.Tools.Util;
namespace YxLiCai.Dao.Project
{
    /// <summary>
    /// 项目管理数据访问
    /// </summary>
    public class ProjectDao
    {
        #region 添加操作
        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model">项目实体信息</param>
        /// <param name="CreateID">创建人ID </param>
        /// <returns></returns>
        public bool Add(ProjectModel model, int CreateID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into pjt(");
            strSql.Append(@"name,ord_number,amount,pjt_status,verify_status,pjt_type,borrower,weight,amount_able,amount_sold,c_time,launch_ime,e_time,borrowing_rate,remark,is_deleted ,repayment_mode,loan_period,fer_account_id,pdt_collection)");
            strSql.Append(" values (");
            strSql.Append("?ProjectName,?OrderNumber,?Amount,?ProjectStatus,?VerifyStatus,?ProjectType,?Borrower,?Weight,?AvailableAmount,?AmountSold,?AddTime,?LaunchTime,?EndTime,?BorrowingRate,?Remark,?IsDeleted,?RepaymentMode,?LoanPeriod,?account_id,?pdt_collection);select @@IDENTITY as InsertId;");
            MySqlParameter[] parameters = {					
                    new MySqlParameter("?ProjectName", MySqlDbType.VarChar,100),
                    new MySqlParameter("?OrderNumber", MySqlDbType.VarChar,16),
                    new MySqlParameter("?Amount", MySqlDbType.Decimal,18),
                    new MySqlParameter("?ProjectStatus", MySqlDbType.Int32,4),
                    new MySqlParameter("?VerifyStatus", MySqlDbType.Int32,4),
                    new MySqlParameter("?ProjectType", MySqlDbType.Int32,4),
                    new MySqlParameter("?Borrower", MySqlDbType.VarChar,60),
                    new MySqlParameter("?Weight", MySqlDbType.Int32,4),
                    new MySqlParameter("?AmountSold", MySqlDbType.Decimal,18),
                    new MySqlParameter("?AvailableAmount", MySqlDbType.Decimal,18),
                    new MySqlParameter("?AddTime", MySqlDbType.DateTime),
                    new MySqlParameter("?LaunchTime", MySqlDbType.DateTime),
                    new MySqlParameter("?EndTime", MySqlDbType.DateTime),
                    new MySqlParameter("?BorrowingRate", MySqlDbType.Decimal,18),
                    new MySqlParameter("?IsDeleted", MySqlDbType.Bit),
                    new MySqlParameter("?Remark", MySqlDbType.VarChar,200),
                    new MySqlParameter("?RepaymentMode", MySqlDbType.VarChar,50),
					new MySqlParameter("?LoanPeriod", MySqlDbType.Int32,4),
                    new MySqlParameter("?account_id", MySqlDbType.Int32,4),
                    new MySqlParameter("?pdt_collection", MySqlDbType.VarChar,100)};
            parameters[0].Value = model.ProjectName;
            parameters[1].Value = model.OrderNumber;
            parameters[2].Value = model.Amount;
            parameters[3].Value = model.ProjectStatus;
            parameters[4].Value = model.VerifyStatus;
            parameters[5].Value = model.ProjectType;
            parameters[6].Value = model.Borrower;
            parameters[7].Value = model.Weight;
            parameters[8].Value = model.AmountSold;
            parameters[9].Value = model.AvailableAmount;
            parameters[10].Value = DateTime.Now;
            parameters[11].Value = model.LaunchTime;
            parameters[12].Value = model.EndTime;
            parameters[13].Value = model.BorrowingRate;
            parameters[14].Value = 0;
            parameters[15].Value = model.Remark;
            parameters[16].Value = model.RepaymentMode;
            parameters[17].Value = model.LoanPeriod;
            parameters[18].Value = model.account_id;
            parameters[19].Value = model.pdt_collection;
            //return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql.ToString(), "2015年5月25日 10:48:12 侯裕祥 添加一条项目信息", parameters);
            object i = DataBaseOperator.MoneyWriteDbHelper.ExecuteScalar(strSql.ToString(), "2015年6月16日 17:55:01 侯裕祥 添加一条融资方账户金额信息", parameters);
            if (i != null)
            {
                if (int.Parse(i.ToString()) > 0)
                {
                    // 2) 添加项目日志表                   
                    var sql2 = @"insert into pjt_log (c_time,pjt_id,amount_sold,amount_able,amount_able_fz,version,remark,creator_id,m_time) values (?c_time,?pjt_id,?amount_sold,?amount_able,?amount_able_fz,?version,?remark,?creator_id,?m_time)";
                    MySqlParameter[] param2 = {
					new MySqlParameter("?c_time", MySqlDbType.DateTime),
					new MySqlParameter("?pjt_id", MySqlDbType.Int32,11),
					new MySqlParameter("?amount_sold", MySqlDbType.Decimal,20),
					new MySqlParameter("?amount_able", MySqlDbType.Decimal,20),
					new MySqlParameter("?amount_able_fz", MySqlDbType.Decimal,20),
					new MySqlParameter("?version", MySqlDbType.Int32,11),
					new MySqlParameter("?remark", MySqlDbType.VarChar,500),
				 	new MySqlParameter("?creator_id", MySqlDbType.Int32,11),
					new MySqlParameter("?m_time", MySqlDbType.DateTime)};
                    param2[0].Value = DateTime.Now;
                    param2[1].Value = i;
                    param2[2].Value = model.AmountSold;
                    param2[3].Value = model.AvailableAmount;
                    param2[4].Value = 0;
                    param2[5].Value = 0;
                    param2[6].Value = "新增项目记录初始化信息[amount_sold:" + model.AmountSold + "] [amount_able:" + model.AvailableAmount + "] [amount_able_fz:0.0000000000]";
                    param2[7].Value = CreateID;
                    param2[8].Value = DateTime.Now;
                    DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(sql2, "2015年6月16日 17:55:01 添加一条项目日志记录信息", param2);

                    // 3) insert into pjt_pdtcategory
                    StringBuilder sqq = new StringBuilder();
                    var uca = model.pdt_collection.Split(',');
                    for (var v = 0; v < uca.Length; v++)
                    {
                        sqq.Append("insert into pjt_pdtcategory(");
                        sqq.Append("pjt_id,pdt_c_id)");
                        sqq.Append(" values (");
                        sqq.Append(i + "," + uca[v] + ");");
                    }
                    DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(sqq.ToString(), "2015年6月16日 17:55:01 添加项目关联产品表信息");

                    return true;
                }
            }
            return false;

        }
        #endregion

        #region 更新操作
        /// <summary>
        ///  更新一条数据
        /// </summary>
        /// <param name="model">修改的实体信息</param>
        /// <param name="OldModel">原来的数据信息</param>
        /// <param name="CreadtID">创建人ID</param>
        /// <returns></returns>
        public bool Update(ProjectModel model, ProjectModel OldModel, int CreadtID)
        {
            var sqlArray = new List<string>();
            var paramArray = new List<DbParameter[]>();

            // 1) 修改项目
            #region
            var sql1 = @"UPDATE pjt 
                            SET    NAME=?projectname, 
                                   ord_number=?ordernumber, 
                                   amount=?amount, 
                                   pjt_status=?projectstatus, 
                                   verify_status=?verifystatus, 
                                   pjt_type=?projecttype, 
                                   borrower=?borrower, 
                                   weight=?weight, 
                                   amount_sold=?amountsold, 
                                   amount_able=?availableamount, 
                                   c_time=?addtime, 
                                   launch_ime=?launchtime, 
                                   e_time=?endtime, 
                                   borrowing_rate=?borrowingrate, 
                                   remark=?remark, 
                                   repayment_mode=?repaymentmode, 
                                   loan_period=?loanperiod, 
                                   fer_account_id=?account_id,
                                   pdt_collection=?pdt_collection 
                                   WHERE  id=?id ";
            MySqlParameter[] param1 = {					
                  	new MySqlParameter("?ProjectName", MySqlDbType.VarChar,100),
					new MySqlParameter("?OrderNumber", MySqlDbType.VarChar,16),
					new MySqlParameter("?Amount", MySqlDbType.Decimal,18),
					new MySqlParameter("?ProjectStatus", MySqlDbType.Int32,4),
					new MySqlParameter("?VerifyStatus", MySqlDbType.Int32,4),
					new MySqlParameter("?ProjectType", MySqlDbType.Int32,4),
					new MySqlParameter("?Borrower", MySqlDbType.VarChar,60),
					new MySqlParameter("?Weight", MySqlDbType.Int32,4),
					new MySqlParameter("?AvailableAmount", MySqlDbType.Decimal,18),
					new MySqlParameter("?AmountSold", MySqlDbType.Decimal,18),
					new MySqlParameter("?AddTime", MySqlDbType.DateTime),
					new MySqlParameter("?LaunchTime", MySqlDbType.DateTime),
					new MySqlParameter("?EndTime", MySqlDbType.DateTime),
					new MySqlParameter("?BorrowingRate", MySqlDbType.Decimal,18),
					new MySqlParameter("?Remark", MySqlDbType.VarChar,200),
                    new MySqlParameter("?RepaymentMode", MySqlDbType.VarChar,50),
					new MySqlParameter("?LoanPeriod", MySqlDbType.Int32,4),
                    new MySqlParameter("?account_id", MySqlDbType.Int32,4),
                    new MySqlParameter("?pdt_collection", MySqlDbType.VarChar,100),
					new MySqlParameter("?Id", MySqlDbType.Int32,4)};
            param1[0].Value = model.ProjectName;
            param1[1].Value = model.OrderNumber;
            param1[2].Value = model.Amount;
            param1[3].Value = model.ProjectStatus;
            param1[4].Value = model.VerifyStatus;
            param1[5].Value = model.ProjectType;
            param1[6].Value = model.Borrower;
            param1[7].Value = model.Weight;
            param1[8].Value = model.AvailableAmount;
            param1[9].Value = model.AmountSold;
            param1[10].Value = model.AddTime;
            param1[11].Value = model.LaunchTime;
            param1[12].Value = model.EndTime;
            param1[13].Value = model.BorrowingRate;
            param1[14].Value = model.Remark;
            param1[15].Value = model.RepaymentMode;
            param1[16].Value = model.LoanPeriod;
            param1[17].Value = model.account_id;
            param1[18].Value = model.pdt_collection;
            param1[19].Value = model.Id;
            sqlArray.Add(sql1);
            paramArray.Add(param1);
            #endregion

            // 2) 插入项目日志表记录     
            #region
            var sql2 = @"INSERT INTO pjt_log 
                                    (c_time, 
                                     pjt_id, 
                                     amount_sold, 
                                     amount_able, 
                                     amount_able_fz, 
                                     version, 
                                     remark, 
                                     creator_id, 
                                     m_time) 
                        VALUES      (?c_time, 
                                     ?pjt_id, 
                                     ?amount_sold, 
                                     ?amount_able, 
                                     ?amount_able_fz, 
                                     ?version, 
                                     ?remark, 
                                     ?creator_id, 
                                     ?m_time) ";
            MySqlParameter[] param2 = {
					new MySqlParameter("?c_time", MySqlDbType.DateTime),
					new MySqlParameter("?pjt_id", MySqlDbType.Int32,11),
					new MySqlParameter("?amount_sold", MySqlDbType.Decimal,20),
					new MySqlParameter("?amount_able", MySqlDbType.Decimal,20),
					new MySqlParameter("?amount_able_fz", MySqlDbType.Decimal,20),
					new MySqlParameter("?version", MySqlDbType.Int32,11),
					new MySqlParameter("?remark", MySqlDbType.VarChar,500),
					new MySqlParameter("?creator_id", MySqlDbType.Int32,11),
					new MySqlParameter("?m_time", MySqlDbType.DateTime)};
            param2[0].Value = DateTime.Now;
            param2[1].Value = model.Id;
            param2[2].Value = model.AmountSold;
            param2[3].Value = model.AvailableAmount;
            param2[4].Value = model.Amount_able_fz;
            param2[5].Value = 0;
            param2[6].Value = "项目修改时信息变化-[amount_sold:" + OldModel.AmountSold + "变为 " + model.AmountSold + "] [amount_able:" + OldModel.AvailableAmount + "变为 " + model.AvailableAmount + "] [amount_able_fz:" + OldModel.Amount_able_fz + "变为 " + model.Amount_able_fz + "]";
            param2[7].Value = CreadtID;
            param2[8].Value = DateTime.Now;
            sqlArray.Add(sql2);
            paramArray.Add(param2);
            #endregion

            // 3) insert into pjt_pdtcategory

            #region
            StringBuilder sqq = new StringBuilder();
            sqq.Append("DELETE FROM pjt_pdtcategory WHERE pjt_id = " + model.Id + ";");
            var uca = model.pdt_collection.Split(',');
            for (var v = 0; v < uca.Length; v++)
            {
                sqq.Append("insert into pjt_pdtcategory(");
                sqq.Append("pjt_id,pdt_c_id)");
                sqq.Append(" values (");
                sqq.Append(model.Id + "," + uca[v] + ");");
            }
            MySqlParameter[] param3 = {
					new MySqlParameter("id", MySqlDbType.Int32,11)};
            param3[0].Value = model.Id;
            sqlArray.Add(sqq.ToString());
            paramArray.Add(param3);
            #endregion

            return DataBaseOperator.MoneyWriteDbHelper.ExecutMultiSQLwithTransaction(sqlArray, paramArray);



        }
        /// <summary>
        /// 更新一条数据(修改项目审核状态)
        /// </summary>
        /// <param name="model"></param>
        /// <param name="CreadtID">创建人ID</param>
        /// <returns></returns>
        public bool UpdateProjectStatus(ProjectModel model, int CreadtID)
        {
            var sqlArray = new List<string>();
            var paramArray = new List<DbParameter[]>();

            // 1) 修改项目项目     
            #region
            var sql1 = @"update pjt set pjt_status=?ProjectStatus,remark=?Remark where id=?Id ";
            MySqlParameter[] param1 = {					
                  new MySqlParameter("?ProjectStatus", MySqlDbType.Int32,4),	
				  new MySqlParameter("?Remark", MySqlDbType.VarChar,200),
				  new MySqlParameter("?Id", MySqlDbType.Int32,4)};
            param1[0].Value = model.ProjectStatus;
            param1[1].Value = model.Remark;
            param1[2].Value = model.Id;
            sqlArray.Add(sql1);
            paramArray.Add(param1);
            #endregion
            if (model.ProjectStatus == 2)
            { // 2）审核成功后修改融资方账户金钱表中借款字段   fer_account
                #region
                var sql2 = @" UPDATE fer_account 
                                    SET  fer_account.amount_borrow =(SELECT Sum(amount) AS pamount 
                                      FROM   pjt 
                                      WHERE  fer_account_id =?account_id AND pjt_status>1) 
                                      WHERE  fer_account.id =?account_id";
                MySqlParameter[] param2 = {
				new MySqlParameter("?account_id", MySqlDbType.Int32,4)};
                param2[0].Value = model.account_id;

                sqlArray.Add(sql2);
                paramArray.Add(param2);
                #endregion
            }
            // 2) 插入项目日志表记录     
            #region
            var sql3 = @"INSERT INTO pjt_log 
                                    (c_time, 
                                     pjt_id, 
                                     amount_sold, 
                                     amount_able, 
                                     amount_able_fz, 
                                     version, 
                                     remark, 
                                     creator_id, 
                                     m_time) 
                        VALUES      (?c_time, 
                                     ?pjt_id, 
                                     ?amount_sold, 
                                     ?amount_able, 
                                     ?amount_able_fz, 
                                     ?version, 
                                     ?remark, 
                                     ?creator_id, 
                                     ?m_time) ";
            MySqlParameter[] param3 = {
					new MySqlParameter("?c_time", MySqlDbType.DateTime),
					new MySqlParameter("?pjt_id", MySqlDbType.Int32,11),
					new MySqlParameter("?amount_sold", MySqlDbType.Decimal,20),
					new MySqlParameter("?amount_able", MySqlDbType.Decimal,20),
					new MySqlParameter("?amount_able_fz", MySqlDbType.Decimal,20),
					new MySqlParameter("?version", MySqlDbType.Int32,11),
					new MySqlParameter("?remark", MySqlDbType.VarChar,500),
					new MySqlParameter("?creator_id", MySqlDbType.Int32,11),
					new MySqlParameter("?m_time", MySqlDbType.DateTime)};
            param3[0].Value = DateTime.Now;
            param3[1].Value = model.Id;
            param3[2].Value = model.AmountSold;
            param3[3].Value = model.AvailableAmount;
            param3[4].Value = model.Amount_able_fz;
            param3[5].Value = 0;
            if (model.ProjectStatus == 2)
            {
                param3[6].Value = "项目审核通过时信息变化-[amount_sold:" + model.AmountSold + "变为 " + model.AmountSold + "] [amount_able:" + model.AvailableAmount + "变为 " + model.AvailableAmount + "] [amount_able_fz:" + model.Amount_able_fz + "变为 " + model.Amount_able_fz + "]";
            }
            else { param3[6].Value = "项目审核不通过时信息变化-[amount_sold:" + model.AmountSold + "变为 " + model.AmountSold + "] [amount_able:" + model.AvailableAmount + "变为 " + model.AvailableAmount + "] [amount_able_fz:" + model.Amount_able_fz + "变为 " + model.Amount_able_fz + "]"; }

            param3[7].Value = CreadtID;
            param3[8].Value = DateTime.Now;
            sqlArray.Add(sql3);
            paramArray.Add(param3);
            #endregion

            return DataBaseOperator.MoneyWriteDbHelper.ExecutMultiSQLwithTransaction(sqlArray, paramArray);

        }
        /// <summary>
        /// 更新一条数据(修改项目权值)
        /// </summary>
        public bool UpdateProjectWeight(int Weight, int ProjectID)
        {
            string strSql = "update pjt set weight=?Weight  where id=?ProjectID";
            MySqlParameter[] parameters = {	
					new MySqlParameter("?Weight", MySqlDbType.Int32,4),					
					new MySqlParameter("?ProjectID", MySqlDbType.Int32,4)};
            parameters[0].Value = Weight;
            parameters[1].Value = ProjectID;
            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql, "2015年5月25日 10:47:46 侯裕祥 修改项目权值", parameters);
        }
        #endregion

        #region 删除操作
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int Id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update pjt set is_deleted=1 ");
            strSql.Append(" where pjt_status in(0,1) and id=?Id ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?Id", MySqlDbType.Int32,4)			};
            parameters[0].Value = Id;
            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql.ToString(), "2015年5月25日 10:46:45 侯裕祥 删除项目信息", parameters);
        }
        /// <summary>
        /// 批量删除数据
        /// </summary>
        public bool DeleteList(string Idlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update pjt set is_deleted=1 ");
            strSql.Append(" where pjt_status in(0,1) and id in (" + Idlist.TrimEnd(',') + ")  ");
            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql.ToString(), "批量删除");

        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="deleteIds">删除的ID( ,分割)</param>
        /// <returns>删除成功条数</returns>
        public int DeleteBatch(string deleteIds)
        {
            int delNum = 0;
            if (deleteIds.Trim() == "") { return 0; }
            string[] ids = deleteIds.Split(',');

            foreach (string idStr in ids)
            {
                if (Delete(int.Parse(idStr.Trim())))
                {
                    delNum++;
                }

            }
            return delNum;
        }
        #endregion

        #region 查询操作
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ProjectModel GetModel(int Id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT p.id, 
                                   p.NAME, 
                                   p.ord_number, 
                                   p.amount, 
                                   p.pjt_status, 
                                   p.verify_status, 
                                   p.pjt_type, 
                                   p.borrower, 
                                   p.weight, 
                                   p.amount_sold, 
                                   p.amount_able, 
                                   p.c_time, 
                                   p.launch_ime, 
                                   p.e_time, 
                                   p.borrowing_rate, 
                                   p.remark, 
                                   p.is_deleted, 
                                   p.repayment_mode, 
                                   p.loan_period, 
                                   f.NAME AS fname, 
                                   p.fer_account_id as account_id,
                                   p.amount_able_fz,
                                   p.pdt_collection
                            FROM   pjt AS p       
                                          LEFT JOIN fer_account AS fa 
                                          ON p.fer_account_id = fa.id 
                                          LEFT JOIN fer_info f ON 	 f.id=fa.fer_id");
            strSql.Append(" where p.id=?Id ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?Id", MySqlDbType.Int32,4)			};
            parameters[0].Value = Id;

            ProjectModel model = new ProjectModel();
            DataSet ds = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql.ToString(), CommandType.Text, parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            return null;
        }
        /// <summary>
        /// 获得数据列表(没有用-别人再用)
        /// </summary>
        public List<ProjectModel> GetList(string strWhere)
        {
            var model = new List<ProjectModel>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select p.id,p.name,ord_number,amount,pjt_status,verify_status,pjt_type,borrower,weight,amount_sold,amount_able,c_time,launch_ime,e_time,borrowing_rate,remark,is_deleted,
                repayment_mode,loan_period,f.name,p.fer_account_id as account_id from pjt as p left join  fer_info as f on  p.fer_account_id=f.id ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            DataSet ds = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql.ToString(), "获得所有的项目信息列表");
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    ProjectModel mod = new ProjectModel();
                    mod = DataRowToModel(row);
                    model.Add(mod);
                }
            }
            return model;
        }
        /// <summary>
        /// 获取项目 id name 键值对列表
        /// </summary>
        /// <returns></returns>
        public List<ProjectModel> GetProjectIDNameList(int categoryID)
        {
            var model = new List<ProjectModel>();
            var strSql = @" SELECT id,NAME,weight,amount_able 
                            FROM pjt A
                            INNER JOIN pjt_pdtcategory B ON A.`id`=B.`pjt_id`
                            WHERE pjt_status=2 
                            AND B.`pdt_c_id` = " + categoryID;
            DataSet ds = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql, "获取项目 id name 键值对列表");
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    ProjectModel mod = new ProjectModel();
                    mod = DataRowToModel(row);
                    model.Add(mod);
                }
            }
            return model;
        }
        /// <summary>
        /// 获取项目列表
        /// </summary>
        /// <returns></returns>
        public DataSet GetListM(string Panme, int Pstate, DateTime Sdate, DateTime Edate)
        {
            if (Edate != DateTime.MinValue)
            {
                Edate = new DateTime(Edate.Year, Edate.Month, Edate.Day, 23, 59, 59);
            }
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT p.e_time,
                                   p.id, 
                                   p.NAME, 
                                   p.ord_number, 
                                   p.amount, 
                                   p.pjt_status, 
                                   p.verify_status, 
                                   p.pjt_type, 
                                   p.borrower, 
                                   p.weight, 
                                   p.amount_able, 
                                   p.amount_sold, 
                                   p.c_time, 
                                   p.launch_ime, 
                                   p.launch_ime, 
                                   p.borrowing_rate, 
                                   p.remark, 
                                   p.is_deleted, 
                                   p.repayment_mode, 
                                   p.loan_period, 
                                   f.NAME AS fname, 
                                   p.fer_account_id as account_id,
                                   p.amount_able_fz,
                                   p.pdt_collection
                            FROM   pjt AS p        
                                   LEFT JOIN fer_account AS fa 
                                          ON p.fer_account_id = fa.id 
                                    LEFT JOIN fer_info f ON   f.id=fa.fer_id  ");
            strSql.Append(" where p.is_deleted=0  ");

            if (!string.IsNullOrEmpty(Panme))
            {
                Panme = "%" + Panme.Replace(" ", "") + "%";
                strSql.Append(" And p.name like ?Panme ");
            }
            if (Pstate != -1)
            {
                strSql.Append(" And p.pjt_status =?Pstate");
            }
            string sqlWhere = " ";
            if (Sdate != DateTime.MinValue)
            {
                sqlWhere += " And p.launch_ime  >= ?Sdate ";
            }
            if (Edate != DateTime.MinValue)
            {
                sqlWhere += " And p.e_time  <= ?Edate ";
            }
            if (Sdate != DateTime.MinValue && Edate != DateTime.MinValue)
            {
                sqlWhere = " And p.launch_ime  >= ?Sdate And p.e_time  <= ?Edate ";
            }
            strSql.Append("  " + sqlWhere + " ");

            strSql.Append(" order by p.c_time desc ");

            MySqlParameter[] parameters = {
					new MySqlParameter("?Panme", MySqlDbType.VarChar),
                    new MySqlParameter("?Pstate", MySqlDbType.Int16),
                    new MySqlParameter("?Sdate", MySqlDbType.DateTime),           
                    new MySqlParameter("?Edate", MySqlDbType.DateTime)             
            };
            parameters[0].Value = Panme;
            parameters[1].Value = Pstate;
            parameters[2].Value = Sdate;
            parameters[3].Value = Edate;

            return DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql.ToString(), "获取项目列表", parameters);
        }
        /// <summary>
        /// 获得数据列表(修改权值)
        /// </summary>
        public DataSet GetListM(string ProjectName, DateTime Sdate, DateTime Edate, int Sweight, int Eweight)
        {
            if (Edate != DateTime.MinValue)
            {
                Edate = new DateTime(Edate.Year, Edate.Month, Edate.Day, 23, 59, 59);
            }
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT p.e_time, 
                                   p.id, 
                                   p.NAME, 
                                   p.ord_number, 
                                   p.amount, 
                                   p.pjt_status, 
                                   p.verify_status, 
                                   p.pjt_type, 
                                   p.borrower, 
                                   p.weight, 
                                   p.amount_able, 
                                   p.amount_sold, 
                                   p.c_time, 
                                   p.launch_ime, 
                                   p.launch_ime, 
                                   p.borrowing_rate, 
                                   p.remark, 
                                   p.is_deleted, 
                                   p.repayment_mode, 
                                   p.loan_period, 
                                   f.NAME AS fname, 
                                   p.fer_account_id as account_id,
                                   p.amount_able_fz,
                                   p.pdt_collection
                            FROM   pjt AS p 
                                   LEFT JOIN fer_account AS fa 
                                          ON p.fer_account_id = fa.id 
                                          LEFT JOIN fer_info f ON 	 f.id=fa.fer_id   ");
            strSql.Append(" where p.is_deleted=0 And p.pjt_status =2 ");

            if (!string.IsNullOrEmpty(ProjectName))
            {
                ProjectName = "%" + ProjectName.Replace(" ", "") + "%";
                strSql.Append(" And p.name like ?ProjectName ");
            }


            string sqlWhere = " ";
            if (Sdate != DateTime.MinValue)
            {
                sqlWhere += " And p.launch_ime  >= ?Sdate ";
            }
            if (Edate != DateTime.MinValue)
            {
                sqlWhere += " And p.launch_ime  <= ?Edate ";
            }
            if (Sdate != DateTime.MinValue && Edate != DateTime.MinValue)
            {

                sqlWhere = " And p.launch_ime Between  ?Sdate   And  ?Edate ";
            }
            strSql.Append("  " + sqlWhere + " ");

            if (Sweight > 0)
            {
                strSql.Append(" And p.weight >=?Sweight ");
            }
            if (Eweight > 0)
            {
                strSql.Append(" And p.weight <=?Eweight ");
            }
            if (Sweight > 0 && Eweight > 0)
            {
                strSql.Append(" And p.weight Between ?Sweight  And ?Eweight  ");
            }


            strSql.Append(" order by p.c_time desc ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?ProjectName", MySqlDbType.VarChar),
                    new MySqlParameter("?Sdate", MySqlDbType.DateTime),
                    new MySqlParameter("?Edate", MySqlDbType.DateTime),           
                    new MySqlParameter("?Sweight", MySqlDbType.Decimal),
                    new MySqlParameter("?Eweight", MySqlDbType.Decimal) 
            };
            parameters[0].Value = ProjectName;
            parameters[1].Value = Sdate;
            parameters[2].Value = Edate;
            parameters[3].Value = Sweight;
            parameters[4].Value = Eweight;

            return DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql.ToString(), "获取项目列表", parameters);
        }
        #endregion

        #region 对象转换操作
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ProjectModel DataRowToModel(DataRow row)
        {
            ProjectModel model = new ProjectModel();
            if (row != null)
            {
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.Id = int.Parse(row["id"].ToString());
                }
                if (row.Table.Columns.Contains("amount_able_fz") && row["amount_able_fz"] != null && row["amount_able_fz"].ToString() != "")
                {
                    model.Amount_able_fz = decimal.Parse(row["amount_able_fz"].ToString());
                }

                if (row["name"] != null)
                {
                    model.ProjectName = row["name"].ToString();
                }
                if (row.Table.Columns.Contains("ord_number") && row["ord_number"] != null)
                {
                    model.OrderNumber = row["ord_number"].ToString();
                }
                if (row.Table.Columns.Contains("amount") && row["amount"] != null && row["amount"].ToString() != "")
                {
                    model.Amount = decimal.Parse(row["amount"].ToString());
                }
                if (row.Table.Columns.Contains("verify_status") && row["verify_status"] != null && row["verify_status"].ToString() != "")
                {
                    model.VerifyStatus = int.Parse(row["verify_status"].ToString());
                }
                if (row.Table.Columns.Contains("pjt_type") && row["pjt_type"] != null && row["pjt_type"].ToString() != "")
                {
                    model.ProjectType = int.Parse(row["pjt_type"].ToString());
                }
                if (row.Table.Columns.Contains("borrower") && row["borrower"] != null)
                {
                    model.Borrower = row["borrower"].ToString();
                }
                if (row.Table.Columns.Contains("weight") && row["weight"] != null && row["weight"].ToString() != "")
                {
                    model.Weight = int.Parse(row["weight"].ToString());
                }
                if (row.Table.Columns.Contains("amount_able") && row["amount_able"] != null && row["amount_able"].ToString() != "")
                {
                    model.AvailableAmount = decimal.Parse(row["amount_able"].ToString());
                }
                if (row.Table.Columns.Contains("amount_sold") && row["amount_sold"] != null && row["amount_sold"].ToString() != "")
                {
                    model.AmountSold = decimal.Parse(row["amount_sold"].ToString());
                }
                if (row.Table.Columns.Contains("c_time") && row["c_time"] != null && row["c_time"].ToString() != "")
                {
                    model.AddTime = DateTime.Parse(row["c_time"].ToString());
                }
                if (row.Table.Columns.Contains("launch_ime") && row["launch_ime"] != null && row["launch_ime"].ToString() != "")
                {
                    model.LaunchTime = DateTime.Parse(row["launch_ime"].ToString());
                }
                if (row.Table.Columns.Contains("e_time") && row["e_time"] != null && row["e_time"].ToString() != "")
                {
                    model.EndTime = DateTime.Parse(row["e_time"].ToString());
                }
                if (row.Table.Columns.Contains("borrowing_rate") && row["borrowing_rate"] != null && row["borrowing_rate"].ToString() != "")
                {
                    model.BorrowingRate = decimal.Parse(row["borrowing_rate"].ToString());
                }
                if (row.Table.Columns.Contains("remark") && row["remark"] != null)
                {
                    model.Remark = row["remark"].ToString();
                }
                if (row.Table.Columns.Contains("is_deleted") && row["is_deleted"] != null && row["is_deleted"].ToString() != "")
                {
                    if ((row["is_deleted"].ToString() == "1") || (row["is_deleted"].ToString().ToLower() == "true"))
                    {
                        model.IsDeleted = true;
                    }
                    else
                    {
                        model.IsDeleted = false;
                    }
                }
                if (row.Table.Columns.Contains("repayment_mode") && row["repayment_mode"] != null)
                {
                    model.RepaymentMode = row["repayment_mode"].ToString();
                }
                if (row.Table.Columns.Contains("loan_period") && row["loan_period"] != null && row["loan_period"].ToString() != "")
                {
                    model.LoanPeriod = int.Parse(row["loan_period"].ToString());
                }
                if (row.Table.Columns.Contains("account_id") && row["account_id"] != null && row["account_id"].ToString() != "")
                {
                    model.account_id = int.Parse(row["account_id"].ToString());
                }

                if (row.Table.Columns.Contains("fname") && row["fname"] != null && row["fname"].ToString() != "")
                {
                    model.account_id_name = row["fname"].ToString();
                }
                if (row.Table.Columns.Contains("pdt_collection") && row["pdt_collection"] != null && row["pdt_collection"].ToString() != "")
                {
                    model.pdt_collection = row["pdt_collection"].ToString();
                }

                if (row.Table.Columns.Contains("pjt_status") && row["pjt_status"] != null && row["pjt_status"].ToString() != "")
                {
                    model.ProjectStatus = int.Parse(row["pjt_status"].ToString());
                    if ((row["pjt_status"].ToString() == "0"))
                    {
                        model.Checkstatuss = " 待审核";
                    }
                    else if ((row["pjt_status"].ToString() == "1"))
                    {
                        model.Checkstatuss = "审核未通过";

                    }
                    else if ((row["pjt_status"].ToString() == "2"))
                    {
                        model.Checkstatuss = "售卖中";

                    }
                    else if ((row["pjt_status"].ToString() == "3"))
                    {
                        model.Checkstatuss = "已售罄";
                    }
                    else
                    {
                        model.Checkstatuss = "完成";
                        model.ProjectStatuss = "完成";
                    }
                    if (model.ProjectStatus <= 1)
                    {
                        model.ProjectStatuss = "未开始";
                    }
                    else
                    {
                        if (model.AmountSold <= 0)
                        {
                            model.ProjectStatuss = "未开始";
                        }
                        if (model.AmountSold == model.Amount && model.Amount != 0)
                        {
                            model.ProjectStatuss = "已结束";
                        }
                        if (model.AmountSold > 0 && model.AmountSold < model.Amount)
                        {
                            model.ProjectStatuss = "售卖中";
                        }
                    }
                    model.ProjectStatus = int.Parse(row["pjt_status"].ToString());
                }

                if (row.Table.Columns.Contains("launch_ime") && row["launch_ime"] != null && row["launch_ime"].ToString() != "" && row["amount_sold"] != null && row["amount_sold"].ToString() != "" && row["amount"] != null && row["amount"].ToString() != "")
                {
                    DateTime t1 = Convert.ToDateTime(System.DateTime.Now);
                    DateTime t2 = Convert.ToDateTime(row["launch_ime"].ToString());
                    TimeSpan ts = t1 - t2;
                    ///当前日期-上线日期
                    int d = ts.Days;
                    if (d > 0 && model.AmountSold > 0 && model.Amount > 0)
                    {
                        model.Chushoudays = d;
                        model.Quantity = model.AmountSold / model.Amount;
                        model.Singleday = model.AmountSold / (model.Amount * d);
                        model.Fullscaledays = (1 - model.Quantity) / (model.Singleday);
                    }
                }
            }
            return model;
        }
        #endregion

        #region 获取融资方信息ID、单位信息
        public List<YxLiCai.Model.UserFinancingManagement.UserInfo_FinancingManagement_Model> GetUserInfo_FinancingManagement_Model()
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT fer_account.id,NAME FROM fer_info INNER JOIN fer_account ON fer_info.`id`=fer_account.`fer_id` ");

            var model = new List<YxLiCai.Model.UserFinancingManagement.UserInfo_FinancingManagement_Model>();
            DataSet ds = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql.ToString(), CommandType.Text);
            if (ds.Tables[0].Rows.Count > 0)
            {
                //return DataRowToModel(ds.Tables[0].Rows[0]);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    YxLiCai.Model.UserFinancingManagement.UserInfo_FinancingManagement_Model mod = new YxLiCai.Model.UserFinancingManagement.UserInfo_FinancingManagement_Model();
                    mod.financier_id = int.Parse(row["id"].ToString());
                    mod.financier_name = row["name"].ToString();
                    model.Add(mod);
                }
            }
            return model;
        }
        #endregion

        #region 判断项目名称是否重复
        /// <summary>
        /// 判断项目名称是否重复
        /// </summary>
        /// <param ProjectName="">项目名称</param>  
        /// <param Pid="">项目ID</param> 
        /// <returns></returns>
        public int ISProtExist(string ProjectName, int Pid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id from pjt  ");
            if (Pid > 0) { strSql.Append(" where name =?ProjectName and is_deleted !=1 and id !=?Pid "); } else { strSql.Append(" where name =?ProjectName and is_deleted !=1 "); }
            MySqlParameter[] parameters = {
					new MySqlParameter("?ProjectName", MySqlDbType.VarChar,100),
                 new MySqlParameter("?Pid", MySqlDbType.Int64)
			};
            parameters[0].Value = ProjectName;
            parameters[1].Value = Pid;
            object i = DataBaseOperator.MoneyReadDbHelper.ExecuteScalar(strSql.ToString(), "2015年6月16日 18:04:54-侯裕祥-判断项目方是否信息存在", parameters);

            if (i != null)
            {
                if (Convert.ToInt64(i) > 0)
                {
                    return int.Parse(i.ToString());
                }
            }
            return 0;
        }
        #endregion

        #region 添加项目日志记录信息
        /// <summary>
        /// 添加项目日志记录信息
        /// </summary>
        /// <param name="PjtID">项目ID</param>
        /// <param name="amount_sold_Old">已售金额(变化前)</param>
        /// <param name="amount_able_Old">可售金额(变化前)</param>
        /// <param name="amount_able_fz_Old">冻结金额(变化前)</param>
        /// <param name="amount_sold_New">已售金额(变化后)</param>
        /// <param name="amount_able_New">可售金额(变化后)</param>
        /// <param name="amount_able_fz_New">冻结金额(变化后)</param>
        /// <param name="creator_id">创建人ID</param>
        /// <param name="type">插入表示（0：项目修改，1：项目审核）</param>
        /// <returns></returns>
        public bool Add_Pjt_Log(int PjtID, decimal amount_sold_Old, decimal amount_able_Old, decimal amount_able_fz_Old, decimal amount_sold_New, decimal amount_able_New, decimal amount_able_fz_New, int creator_id, int type)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into pjt_log(");
            strSql.Append("c_time,pjt_id,amount_sold,amount_able,amount_able_fz,version,remark,creator_id,m_time)");
            strSql.Append(" values (");
            strSql.Append("@c_time,@pjt_id,@amount_sold,@amount_able,@amount_able_fz,@version,@remark,@creator_id,@m_time)");
            MySqlParameter[] parameters = {
					new MySqlParameter("@c_time", MySqlDbType.DateTime),
					new MySqlParameter("@pjt_id", MySqlDbType.Int32,11),
					new MySqlParameter("@amount_sold", MySqlDbType.Decimal,20),
					new MySqlParameter("@amount_able", MySqlDbType.Decimal,20),
					new MySqlParameter("@amount_able_fz", MySqlDbType.Decimal,20),
					new MySqlParameter("@version", MySqlDbType.Int32,11),
					new MySqlParameter("@remark", MySqlDbType.VarChar,500),
					new MySqlParameter("@creator_id", MySqlDbType.Int32,11),
					new MySqlParameter("@m_time", MySqlDbType.DateTime)};
            parameters[0].Value = DateTime.Now;
            parameters[1].Value = PjtID;
            parameters[2].Value = amount_sold_Old;
            parameters[3].Value = amount_able_Old;
            parameters[4].Value = amount_able_fz_Old;
            parameters[5].Value = 0;
            if (type == 0)
            { parameters[6].Value = "项目修改时信息变化-[amount_sold:" + amount_sold_Old + "变为 " + amount_sold_New + "] [amount_able:" + amount_able_Old + "变为 " + amount_able_New + "] [amount_able_fz:" + amount_able_fz_Old + "变为 " + amount_able_fz_New + "]"; }
            parameters[6].Value = "项目审核时信息变化-[amount_sold:" + amount_sold_Old + "变为 " + amount_sold_New + "] [amount_able:" + amount_able_Old + "变为 " + amount_able_New + "] [amount_able_fz:" + amount_able_fz_Old + "变为 " + amount_able_fz_New + "]";
            parameters[7].Value = creator_id;
            parameters[8].Value = DateTime.Now;

            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql.ToString(), "添加一条项目日志记录信息", parameters);
        }
        #endregion



        #region 查询简单的项目信息
        /// <summary>
        /// 通过项目ID得到一个简单的项目信息
        /// </summary>
        /// <param name="PID">项目ID</param>
        /// <returns></returns>
        public ProjectModel GetProjectModelSmple(int PID)
        {
            string sql = @"SELECT    amount_sold,amount_able,amount_able_fz                           
                            FROM       pjt  where id=?PID";
            MySqlParameter[] parms;
            parms = new MySqlParameter[]
            { 
                new MySqlParameter("?PID",MySqlDbType.Int64)
            };
            parms[0].Value = PID;
            var dr = DataBaseOperator.MoneyReadDbHelper.ExecuteReader(sql, CommandType.Text, parms);
            if (dr.Read())
            {

                ProjectModel entity = new ProjectModel
                {
                    AmountSold = ParseHelper.ToDecimal(dr["amount_sold"]),
                    AvailableAmount = ParseHelper.ToDecimal(dr["amount_able"]),
                    Amount_able_fz = ParseHelper.ToDecimal(dr["amount_able_fz"]),

                };
                dr.Close();
                dr.Dispose();
                return entity;
            }
            return new ProjectModel();
        }
        #endregion

        #region 根据项目ID获得项目的权值和审核状态
        /// <summary>
        /// 根据项目ID获得项目的权值和审核状态
        /// </summary>
        /// <param name="PID">项目ID</param>
        /// <returns></returns>
        public ProjectModel GetProjectModelWeightOrvaidate(int PID)
        {
            string sql = @"SELECT    weight,pjt_status                      
                            FROM       pjt  where id=?PID";
            MySqlParameter[] parms;
            parms = new MySqlParameter[]
            { 
                new MySqlParameter("?PID",MySqlDbType.Int64)
            };
            parms[0].Value = PID;
            var dr = DataBaseOperator.MoneyReadDbHelper.ExecuteReader(sql, CommandType.Text, parms);
            if (dr.Read())
            {

                ProjectModel entity = new ProjectModel
                {
                    Weight = ParseHelper.ToInt(dr["weight"]),
                    ProjectStatus = ParseHelper.ToInt(dr["pjt_status"]),

                };
                dr.Close();
                dr.Dispose();
                return entity;
            }
            return new ProjectModel();
        }
        #endregion
    }
}
