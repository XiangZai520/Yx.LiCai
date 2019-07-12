using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Model.User
{
    /// <summary>
    /// 实名认证发送的数据类别
    /// </summary>
    public class IDCard
    {
        /// <summary>
        /// 身份证姓名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string id_number { get; set; }
    }
}
