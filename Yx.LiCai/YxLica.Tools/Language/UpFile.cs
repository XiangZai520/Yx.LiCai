using System;

namespace YxLiCai.Tools.Language
{
    /// <summary>
    /// 上传文件报错，包括中文和英文的错误信息。
    /// </summary>
    public class UpFile
    {
        /// <summary>
        /// 上传的文件太大
        /// </summary>
        public string Error_FileLength;
        /// <summary>
        /// 上传的文件格式不正确
        /// </summary>
        public string Error_FileFormat;
        /// <summary>
        /// 系统错误
        /// </summary>
        public string Error_System;
        /// <summary>
        /// 文件上传成功
        /// </summary>
        public string Error_Succeed;
        /// <summary>
        /// 没有选择上传的文件或者选择的文件路径不正确
        /// </summary>
        public string Error_NoFile;

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="Att">1为中文，2为英文</param>
        public UpFile(int Att)
        {
            if (Att == 1)
            {
                this.set_Cn();
            }
            else
            {
                this.set_En();
            }
        }
        /// <summary>
        /// 构造方法，返回错误信息为中文
        /// </summary>
        public UpFile()
        {
            this.set_Cn();
        }

        private void set_Cn()
        {
            this.Error_FileLength = "上传的文件太大";
            this.Error_FileFormat = "上传的文件格式不正确";
            this.Error_System = "系统错误";
            this.Error_Succeed = "文件上传成功";
            this.Error_NoFile = "没有选择上传的文件或者选择的文件路径不正确";
        }
        private void set_En()
        {
            this.Error_FileLength = "上传的文件太大";
            this.Error_FileFormat = "上传的文件格式不正确";
            this.Error_System = "系统错误";
            this.Error_Succeed = "文件上传成功";
            this.Error_NoFile = "没有选择上传的文件或者选择的文件路径不正确";
        }
    }
}