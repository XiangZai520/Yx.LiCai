using System;

namespace YxLiCai.Tools.IOOper
{
    /// <summary>
    /// ��web������·��ת������·����
    /// �����ڲ���ִ��Server.MapPath�����������
    /// </summary>
    public class DToF : System.Web.UI.Page
    {
        /// <summary>
        /// ��web������·��ת������·����
        /// </summary>
        /// <param name="path">����·����"/web/cn"</param>
        /// <returns></returns>
        public string DummyToFact(string path)
        {
            return Server.MapPath(path);
        }
    }
}