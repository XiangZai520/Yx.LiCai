using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YxLiCai.Dao.User;
using YxLiCai.Model;
using YxLiCai.Model.User;

namespace YxLiCai.Server.User
{
    /// <summary>
    /// 用户银行卡业务层
    /// </summary>
    public class UserBankCardService
    {

        UserBankCardDao dao = new UserBankCardDao();
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="UserID">用户id</param>
        /// <returns></returns>
        public ResultInfo<UserBankCardModel> GetEntity(long UserID)
        {
            ResultInfo<UserBankCardModel> result = new ResultInfo<UserBankCardModel>();
            UserBankCardModel entity = null;
            result.Result = false;
            try
            {
                DataSet ds = dao.GetEntity(UserID);
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        entity = new UserBankCardModel();
                        //Id,UserId,Bank,BankName,BankCard,BankRegion,BankAddress,BankCode,FirstNum,LastNum,BankPhone,Requestid,Status
                        entity.Id = Convert.ToInt32(dr["Id"]);
                        entity.UserId = Convert.ToInt32(dr["UserId"]);
                        entity.Bank = Convert.ToInt32(dr["Bank"]);
                        entity.BankName = Convert.ToString(dr["BankName"]);
                        entity.BankCard = Convert.ToString(dr["BankCard"]);
                        entity.BankRegion = Convert.ToString(dr["BankRegion"]);
                        entity.BankAddress = Convert.ToString(dr["BankAddress"]);
                        entity.BankCode = Convert.ToString(dr["BankCode"]);
                        entity.FirstNum = Convert.ToString(dr["FirstNum"]);
                        entity.LastNum = Convert.ToString(dr["LastNum"]);
                        entity.BankPhone = Convert.ToString(dr["BankPhone"]);
                        entity.Requestid = Convert.ToString(dr["Requestid"]);
                        entity.Status = Convert.ToInt32(dr["Status"]);
                    }
                }

                result.Result = true;
                result.Data = entity;
            }
            catch (Exception ex)
            {
                YxLiCai.Tools.LogHelper.LogWriterFromFilter(ex);
                result.Data = null;
                result.Result = false;
            }

            return result;
        }
        /// <summary>
        /// 解除绑定银行卡
        /// </summary>
        /// <param name="UserID">用户id</param>
        /// <returns></returns>
        public ResultInfo<bool> UnbindBankCard(long UserID)
        {
            ResultInfo<bool> result = new ResultInfo<bool>();
            result.Result = false;
            try
            {
                result.Result = true;
                result.Data = dao.UnbindBankCard(UserID);
            }
            catch (Exception ex)
            {
                YxLiCai.Tools.LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
                result.Data = false;
            }
            return result;
        }
    }
}
