using System;
using System.IO;
using System.Drawing; 
using System.Text;
using System.Configuration;
using System.Collections.Generic;

namespace YxLiCai.Tools.IOOper
{
    /// <summary>
    /// 文件上传，可生成微缩图，可添加文字、图片版权信息。
    /// </summary>
    public class UpLoadFile
    {
        /// <summary>
        /// 错误码1：上传的文件太大
        /// </summary>
        public const int Error_FileLength = 1;
        /// <summary>
        /// 错误码2：不允许上传的文件格式
        /// </summary>
        public const int Error_FileFormat = 2;
        /// <summary>
        /// 错误码3：没有选择要文件或者选择的文件路径不正确
        /// </summary>
        public const int Error_NoFile = 3;
        /// <summary>
        /// 错误码9：系统错误
        /// </summary>
        public const int Error_System = 9;
        /// <summary>
        /// 操作成功
        /// </summary>
        public const int Error_Succeed = 0;

        /// <summary>
        /// 可上传文件的扩展名，如"swf|doc|rar|zip|xls|mdb|txt|pdf"
        /// </summary>
        public string FileExt = IOPublic.FileExt;
        /// <summary>
        /// 是否是图片
        /// </summary>
        public bool isImg = true;
        /// <summary>
        /// HtmlInputFile对象
        /// </summary>
        public System.Web.UI.HtmlControls.HtmlInputFile InputFile;
        /// <summary>
        /// HttpPostedFile对象
        /// </summary>
        public System.Web.HttpPostedFile PostedFile;
        /// <summary>
        /// 保存路径(文件夹)
        /// </summary>
        public string SaveForder;
        /// <summary>
        /// 用于生成微缩图的对象的数组
        /// </summary>
        public IOOper.SmallImage[] SmallImg;
        /// <summary>
        /// 版权图片的地址
        /// </summary>
        public string CopyRightImgPath;
        /// <summary>
        /// 版权图片对象
        /// </summary>
        public Image CopyRightImg;

        /// <summary>
        /// 是否在图片中写入文字版权
        /// </summary>
        public bool isWriteTxt = false;

        /// <summary>
        /// 写入文字版权的信息
        /// </summary>
        public string CopyRightTxt = "";

        /// <summary>
        /// 文件名
        /// </summary>
        private string FileName;
        /// <summary>
        /// 返回的错误码
        /// </summary>
        private int ErrorValue = Error_Succeed;
        /// <summary>
        /// 上传文件的最大大小，默认为2M
        /// </summary>
        private int FileLength = 1024 * 1024 * 2;//默认2MB

        /// <summary>
        /// 文件上传大小
        /// </summary>
        public int FileSize;
 
        /// <summary>
        /// 文件上传,构造方法
        /// </summary>
        /// <param name="InputFile">HtmlInputFile对象</param>
        public UpLoadFile(System.Web.UI.HtmlControls.HtmlInputFile InputFile)
        {
            this.InputFile = InputFile;
        }

        /// <summary>
        /// 文件上传,构造方法
        /// </summary>
        /// <param name="InputFile">HtmlInputFile对象</param>
        /// <param name="SaveForder">保存路径</param>
        public UpLoadFile(System.Web.UI.HtmlControls.HtmlInputFile InputFile, string SaveForder)
        {
            this.InputFile = InputFile;
            this.SaveForder = SaveForder;
        }

        /// <summary>
        /// 文件上传,构造方法
        /// </summary>
        /// <param name="InputFile">HtmlInputFile对象</param>
        /// <param name="SaveForder">保存路径</param>
        /// <param name="SmallImg">微缩图对象数组</param>
        public UpLoadFile(System.Web.UI.HtmlControls.HtmlInputFile InputFile, string SaveForder, IOOper.SmallImage[] SmallImg)
        {
            this.InputFile = InputFile;
            this.SaveForder = SaveForder;
            this.SmallImg = SmallImg;
        }

        /// <summary>
        /// 文件上传,构造方法
        /// </summary>
        /// <param name="InputFile">HtmlInputFile对象</param>
        /// <param name="SaveForder">保存路径</param>
        /// <param name="SmallImg">微缩图对象数组</param>
        /// <param name="CopyRightImgPath">版权图片的地址</param>
        public UpLoadFile(System.Web.UI.HtmlControls.HtmlInputFile InputFile, string SaveForder, IOOper.SmallImage[] SmallImg, string CopyRightImgPath)
        {
            this.InputFile = InputFile;
            this.SaveForder = SaveForder;
            this.CopyRightImgPath = CopyRightImgPath;
            this.CopyRightImg = System.Drawing.Image.FromFile(IOOper.FolderOper.DummyToFact(CopyRightImgPath));
            this.SmallImg = SmallImg;
        }
        /// <summary>
        /// 文件上传,构造方法
        /// </summary>
        /// <param name="InputFile">HtmlInputFile对象</param>
        /// <param name="SaveForder">保存路径</param>
        /// <param name="SmallImg">微缩图对象数组</param>
        /// <param name="isWriteTxt">是否在图片中写入文字版权</param>
        /// <param name="CopyRightTxt">版权文字</param>
        public UpLoadFile(System.Web.UI.HtmlControls.HtmlInputFile InputFile, string SaveForder, IOOper.SmallImage[] SmallImg, bool isWriteTxt, string CopyRightTxt)
        {
            this.InputFile = InputFile;
            this.SaveForder = SaveForder;
            this.SmallImg = SmallImg;
            this.isWriteTxt = isWriteTxt;
            this.CopyRightTxt = CopyRightTxt;
        }

        /// <summary>
        /// 文件上传,构造方法
        /// </summary>
        /// <param name="InputFile">HtmlInputFile对象</param>
        /// <param name="SaveForder">保存路径</param>
        /// <param name="SmallImg">微缩图对象数组</param>
        /// <param name="CopyRightImg">版权图片对象</param>
        public UpLoadFile(System.Web.UI.HtmlControls.HtmlInputFile InputFile, string SaveForder, IOOper.SmallImage[] SmallImg, Image CopyRightImg)
        {
            this.InputFile = InputFile;
            this.SaveForder = SaveForder;
            this.CopyRightImg = CopyRightImg;
            this.SmallImg = SmallImg;
        }

        /// <summary>
        /// HtmlInputFile上传文件
        /// </summary>
        /// <returns></returns>
        public string upLoading()
        {
            this.FileName = "";
            try
            {
                if (this.InputFile != null && this.InputFile.PostedFile.ContentLength > 0)
                {
                    if (!(this.InputFile.PostedFile.ContentLength > this.FileLength))
                    {
                        this.FileSize = this.InputFile.PostedFile.ContentLength;
                        this.FileName = IOOper.IOPublic.getRandomFileName(IOPublic.getFileExt(this.InputFile.PostedFile.FileName));


                        System.IO.Stream imgStream = this.InputFile.PostedFile.InputStream;
                        //是否是图片
                        if (this.isImg)
                        {
                            if (IOOper.IOPublic.uploadFileIsImg(this.FileName))
                            {
                                Image img = System.Drawing.Image.FromStream(imgStream);
                                if (this.CopyRightImgPath != null && this.CopyRightImgPath != "")
                                {
                                    //添加版权
                                    img = IOOper.IOPublic.writeCopyRight(img, CopyRightImg);
                                }

                                //写入文字版权
                                if (this.isWriteTxt)
                                {
                                    img = IOOper.IOPublic.writeCopyRightTxt(img, this.CopyRightTxt);
                                }

                                if (this.SmallImg != null && this.SmallImg.Length > 0)
                                {
                                    //生成微缩图
                                    IOOper.IOPublic.writeThumbnailImage(img, this.SmallImg, this.FileName);
                                }
                                //文件夹是否存在
                                if (!IOOper.FolderOper.FolderExists(this.SaveForder)) IOOper.FolderOper.CreateFolders(this.SaveForder);
                                //保存文件
                                img.Save(this.SaveForder + "\\" + this.FileName, IOOper.IOPublic.getImgesFormat(this.FileName));
                            }
                            else
                            {
                                this.ErrorValue = Error_FileFormat;
                                //this.InputFile.PostedFile.SaveAs(this.SaveForder+"\\"+this.FileName);
                            }
                        }
                        else
                        {
                            if (IOOper.IOPublic.uploadRightFormat(this.FileName, this.FileExt))
                            {
                                if (!IOOper.FolderOper.FolderExists(this.SaveForder))
                                {
                                    IOOper.FolderOper.CreateFolder(this.SaveForder);
                                }
                                this.InputFile.PostedFile.SaveAs(this.SaveForder + "\\" + this.FileName);
                            }
                            else
                            {
                                this.ErrorValue = Error_FileFormat;
                            }
                        }
                        imgStream.Close();
                    }
                    else
                    {
                        this.ErrorValue = Error_FileLength;
                    }
                }
                else
                {
                    //this.ErrorValue=Error_NoFile;
                }
            }
            catch
            {
                //this.SaveForder
                //this.System_Error="StackTrace:"+ex.StackTrace;
                this.ErrorValue = Error_System;
            }
            return FileName;
        }

        /// <summary>
        /// HttpPostedFile上传文件
        /// </summary>
        /// <returns></returns>
        public string upLoading(string s)
        {
            this.FileName = "";
            try
            {
                if (this.PostedFile != null && this.PostedFile.ContentLength > 0)
                {
                    if (!(this.PostedFile.ContentLength > this.FileLength))
                    {
                        this.FileSize = this.PostedFile.ContentLength;
                        this.FileName = IOOper.IOPublic.getRandomFileName(IOPublic.getFileExt(this.PostedFile.FileName));


                        System.IO.Stream imgStream = this.PostedFile.InputStream;
                        //是否是图片
                        if (this.isImg)
                        {
                            if (IOOper.IOPublic.uploadFileIsImg(this.FileName))
                            {
                                Image img = System.Drawing.Image.FromStream(imgStream);
                                if (this.CopyRightImgPath != null && this.CopyRightImgPath != "")
                                {
                                    //添加版权
                                    img = IOOper.IOPublic.writeCopyRight(img, CopyRightImg);
                                }

                                //写入文字版权
                                if (this.isWriteTxt)
                                {
                                    img = IOOper.IOPublic.writeCopyRightTxt(img, this.CopyRightTxt);
                                }

                                if (this.SmallImg != null && this.SmallImg.Length > 0)
                                {
                                    //生成微缩图
                                    IOOper.IOPublic.writeThumbnailImage(img, this.SmallImg, this.FileName);
                                }
                                //文件夹是否存在
                                if (!IOOper.FolderOper.FolderExists(this.SaveForder)) IOOper.FolderOper.CreateFolders(this.SaveForder);
                                //保存文件
                                img.Save(this.SaveForder + "\\" + this.FileName, IOOper.IOPublic.getImgesFormat(this.FileName));
                            }
                            else
                            {
                                this.ErrorValue = Error_FileFormat;
                                //this.InputFile.PostedFile.SaveAs(this.SaveForder+"\\"+this.FileName);
                            }
                        }
                        else
                        {
                            if (IOOper.IOPublic.uploadRightFormat(this.FileName, this.FileExt))
                            {
                                if (!IOOper.FolderOper.FolderExists(this.SaveForder))
                                {
                                    IOOper.FolderOper.CreateFolder(this.SaveForder);
                                }
                                this.PostedFile.SaveAs(this.SaveForder + "\\" + this.FileName);
                            }
                            else
                            {
                                this.ErrorValue = Error_FileFormat;
                            }
                        }
                        imgStream.Close();
                    }
                    else
                    {
                        this.ErrorValue = Error_FileLength;
                    }
                }
                else
                {
                    //this.ErrorValue=Error_NoFile;
                }
            }
            catch
            {
                //this.SaveForder
                //this.System_Error="StackTrace:"+ex.StackTrace;
                this.ErrorValue = Error_System;
            }
            return FileName;
        }

        /// <summary>
        /// 上传指定大小的图片
        /// </summary>
        public string upLoadingThumbnailImage()
        {
            this.FileName = "";
            try
            {
                if (this.InputFile != null && this.InputFile.PostedFile.ContentLength > 0)
                {
                    if (!(this.InputFile.PostedFile.ContentLength > this.FileLength))
                    {
                        this.FileSize = this.InputFile.PostedFile.ContentLength;
                        this.FileName = IOOper.IOPublic.getRandomFileName(IOPublic.getFileExt(this.InputFile.PostedFile.FileName));
                        //this.SaveForder=IOOper.FolderOper.DummyToFact(SaveForder);
                        System.IO.Stream imgStream = this.InputFile.PostedFile.InputStream;

                        //是否是图片
                        if (IOOper.IOPublic.uploadFileIsImg(this.FileName))
                        {
                            Image img = System.Drawing.Image.FromStream(imgStream);
                            if (this.CopyRightImgPath != null && this.CopyRightImgPath != "")
                            {
                                //添加版权
                                img = IOOper.IOPublic.writeCopyRight(img, CopyRightImg);
                            }

                            //写入文字版权
                            if (this.isWriteTxt)
                            {
                                img = IOOper.IOPublic.writeCopyRightTxt(img, this.CopyRightTxt);
                            }

                            if (this.SmallImg != null && this.SmallImg.Length > 0)
                            {
                                //生成微缩图
                                IOOper.IOPublic.writeThumbnailImage(img, this.SmallImg, this.FileName);
                            }
                        }
                        else
                        {
                            this.ErrorValue = Error_FileFormat;
                        }
                        imgStream.Close();
                    }
                    else
                    {
                        this.ErrorValue = Error_FileLength;
                    }
                }
            }
            catch { this.ErrorValue = Error_System; }
            return FileName;
        }

        /// <summary>
        /// 返回错误码
        /// </summary>
        /// <returns></returns>
        public int getErrorValue()
        {
            return this.ErrorValue;
        }


        /// <summary>
        /// 设置上传文件的最大长度
        /// </summary>
        /// <param name="FileLength">文件的最大长度，以字节为单位</param>
        public void setFileLength(int FileLength)
        {
            this.FileLength = FileLength;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            this.CopyRightImg = null;
            this.CopyRightImgPath = null;
            this.CopyRightTxt = null;
            this.FileExt = null;
            this.FileLength = 0;
            this.FileName = null;
            this.FileSize = 0;
            this.InputFile = null;
            this.SaveForder = null;
            this.SmallImg = null;
        }
    }
}