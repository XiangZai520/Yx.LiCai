using System;
using System.IO;
using System.Drawing;

namespace YxLiCai.Tools.IOOper
{
    /// <summary>
    /// 文件与文件夹相关方法。
    /// </summary>
    public class IOPublic
    {
        /// <summary>
        /// 可上传的图形文件
        /// </summary>
        public const string FileExtImg = "jpg|gif|jpeg|bmp";
        /// <summary>
        /// 可上传的其他文件
        /// </summary>
        public const string FileExtNoImg = "swf|doc|rar|zip|xls|mdb|txt|pdf";
        /// <summary>
        /// 可上传的文件
        /// </summary>
        public const string FileExt = FileExtImg + "|" + FileExtNoImg;


        /// <summary>
        /// 返回上传文件的错误信息
        /// </summary>
        /// <param name="ErrorValue">错误代码</param>
        /// <param name="Att">中文或英文，1为中文，2为英文</param>
        /// <returns>错误信息</returns>
        public static string getUpFileErrorInfo(int ErrorValue, int Att)
        {
            string ErrorInfo = "";
            Language.UpFile uf = new YxLiCai.Tools.Language.UpFile(Att);
            if (ErrorValue == IOOper.UpLoadFile.Error_FileLength)
            {
                ErrorInfo = uf.Error_FileLength;
            }
            else if (ErrorValue == IOOper.UpLoadFile.Error_FileFormat)
            {
                ErrorInfo = uf.Error_FileFormat;
            }
            else if (ErrorValue == IOOper.UpLoadFile.Error_NoFile)
            {
                ErrorInfo = uf.Error_NoFile;
            }
            else if (ErrorValue == IOOper.UpLoadFile.Error_System)
            {
                ErrorInfo = uf.Error_System;
            }
            else if (ErrorValue == IOOper.UpLoadFile.Error_Succeed)
            {
                ErrorInfo = uf.Error_Succeed;
            }
            return ErrorInfo;
        }

        /// <summary>
        /// 上传的文件类型是否合法
        /// </summary>
        /// <param name="Filename">文件名</param>
        /// <returns></returns>
        public static bool uploadRightFormat(string Filename)
        {
            return uploadRightFormat(Filename, FileExt);

        }

        /// <summary>
        /// 上传的文件类型是否合法
        /// </summary>
        /// <param name="Filename">上传的文件名</param>
        /// <param name="FileExt">允许的扩展名(如：pdf|xls|rar)</param>
        /// <returns></returns>
        public static bool uploadRightFormat(string Filename, string FileExt)
        {
            bool b = false;
            try
            {
                string TheFileExt = getFileExt(Filename).ToLower();
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
        /// <summary>
        /// 是否是图形文件
        /// </summary>
        /// <param name="Filename">文件名</param>
        /// <returns></returns>
        public static bool uploadFileIsImg(string Filename)
        {
            return uploadRightFormat(Filename, FileExtImg);
        }

        /// <summary>
        /// 返回图片的编码格式
        /// </summary>
        /// <param name="Filename"></param>
        /// <returns></returns>
        public static System.Drawing.Imaging.ImageFormat getImgesFormat(string Filename)
        {
            System.Drawing.Imaging.ImageFormat imgform = null;
            //string[] TheFile=Filename.Split(new char[]{'.'});
            string TheFileExt = getFileExt(Filename).ToLower();
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


        /// <summary>
        /// 取得一个路径的目录部分,如d;\12\124.txt则返回d;\12
        /// </summary>
        /// <param name="path">完整路径</param>
        /// <returns></returns>
        public static string getFolderOnly(string path)
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

        /// <summary>
        /// 取得一个URL的最后部分即文件名+扩展名
        /// </summary>
        /// <param name="path">完整路径</param>
        /// <returns></returns>
        public static string getFilename(string path)
        {
            string Filename = "";
            if (path != "")
            {
                string[] foldera;
                foldera = path.Split(new char[] { '\\' });
                Filename = foldera[(foldera.Length - 1)];
            }
            return Filename;
        }

        /// <summary>
        /// 取得一个URL的最后部分,仅文件名，如d:\124\123.txt，返回 123
        /// </summary>
        /// <param name="Filename">完整路径或文件名</param>
        /// <returns></returns>
        public static string getFileOnlyName(string Filename)
        {
            string FileName = "";
            if (Filename != "")
            {
                FileName = Filename.Split(new char[] { '.' })[0];
            }
            return FileName;
        }

        /// <summary>
        /// 取得一个URL的最后部分即扩展名
        /// </summary>
        /// <param name="path">完整路径或文件名</param>
        /// <returns></returns>
        public static string getFileExt(string path)
        {
            string FileExt = "";
            if (path != "")
            {
                string[] patha = path.Split(new char[] { '.' });
                FileExt = patha[patha.Length - 1];
            }
            return FileExt;
        }

        /// <summary>
        /// 自动命名,返回文件名：年_月_日_时_分_秒_随机数.扩展名
        /// </summary>
        /// <param name="Ext">扩展名</param>
        /// <returns>返回文件名如：2005_10_1_12_10_10_2345.gif</returns>
        public static string getRandomFileName(string Ext)
        {
            Random rdom = new Random();
            DateTime dtime = DateTime.Now;
            string Filename = dtime.Year + "_" + dtime.Month + "_" + dtime.Day + "_" + dtime.Hour + "_" + dtime.Minute + "_" + dtime.Second + "_" + rdom.Next(10000) + "." + Ext;
            return Filename;
        }

        /// <summary>
        /// 在图片中写上文字的版权信息
        /// 版权信息在右下角
        /// </summary>
        /// <param name="img"></param>
        /// <param name="Txt"></param>
        /// <returns></returns>
        public static Image writeCopyRightTxt(Image img, string Txt)
        {
            if (Txt != "")
            {
                Bitmap output = new Bitmap(img);
                Graphics g = Graphics.FromImage(output);
                Font font = new Font("Arial Black", 12);
                g.DrawString(Txt, font, new SolidBrush(Color.Beige), 10, 10);
                img = Image.FromHbitmap(output.GetHbitmap());
                output.Dispose();
            }
            return img;
        }
        /// <summary>
        /// 在图片上叠加另一个图片,
        /// </summary>
        /// <param name="img">原图</param>
        /// <param name="CopyRightImgPath">要叠加的图片(小图)的路径</param>
        /// <returns>返回叠加后的图片</returns>
        public static Image writeCopyRight(Image img, string CopyRightImgPath)
        {
            System.Drawing.Image CopyRigthimg = System.Drawing.Image.FromFile(CopyRightImgPath);
            return writeCopyRight(img, CopyRigthimg);
        }
        /// <summary>
        /// 在图片上叠加另一个图片
        /// </summary>
        /// <param name="img">原图</param>
        /// <param name="CopyRigthimg">要叠加的图片(小图)</param>
        /// <returns>返回叠加后的图片</returns>
        public static Image writeCopyRight(Image img, Image CopyRigthimg)
        {
            Size sizeimg = new Size();
            Size sizecopy = new Size();
            Size sizenew = new Size();
            sizeimg.Width = img.Width / 2;
            sizeimg.Height = img.Height / 2;
            sizecopy.Width = CopyRigthimg.Width;
            sizecopy.Height = CopyRigthimg.Height;
            sizenew = getThumbnailSize(sizecopy, sizeimg);

            Bitmap output = new Bitmap(img);
            Graphics g = Graphics.FromImage(output);
            Point point = new Point((sizeimg.Width) - (sizenew.Width / 2), (sizeimg.Height) - (sizenew.Height / 2));
            g.DrawImage(CopyRigthimg, new Rectangle(point, sizenew));
            img = Image.FromHbitmap(output.GetHbitmap());
            output.Dispose();
            CopyRigthimg.Dispose();
            return img;
        }

        /// <summary>
        /// 生成多个微缩图,没有返回值
        /// </summary>
        /// <param name="img">要生成微缩图的图片(对象)</param>
        /// <param name="smallImgInfos">微缩图的对象</param>
        /// <param name="FileName">保存用的文件名</param>
        public static void writeThumbnailImage(Image img, IOOper.SmallImage[] smallImgInfos, string FileName)
        {
            int img_width = img.Width;
            int img_height = img.Height;

            Size size;

            IOOper.SmallImage smallImgInfo;

            for (int i = 0; i < smallImgInfos.Length; i++)
            {
                smallImgInfo = smallImgInfos[i];
                //smallImgInfo.SaveFolder=IOOper.FolderOper.DummyToFact(smallImgInfo.SaveFolder);
                size = getThumbnailSize(new Size(img_width, img_height), new Size(smallImgInfo.Width, smallImgInfo.Heigth));
                //保存目录是否存在				
                if (!IOOper.FolderOper.FolderExists(smallImgInfo.SaveFolder))
                {
                    IOOper.FolderOper.CreateFolders(smallImgInfo.SaveFolder);
                }
                Image thumimg = getThumbnailImage(img, size.Width, size.Height);
                thumimg.Save(smallImgInfo.SaveFolder + "\\" + FileName, getImgesFormat(FileName));
            }
        }

        /// <summary>
        /// 生成微缩图，返回缩小后的图片对象
        /// </summary>
        /// <param name="img">要生成微缩图的图片(对象)</param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        /// <returns></returns>
        public static Image getThumbnailImage(Image img, int width, int height)
        {
            return getThumbnailImage(img, new Size(width, height));
        }

        /// <summary>
        /// 生成微缩图，返回缩小后的图片对象
        /// </summary>
        /// <param name="img">要生成微缩图的图片(对象)</param>
        /// <param name="ThumbnailImageSize">微缩图的大小</param>
        /// <returns></returns>
        public static Image getThumbnailImage(Image img, Size ThumbnailImageSize)
        {
            Image smallimg = img.GetThumbnailImage(ThumbnailImageSize.Width, ThumbnailImageSize.Height, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
            return smallimg;
        }

        /// <summary>
        /// 计算图片缩放比例，返回微缩图的大小
        /// </summary>
        /// <param name="ImageSize">原图大小</param>
        /// <param name="ThumbnailSize">要求大小</param>
        /// <returns></returns>
        public static Size getThumbnailSize(Size ImageSize, Size ThumbnailSize)
        {
            Size newsize = new Size();
            if (ImageSize.Width > ThumbnailSize.Width || ImageSize.Height > ThumbnailSize.Height)
            {
                double a = ImageSize.Width / Convert.ToDouble(ThumbnailSize.Width);
                double b = ImageSize.Height / Convert.ToDouble(ThumbnailSize.Height);
                if (b > a) a = b;
                newsize.Width = Convert.ToInt32(ImageSize.Width / a);
                newsize.Height = Convert.ToInt32(ImageSize.Height / a);
            }
            else
            {
                newsize = ImageSize;
            }
            return newsize;
        }
        /// <summary>
        /// 生成微缩图Callback
        /// </summary>
        /// <returns></returns>
        public static bool ThumbnailCallback()
        {
            return true;
        }
    }
}