using System;
using System.Data;
using System.Data.Common;
namespace YxLiCai.DataHelper.IDataHelper
{
    /// <summary>
    /// 共用方法
    /// </summary>
    public interface ICommand
    {

        #region 创建输入参数
        /// <summary>
        /// 创建输入参数
        /// </summary>
        /// <param name="ParamName">参数名称</param>
        ///<param name="dbType">数据类型</param>
        ///<param name="Size">数据大小</param>
        /// <param name="Value">参数值</param>
        /// <returns></returns>
        DbParameter MakeInParam(
            string ParamName,
            DbType dbType,
            int Size,
            object Value
            );
        #endregion

        #region 传入返回值参数
        /// <summary>
        /// 传入返回值参数
        /// </summary>
        /// <param name="ParamName">参数名称</param>
        ///<param name="dbType">数据类型</param>
        ///<param name="Size">数据大小</param>
        /// <returns></returns>
        DbParameter MakeOutParam(
            string ParamName,
            DbType dbType,
            int Size
            );
        #endregion
    }
}
