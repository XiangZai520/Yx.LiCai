using System;

namespace YxLiCai.Tools.IOOper
{
    /// <summary>
    /// 将web的虚拟路径转成物理路径。
    /// 适用于不能执行Server.MapPath方法的类调用
    /// </summary>
    public class DToF : System.Web.UI.Page
    {
        /// <summary>
        /// 将web的虚拟路径转成物理路径。
        /// </summary>
        /// <param name="path">虚拟路径如"/web/cn"</param>
        /// <returns></returns>
        public string DummyToFact(string path)
        {
            return Server.MapPath(path);
        }
    }
}