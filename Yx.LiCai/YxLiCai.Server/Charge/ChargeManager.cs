using System;
using System.Collections.Generic;
using System.Data;
using YxLiCai.Dao.ChargeManager;
using YxLiCai.Model.Charge;
using YxLiCai.Tools;

namespace YxLiCai.Server.Charge
{
    /// <summary>
    /// 充值管理 业务逻辑处理类
    /// add by lxm 2015年7月2日
    /// </summary>
    public class ChargeManager
    {
        private ChargeDAO _dao = new ChargeDAO();

        /// <summary>
        /// 获取充值提醒列表
        /// </summary>
        /// <returns></returns>
        public List<ChargeModel> GetChargeReminderList(string name, int uType, decimal amount_min, decimal amount_max)
        {
            var ds = _dao.GetChargeReminderList(name, uType, amount_min, amount_max);
            var list = new List<ChargeModel>();

            try
            {
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        list.Add(_dao.DataToModel(row));
                    }
                }
                return list;
            }
            catch (Exception e)
            {
                LogHelper.LogWriterFromFilter(e);
                return null;
            }
        }
    }
}
