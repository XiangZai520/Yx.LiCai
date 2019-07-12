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
    /// 用户身份提交的实体信息类
    /// </summary>
    public class IDCardNo
    {
        public IDCardNo(string _name, string idnum)
        {
            this.name = _name;
            this.id_number = idnum;
            //this.sub_client = "";
            //this.type = "";
            //this.location = "";
        }
        public string name { get; set; }
        public string id_number { get; set; }
        //public string sub_client { get; set; }
        //public string type { get; set; }
        //public string location { get; set; }
    }
}
