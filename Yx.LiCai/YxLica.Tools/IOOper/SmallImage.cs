using System;

namespace YxLiCai.Tools.IOOper
{
    /// <summary>
    /// ��������΢��ͼ�Ķ���
    /// ����΢��ͼ�Ŀ��ߡ�����Ŀ¼
    /// </summary>
    public class SmallImage
    {
        /// <summary>
        /// ���
        /// </summary>
        public int Width;
        /// <summary>
        /// �߶�
        /// </summary>
        public int Heigth;
        /// <summary>
        /// ΢��ͼ����·��
        /// </summary>
        public string SaveFolder;

        /// <summary>
        /// ���췽��
        /// </summary>
        public SmallImage()
        {
            this.SaveFolder = "";
        }
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="_width">���</param>
        /// <param name="_hergth">�߶�</param>
        /// <param name="_savefolder">΢��ͼ����·��</param>
        public SmallImage(int _width, int _hergth, string _savefolder)
        {
            this.Width = _width;
            this.Heigth = _hergth;
            this.SaveFolder = _savefolder;
        }
    }
}