using System;
using System.Data;
namespace YxLiCai.DataHelper.DataType
{
    /// <summary>
    /// 数据类型管理
    /// </summary>
    internal static class DbTypeHelper
    {

        #region 返回数据类型
        /// <summary>
        /// 返回数据类型
        /// </summary>
        /// <param name="dbType">数据类型</param>
        /// <returns></returns>
        public static DbType GetSqlServerType(
            string dbType
            )
        {
            DbType dbTypeResult=DbType.Object;
            switch (dbType)
            {
                case"nvarchar":
                case"ntext":
                    dbTypeResult= DbType.String;
                    break;
                case"varchar":
                case"text":
                    dbTypeResult= DbType.AnsiString;
                    break;
                case "int":
                    dbTypeResult= DbType.Int32;
                    break;
                case"datetime":
                case"smalldatetime":
                    dbTypeResult= DbType.DateTime;
                    break;
                case "smallint":
                    dbTypeResult = DbType.Int16;
                    break;
                case "tinyint":
                    dbTypeResult = DbType.Byte;
                    break;
                case"decimal":
                    dbTypeResult = DbType.Decimal;
                    break;
                case"bit":
                    dbTypeResult = DbType.Boolean;
                    break;
                case"char":
                    dbTypeResult = DbType.AnsiStringFixedLength;
                    break;
                case"nchar":
                    dbTypeResult = DbType.StringFixedLength;
                    break;
                case"bigint":
                    dbTypeResult = DbType.Int64;
                    break;
                case "varbinary":
                    dbTypeResult = DbType.Binary;
                    break;
                case"float":
                    dbTypeResult = DbType.Double;
                    break;

            }
            return dbTypeResult;
        }
        #endregion

    }
}
