using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Collections.Generic;

using YxLiCai.DataHelper;
using YxLiCai.Model.Product;
using MySql.Data.MySqlClient;
using YxLiCai.Model.UserRaise;
using YxLiCai.Tools.Util;

namespace YxLiCai.Dao.Product
{
    /// <summary>
    /// 产品管理 数据处理类 
    /// add by Lxm 
    /// </summary>
    public class ProductDao
    {
        #region Insert
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(YxLiCai.Model.Product.ProductModel model)
        {
            var sqlArray = new List<string>();
            var paramArray = new List<DbParameter[]>();
            var strSql = new StringBuilder();
            // 1) insert into ProductInfo
            strSql.Append("insert into pdt_info(");
            strSql.Append(@"name,category,creator_id,yield_base,yield_increase,yield_top,c_time,pdt_order,isauto_enable,
                            expect_time,amount,duration)");
            strSql.Append(" values (");
            strSql.Append("?ProductName,?ProductCategory,?CreatorID,?YieldBase,?YieldIncrease,?YieldTop,?CreateDate,?ProductOrder,?IsAutoEnable,?ExpectedEnableDate,?ProductAmount,?ProductDuration);");
            // 2) insert into ProductCount
            strSql.Append("insert into pdt_count(");
            strSql.Append("pdt_id,available_amount)");
            strSql.Append(" values (");
            strSql.Append("last_insert_id(),?ProductAmount);");
            // 3) insert into pdt_pjt
            foreach (var projectId in model.ProjectList)
            {
                strSql.Append("insert into pdt_pjt(");
                strSql.Append("pdt_id,pjt_id,a_date)");
                strSql.Append(" values (");
                strSql.Append("last_insert_id()," + projectId + ",?CreateDate);");
            }
            sqlArray.Add(strSql.ToString());

            MySqlParameter[] parameters = {
					new MySqlParameter("?ProductName", MySqlDbType.VarChar,200),
					new MySqlParameter("?ProductCategory", MySqlDbType.Int32,11),
					new MySqlParameter("?CreatorID", MySqlDbType.Int32,11),
					new MySqlParameter("?YieldBase", MySqlDbType.Decimal,10),
					new MySqlParameter("?YieldIncrease", MySqlDbType.Decimal,10),
					new MySqlParameter("?YieldTop", MySqlDbType.Decimal,10),
					new MySqlParameter("?CreateDate", MySqlDbType.DateTime),
					new MySqlParameter("?ProductOrder", MySqlDbType.Int32,11),
					new MySqlParameter("?IsAutoEnable", MySqlDbType.Bit),
					new MySqlParameter("?ExpectedEnableDate", MySqlDbType.DateTime),
					new MySqlParameter("?ProductAmount", MySqlDbType.Decimal,18),
                    new MySqlParameter("?ProductDuration", MySqlDbType.Int32,4)};
            parameters[0].Value = model.ProductName;
            parameters[1].Value = model.ProductCategory;
            parameters[2].Value = model.CreatorID;
            parameters[3].Value = model.YieldBase;
            parameters[4].Value = model.YieldIncrease;
            parameters[5].Value = model.YieldTop;
            parameters[6].Value = DateTime.Now;
            parameters[7].Value = model.ProductOrder;
            parameters[8].Value = model.IsAutoEnable;
            parameters[9].Value = model.ExpectedEnableDate;
            parameters[10].Value = model.ProductAmount;
            parameters[11].Value = model.ProductDuration;
            //parameter array
            paramArray.Add(parameters);

            return DataBaseOperator.MoneyWriteDbHelper.ExecutMultiSQLwithTransaction(sqlArray, paramArray);
        }
        #endregion

        #region Update
        /// <summary>
        /// 更新产品信息 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Update(YxLiCai.Model.Product.ProductModel model)
        {
            var sqlArray = new List<string>();
            var paramArray = new List<DbParameter[]>();
            var strSql = new StringBuilder();
            // 1) Update ProductInfo
            strSql.Append("update pdt_info set ");
            strSql.Append("name=?ProductName,");
            strSql.Append("category=?ProductCategory,");
            strSql.Append("status=?ProductStatus,");
            strSql.Append("yield_base=?YieldBase,");
            strSql.Append("yield_increase=?YieldIncrease,");
            strSql.Append("yield_top=?YieldTop,");
            strSql.Append("audit_time=?AuditDate,");
            strSql.Append("auditor_id=?AuditorID,");
            strSql.Append("pdt_order=?ProductOrder,");
            strSql.Append("isauto_enable=?IsAutoEnable,");
            strSql.Append("amount=?ProductAmount,");
            strSql.Append("remark=?Remark,");
            strSql.Append("duration=?ProductDuration, ");
            strSql.Append("expect_time=?ExpectedEnableDate ");
            strSql.Append(" where Id=?Id ;");
            // 2)Update pdt_pjt
            strSql.Append("DELETE FROM pdt_pjt WHERE pdt_id = " + model.Id + ";");
            foreach (var projectId in model.ProjectList)
            {
                strSql.Append("insert into pdt_pjt(");
                strSql.Append("pdt_id,pjt_id,a_date)");
                strSql.Append(" values (");
                strSql.Append(model.Id + ", " + projectId + ",'" + DateTime.Now + "'); ");
            }
            // 3)Update pdt_count
            strSql.Append(@"UPDATE pdt_count SET available_amount=?ProductAmount 
                            WHERE pdt_id=?Id;");
            //sql array
            sqlArray.Add(strSql.ToString());

            MySqlParameter[] parameters = {
					new MySqlParameter("?ProductName", MySqlDbType.VarChar,200),
					new MySqlParameter("?ProductCategory", MySqlDbType.Int32,11),
					new MySqlParameter("?ProductStatus", MySqlDbType.Int32,11),
					new MySqlParameter("?YieldBase", MySqlDbType.Decimal,10),
					new MySqlParameter("?YieldIncrease", MySqlDbType.Decimal,10),
					new MySqlParameter("?YieldTop", MySqlDbType.Decimal,10),
					new MySqlParameter("?AuditDate", MySqlDbType.DateTime),
					new MySqlParameter("?AuditorID", MySqlDbType.Int32,11),
					new MySqlParameter("?ProductOrder", MySqlDbType.Int32,11),
					new MySqlParameter("?IsAutoEnable", MySqlDbType.Bit),
					new MySqlParameter("?ProductAmount", MySqlDbType.Decimal,20),
					new MySqlParameter("?Remark", MySqlDbType.VarChar,500),     
                    new MySqlParameter("?ProductDuration", MySqlDbType.Int32,11),
                    new MySqlParameter("?ExpectedEnableDate", MySqlDbType.DateTime),
					new MySqlParameter("?Id", MySqlDbType.Int32,11)};
            parameters[0].Value = model.ProductName;
            parameters[1].Value = model.ProductCategory;
            parameters[2].Value = model.ProductStatus;
            parameters[3].Value = model.YieldBase;
            parameters[4].Value = model.YieldIncrease;
            parameters[5].Value = model.YieldTop;
            parameters[6].Value = model.AuditDate;
            parameters[7].Value = model.AuditorID;
            parameters[8].Value = model.ProductOrder;
            parameters[9].Value = model.IsAutoEnable;
            parameters[10].Value = model.ProductAmount;
            parameters[11].Value = model.Remark;
            parameters[12].Value = model.ProductDuration;
            parameters[13].Value = model.ExpectedEnableDate;
            parameters[14].Value = model.Id;
            //parameter array
            paramArray.Add(parameters);

            return DataBaseOperator.MoneyWriteDbHelper.ExecutMultiSQLwithTransaction(sqlArray, paramArray);
        }
        #endregion

        #region Select
        /// <summary>
        /// 获取单条记录
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public DataSet GetModel(int Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT id AS Id,name AS ProductName,category AS ProductCategory,status AS ProductStatus,creator_id AS CreatorID,yield_base AS YieldBase,
                                yield_increase AS YieldIncrease,yield_top AS YieldTop,c_time AS CreateDate,audit_time AS AuditDate,auditor_id AS AuditorID,
                                pdt_order AS ProductOrder,isauto_enable AS IsAutoEnable,isdelete AS IsDeleted,expect_time AS ExpectedEnableDate,
                                enable_time AS EnableDate,e_time AS EndDate,amount AS ProductAmount,remark AS Remark,duration AS ProductDuration 
                            FROM pdt_info ");
            strSql.Append(" where id=?Id ;");
            strSql.Append("SELECT pdt_id,pjt_id,a_date FROM pdt_pjt ");
            strSql.Append(" where pdt_id=?Id ; ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?Id", MySqlDbType.Int32,11)			
                                          };
            parameters[0].Value = Id;

            return DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql.ToString(), "根据id获取产品信息", parameters);
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public YxLiCai.Model.Product.ProductModel DataRowToModel(DataRow row)
        {
            var model = new YxLiCai.Model.Product.ProductModel();
            model.ProductCount = new Model.Product.ProductCount();

            #region match
            if (row != null)
            {
                if (row["Id"] != null && row.Table.Columns.Contains("Id"))
                {
                    model.Id = int.Parse(row["Id"].ToString());
                }
                if (row.Table.Columns.Contains("ProductName") && row["ProductName"] != null)
                {
                    model.ProductName = row["ProductName"].ToString();
                }
                if (row.Table.Columns.Contains("ProductCategory") && row["ProductCategory"] != null)
                {
                    model.ProductCategory = int.Parse(row["ProductCategory"].ToString());
                    switch (model.ProductCategory)
                    {
                        case 1: model.PCategory = "月月盈"; break;
                        case 2: model.PCategory = "季季享3个月"; break;
                        case 3: model.PCategory = "季季享6个月"; break;
                        case 4: model.PCategory = "年年丰"; break;
                        default: model.PCategory = "未定义"; break;
                    }
                }
                if (row.Table.Columns.Contains("ProductStatus") && row["ProductStatus"] != null && row["ProductStatus"].ToString() != "")
                {
                    model.ProductStatus = int.Parse(row["ProductStatus"].ToString());
                    switch (model.ProductStatus)
                    {
                        case 0: model.PStatus = "待审核"; break;
                        case 1: model.PStatus = "待发布"; break;
                        case 2: model.PStatus = "审核未通过"; break;
                        case 3: model.PStatus = "售卖中"; break;
                        case 4: model.PStatus = "已售罄"; break;
                        default: model.PStatus = "未定义"; break;
                    }
                }
                if (row.Table.Columns.Contains("CreatorID") && row["CreatorID"] != null && row["CreatorID"].ToString() != "")
                {
                    model.CreatorID = int.Parse(row["CreatorID"].ToString());
                }
                if (row.Table.Columns.Contains("YieldBase") && row["YieldBase"] != null && row["YieldBase"].ToString() != "")
                {
                    model.YieldBase = decimal.Parse(row["YieldBase"].ToString());
                }
                if (row.Table.Columns.Contains("YieldIncrease") && row["YieldIncrease"] != null && row["YieldIncrease"].ToString() != "")
                {
                    model.YieldIncrease = decimal.Parse(row["YieldIncrease"].ToString());
                }
                if (row.Table.Columns.Contains("YieldTop") && row["YieldTop"] != null && row["YieldTop"].ToString() != "")
                {
                    model.YieldTop = decimal.Parse(row["YieldTop"].ToString());
                }
                if (row.Table.Columns.Contains("CreateDate") && row["CreateDate"] != null && row["CreateDate"].ToString() != "")
                {
                    model.CreateDate = DateTime.Parse(row["CreateDate"].ToString());
                }
                if (row.Table.Columns.Contains("AuditDate") && row["AuditDate"] != null && row["AuditDate"].ToString() != "")
                {
                    model.AuditDate = DateTime.Parse(row["AuditDate"].ToString());
                }
                if (row.Table.Columns.Contains("AuditorID") && row["AuditorID"] != null && row["AuditorID"].ToString() != "")
                {
                    model.AuditorID = int.Parse(row["AuditorID"].ToString());
                }
                if (row.Table.Columns.Contains("ProductOrder") && row["ProductOrder"] != null && row["ProductOrder"].ToString() != "")
                {
                    model.ProductOrder = int.Parse(row["ProductOrder"].ToString());
                }
                if (row.Table.Columns.Contains("IsAutoEnable") && row["IsAutoEnable"] != null && row["IsAutoEnable"].ToString() != "")
                {
                    model.IsAutoEnable = Convert.ToInt32(row["IsAutoEnable"]);
                    model.AutoEnable = model.IsAutoEnable > 0 ? "是" : "否";
                }
                if (row.Table.Columns.Contains("IsDeleted") && row["IsDeleted"] != null && row["IsDeleted"].ToString() != "")
                {
                    if ((row["IsDeleted"].ToString() == "1") || (row["IsDeleted"].ToString().ToLower() == "true"))
                    {
                        model.IsDeleted = true;
                    }
                    else
                    {
                        model.IsDeleted = false;
                    }
                }
                if (row.Table.Columns.Contains("ExpectedEnableDate") && row["ExpectedEnableDate"] != null && row["ExpectedEnableDate"].ToString() != "")
                {
                    model.ExpectedEnableDate = DateTime.Parse(row["ExpectedEnableDate"].ToString());
                }
                if (row.Table.Columns.Contains("EnableDate") && row["EnableDate"] != null && row["EnableDate"].ToString() != "")
                {
                    model.EnableDate = DateTime.Parse(row["EnableDate"].ToString());
                }
                if (row.Table.Columns.Contains("EndDate") && row["EndDate"] != null && row["EndDate"].ToString() != "")
                {
                    model.EndDate = DateTime.Parse(row["EndDate"].ToString());
                }
                if (row.Table.Columns.Contains("ProductAmount") && row["ProductAmount"] != null && row["ProductAmount"].ToString() != "")
                {
                    model.ProductAmount = Math.Round(Convert.ToDecimal(decimal.Parse(row["ProductAmount"].ToString()) / 10000), 6);
                }
                if (row.Table.Columns.Contains("Remark") && row["Remark"] != null)
                {
                    model.Remark = row["Remark"].ToString();
                }
                if (row.Table.Columns.Contains("ProductDuration") && row["ProductDuration"] != null)
                {
                    model.ProductDuration = Convert.ToInt32(row["ProductDuration"]);
                }
                if (row.Table.Columns.Contains("available_amount") && !row.IsNull("available_amount"))
                {
                    model.ProductCount.AvailableAmount = Convert.ToDecimal(row["available_amount"]);
                }
                if (row.Table.Columns.Contains("PurchasedMemberSum") && !row.IsNull("PurchasedMemberSum"))
                {
                    model.ProductCount.PurchasedMemberSum = row["PurchasedMemberSum"].ToString();
                }
                if (row.Table.Columns.Contains("RaisedAmount") && row["RaisedAmount"] != null)
                {
                    model.ProductCount.RaisedAmount = Convert.ToDecimal(row["RaisedAmount"]);
                }
                if (row.Table.Columns.Contains("is_alert") && row["is_alert"] != null)
                {
                    model.IsAlert = row["is_alert"].ToString();
                }
            }
            #endregion

            return model;
        }
        /// <summary>
        /// 查询产品列表
        /// </summary>
        /// <param name="ProductName">名称</param>
        /// <param name="ProductCategory">类型</param>
        /// <param name="ProductStatus">状态</param>
        /// <param name="StartDate">预期日期</param>
        /// <param name="EndDate">预期日期</param>
        /// <param name="take">查询记录数</param>
        /// <param name="skip">起始记录</param>
        public DataSet GetList(string ProductName, int ProductCategory, int ProductStatus,
            DateTime? StartDate, DateTime? EndDate, int take, int skip)
        {
            ProductName = "%" + ProductName.Trim() + "%";
            var strSql = new StringBuilder();
            strSql.Append(@"SELECT 
                                id AS Id,NAME AS ProductName,category AS ProductCategory,STATUS AS ProductStatus,creator_id AS CreatorID,yield_base AS YieldBase,
                                yield_increase AS YieldIncrease,yield_top AS YieldTop,c_time AS CreateDate,audit_time AS AuditDate,auditor_id AS AuditorID,
                                pdt_order AS ProductOrder,isauto_enable AS IsAutoEnable,isdelete AS IsDeleted,expect_time AS ExpectedEnableDate,
                                enable_time AS EnableDate,e_time AS EndDate,amount AS ProductAmount,remark AS Remark,duration AS ProductDuration,
                                B.available_amount,B.buy_sum,B.raised_amount  AS RaisedAmount 
                            FROM pdt_info A INNER JOIN pdt_count B ON A.id = B.pdt_id
                            WHERE A.`isdelete`=0 
                            AND (?PName='%%' OR A.`name` LIKE ?PName)
                            AND (?PCategory=-1 OR A.`category`=?PCategory) 
                            AND (?PStatus=-1 OR A.`status`=?PStatus) 
                            AND (?SDate IS NULL OR A.`expect_time` >= ?SDate) AND (?EDate IS NULL OR A.`expect_time` <= ?EDate)
                            ORDER BY A.`c_time` DESC
                            LIMIT ?StartIndex,?PageSize; 

                            SELECT COUNT(1)  
                            FROM pdt_info A INNER JOIN pdt_count B ON A.id = B.pdt_id
                            WHERE A.`isdelete`=0 
                            AND (?PName='%%' OR A.`name` LIKE ?PName)
                            AND (?PCategory=-1 OR A.`category`=?PCategory) 
                            AND (?PStatus=-1 OR A.`status`=?PStatus) 
                            AND (?SDate IS NULL OR A.`expect_time` >= ?SDate) AND (?EDate IS NULL OR A.`expect_time` <= ?EDate);");
            MySqlParameter[] parameters = {
					new MySqlParameter("?PName", MySqlDbType.VarChar),
			        new MySqlParameter("?PCategory", MySqlDbType.Int16),
                    new MySqlParameter("?PStatus", MySqlDbType.Int16),
                    new MySqlParameter("?SDate", MySqlDbType.DateTime),
                    new MySqlParameter("?EDate", MySqlDbType.DateTime),
                    new MySqlParameter("?StartIndex", MySqlDbType.Int32),
                    new MySqlParameter("?PageSize", MySqlDbType.Int32)};
            parameters[0].Value = ProductName;
            parameters[1].Value = ProductCategory;
            parameters[2].Value = ProductStatus;
            parameters[3].Value = StartDate;
            parameters[4].Value = EndDate;
            parameters[5].Value = skip;
            parameters[6].Value = take;

            return DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql.ToString(), "查询产品列表", parameters);
        }
        /// <summary>
        /// 获取一条在售商品信息
        /// </summary>
        /// <returns></returns>
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public SallProductModel GetSallProduct(int ProductCategory)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT A.id AS Id,A.isauto_enable AS IsAutoEnable,IFNULL(A.expect_time,'1900-09-22') AS ExpectedEnableDate,A.amount AS ProductAmount,
	                            1 AS PurchasedMemberSum,
	                            B.raised_amount AS RaisedAmount,B.available_amount AS AvailableAmount 
                            FROM pdt_info A INNER JOIN pdt_count B ON A.id = B.pdt_id 
                            WHERE A.isdelete=0 AND A.STATUS=3 AND A.category=?ProductCategory 
                            ORDER BY A.c_time DESC;  ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?ProductCategory", MySqlDbType.Int32,11)			
                                          };
            parameters[0].Value = ProductCategory;

            var dt = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql.ToString(), "获取一条在售商品信息", parameters).Tables[0];
            SallProductModel mod = null;
            if (dt != null && dt.Rows.Count > 0)
            {
                mod = new SallProductModel();
                mod.Id = YxLiCai.Tools.Util.ParseHelper.ToInt(dt.Rows[0]["Id"].ToString());
                mod.IsAutoEnable = YxLiCai.Tools.Util.ParseHelper.ToBool(dt.Rows[0]["IsAutoEnable"].ToString());
                mod.AvailableAmount = YxLiCai.Tools.Util.ParseHelper.ToDecimal(dt.Rows[0]["AvailableAmount"].ToString());
                mod.ExpectedEnableDate = YxLiCai.Tools.Util.ParseHelper.ToDatetime(dt.Rows[0]["ExpectedEnableDate"].ToString());
                mod.ProductAmount = YxLiCai.Tools.Util.ParseHelper.ToDecimal(dt.Rows[0]["ProductAmount"].ToString());
                mod.RaisedAmount = YxLiCai.Tools.Util.ParseHelper.ToDecimal(dt.Rows[0]["RaisedAmount"].ToString());
                mod.PurchasedMemberSum = YxLiCai.Tools.Util.ParseHelper.ToInt(dt.Rows[0]["PurchasedMemberSum"].ToString());
            }
            return mod;
        }

        /// <summary>
        /// 根据商品类型获取累计销售人数 by 张浩然 2015-7-24
        /// </summary>
        /// <param name="ptype">商品类型</param>
        /// <returns></returns>
        public int pdtSellCountByptype(int ptype)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT SUM(buy_sum) as sellCount  from pdt_count INNER JOIN pdt_info on pdt_count.pdt_id=pdt_info.id ");
            strSql.Append(" where pdt_info.category=?ptype ");
            MySqlParameter[] parameters = {					
                    new MySqlParameter("?ptype", MySqlDbType.Int32,4)
            };
            parameters[0].Value = ptype;
            object ob = DataBaseOperator.MoneyReadDbHelper.ExecuteScalar(strSql.ToString(), "根据商品类型获取累计销售人数 by 张浩然 2015-7-24", parameters);
            if (ob != null)
            {
                return ParseHelper.ToInt(ob);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 查询预警产品列表
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataSet GetAlertList(int pCategory)
        {
            var strSql = @"SELECT  id AS Id,NAME AS ProductName,category AS ProductCategory,STATUS AS ProductStatus,
	                            pdt_order AS ProductOrder,enable_time AS EnableDate,
	                            amount AS ProductAmount,duration AS ProductDuration,
	                            B.available_amount AS AvailableAmount
                            FROM pdt_info A INNER JOIN pdt_count B
                            ON A.id = B.pdt_id 
                            WHERE  STATUS=3 AND B.available_amount <= 100 AND isdelete=0 
                            AND category=" + pCategory;

            return DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql, "查询预警产品列表");
        }
        /// <summary>
        /// 查询项目名称是否已存在
        /// </summary>
        /// <param name="pName"></param>
        /// <returns></returns>
        public object IsProducNameExisted(string pName)
        {
            string strSql = "SELECT 1 FROM pdt_info WHERE name=?Name";
            MySqlParameter[] parameters = {
					 new MySqlParameter("?Name", MySqlDbType.VarChar, 200)	
            };
            parameters[0].Value = pName;

            return DataBaseOperator.MoneyReadDbHelper.ExecuteScalar(strSql, "查询项目名称是否已存在", parameters);
        }
        /// <summary>
        /// 查询售卖中的产品列表
        /// </summary>
        /// <returns></returns>
        public DataSet GetOnlineList()
        {
            string strSql = @"
                            SELECT 
	                            A.id AS Id,A.name AS ProductName,A.category AS ProductCategory,
	                            A.c_time AS CreateDate,A.enable_time AS EnableDate,A.e_time AS EndDate,
	                            A.amount AS ProductAmount,B.available_amount,B.buy_sum,B.raised_amount AS RaisedAmount,
	                            CASE WHEN B.available_amount <= 100 AND SUM(D.amount_able) <= 100 THEN 1 ELSE 0 END AS is_alert
                            FROM pdt_info A
                            INNER JOIN pdt_count B ON A.id = B.pdt_id
                            INNER JOIN pdt_pjt C ON A.id = C.pdt_id
                            INNER JOIN pjt D ON C.pjt_id = D.id 
                            WHERE A.`isdelete`=0 
                            AND STATUS=3 group by Id ";
            return DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql, "查询售卖中的产品列表");
        }
        #endregion

        #region Delete
        public bool Delete(string IdList)
        {
            string strSql = " Update pdt_info Set isdelete = 1 Where Id IN (" + IdList + ")";

            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql, System.Data.CommandType.Text);
        }
        #endregion

        #region 前台相关操作
        /// <summary>
        ///获取产品可投金额
        /// </summary>
        public decimal ProductAvailableAmount(int pid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT available_amount AS AvailableAmount FROM pdt_count WHERE pdt_id=?Id ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?Id", MySqlDbType.Int32,11)			
                                          };
            parameters[0].Value = pid;
            object obj = DataBaseOperator.MoneyReadDbHelper.ExecuteScalar(strSql.ToString(), "获取产品可投金额-平扬-20150529", parameters);
            if (obj == null)
            {
                return 0m;
            }
            else
            {
                return Convert.ToDecimal(obj);
            }
        }
        /// <summary>
        ///获取产品利率
        /// </summary>
        public decimal ProductRate(int pid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT yield_base AS YieldBase FROM pdt_info WHERE id=?Id ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?Id", MySqlDbType.Int32,11)			
                                          };
            parameters[0].Value = pid;
            object obj = DataBaseOperator.MoneyReadDbHelper.ExecuteScalar(strSql.ToString(), "获取产品利率-平扬-20150529", parameters);
            if (obj == null)
            {
                return 0m;
            }
            else
            {
                return Convert.ToDecimal(obj);
            }
        }
        /// <summary>
        /// 判断产品是否已经完成，下架
        /// </summary>
        public bool ProductHasDone(int pid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT 1 FROM pdt_info WHERE id=?Id AND status=4 ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?Id", MySqlDbType.Int32,11)			
                                          };
            parameters[0].Value = pid;
            object obj = DataBaseOperator.MoneyReadDbHelper.ExecuteScalar(strSql.ToString(), "判断产品是否已经完成，下架-平扬-20150529", parameters);
            if (obj == null)
            {
                return false;
            }
            else
            {
                return Convert.ToInt32(obj) > 0;
            }
        }
        /// <summary>
        /// 插入用户投资产品记录表
        /// </summary>
        public bool UserRaise_ProductAdd(UserRaiseProductModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into user_invest_rec(");
            strSql.Append("c_time,user_id,e_time,purchase_amount,purchase_amount_balance,purchase_amount_bank,status,op_status,interest_rate,interest_rate_add,pdt_id,pdt_type,interest_rate_coupons,third_ord_id,records_summary_id,ord_id,version,remark)");
            strSql.Append(" values (");
            strSql.Append("?c_time,?user_id,?e_time,?purchase_amount,?purchase_amount_balance,?purchase_amount_bank,?status,?op_status,?interest_rate,?interest_rate_add,?pdt_id,?pdt_type,?interest_rate_coupons,?third_ord_id,?records_summary_id,?ord_id,?version,?remark)");
            MySqlParameter[] parameters = {
					new MySqlParameter("?c_time", MySqlDbType.DateTime),
					new MySqlParameter("?user_id", MySqlDbType.Int32,11),
                    new MySqlParameter("?e_time", MySqlDbType.DateTime),
					new MySqlParameter("?purchase_amount", MySqlDbType.Decimal,20),
					new MySqlParameter("?purchase_amount_balance", MySqlDbType.Decimal,20),
					new MySqlParameter("?purchase_amount_bank", MySqlDbType.Decimal,20),
					new MySqlParameter("?status", MySqlDbType.Int16,3),
                    new MySqlParameter("?op_status", MySqlDbType.Int16,3),
					new MySqlParameter("?interest_rate", MySqlDbType.Decimal,20),
					new MySqlParameter("?interest_rate_add", MySqlDbType.Decimal,20),
					new MySqlParameter("?pdt_id", MySqlDbType.Int32,11),
					new MySqlParameter("?pdt_type", MySqlDbType.Int16,4),
					new MySqlParameter("?interest_rate_coupons", MySqlDbType.VarChar,50),
					new MySqlParameter("?third_ord_id", MySqlDbType.VarChar,100),
					new MySqlParameter("?records_summary_id", MySqlDbType.Int32,11),
					new MySqlParameter("?ord_id", MySqlDbType.Int32,11),
					new MySqlParameter("?version", MySqlDbType.Int32,11),
					new MySqlParameter("?remark", MySqlDbType.VarChar,500)};
            parameters[0].Value = model.create_time;
            parameters[1].Value = model.user_id;
            parameters[2].Value = model.e_time;
            parameters[3].Value = model.purchase_amount;
            parameters[4].Value = model.purchase_amount_balance;
            parameters[5].Value = model.purchase_amount_bank;
            parameters[6].Value = model.status;
            parameters[7].Value = model.op_status;
            parameters[8].Value = model.interest_rate;
            parameters[9].Value = model.interest_rate_added;
            parameters[10].Value = model.product_id;
            parameters[11].Value = model.product_type;
            parameters[12].Value = model.interest_rate_coupons;
            parameters[13].Value = model.third_order_id;
            parameters[14].Value = model.records_summary_id;
            parameters[15].Value = 0;
            parameters[16].Value = model.version;
            parameters[17].Value = model.remark;
            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql.ToString(), "增加用户投资产品记录-平扬-20150529", parameters);

        }
        /// <summary>
        /// 更新产品可投金额，已投金额等
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="money"></param>
        /// <returns></returns>
        public bool UpdateProductBalance(int pid, decimal money,int buysum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"Update pdt_count Set raised_amount=raised_amount+?RaisedAmount,available_amount=available_amount-?RaisedAmount,
                            buy_sum=buy_sum+?buysum WHERE pdt_id=?Id");
            MySqlParameter[] parameters = {
					new MySqlParameter("?Id", MySqlDbType.Int32,11),
                    new MySqlParameter("?RaisedAmount", MySqlDbType.Decimal,20),
					new MySqlParameter("?buysum",MySqlDbType.Int32,4)
                                          };
            parameters[0].Value = pid;
            parameters[1].Value = money;
            parameters[2].Value = buysum;
            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql.ToString(), "更新产品可投金额，已投金额等，-平扬-20150529", parameters);

        }
        /// <summary>
        /// 更新产品下架
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="money"></param>
        /// <returns></returns>
        public bool UpdateProductStatus(int pid)
        {
            StringBuilder strSql = new StringBuilder();
            //更新当前在售产品状态为 售罄
            strSql.Append(@"UPDATE pdt_info SET status = 4,e_time=NOW() WHERE ID = ?id ; ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?Id", MySqlDbType.Int32,11)		
                                          };
            parameters[0].Value = pid;

            if (DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql.ToString(),
                "产品下架，-平扬-20150529", parameters))
            {
                //上架新产品
                return UpProduct(pid);
            }

            return false;
        }
        /// <summary>
        /// 获取列表记录
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public List<YxLiCai.Model.Product.ProductInfo> GetListByIDS(List<int> Ids)
        {
            string IDstr = "";
            foreach (int item in Ids)
            {
                IDstr = IDstr + (IDstr == "" ? "" : ",") + item.ToString();
            }
            if (IDstr == "")
            {
                return null;
            }
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT id AS Id,NAME AS ProductName,category AS ProductCategory,STATUS AS ProductStatus,creator_id AS CreatorID,yield_base AS YieldBase,
                            yield_increase AS YieldIncrease,yield_top AS YieldTop,c_time AS CreateDate,audit_time AS AuditDate,auditor_id AS AuditorID,
                            pdt_order AS ProductOrder,isauto_enable AS IsAutoEnable,isdelete AS IsDeleted,expect_time AS ExpectedEnableDate,
                            enable_time AS EnableDate,e_time AS EndDate,amount AS ProductAmount,remark AS Remark,duration AS ProductDuration 
                             FROM  pdt_info ");
            strSql.Append(" where Id in (" + IDstr + ");");

            DataSet ds = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql.ToString(), CommandType.Text, null);

            List<YxLiCai.Model.Product.ProductInfo> list = new List<Model.Product.ProductInfo>();
            YxLiCai.Model.Product.ProductInfo pi;
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        pi = new Model.Product.ProductInfo();
                        pi.Id = Convert.ToInt32(dr["Id"]);
                        pi.ProductName = Convert.ToString(dr["ProductName"]);
                        pi.ProductCategory = Convert.ToInt32(dr["ProductCategory"]);
                        pi.ProductStatus = Convert.ToInt32(dr["ProductStatus"]);
                        pi.CreatorID = Convert.ToInt32(dr["CreatorID"]);
                        pi.YieldBase = Convert.ToInt32(dr["YieldBase"]);
                        pi.YieldIncrease = Convert.ToInt32(dr["YieldIncrease"]);
                        pi.YieldTop = Convert.ToInt32(dr["YieldTop"]);
                        pi.CreateDate = Convert.ToDateTime(dr["CreateDate"] == DBNull.Value ? "1900-01-01" : dr["CreateDate"]);
                        pi.AuditDate = Convert.ToDateTime(dr["AuditDate"] == DBNull.Value ? "1900-01-01" : dr["AuditDate"]);
                        pi.AuditorID = Convert.ToInt32(dr["AuditorID"] == DBNull.Value ? 0 : dr["AuditorID"]);
                        pi.ProductOrder = Convert.ToInt32(dr["ProductOrder"] == DBNull.Value ? 0 : dr["ProductOrder"]);
                        pi.IsAutoEnable = Convert.ToInt32(dr["IsAutoEnable"] == DBNull.Value ? 0 : dr["IsAutoEnable"]);
                        pi.IsDeleted = Convert.ToBoolean(dr["IsDeleted"] == DBNull.Value ? 0 : dr["IsDeleted"]);
                        pi.ExpectedEnableDate = Convert.ToDateTime((dr["ExpectedEnableDate"] == DBNull.Value || dr["ExpectedEnableDate"].ToString().Trim() == "") ? DateTime.Now.AddDays(1) : dr["ExpectedEnableDate"]);
                        pi.EnableDate = Convert.ToDateTime(dr["EnableDate"] == DBNull.Value ? "1900-01-01" : dr["EnableDate"]);
                        pi.EndDate = Convert.ToDateTime(dr["EndDate"] == DBNull.Value ? "1900-01-01" : dr["EndDate"]);
                        pi.ProductAmount = Convert.ToInt32(dr["ProductAmount"] == DBNull.Value ? 0 : dr["ProductAmount"]);
                        pi.Remark = Convert.ToString(dr["Remark"]);
                        pi.ProductDuration = Convert.ToInt32(dr["ProductDuration"] == DBNull.Value ? 0 : dr["ProductDuration"]);

                        list.Add(pi);
                    }
                }
            }
            return list;

        }
        /// <summary>
        /// 获取列表记录(所有待上架的)----根据产品类型
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public List<YxLiCai.Model.Product.ProductInfo> GetListByPType(int producttype, int status = 1)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT id AS Id,NAME AS ProductName,category AS ProductCategory,STATUS AS ProductStatus,creator_id AS CreatorID,yield_base AS YieldBase,
                                yield_increase AS YieldIncrease,yield_top AS YieldTop,c_time AS CreateDate,audit_time AS AuditDate,auditor_id AS AuditorID,
                                pdt_order AS ProductOrder,isauto_enable AS IsAutoEnable,isdelete AS IsDeleted,expect_time AS ExpectedEnableDate,
                                enable_time AS EnableDate,e_time AS EndDate,amount AS ProductAmount,remark AS Remark,duration AS ProductDuration 
                            FROM pdt_info
                            WHERE category=?producttype AND STATUS=?status ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?producttype", MySqlDbType.Int32,11),
	                new MySqlParameter("?status", MySqlDbType.Int16)};
            parameters[0].Value = producttype;
            parameters[1].Value = status;

            DataSet ds = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql.ToString(), CommandType.Text, parameters);

            List<YxLiCai.Model.Product.ProductInfo> list = new List<Model.Product.ProductInfo>();
            YxLiCai.Model.Product.ProductInfo pi;
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        pi = new Model.Product.ProductInfo();
                        pi.Id = Convert.ToInt32(dr["Id"]);
                        pi.ProductName = Convert.ToString(dr["ProductName"]);
                        pi.ProductCategory = Convert.ToInt32(dr["ProductCategory"]);
                        pi.ProductStatus = Convert.ToInt32(dr["ProductStatus"]);
                        pi.CreatorID = Convert.ToInt32(dr["CreatorID"]);
                        pi.YieldBase = Convert.ToInt32(dr["YieldBase"]);
                        pi.YieldIncrease = Convert.ToInt32(dr["YieldIncrease"]);
                        pi.YieldTop = Convert.ToInt32(dr["YieldTop"]);
                        pi.CreateDate = Convert.ToDateTime(dr["CreateDate"] == DBNull.Value ? "1900-01-01" : dr["CreateDate"]);
                        pi.AuditDate = Convert.ToDateTime(dr["AuditDate"] == DBNull.Value ? "1900-01-01" : dr["AuditDate"]);
                        pi.AuditorID = Convert.ToInt32(dr["AuditorID"] == DBNull.Value ? 0 : dr["AuditorID"]);
                        pi.ProductOrder = Convert.ToInt32(dr["ProductOrder"] == DBNull.Value ? 0 : dr["ProductOrder"]);
                        pi.IsAutoEnable = Convert.ToInt32(dr["IsAutoEnable"] == DBNull.Value ? 0 : dr["IsAutoEnable"]);
                        pi.IsDeleted = Convert.ToBoolean(dr["IsDeleted"] == DBNull.Value ? 0 : dr["IsDeleted"]);
                        pi.ExpectedEnableDate = Convert.ToDateTime((dr["ExpectedEnableDate"] == DBNull.Value || dr["ExpectedEnableDate"].ToString().Trim() == "") ? DateTime.Now.AddDays(1) : dr["ExpectedEnableDate"]);
                        pi.EnableDate = Convert.ToDateTime(dr["EnableDate"] == DBNull.Value ? "1900-01-01" : dr["EnableDate"]);
                        pi.EndDate = Convert.ToDateTime(dr["EndDate"] == DBNull.Value ? "1900-01-01" : dr["EndDate"]);
                        pi.ProductAmount = Convert.ToDecimal(dr["ProductAmount"] == DBNull.Value ? 0 : dr["ProductAmount"]);
                        pi.Remark = Convert.ToString(dr["Remark"]);
                        pi.ProductDuration = Convert.ToInt32(dr["ProductDuration"] == DBNull.Value ? 0 : dr["ProductDuration"]);


                        list.Add(pi);
                    }
                }
            }
            return list;

        }
        /// <summary>
        /// //上架产品
        /// </summary>
        /// <param name="PID">产品id</param>
        /// <param name="status">3 产品上架</param>
        /// <returns></returns>
        public bool UpProductStatusByID(int PID, int status)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE pdt_info SET STATUS=?Status,enable_time=?EnableTime WHERE id=?PID");
            MySqlParameter[] parameters = {
					new MySqlParameter("?PID", MySqlDbType.Int32,11),
                    new MySqlParameter("?EnableTime", MySqlDbType.DateTime),
                    new MySqlParameter("?Status", MySqlDbType.Int16)};
            parameters[0].Value = PID;
            parameters[1].Value = DateTime.Now;
            parameters[2].Value = status;

            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql.ToString(), "上架新产品", parameters);
        }
        /// <summary>
        /// 上架产品----根据下架产品ID(假设此产品已经下架或者即将下架）
        /// </summary>
        /// <param name="ProductID">产品id</param>
        /// <returns></returns>
        public bool UpProduct(int ProductID)
        {
            try
            {
                List<int> pids = new List<int>();
                pids.Add(ProductID);
                int producttype = 0;
                List<ProductInfo> list = GetListByIDS(pids);

                int Up_PID = 0;
                if (list != null && list.Count > 0)
                {
                    //获取下架产品的类型
                    producttype = list[0].ProductCategory;

                    //判断当前产品类型是否存在 状态为 “售卖中”的产品
                    var list_online = GetListByPType(producttype, 3);
                    if (list_online != null && list_online.Count > 0)
                        return false;

                    //根据产品类型获取所有 产品列表
                    List<ProductInfo> list_ByPType = GetListByPType(producttype);

                    List<ProductInfo> list_temp;


                    //查找预期上架时间在今天之前的
                    list_temp = list_ByPType.Where(a => (a.ExpectedEnableDate < DateTime.Now && a.ProductStatus == 1 && a.IsDeleted == false)).ToList();

                    //有 预期上架时间 在现在之前的 上架 此产品
                    if (list_temp != null && list_temp.Count > 0)
                    {
                        list_temp = list_temp.OrderBy(a => a.ExpectedEnableDate).ToList();
                        Up_PID = list_temp[0].Id;
                    }
                    //没有预期上架时间 在现在之前的
                    else
                    {
                        //查找所有已设置自动上架的
                        list_temp = list_ByPType.Where(a => (a.IsAutoEnable == 1 && a.ProductStatus == 1 && a.IsDeleted == false)).ToList();
                        if (list_temp != null && list_temp.Count > 0)
                        {
                            list_temp = list_temp.OrderBy(a => a.ProductOrder).ToList();
                            Up_PID = list_temp[0].Id;
                        }
                        else
                        {
                            //查找预期上线时间最早的，改为上架状态
                            list_temp = list_ByPType.Where(a => (a.ProductStatus == 1 && a.IsDeleted == false)).ToList();
                            if (list_temp != null && list_temp.Count > 0)
                            {
                                list_temp = list_temp.OrderBy(a => a.ExpectedEnableDate).ToList();
                                Up_PID = list_temp[0].Id;
                            }
                        }
                    }
                    //上架 产品
                    if (Up_PID != 0)
                    {
                        return UpProductStatusByID(Up_PID, 3);
                    }
                }
                return true;

            }
            catch { return false; }
        }

        #endregion

        /// <summary>
        /// 根据类型判断是否存在售卖中的商品 by张浩然 2015-7-17
        /// </summary>
        /// <param name="productType">产品类型</param>
        /// <returns></returns>
        public int IsSallProduct(int productType)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select COUNT(id) from pdt_info ");
            strSql.Append(" where category=?productType and `status`=3 and isdelete=0 ");
            MySqlParameter[] parameters = {					
                    new MySqlParameter("?productType", MySqlDbType.Int32,4)
            };
            parameters[0].Value = productType;
            object ob = DataBaseOperator.MoneyReadDbHelper.ExecuteScalar(strSql.ToString(), "根据类型判断是否存在售卖中的商品 by张浩然 2015-7-17", parameters);
            if (ob != null)
            {
                return ParseHelper.ToInt(ob);
            }
            else
            {
                return 0;
            }


        }
    }
}
