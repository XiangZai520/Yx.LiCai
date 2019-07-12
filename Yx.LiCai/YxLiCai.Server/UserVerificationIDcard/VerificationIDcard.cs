/*
 * 身份证验证业务工具
 * 作者：侯裕祥
 * 时间：2015年6月16日 09:53:53
 * 版本：1.0.0.0
 * 
 */
using System;
using System.Collections.Generic;
using YxLiCai.Model;
using YxLiCai.Tools;
using YxLiCai.Tools.Util;
namespace YxLiCai.Server.UserVerificationIDcard
{
    /// <summary>
    /// 身份证验证业务工具
    /// </summary>
    public class VerificationIDcard
    {
        #region 变量
        private readonly string IDCard = System.Configuration.ConfigurationManager.AppSettings["IDCard"];
        private readonly string IDCard_token = System.Configuration.ConfigurationManager.AppSettings["IDCard_token"];

        #endregion
        #region 公共方法
        /// <summary>
        /// 身证件实名认证
        /// </summary>
        /// <param name="Name">姓名</param>
        /// <param name="ID">身份证</param>
        /// <returns></returns>
        public ResultInfo<string> CheckUserIDCard(string Name, string ID)
        {
            ResultInfo<string> result = new ResultInfo<string>();
            result.Result = false;
            result.Message = "未知错误!";
            result.Data = string.Empty;
            try
            {
                YxLiCai.Model.UserVerification.IDCardQuery query = new YxLiCai.Model.UserVerification.IDCardQuery();
                query.access_token = IDCard_token;
                query.query = new List<YxLiCai.Model.UserVerification.IDCardNo>();
                query.query.Add(new YxLiCai.Model.UserVerification.IDCardNo(Name, ID));
                string jsonDate = YxLiCai.Tools.SerializeHelper.JsonSerializer(query);
                string strResult = UrlResponse.GetResponseString(IDCard, jsonDate);
                result.Data = strResult;
                result.Message = "执行成功!";
                result.Result = true;
            }
            catch (Exception ex)
            {                
                LogHelper.LogWriterFromFilter(ex);
                result.Result = false;
            }
            return result;

        }   
        #endregion
    }
}
