using System;

namespace YxLiCai.Tools.IOOper
{
    /// <summary>
    /// 用于生成微缩图的对象。
    /// 包括微缩图的宽、高、保存目录
    /// </summary>
    public class SmallImage
    {
        /// <summary>
        /// 宽度
        /// </summary>
        public int Width;
        /// <summary>
        /// 高度
        /// </summary>
        public int Heigth;
        /// <summary>
        /// 微缩图保存路径
        /// </summary>
        public string SaveFolder;

        /// <summary>
        /// 构造方法
        /// </summary>
        public SmallImage()
        {
            this.SaveFolder = "";
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_width">宽度</param>
        /// <param name="_hergth">高度</param>
        /// <param name="_savefolder">微缩图保存路径</param>
        public SmallImage(int _width, int _hergth, string _savefolder)
        {
            this.Width = _width;
            this.Heigth = _hergth;
            this.SaveFolder = _savefolder;
        }
    }
}