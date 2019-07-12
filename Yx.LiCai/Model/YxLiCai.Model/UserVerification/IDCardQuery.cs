/*
 * 身份证验证
 * 作者：侯裕祥
 * 时间：2015年6月16日 09:53:53
 * 版本：1.0.0.0
 * 
 */
using System.Collections.Generic;

namespace YxLiCai.Model.UserVerification
{
    /// <summary>
    /// 用户身份证验证(提交需要带access_token)
    /// </summary>
    public class IDCardQuery
    {
        public string access_token { get; set; }
        public List<IDCardNo> query { get; set; }
    }

}
