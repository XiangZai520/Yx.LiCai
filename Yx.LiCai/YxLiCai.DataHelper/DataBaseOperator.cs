using System;
using System.Collections.Generic;
using System.Text;

namespace YxLiCai.DataHelper
{
    public class DataBaseOperator
    {
        /// <summary>
        /// 禁止实例化
        /// </summary>
        private DataBaseOperator()
        {
            //可以考虑增加一个默认的实例
        }
        private static object lockObject = new object();

        private static volatile YxLiCai.DataHelper.Common.DbHelper _dbMySqlRead;
        private static volatile YxLiCai.DataHelper.Common.DbHelper _dbMySqlWrite;

        #region 财务数据库 
        /// <summary>
        /// 财务数据库写入权限
        /// </summary>
        public static YxLiCai.DataHelper.Common.DbHelper MoneyReadDbHelper
        {
            get
            {
                if (_dbMySqlRead == null)
                {
                    lock (lockObject)
                    {
                        if (_dbMySqlRead == null)
                        {
                            _dbMySqlRead = new DataHelper.DBUtility.MySqlHelper("MoneyReadMySqlDBConnectionString");//获取连接字符串
                        }
                    }
                }
                return _dbMySqlRead;
            }
        }
        /// <summary>
        /// 财务数据库写入权限
        /// </summary>
        public static YxLiCai.DataHelper.Common.DbHelper MoneyWriteDbHelper
        {
            get
            {
                if (_dbMySqlWrite == null)
                {
                    lock (lockObject)
                    {
                        if (_dbMySqlWrite == null)
                        {
                            _dbMySqlWrite = new DataHelper.DBUtility.MySqlHelper("MoenyWriteMySqlDBConnectionString");//获取连接字符串
                        }
                    }
                }
                return _dbMySqlWrite;
            }
        }
        #endregion

        #region 用户数据库

        private static volatile YxLiCai.DataHelper.Common.DbHelper _userWrite;

        private static volatile YxLiCai.DataHelper.Common.DbHelper _yxLiCalUserRead;

        /// <summary>
        /// YxLiCalUser写入权限
        /// </summary>
        public static YxLiCai.DataHelper.Common.DbHelper YxLiCalUserWrite
        {
            get
            {
                if (_userWrite == null)
                {
                    lock (lockObject)
                    {
                        if (_userWrite == null)
                        {
                            _userWrite = new DataHelper.DBUtility.MySqlHelper("MoenyWriteMySqlDBConnectionString");//获取连接字符串
                        }
                    }
                }
                return _userWrite;
            }
        }
        /// <summary>
        /// YxLiCalUser读取权限
        /// </summary>
        public static YxLiCai.DataHelper.Common.DbHelper YxLiCalUserRead
        {
            get
            {
                if (_yxLiCalUserRead == null)
                {
                    lock (lockObject)
                    {   
                        if (_yxLiCalUserRead == null)
                        {
                            _yxLiCalUserRead = new DataHelper.DBUtility.MySqlHelper("MoneyReadMySqlDBConnectionString");//获取连接字符串
                        }
                    }
                }
                return _yxLiCalUserRead;
            }
        }
        #endregion 

        #region 后台数据库

        private static volatile YxLiCai.DataHelper.Common.DbHelper _yxLiCaiAdminWrite;

        private static volatile YxLiCai.DataHelper.Common.DbHelper _YxLiCalAdminRead;

        /// <summary>
        /// 后台数据库写入权限
        /// </summary>
        public static YxLiCai.DataHelper.Common.DbHelper AdminDbHelperWrite
        {
            get
            {
                if (_yxLiCaiAdminWrite == null)
                {
                    lock (lockObject)
                    {
                        if (_yxLiCaiAdminWrite == null)
                        {
                            _yxLiCaiAdminWrite = new DataHelper.DBUtility.MySqlHelper("AdminWriteMySqlDBConnectionString");//获取连接字符串
                        }
                    }
                }
                return _yxLiCaiAdminWrite;
            }
        }
        /// <summary>
        /// 后台数据库读取权限
        /// </summary>
        public static YxLiCai.DataHelper.Common.DbHelper AdminDbHelperRead
        {
            get
            {
                if (_YxLiCalAdminRead == null)
                {
                    lock (lockObject)
                    {
                        if (_YxLiCalAdminRead == null)
                        {
                            _YxLiCalAdminRead = new DataHelper.DBUtility.MySqlHelper("AdminReadMySqlDBConnectionString");//获取连接字符串
                        }
                    }
                }
                return _YxLiCalAdminRead;
            }
        }
        #endregion 
    }
}
