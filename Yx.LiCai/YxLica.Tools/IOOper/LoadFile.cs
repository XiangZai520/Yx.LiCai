using System;
using System.IO;
using System.Drawing;

namespace YxLiCai.Tools.IOOper
{
    /// <summary>
    /// 上传文件类。
    /// </summary>
    public class LoadFile
    {
        public int FileError;
        public string LoadFileFolder;
        public string LoadFileFolderSmall;
        public string LoadFileFolderTemp;
        public string LoadFileFolder_Users;

        public int width = 340;
        public int heigth = 340;

        private static string FileExtImg = "jpg|gif|jpeg|bmp";
        private static string FileExtNoImg = "swf|doc|rar|zip";
        private string FileExt = FileExtImg + "|" + FileExtNoImg;

        /// <summary>
        /// 构造方法
        /// </summary>
        public LoadFile()
        {
            FileError = 0;//没有发生错误
            this.LoadFileFolder = "";//上传的路径
            this.LoadFileFolderSmall = "";
            this.LoadFileFolderTemp = "";
            this.LoadFileFolder_Users = "";
        }

        /// <summary>
        /// 自动命名,返回文件名：年_月_日_时_分_秒_随机数.扩展名
        /// </summary>
        /// <param name="Ext">扩展名</param>
        /// <returns>返回文件名如：2005_10_1_12_10_10_2345.gif</returns>
        public string RandomFileName(string Ext)
        {
            return IOPublic.getRandomFileName(Ext);
        }

        /// <summary>
        /// 删除上传的图片以及其微缩图
        /// </summary>
        /// <param name="FileName">文件名</param>
        public void Delete_LoadFile(string FileName)
        {
            IOOper.FileOper.DeleteFile(IOOper.FolderOper.DummyToFact(LoadFileFolder + "/" + FileName));
            IOOper.FileOper.DeleteFile(IOOper.FolderOper.DummyToFact(LoadFileFolderSmall + "/" + FileName));
        }

        /// <summary>
        /// 上传文件，　isproduct为true　则生成微缩图并写入版权信息,把文件保存到临时目录
        /// </summary>
        /// <param name="UpFile">HtmlInputFile对象</param>
        /// <param name="isproduct">是否生成微缩图</param>
        /// <param name="FileCopyRight">叠加图片的地址</param>
        /// <returns></returns>
        public string UpLoadFile(System.Web.UI.HtmlControls.HtmlInputFile UpFile, bool isproduct, string FileCopyRight)
        {
            string Filename = "";
            if (UpFile.PostedFile != null && UpFile.PostedFile.ContentLength > 0)
            {
                Filename = this.RandomFileName(this.GetFileExt(UpFile.PostedFile.FileName));
                if (this.UploadFileFormat(Filename))//合法的文件类型
                {
                    string Folder = IOOper.FolderOper.DummyToFact(LoadFileFolder);
                    Filename = IOOper.FileOper.FileAutoReName(Folder, Filename);
                    UpFile.PostedFile.SaveAs(Folder + "\\" + Filename);
                    if (this.UploadFileFormatImg(Filename) && isproduct)
                    {
                        this.GetThumbnailImage(Folder + "\\" + Filename, FileCopyRight);
                    }
                    else { }
                }
            }
            return Filename;
        }

        /// <summary>
        /// 上传并生成微缩图，返回文件名
        /// </summary>
        /// <param name="UpFile">HtmlInputFile对象</param>
        /// <param name="width">微缩图宽度</param>
        /// <param name="height">微缩图宽度</param>
        /// <returns></returns>
        public string UpLoadFile(System.Web.UI.HtmlControls.HtmlInputFile UpFile, int width, int height)
        {
            string Filename = "";
            if (UpFile.PostedFile != null && UpFile.PostedFile.ContentLength > 0)
            {
                Filename = this.RandomFileName(this.GetFileExt(UpFile.PostedFile.FileName));
                if (this.UploadFileFormat(Filename))//合法的文件类型
                {
                    string Folder = IOOper.FolderOper.DummyToFact(LoadFileFolder);
                    Filename = IOOper.FileOper.FileAutoReName(Folder, Filename);
                    UpFile.PostedFile.SaveAs(Folder + "\\" + Filename);
                    if (this.UploadFileFormatImg(Filename))
                    {
                        this.GetThumbnailImage(Folder + "\\" + Filename, width, height);
                    }
                }
            }
            return Filename;
        }

        /// <summary>
        /// 生成带版权的微缩图,并将版权写入原图片
        /// </summary>
        /// <param name="imgpath">要生成的原图路径</param>
        /// <param name="Copyright"></param>
        public void GetThumbnailImage(string imgpath, string Copyright)
        {
            System.Drawing.Image img = System.Drawing.Image.FromFile(imgpath);
            int wimgwidth = img.Width;
            int wimgheight = img.Height;
            if (wimgwidth > width || wimgheight > heigth)
            {
                double a = wimgwidth / width;
                double b = wimgheight / heigth;
                if (b > a) a = b;
                wimgwidth = Convert.ToInt32(wimgwidth / a);
                wimgheight = Convert.ToInt32(wimgheight / a);
            }
            if (Copyright != "")
            {
                Bitmap output = new Bitmap(img);
                Graphics g = Graphics.FromImage(output);
                Font font = new Font("Arial Black", 14);
                //System.Drawing.Drawing2D.LinearGradientBrush myBrush=new System.Drawing.Drawing2D.LinearGradientBrush(new System.Drawing.Rectangle(20,20,100,100),Color.Red,Color.Gray, System.Drawing.Drawing2D.LinearGradientMode.Horizontal);
                g.DrawString(Copyright, font, new SolidBrush(Color.Blue), 10, 10);
                FileOper.DeleteFile(imgpath);
                img.Dispose();
                output.Save(imgpath, this.GetImgesForm(imgpath));
                img = System.Drawing.Image.FromFile(imgpath);
            }
            System.Drawing.Image imgsmall = img.GetThumbnailImage(wimgwidth, wimgheight, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
            //imgsmall.SetPropertyItem(img.GetPropertyItem(1));
            string Filename = this.GetFilename(imgpath);
            imgsmall.Save(FolderOper.DummyToFact(LoadFileFolderSmall) + "\\" + Filename, this.GetImgesForm(Filename));
            img.Dispose();
            imgsmall.Dispose();
        }

        /// <summary>
        /// 生成微缩图并代替原图
        /// </summary>
        /// <param name="imgpath">原图地址</param>
        /// <param name="width">宽</param>
        /// <param name="heigth">高</param>
        public void GetThumbnailImage(string imgpath, int width, int heigth)
        {
            System.Drawing.Image img = System.Drawing.Image.FromFile(imgpath);
            int wimgwidth = img.Width;
            int wimgheight = img.Height;
            if (wimgwidth > width || wimgheight > heigth)
            {
                double a = wimgwidth / width;
                double b = wimgheight / heigth;
                if (b > a) a = b;
                wimgwidth = Convert.ToInt32(wimgwidth / a);
                wimgheight = Convert.ToInt32(wimgheight / a);
            }
            System.Drawing.Image imgsmall = img.GetThumbnailImage(wimgwidth, wimgheight, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
            //imgsmall.SetPropertyItem(img.GetPropertyItem(1));
            string Filename = this.GetFilename(imgpath);
            FileOper.DeleteFile(imgpath);//删除原图
            img.Dispose();
            imgsmall.Save(imgpath, this.GetImgesForm(Filename));
            imgsmall.Dispose();
        }


        bool ThumbnailCallback()
        {
            return true;
        }



        /// <summary>
        /// 上传的文件类型是否合法
        /// </summary>
        /// <param name="Filename">文件名</param>
        /// <returns></returns>
        public bool UploadFileFormat(string Filename)
        {
            bool b = false;
            try
            {
                string TheFileExt = this.GetFileExt(Filename);
                string[] FileExtArray = FileExt.Split(new char[] { '|' });
                for (int i = 0; i < FileExtArray.Length; i++)
                {
                    if (TheFileExt == FileExtArray[i])
                    {
                        b = true;
                        break;
                    }
                }
            }
            catch { }
            return b;
        }

        //是否是图片
        public bool UploadFileFormatImg(string Filename)
        {
            bool b = false;
            try
            {
                string TheFileExt = this.GetFileExt(Filename);
                string[] FileExtArray = FileExtImg.Split(new char[] { '|' });
                for (int i = 0; i < FileExtArray.Length; i++)
                {
                    if (TheFileExt == FileExtArray[i])
                    {
                        b = true;
                        break;
                    }
                }
            }
            catch { }
            return b;
        }

        //返回图片的编码
        public System.Drawing.Imaging.ImageFormat GetImgesForm(string Filename)
        {
            System.Drawing.Imaging.ImageFormat imgform = null;
            string[] TheFile = Filename.Split(new char[] { '.' });
            string TheFileExt = TheFile[TheFile.Length - 1].ToLower();
            if (TheFileExt == "gif")
            {
                imgform = System.Drawing.Imaging.ImageFormat.Gif;
            }
            else if (TheFileExt == "jpg" || TheFileExt == "jpeg")
            {
                imgform = System.Drawing.Imaging.ImageFormat.Jpeg;
            }
            else if (TheFileExt == "png")
            {
                imgform = System.Drawing.Imaging.ImageFormat.Png;
            }
            else if (TheFileExt == "bmp")
            {
                imgform = System.Drawing.Imaging.ImageFormat.Bmp;
            }
            return imgform;
        }


        //取得一个URL的目录部分
        public string GetFolderOnly(string path)
        {
            string Folder = "";
            if (path != "")
            {
                string[] foldera;
                foldera = path.Split(new char[] { '\\' });
                for (int i = 0; i < foldera.Length - 2; i++)
                {
                    if (Folder != "")
                    {
                        Folder = foldera[i];
                    }
                    else
                    {
                        Folder += "\\" + foldera[i];
                    }
                }
            }
            return Folder;

        }

        //取得一个URL的最后部分文件名+扩展名
        public string GetFilename(string folder)
        {
            string Filename = "";
            if (folder != "")
            {
                string[] foldera;
                foldera = folder.Split(new char[] { '\\' });
                Filename = foldera[(foldera.Length - 1)];
            }
            return Filename;
        }

        //取得一个URL的最后部分扩展名
        public string GetFileOnlyName(string Filename)
        {
            string FileName = "";
            if (Filename != "")
            {
                FileName = Filename.Split(new char[] { '.' })[0];
            }
            return FileName;
        }

        //取得一个URL的最后部分扩展名
        public string GetFileExt(string path)
        {
            string FileExt = "";
            if (path != "")
            {
                string[] patha = path.Split(new char[] { '.' });
                FileExt = patha[patha.Length - 1];
            }
            return FileExt;
        }
    }
}