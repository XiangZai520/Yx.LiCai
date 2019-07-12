using System;
using System.Data;
using MySql.Data.MySqlClient;
using YxLiCai.DataHelper;
using System.Collections.Generic;
using System.Data.Common;

namespace YxLiCai.Dao.ActivityManege
{
    /// <summary>
    /// 活动管理数据层
    /// </summary>
    public class ActivityManegeDao
    {
        #region 加息券
        /// <summary>
        /// 新增活动加息券
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="amount">面值</param>
        /// <param name="enableday">有效期</param>
        /// <param name="usecondition">使用条件</param>
        /// <param name="creator_id">新增人id</param>
        /// <param name="c_time">新增时间</param>
        /// <returns></returns>
        public bool InsertActInterestCoupon(string name, decimal amount, int enableday, string usecondition, int creator_id, DateTime c_time)
        {
            string strSql = @"INSERT INTO act_interest_coupon(NAME,interest_rate,enable_day,use_condition,c_time,creator_id)
VALUES(?name,?amount,?enableday,?usecondition,?c_time,?creator_id);";
            MySqlParameter[] parameters = {
					new MySqlParameter("?name", MySqlDbType.VarChar,20),
                    new MySqlParameter("?amount", MySqlDbType.Decimal),
                    new MySqlParameter("?enableday", MySqlDbType.Int32),
                    new MySqlParameter("?usecondition", MySqlDbType.VarChar,100),
                    new MySqlParameter("?creator_id", MySqlDbType.Int32),
                    new MySqlParameter("?c_time", MySqlDbType.DateTime)
			};
            parameters[0].Value = name;
            parameters[1].Value = amount;
            parameters[2].Value = enableday;
            parameters[3].Value = usecondition;
            parameters[4].Value = creator_id;
            parameters[5].Value = c_time;
            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql, "新增活动加息券", parameters);
        }

        /// <summary>
        /// 获取加息券列表
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="amount">面值</param>
        /// <param name="s_ctime">创建开始时间</param>
        /// <param name="e_ctime">创建结束时间</param>
        /// <param name="status">状态</param>
        /// <param name="SCount">开始条数</param>
        /// <param name="PageSize">页容量</param>
        /// <param name="TotalCount">总数量</param>
        /// <returns></returns>
        public DataSet GetInterestCouponList(string name, decimal amount, DateTime s_ctime, DateTime e_ctime, int status, int SCount, int PageSize)
        {
            name ="%" +name.Trim() + "%";

            string strSql = @"SELECT a.id,a.name,a.interest_rate,a.use_condition,a.c_time,a.enable_day,a.status,a.use_count
FROM act_interest_coupon a
WHERE a.is_delete=0 AND (?name='%%' OR a.name LIKE ?name) AND (?amount=-1 OR a.interest_rate=?amount) AND (?status=-1 OR a.status=?status) AND (?s_ctime='1900-01-01' OR a.c_time>=?s_ctime) AND (?e_ctime='9999-01-02' OR a.c_time<?e_ctime)
ORDER BY a.id DESC
LIMIT ?SCount,?PageSize;
SELECT COUNT(1)
FROM act_interest_coupon a
WHERE a.is_delete=0 AND (?name='%%' OR a.name LIKE ?name) AND (?amount=-1 OR a.interest_rate=?amount) AND (?status=-1 OR a.status=?status) AND (?s_ctime='1900-01-01' OR a.c_time>=?s_ctime) AND (?e_ctime='9999-01-02' OR a.c_time<?e_ctime); ";
            MySqlParameter[] parameters = {
					new MySqlParameter("?name", MySqlDbType.VarChar),
                    new MySqlParameter("?amount", MySqlDbType.VarChar),
                    new MySqlParameter("?s_ctime", MySqlDbType.DateTime),
                    new MySqlParameter("?e_ctime", MySqlDbType.DateTime),
                    new MySqlParameter("?status", MySqlDbType.Int32),
                    new MySqlParameter("?SCount", MySqlDbType.Int32),
                    new MySqlParameter("?PageSize", MySqlDbType.Int32)
			};
            parameters[0].Value = name;
            parameters[1].Value = amount;
            parameters[2].Value = s_ctime;
            parameters[3].Value = e_ctime;
            parameters[4].Value = status;
            parameters[5].Value = SCount;
            parameters[6].Value = PageSize;
            DataSet ds = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql, "获取加息券列表", parameters);


            return ds;
        }
        /// <summary>
        /// 获取加息券实体
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public DataSet GetActInterestCouponEntity(int id)
        {
            string strSql = @"SELECT a.id,a.status,a.name,a.interest_rate,a.enable_day,a.use_condition,a.c_time,a.creator_id,a.m_time,a.m_id,a.is_delete,a.remark
FROM act_interest_coupon a
WHERE a.id=?id AND a.is_delete=0; ";
            MySqlParameter[] parameters = {
					new MySqlParameter("?id", MySqlDbType.Int32)
			};
            parameters[0].Value = id;

            DataSet ds = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql, "根据主键获取加息券实体", parameters);
            return ds;
        }
        /// <summary>
        /// 修改活动加息券
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="amount">面值</param>
        /// <param name="enableday">有效期</param>
        /// <param name="usecondition">使用条件</param>
        /// <param name="creator_id">新增人id</param>
        /// <param name="c_time">新增时间</param>
        /// <returns></returns>
        public bool ModifyActInterestCoupon(int id, string name, decimal amount, int enableday, string usecondition, int m_id, DateTime m_time)
        {
            string strSql = @"UPDATE act_interest_coupon SET status=0,name=?name,interest_rate=?amount,enable_day=?enableday,use_condition=?usecondition,m_id=?m_id,m_time=?m_time WHERE id=?id;";
            MySqlParameter[] parameters = {
                    new MySqlParameter("?id", MySqlDbType.Int32),
					new MySqlParameter("?name", MySqlDbType.VarChar,20),
                    new MySqlParameter("?amount", MySqlDbType.Decimal),
                    new MySqlParameter("?enableday", MySqlDbType.Int32),
                    new MySqlParameter("?usecondition", MySqlDbType.VarChar,100),
                    new MySqlParameter("?m_id", MySqlDbType.Int32),
                    new MySqlParameter("?m_time", MySqlDbType.DateTime)
			};
            parameters[0].Value = id;
            parameters[1].Value = name;
            parameters[2].Value = amount;
            parameters[3].Value = enableday;
            parameters[4].Value = usecondition;
            parameters[5].Value = m_id;
            parameters[6].Value = m_time;
            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql, "修改活动加息券", parameters);
        }

        /// <summary>
        /// 更新加息券状态
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="status">status</param>
        /// <param name="m_id">修改人</param>
        /// <param name="m_time">修改时间</param>
        /// <returns></returns>
        public bool UpdateInterestCouponStatus(int id, int status, int m_id, DateTime m_time, string remark)
        {
            string strSql = @"UPDATE act_interest_coupon SET status=?status,m_id=?m_id,m_time=?m_time,remark=?remark WHERE id=?id;";
            MySqlParameter[] parameters = {
                    new MySqlParameter("?id", MySqlDbType.Int32),
                    new MySqlParameter("?status", MySqlDbType.Int32),
                    new MySqlParameter("?m_id", MySqlDbType.Int32),
                    new MySqlParameter("?m_time", MySqlDbType.DateTime),
                    new MySqlParameter("?remark", MySqlDbType.VarChar,100)
			};
            parameters[0].Value = id;
            parameters[1].Value = status;
            parameters[2].Value = m_id;
            parameters[3].Value = m_time;
            parameters[4].Value = remark;
            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql, "更新加息券状态", parameters);
        }
        /// <summary>
        /// 删除加息券
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="m_id">修改人</param>
        /// <param name="m_time">修改时间</param>
        /// <returns></returns>
        public bool DelInterestCoupon(int id, int m_id, DateTime m_time)
        {
            string strSql = @"UPDATE act_interest_coupon SET is_delete=1,m_id=?m_id,m_time=?m_time WHERE id=?id;";
            MySqlParameter[] parameters = {
                    new MySqlParameter("?id", MySqlDbType.Int32),
                    new MySqlParameter("?m_id", MySqlDbType.Int32),
                    new MySqlParameter("?m_time", MySqlDbType.DateTime)
			};
            parameters[0].Value = id;
            parameters[1].Value = m_id;
            parameters[2].Value = m_time;

            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql, "删除加息券", parameters);
        }
        /// <summary>
        /// 获取加息券发放使用明细
        /// </summary>
        /// <param name="name">特享金名称</param>
        /// <param name="user_id">用户id</param>
        /// <param name="s_ctime">开始时间</param>
        /// <param name="e_ctime">结束时间</param>
        /// <param name="CurrentPage"></param>
        /// <param name="PageSize"></param>
        /// <param name="TotalCount"></param>
        /// <returns></returns>
        public DataSet InterestCouponSendDetail(string name, long user_id, int use_status, DateTime s_ctime, DateTime e_ctime, int SCount, int PageSize)
        {
            name = "%" + name.Trim() + "%";

            string strSql = @"SELECT a.user_id,b.name,b.interest_rate,a.c_time,a.enable_day,a.use_status,c.pdt_type,c.c_time use_date
FROM user_add_interest a
INNER JOIN act_interest_coupon b ON a.interest_id=b.id
LEFT JOIN user_invest_rec c ON a.invest_id=c.id
WHERE (?name='%%' OR b.name LIKE ?name) AND (?user_id=-1 OR a.user_id=?user_id) AND (?s_ctime='1900-01-01' OR a.c_time>=?s_ctime) AND (?e_ctime='9999-01-02' OR a.c_time<?e_ctime) AND (?use_status=-1 OR a.use_status=?use_status)
ORDER BY a.c_time DESC
LIMIT ?SCount,?PageSize;
SELECT COUNT(1)
FROM user_add_interest a
INNER JOIN act_interest_coupon b ON a.interest_id=b.id
LEFT JOIN user_invest_rec c ON a.invest_id=c.id
WHERE (?name='%%' OR b.name LIKE ?name) AND (?user_id=-1 OR a.user_id=?user_id) AND (?s_ctime='1900-01-01' OR a.c_time>=?s_ctime) AND (?e_ctime='9999-01-02' OR a.c_time<?e_ctime) AND (?use_status=-1 OR a.use_status=?use_status);";
            MySqlParameter[] parameters = {
					new MySqlParameter("?name", MySqlDbType.VarChar,20),
                    new MySqlParameter("?user_id", MySqlDbType.Int64),
                    new MySqlParameter("?s_ctime", MySqlDbType.DateTime),
                    new MySqlParameter("?e_ctime", MySqlDbType.DateTime),
                    new MySqlParameter("?SCount", MySqlDbType.Int32),
                    new MySqlParameter("?PageSize", MySqlDbType.Int32),
                    new MySqlParameter("?use_status", MySqlDbType.Int32)
			};
            parameters[0].Value = name;
            parameters[1].Value = user_id;
            parameters[2].Value = s_ctime;
            parameters[3].Value = e_ctime;
            parameters[4].Value = SCount;
            parameters[5].Value = PageSize;
            parameters[6].Value = use_status;
            DataSet ds = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql, "获取加息券发放使用明细", parameters);
            return ds;
        }
        /// <summary>
        /// 根据名称获取总数量
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int GetInterestCouponCountByName(string name,int id=-1)
        {
            string strSql = @"SELECT COUNT(1) FROM act_interest_coupon WHERE name=?name AND is_delete=0 AND id!=?id;";
            MySqlParameter[] parameters = {
                    new MySqlParameter("?name", MySqlDbType.VarChar,100),
                    new MySqlParameter("?id", MySqlDbType.Int32)
			};
            parameters[0].Value = name;
            parameters[1].Value = id;
            return Convert.ToInt32(DataBaseOperator.MoneyWriteDbHelper.ExecuteScalar(strSql, "根据名称获取总数量", parameters));
        }
        #endregion
        #region 特享金
        /// <summary>
        /// 新增活动特享金
        /// </summary>
        /// <param name="name">名次</param>
        /// <param name="amount">面值</param>
        /// <param name="enableday">使用期限</param>
        /// <param name="rate">利率</param>
        /// <param name="rate_increase">递增利率</param>
        /// <param name="creator_id">创建人ID</param>
        /// <param name="c_time">创建时间</param>
        /// <returns></returns>
        public bool InsertActSpecialAssets(string name, decimal amount, int enableday, decimal rate, decimal rate_increase, int creator_id, DateTime c_time)
        {
            string strSql = @"INSERT INTO act_special_assets(NAME,amount,enable_day,rate,rate_increase,creator_id,c_time)
                              VALUES(?name,?amount,?enableday,?rate,?rate_increase,?creator_id,?c_time);";
            MySqlParameter[] parameters = {
					new MySqlParameter("?name", MySqlDbType.VarChar,20),
                    new MySqlParameter("?amount", MySqlDbType.Decimal),
                    new MySqlParameter("?enableday", MySqlDbType.Int32),
                    new MySqlParameter("?rate", MySqlDbType.Decimal),
                    new MySqlParameter("?rate_increase", MySqlDbType.Decimal),
                    new MySqlParameter("?creator_id", MySqlDbType.Int32),
                    new MySqlParameter("?c_time", MySqlDbType.DateTime)
			};
            parameters[0].Value = name;
            parameters[1].Value = amount;
            parameters[2].Value = enableday;
            parameters[3].Value = rate;
            parameters[4].Value = rate_increase;
            parameters[5].Value = creator_id;
            parameters[6].Value = c_time;
            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql, "新增活动特享金", parameters);
        }
        /// <summary>
        /// 获取特享金列表
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="amount">面值</param>
        /// <param name="s_ctime">创建开始时间</param>
        /// <param name="e_ctime">创建结束时间</param>
        /// <param name="status">状态</param>
        /// <param name="SCount">开始条数</param>
        /// <param name="PageSize">页容量</param>
        /// <param name="TotalCount">总数量</param>
        /// <returns></returns>
        public DataSet GetSpecialAssetsList(string name, decimal amount, DateTime s_ctime, DateTime e_ctime, int status, int SCount, int PageSize)
        {
            name = "%" + name.Trim() + "%";

            string strSql = @"SELECT a.id,a.name,a.amount,a.rate,a.rate_increase,a.c_time,a.enable_day,a.status,a.send_count
FROM act_special_assets a
WHERE a.is_delete=0 AND (?name='%%' OR a.name LIKE ?name) AND (?amount=-1 OR a.amount=?amount) AND (?status=-1 OR a.status=?status) AND (?s_ctime='1900-01-01' OR a.c_time>=?s_ctime) AND (?e_ctime='9999-01-02' OR a.c_time<?e_ctime)
ORDER BY a.id DESC
LIMIT ?SCount,?PageSize;
SELECT COUNT(1)
FROM act_special_assets a
WHERE a.is_delete=0 AND (?name='%%' OR a.name LIKE ?name) AND (?amount=-1 OR a.amount=?amount) AND (?status=-1 OR a.status=?status) AND (?s_ctime='1900-01-01' OR a.c_time>=?s_ctime) AND (?e_ctime='9999-01-02' OR a.c_time<?e_ctime); ";
            MySqlParameter[] parameters = {
					new MySqlParameter("?name", MySqlDbType.VarChar,20),
                    new MySqlParameter("?amount", MySqlDbType.Decimal),
                    new MySqlParameter("?s_ctime", MySqlDbType.DateTime),
                    new MySqlParameter("?e_ctime", MySqlDbType.DateTime),
                    new MySqlParameter("?status", MySqlDbType.Int32),
                    new MySqlParameter("?SCount", MySqlDbType.Int32),
                    new MySqlParameter("?PageSize", MySqlDbType.Int32)
			};
            parameters[0].Value = name;
            parameters[1].Value = amount;
            parameters[2].Value = s_ctime;
            parameters[3].Value = e_ctime;
            parameters[4].Value = status;
            parameters[5].Value = SCount;
            parameters[6].Value = PageSize;
            DataSet ds = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql, "获取特享金列表", parameters);


            return ds;
        }
        /// <summary>
        /// 获取特享金实体
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public DataSet GetActSpecialAssetsEntity(int id)
        {
            string strSql = @"SELECT a.id,a.status,a.name,a.amount,a.rate,a.rate_increase,a.enable_day,a.c_time,a.creator_id,a.m_time,a.m_id,a.is_delete,a.remark
FROM act_special_assets a
WHERE a.id=?id AND a.is_delete=0; ";
            MySqlParameter[] parameters = {
					new MySqlParameter("?id", MySqlDbType.Int32)
			};
            parameters[0].Value = id;

            DataSet ds = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql, "根据主键获取特享金实体", parameters);
            return ds;
        }
        /// <summary>
        /// 新增活动特享金
        /// </summary>
        /// <param name="name">名次</param>
        /// <param name="amount">面值</param>
        /// <param name="enableday">使用期限</param>
        /// <param name="rate">利率</param>
        /// <param name="rate_increase">递增利率</param>
        /// <param name="creator_id">创建人ID</param>
        /// <param name="c_time">创建时间</param>
        /// <returns></returns>
        public bool ModifyActSpecialAssets(int id, string name, decimal amount, int enableday, decimal rate, decimal rate_increase, int m_id, DateTime m_time)
        {
            string strSql = @"UPDATE act_special_assets SET status=0,name=?name,amount=?amount,enable_day=?enableday,rate=?rate,rate_increase=?rate_increase,m_id=?m_id,m_time=?m_time WHERE id=?id;";
            MySqlParameter[] parameters = {
					new MySqlParameter("?name", MySqlDbType.VarChar,20),
                    new MySqlParameter("?amount", MySqlDbType.Decimal),
                    new MySqlParameter("?enableday", MySqlDbType.Int32),
                    new MySqlParameter("?rate", MySqlDbType.Decimal),
                    new MySqlParameter("?rate_increase", MySqlDbType.Decimal),
                    new MySqlParameter("?m_id", MySqlDbType.Int32),
                    new MySqlParameter("?m_time", MySqlDbType.DateTime),
                    new MySqlParameter("?id", MySqlDbType.Int32)
			};
            parameters[0].Value = name;
            parameters[1].Value = amount;
            parameters[2].Value = enableday;
            parameters[3].Value = rate;
            parameters[4].Value = rate_increase;
            parameters[5].Value = m_id;
            parameters[6].Value = m_time;
            parameters[7].Value = id;
            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql, "修改活动特享金", parameters);
        }
        /// <summary>
        /// 更新特享金状态
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="status">status</param>
        /// <param name="m_id">修改人</param>
        /// <param name="m_time">修改时间</param>
        /// <returns></returns>
        public bool UpdateSpecialAssetsStatus(int id, int status, int m_id, DateTime m_time, string remark)
        {
            string strSql = @"UPDATE act_special_assets SET status=?status,m_id=?m_id,m_time=?m_time,remark=?remark WHERE id=?id;";
            MySqlParameter[] parameters = {
                    new MySqlParameter("?id", MySqlDbType.Int32),
                    new MySqlParameter("?status", MySqlDbType.Int32),
                    new MySqlParameter("?m_id", MySqlDbType.Int32),
                    new MySqlParameter("?m_time", MySqlDbType.DateTime),
                    new MySqlParameter("?remark", MySqlDbType.VarChar,100)
			};
            parameters[0].Value = id;
            parameters[1].Value = status;
            parameters[2].Value = m_id;
            parameters[3].Value = m_time;
            parameters[4].Value = remark;
            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql, "更新特享金状态", parameters);
        }
        /// <summary>
        /// 删除特享金
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="m_id">修改人</param>
        /// <param name="m_time">修改时间</param>
        /// <returns></returns>
        public bool DelSpecialAssets(int id, int m_id, DateTime m_time)
        {
            string strSql = @"UPDATE act_special_assets SET is_delete=1,m_id=?m_id,m_time=?m_time WHERE id=?id;";
            MySqlParameter[] parameters = {
                    new MySqlParameter("?id", MySqlDbType.Int32),
                    new MySqlParameter("?m_id", MySqlDbType.Int32),
                    new MySqlParameter("?m_time", MySqlDbType.DateTime)
			};
            parameters[0].Value = id;
            parameters[1].Value = m_id;
            parameters[2].Value = m_time;

            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql, "删除特享金", parameters);
        }
        /// <summary>
        /// 获取特享金发放明细
        /// </summary>
        /// <param name="name">特享金名称</param>
        /// <param name="user_id">用户id</param>
        /// <param name="s_ctime">开始时间</param>
        /// <param name="e_ctime">结束时间</param>
        /// <param name="CurrentPage"></param>
        /// <param name="PageSize"></param>
        /// <param name="TotalCount"></param>
        /// <returns></returns>
        public DataSet SpecialAssetsSendDetail(string name, long user_id, DateTime s_ctime, DateTime e_ctime, int SCount, int PageSize)
        {
            name = "%" + name.Trim() + "%";

            string strSql = @"SELECT a.user_id, c.name,a.amount,b.type,c.rate,c.enable_day,a.c_time
                                FROM user_special_assets a
                                INNER JOIN act_promotion b ON a.act_id=b.id
                                INNER JOIN act_special_assets c ON a.special_id=c.id
                                WHERE (?name='%%' OR c.name LIKE ?name) AND (?user_id=-1 OR a.user_id=?user_id) AND (?s_ctime='1900-01-01' OR a.c_time>=?s_ctime) AND (?e_ctime='9999-01-02' OR a.c_time<?e_ctime)
                                ORDER BY a.c_time DESC
                                LIMIT ?SCount,?PageSize;
                                SELECT COUNT(1)
                                FROM user_special_assets a
                                INNER JOIN act_promotion b ON a.act_id=b.id
                                INNER JOIN act_special_assets c ON a.special_id=c.id
                                WHERE (?name='%%' OR c.name LIKE ?name) AND (?user_id=-1 OR a.user_id=?user_id) AND (?s_ctime='1900-01-01' OR a.c_time>=?s_ctime) AND (?e_ctime='9999-01-02' OR a.c_time<?e_ctime);";
            MySqlParameter[] parameters = {
					new MySqlParameter("?name", MySqlDbType.VarChar,20),
                    new MySqlParameter("?user_id", MySqlDbType.Int64),
                    new MySqlParameter("?s_ctime", MySqlDbType.DateTime),
                    new MySqlParameter("?e_ctime", MySqlDbType.DateTime),
                    new MySqlParameter("?SCount", MySqlDbType.Int32),
                    new MySqlParameter("?PageSize", MySqlDbType.Int32)
			};
            parameters[0].Value = name;
            parameters[1].Value = user_id;
            parameters[2].Value = s_ctime;
            parameters[3].Value = e_ctime;
            parameters[4].Value = SCount;
            parameters[5].Value = PageSize;
            DataSet ds = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql, "获取特享金发放明细", parameters);
            return ds;
        }
        /// <summary>
        /// 根据名称获取总数量
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int GetSpecialAssetsCountByName(string name,int id=-1)
        {
            string strSql = @"SELECT COUNT(1) FROM act_special_assets WHERE name=?name AND is_delete=0 AND id!=?id;";
            MySqlParameter[] parameters = {
                    new MySqlParameter("?name", MySqlDbType.VarChar,100),
                    new MySqlParameter("?id", MySqlDbType.Int32)
			};
            parameters[0].Value = name;
            parameters[1].Value = id;
            return Convert.ToInt32(DataBaseOperator.MoneyWriteDbHelper.ExecuteScalar(strSql, "根据名称获取总数量", parameters));
        }
        #endregion
        #region 活动
        /// <summary>
        /// 获取所有的 特享金、加息券----审核过的
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllICAndSA()
        {

            string strSql = @"SELECT 2 item_type,a.id item_id,a.name item_name,interest_rate as amount
                              FROM act_interest_coupon a
                              WHERE a.is_delete=0 AND a.status=1
                              UNION ALL
                              SELECT 1 item_type,a.id item_id,a.name item_name,amount
                              FROM act_special_assets a
                              WHERE a.is_delete=0 AND a.status=1; ";
            DataSet ds = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql, "获取所有的 特享金、加息券", null);


            return ds;
        }
        /// <summary>
        /// 新增活动(主表)
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="type">类型0注册1邀请2购买。。</param>
        /// <param name="max_user">参与人上限</param>
        /// <param name="limit_num">邀请活动限制（0无限制)</param>
        /// <param name="budget">预算</param>
        /// <param name="s_time">开始日期</param>
        /// <param name="e_time">结束日期</param>
        /// <param name="ad_content">广告语</param>
        /// <param name="url">链接</param>
        /// <param name="c_time">创建日期</param>
        /// <param name="creator_id">创建人id</param>
        /// <returns></returns>
        public int InsertActPromotion(string name, int type, int max_user_num, int limit_num, decimal budget, DateTime s_time, DateTime e_time, string ad_content, string url, DateTime c_time, int creator_id)
        {
            int id = 0;

            string strSql = @"INSERT INTO act_promotion (NAME,ad_content,s_time,e_time,TYPE,limit_num,c_time,creator_id,budget,url,max_user_num)
                              VALUES(?name,?ad_content,?s_time,?e_time,?type,?limit_num,?c_time,?creator_id,?budget,?url,?max_user_num);select @@IDENTITY as id;";
            MySqlParameter[] parameters = {
					new MySqlParameter("?name", MySqlDbType.VarChar,250),
                    new MySqlParameter("?ad_content", MySqlDbType.VarChar,500),
                    new MySqlParameter("?s_time", MySqlDbType.DateTime),
                    new MySqlParameter("?e_time", MySqlDbType.DateTime),
                    new MySqlParameter("?type", MySqlDbType.Int32),
                    new MySqlParameter("?limit_num", MySqlDbType.Int32),
                    new MySqlParameter("?c_time", MySqlDbType.DateTime),
                    new MySqlParameter("?creator_id", MySqlDbType.Int32),
                    new MySqlParameter("?budget", MySqlDbType.Decimal),
                    new MySqlParameter("?url", MySqlDbType.VarChar,50),
                    new MySqlParameter("?max_user_num", MySqlDbType.Int32)
			};
            parameters[0].Value = name;
            parameters[1].Value = ad_content;
            parameters[2].Value = s_time;
            parameters[3].Value = e_time;
            parameters[4].Value = type;
            parameters[5].Value = limit_num;
            parameters[6].Value = c_time;
            parameters[7].Value = creator_id;
            parameters[8].Value = budget;
            parameters[9].Value = url;
            parameters[10].Value = max_user_num;
            id = Convert.ToInt32(DataBaseOperator.MoneyWriteDbHelper.ExecuteScalar(strSql, "新增活动(主表)", parameters));

            return id;
        }
        /// <summary>
        /// 新增活动(子表)
        /// </summary>
        /// <param name="act_id">活动id</param>
        /// <param name="item_type">项目类型</param>
        /// <param name="item_id">项目id</param>
        /// <param name="item_name">项目名称</param>
        /// <param name="item_qty">数量</param>
        /// <param name="c_time">创建时间</param>
        /// <param name="creator_id">创建人id</param>
        /// <returns></returns>
        public bool InsertActPromotionItem(int act_id, int item_type, int item_id, string item_name, int item_qty, DateTime c_time, int creator_id)
        {
            string strSql = @"INSERT INTO act_promotion_item (act_id, item_type, item_id, item_name, item_qty, c_time, creator_id)
                              VALUES(?act_id, ?item_type, ?item_id, ?item_name, ?item_qty, ?c_time, ?creator_id);";
            MySqlParameter[] parameters = {
					new MySqlParameter("?act_id", MySqlDbType.Int32),
                    new MySqlParameter("?item_type", MySqlDbType.Int32),
                    new MySqlParameter("?item_id", MySqlDbType.Int32),
                    new MySqlParameter("?item_name", MySqlDbType.VarChar,50),
                    new MySqlParameter("?item_qty", MySqlDbType.Int32),
                    new MySqlParameter("?c_time", MySqlDbType.DateTime),
                    new MySqlParameter("?creator_id", MySqlDbType.Int32)
			};
            parameters[0].Value = act_id;
            parameters[1].Value = item_type;
            parameters[2].Value = item_id;
            parameters[3].Value = item_name;
            parameters[4].Value = item_qty;
            parameters[5].Value = c_time;
            parameters[6].Value = creator_id;

            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql, "新增活动(子表)", parameters);

        }
        /// <summary>
        /// 获取活动列表
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="type">类型</param>
        /// <param name="s_ctime">创建开始时间</param>
        /// <param name="e_ctime">创建结束时间</param>
        /// <param name="status">状态</param>
        /// <param name="SCount">开始条数</param>
        /// <param name="PageSize">页容量</param>
        /// <param name="TotalCount">总数量</param>
        /// <returns></returns>
        public DataSet GetActivityList(string name, int type, DateTime s_ctime, DateTime e_ctime, int status, int SCount, int PageSize, int IsAct)
        {
            name = "%" + name.Trim() + "%";

            string strSql = @"SELECT a.id,a.name,a.type,a.limit_num,a.ad_content,a.c_time,a.max_user_num,a.curt_user_num,a.url,a.budget,a.s_time,a.e_time,a.status
FROM act_promotion a
WHERE a.is_delete=0 AND (?name='%%' OR a.name LIKE ?name) AND (?type=-1 OR a.type=?type) AND (?status=-1 OR a.status=?status) AND (?s_ctime='1900-01-01' OR a.c_time>=?s_ctime) AND (?e_ctime='9999-01-02' OR a.c_time<?e_ctime) AND (?IsAct=0 OR (a.status=1 AND a.s_time<=?cu_time AND a.e_time>=?cu_time))
ORDER BY a.id DESC
LIMIT ?SCount,?PageSize;
SELECT COUNT(1)
FROM act_promotion a
WHERE a.is_delete=0 AND (?name='%%' OR a.name LIKE ?name) AND (?type=-1 OR a.type=?type) AND (?status=-1 OR a.status=?status) AND (?s_ctime='1900-01-01' OR a.c_time>=?s_ctime) AND (?e_ctime='9999-01-02' OR a.c_time<?e_ctime) AND (?IsAct=0 OR (a.status=1 AND a.s_time<=?cu_time AND a.e_time>=?cu_time)); ";
            MySqlParameter[] parameters = {
					new MySqlParameter("?name", MySqlDbType.VarChar),
                    new MySqlParameter("?type", MySqlDbType.Int32),
                    new MySqlParameter("?s_ctime", MySqlDbType.DateTime),
                    new MySqlParameter("?e_ctime", MySqlDbType.DateTime),
                    new MySqlParameter("?status", MySqlDbType.Int32),
                    new MySqlParameter("?SCount", MySqlDbType.Int32),
                    new MySqlParameter("?PageSize", MySqlDbType.Int32),
                    new MySqlParameter("?IsAct", MySqlDbType.Int32),
                    new MySqlParameter("?cu_time", MySqlDbType.DateTime)
			};
            parameters[0].Value = name;
            parameters[1].Value = type;
            parameters[2].Value = s_ctime;
            parameters[3].Value = e_ctime;
            parameters[4].Value = status;
            parameters[5].Value = SCount;
            parameters[6].Value = PageSize;
            parameters[7].Value = IsAct;
            parameters[8].Value = DateTime.Now;
            DataSet ds = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql, "获取活动列表", parameters);


            return ds;
        }
        /// <summary>
        /// 根据主键获取活动实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataSet GetActPromotionEntityByID(int id)
        {
            string strSql = @"SELECT a.id,a.name,a.type,a.limit_num,a.ad_content,a.c_time,a.max_user_num,a.curt_user_num,a.url,a.budget,a.s_time,a.e_time,a.status,a.remark
                              FROM act_promotion a WHERE a.id=?id; ";
            MySqlParameter[] parameters = {
					new MySqlParameter("?id", MySqlDbType.Int32)
			};
            parameters[0].Value = id;

            DataSet ds = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql, "根据主键获取活动实体", parameters);
            return ds;
        }
        /// <summary>
        /// 获取子活动（根据活动id）
        /// </summary>
        /// <returns></returns>
        public DataSet GetActPromotionItemByActID(int act_id)
        {
            string strSql = @"SELECT a.id,a.act_id,a.item_type,a.item_id,a.item_name,a.item_qty,a.c_time,a.creator_id
                              FROM act_promotion_item a WHERE act_id=?act_id; ";
            MySqlParameter[] parameters = {
					new MySqlParameter("?act_id", MySqlDbType.Int32)
			};
            parameters[0].Value = act_id;

            DataSet ds = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql, "获取子活动（根据活动id）", parameters);
            return ds;
        }
        /// <summary>
        /// 修改活动(主表)
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="type">类型0注册1邀请2购买。。</param>
        /// <param name="max_user">参与人上限</param>
        /// <param name="limit_num">邀请活动限制（0无限制)</param>
        /// <param name="budget">预算</param>
        /// <param name="s_time">开始日期</param>
        /// <param name="e_time">结束日期</param>
        /// <param name="ad_content">广告语</param>
        /// <param name="url">链接</param>
        /// <param name="c_time">创建日期</param>
        /// <param name="creator_id">创建人id</param>
        /// <returns></returns>
        public bool ModifyActPromotion(int id, string name, int type, int max_user_num, int limit_num, decimal budget, DateTime s_time, DateTime e_time, string ad_content, string url, DateTime m_time, int m_id)
        {
            string strSql = @"UPDATE act_promotion SET status=0,name=?name,ad_content=?ad_content,s_time=?s_time,e_time=?e_time,type=?type,limit_num=?limit_num,m_time=?m_time,m_id=?m_id,budget=?budget,url=?url,max_user_num=?max_user_num WHERE id=?id;";
            MySqlParameter[] parameters = {
					new MySqlParameter("?name", MySqlDbType.VarChar,250),
                    new MySqlParameter("?ad_content", MySqlDbType.VarChar,500),
                    new MySqlParameter("?s_time", MySqlDbType.DateTime),
                    new MySqlParameter("?e_time", MySqlDbType.DateTime),
                    new MySqlParameter("?type", MySqlDbType.Int32),
                    new MySqlParameter("?limit_num", MySqlDbType.Int32),
                    new MySqlParameter("?m_time", MySqlDbType.DateTime),
                    new MySqlParameter("?m_id", MySqlDbType.Int32),
                    new MySqlParameter("?budget", MySqlDbType.Decimal),
                    new MySqlParameter("?url", MySqlDbType.VarChar,50),
                    new MySqlParameter("?max_user_num", MySqlDbType.Int32),
                    new MySqlParameter("?id", MySqlDbType.Int32)
			};
            parameters[0].Value = name;
            parameters[1].Value = ad_content;
            parameters[2].Value = s_time;
            parameters[3].Value = e_time;
            parameters[4].Value = type;
            parameters[5].Value = limit_num;
            parameters[6].Value = m_time;
            parameters[7].Value = m_id;
            parameters[8].Value = budget;
            parameters[9].Value = url;
            parameters[10].Value = max_user_num;
            parameters[11].Value = id;
            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql, "修改活动(主表)", parameters);

        }
        /// <summary>
        /// 删除已有子活动
        /// </summary>
        /// <param name="act_id"></param>
        /// <returns></returns>
        public bool DelActPromotionItemByActID(int act_id)
        {
            string strSql = @"DELETE FROM act_promotion_item WHERE act_id=?act_id;";
            MySqlParameter[] parameters = {
                    new MySqlParameter("?act_id", MySqlDbType.Int32)
			};
            parameters[0].Value = act_id;
            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql, "删除已有子活动", parameters);
        }
        /// <summary>
        /// 更新活动状态
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="status">status</param>
        /// <param name="m_id">修改人</param>
        /// <param name="m_time">修改时间</param>
        /// <returns></returns>
        public bool UpdateActivityStatus(int id, int status, int m_id, DateTime m_time, string remark)
        {
            string strSql = @"UPDATE act_promotion SET status=?status,m_id=?m_id,m_time=?m_time,remark=?remark WHERE id=?id;";
            MySqlParameter[] parameters = {
                    new MySqlParameter("?id", MySqlDbType.Int32),
                    new MySqlParameter("?status", MySqlDbType.Int32),
                    new MySqlParameter("?m_id", MySqlDbType.Int32),
                    new MySqlParameter("?m_time", MySqlDbType.DateTime),
                    new MySqlParameter("?remark", MySqlDbType.VarChar,100)
			};
            parameters[0].Value = id;
            parameters[1].Value = status;
            parameters[2].Value = m_id;
            parameters[3].Value = m_time;
            parameters[4].Value = remark;
            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql, "更新活动状态", parameters);
        }
        /// <summary>
        /// 删除活动
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="m_id">修改人</param>
        /// <param name="m_time">修改时间</param>
        /// <returns></returns>
        public bool DelActivity(int id, int m_id, DateTime m_time)
        {
            string strSql = @"UPDATE act_promotion SET is_delete=1,m_id=?m_id,m_time=?m_time WHERE id=?id;";
            MySqlParameter[] parameters = {
                    new MySqlParameter("?id", MySqlDbType.Int32),
                    new MySqlParameter("?m_id", MySqlDbType.Int32),
                    new MySqlParameter("?m_time", MySqlDbType.DateTime)
			};
            parameters[0].Value = id;
            parameters[1].Value = m_id;
            parameters[2].Value = m_time;

            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql, "删除活动", parameters);
        }
        /// <summary>
        /// 根据名称获取总数量
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int GetActivityCountByName(string name,int id=-1)
        {
            string strSql = @"SELECT COUNT(1) FROM act_promotion WHERE name=?name AND is_delete=0 AND id!=?id;";
            MySqlParameter[] parameters = {
                    new MySqlParameter("?name", MySqlDbType.VarChar,100),
                    new MySqlParameter("?id", MySqlDbType.Int32)
			};
            parameters[0].Value = name;
            parameters[1].Value = id;
            return Convert.ToInt32(DataBaseOperator.MoneyWriteDbHelper.ExecuteScalar(strSql, "根据名称获取总数量", parameters));
        }
        /// <summary>
        /// 同类型活动同时间段重叠存在数量
        /// </summary>
        /// <param name="s_time"></param>
        /// <param name="e_time"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public int GetActivityCountByTypeAndTime(DateTime s_time, DateTime e_time, int type, int id)
        {
            string strSql = @"SELECT COUNT(1) FROM act_promotion WHERE type=?type AND id!=?id AND status!=3 AND is_delete=0 AND ((?s_time>s_time AND ?s_time<e_time) OR (?e_time>s_time AND ?e_time<e_time));";
            MySqlParameter[] parameters = {
                    new MySqlParameter("?s_time", MySqlDbType.DateTime),
                    new MySqlParameter("?e_time", MySqlDbType.DateTime),
                    new MySqlParameter("?type", MySqlDbType.Int32),
                    new MySqlParameter("?id", MySqlDbType.Int32)
			};
            parameters[0].Value = s_time;
            parameters[1].Value = e_time;
            parameters[2].Value = type;
            parameters[3].Value = id;
            return Convert.ToInt32(DataBaseOperator.MoneyWriteDbHelper.ExecuteScalar(strSql, "同类型活动同时间段重叠存在数量", parameters));
        }
        #endregion

        #region 前台
        /// <summary>
        /// 根据加息券编号更新发放数量（前台） by张浩然 2015年6月30日
        /// </summary>
        /// <param name="id">加息券编号</param>
        /// <returns></returns>
        public bool UpdateActCouponSendCount(int id)
        {
            string strSql = @"UPDATE act_interest_coupon SET send_count=send_count+1 WHERE id=?id;";
            MySqlParameter[] parameters = {
                    new MySqlParameter("?id", MySqlDbType.Int32)
			};
            parameters[0].Value = id;
            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql, "根据加息券编号更新发放数量 by张浩然 2015年6月30日", parameters);
        }

        /// <summary>
        /// 根据特享金编号更新发放数量（前台） by张浩然 2015年6月30日
        /// </summary>
        /// <param name="id">特享金编号</param>
        /// <returns></returns>
        public bool UpdateActSpecialAssetsSendCount(int id)
        {
            string strSql = @"UPDATE act_special_assets SET send_count=send_count+1 WHERE id=?id;";
            MySqlParameter[] parameters = {
                    new MySqlParameter("?id", MySqlDbType.Int32)
			};
            parameters[0].Value = id;
            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql, "根据特享金编号更新发放数量 by张浩然 2015年6月30日", parameters);
        }

        /// <summary>
        /// 根据活动编号更新活动发放次数
        /// </summary>
        /// <param name="act_id">活动编号</param>
        /// <returns></returns>
        public bool UpdateActSendCount(int item_id)
        {
            string strSql = @"UPDATE act_promotion_item SET send_count=send_count+1 WHERE id=?item_id;";
            MySqlParameter[] parameters = {
                    new MySqlParameter("?item_id", MySqlDbType.Int32)
			};
            parameters[0].Value = item_id;
            return DataBaseOperator.MoneyWriteDbHelper.ExecuteNonQuery(strSql, "根据活动编号更新活动发放次数 by张浩然 2015年7月8日", parameters);
        }

        #endregion

        #region 活动充值
        /// <summary>
        /// 根据类型获取平台账户
        /// </summary>
        /// <param name="type">账户类型(1利息账户2活动账户3违约金账户)</param>
        /// <returns></returns>
        public DataTable GetPlatAccountByType(int type)
        {
            string strSql = @"SELECT id,c_time,type,name,amount,amount_recharge,amount_paid,base_amount
                                FROM plat_account WHERE type=?type; ";
            MySqlParameter[] parameters = {
					new MySqlParameter("?type", MySqlDbType.Int32)
			};
            parameters[0].Value = type;

            DataTable dt = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql, "获取子活动（根据活动id）", parameters).Tables[0];
            return dt;
        }
        /// <summary>
        /// 活动充值
        /// </summary>
        /// <param name="plataccountid"></param>
        /// <param name="c_time"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool ActRecharge(int plataccountid, DateTime c_time, decimal amount, int creator_id)
        {
            List<string> strSql_array = new List<string>();
            List<DbParameter[]> parameters_array = new List<DbParameter[]>();
            //增加账户变动日志
            string strSql = @"INSERT INTO plat_account_log (c_time,plat_account_id,plat_account_type,TYPE,before_amount,after_amount,change_amount,account_source_id,from_id)
                                SELECT ?c_time,?id,2,1,amount,amount+?amount,?amount,14,?creator_id
                                FROM plat_account
                                WHERE id=?id;";

            DbParameter[] parameters = new MySqlParameter[]
            {
					new MySqlParameter("?c_time", MySqlDbType.DateTime),
                    new MySqlParameter("?amount", MySqlDbType.Decimal),
                    new MySqlParameter("?id", MySqlDbType.Int32),
                    new MySqlParameter("?creator_id", MySqlDbType.Int32)
			};
            parameters[0].Value = c_time;
            parameters[1].Value = amount;
            parameters[2].Value = plataccountid;
            parameters[3].Value = creator_id;
            strSql_array.Add(strSql);
            parameters_array.Add(parameters);
            //增加余额
            strSql = @"UPDATE plat_account SET amount=amount+?amount,amount_recharge=amount_recharge+?amount WHERE id=?id;";
            parameters = new MySqlParameter[]
            {
                    new MySqlParameter("?amount", MySqlDbType.Decimal),
                    new MySqlParameter("?id", MySqlDbType.Int32)
			};
            parameters[0].Value = amount;
            parameters[1].Value = plataccountid;

            strSql_array.Add(strSql);
            parameters_array.Add(parameters);


            return DataBaseOperator.MoneyWriteDbHelper.ExecutMultiSQLwithTransaction(strSql_array, parameters_array);
        }
        /// <summary>
        /// 活动充值记录
        /// </summary>
        /// <param name="s_ctime"></param>
        /// <param name="e_ctime"></param>
        /// <returns></returns>
        public DataTable ActRechargeRec(DateTime s_ctime, DateTime e_ctime)
        {
            string strSql = @"SELECT b.c_time,b.change_amount amount,b.from_id
                                FROM plat_account a
                                INNER JOIN plat_account_log b ON a.id=b.plat_account_id
                                WHERE a.type=2 AND (?s_ctime='1900-01-01' OR b.c_time>=?s_ctime) AND (?e_ctime='9999-01-02' OR b.c_time<?e_ctime) ORDER BY b.c_time DESC; ";
            MySqlParameter[] parameters = {
					new MySqlParameter("?s_ctime", MySqlDbType.DateTime),
                    new MySqlParameter("?e_ctime", MySqlDbType.DateTime)
			};
            parameters[0].Value = s_ctime;
            parameters[1].Value = e_ctime;
            DataTable dt = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql, "获取活动充值记录", parameters).Tables[0];
            return dt;
        }
        /// <summary>
        /// 活动支付记录
        /// </summary>
        /// <param name="s_ctime"></param>
        /// <param name="e_ctime"></param>
        /// <returns></returns>
        public DataTable ActPaidRec(DateTime s_ctime, DateTime e_ctime)
        {
            string strSql = @"SELECT type,SUM(interest_paid) amount FROM (
                                SELECT '加息券' type,interest_paid 
                                FROM ord_pjt_interestpaid
                                WHERE type=1 AND (?s_ctime='1900-01-01' OR c_time>=?s_ctime) AND (?e_ctime='9999-01-02' OR c_time<?e_ctime)
                                UNION ALL
                                SELECT '特享金' type,interest_paid 
                                FROM user_special_interestpaid
                                WHERE (?s_ctime='1900-01-01' OR c_time>=?s_ctime) AND (?e_ctime='9999-01-02' OR c_time<?e_ctime)
                                ) AS t1 GROUP BY type; ";
            MySqlParameter[] parameters = {
					new MySqlParameter("?s_ctime", MySqlDbType.DateTime),
                    new MySqlParameter("?e_ctime", MySqlDbType.DateTime)
			};
            parameters[0].Value = s_ctime;
            parameters[1].Value = e_ctime;
            DataTable dt = DataBaseOperator.MoneyReadDbHelper.ExecuteDataSet(strSql, "获取活动支付记录", parameters).Tables[0];
            return dt;
        }
        #endregion

    }
}
