using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using YxLiCai.DataHelper;

namespace YxLiCai.Dao.User
{
    /// <summary>
    /// 用户银行卡数据层
    /// </summary>
    public class UserBankCardDao
    {
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="UserID">用户id</param>
        /// <returns></returns>
        public DataSet GetEntity(long UserID)
        {
            string strSql = @"SELECT Id Id,user_id UserId,bank Bank,bank_name BankName,bank_card BankCard,bank_region BankRegion,bank_address BankAddress,bank_code BankCode,first_num FirstNum,last_num LastNum,bank_phone BankPhone,request_id Requestid,status Status 
                              FROM user_bank_card WHERE user_id=?UserID AND status=1;";
            MySqlParameter[] parameters = {
					new MySqlParameter("?UserID", MySqlDbType.Int64,20)			
                                          };
            parameters[0].Value = UserID;

            return DataBaseOperator.YxLiCalUserRead.ExecuteDataSet(strSql, CommandType.Text, parameters);
        }
        /// <summary>
        /// 解除绑定银行卡
        /// </summary>
        /// <param name="UserID">用户id</param>
        /// <returns></returns>
        public bool UnbindBankCard(long UserID)
        {
            string strSql = @"UPDATE user_bank_card SET status=0 WHERE user_id=?UserID AND status=1;";
            MySqlParameter[] parameters = {
					new MySqlParameter("?UserID", MySqlDbType.Int64,20)			
                                          };
            parameters[0].Value = UserID;

            return DataBaseOperator.YxLiCalUserWrite.ExecuteNonQuery(strSql, CommandType.Text, parameters);
        }
    }
}
