/*
 * 身份证验证
 * 作者：侯裕祥
 * 时间：2015年6月16日 09:53:53
 * 版本：1.0.0.0
 * 
 */
namespace YxLiCai.Model.UserVerification
{
    /// <summary>
    /// 身份验证成功时返回的数据
    /// </summary>
    public class People_IDCard
    {
        public string name { get; set; }
        public string id_number { get; set; }
        public string sub_client { get; set; }
        public string type { get; set; }
        public string location { get; set; }
        public int id_status { get; set; }
    }
}
