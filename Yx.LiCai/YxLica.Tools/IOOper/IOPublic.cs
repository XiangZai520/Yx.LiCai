using System;
using System.IO;
using System.Drawing;

namespace YxLiCai.Tools.IOOper
{
    /// <summary>
    /// �ļ����ļ�����ط�����
    /// </summary>
    public class IOPublic
    {
        /// <summary>
        /// ���ϴ���ͼ���ļ�
        /// </summary>
        public const string FileExtImg = "jpg|gif|jpeg|bmp";
        /// <summary>
        /// ���ϴ��������ļ�
        /// </summary>
        public const string FileExtNoImg = "swf|doc|rar|zip|xls|mdb|txt|pdf";
        /// <summary>
        /// ���ϴ����ļ�
        /// </summary>
        public const string FileExt = FileExtImg + "|" + FileExtNoImg;


        /// <summary>
        /// �����ϴ��ļ��Ĵ�����Ϣ
        /// </summary>
        /// <param name="ErrorValue">�������</param>
        /// <param name="Att">���Ļ�Ӣ�ģ�1Ϊ���ģ�2ΪӢ��</param>
        /// <returns>������Ϣ</returns>
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
        /// �ϴ����ļ������Ƿ�Ϸ�
        /// </summary>
        /// <param name="Filename">�ļ���</param>
        /// <returns></returns>
        public static bool uploadRightFormat(string Filename)
        {
            return uploadRightFormat(Filename, FileExt);

        }

        /// <summary>
        /// �ϴ����ļ������Ƿ�Ϸ�
        /// </summary>
        /// <param name="Filename">�ϴ����ļ���</param>
        /// <param name="FileExt">�������չ��(�磺pdf|xls|rar)</param>
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
        /// �Ƿ���ͼ���ļ�
        /// </summary>
        /// <param name="Filename">�ļ���</param>
        /// <returns></returns>
        public static bool uploadFileIsImg(string Filename)
        {
            return uploadRightFormat(Filename, FileExtImg);
        }

        /// <summary>
        /// ����ͼƬ�ı����ʽ
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
        /// ȡ��һ��·����Ŀ¼����,��d;\12\124.txt�򷵻�d;\12
        /// </summary>
        /// <param name="path">����·��</param>
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
        /// ȡ��һ��URL����󲿷ּ��ļ���+��չ��
        /// </summary>
        /// <param name="path">����·��</param>
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
        /// ȡ��һ��URL����󲿷�,���ļ�������d:\124\123.txt������ 123
        /// </summary>
        /// <param name="Filename">����·�����ļ���</param>
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
        /// ȡ��һ��URL����󲿷ּ���չ��
        /// </summary>
        /// <param name="path">����·�����ļ���</param>
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
        /// �Զ�����,�����ļ�������_��_��_ʱ_��_��_�����.��չ��
        /// </summary>
        /// <param name="Ext">��չ��</param>
        /// <returns>�����ļ����磺2005_10_1_12_10_10_2345.gif</returns>
        public static string getRandomFileName(string Ext)
        {
            Random rdom = new Random();
            DateTime dtime = DateTime.Now;
            string Filename = dtime.Year + "_" + dtime.Month + "_" + dtime.Day + "_" + dtime.Hour + "_" + dtime.Minute + "_" + dtime.Second + "_" + rdom.Next(10000) + "." + Ext;
            return Filename;
        }

        /// <summary>
        /// ��ͼƬ��д�����ֵİ�Ȩ��Ϣ
        /// ��Ȩ��Ϣ�����½�
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
        /// ��ͼƬ�ϵ�����һ��ͼƬ,
        /// </summary>
        /// <param name="img">ԭͼ</param>
        /// <param name="CopyRightImgPath">Ҫ���ӵ�ͼƬ(Сͼ)��·��</param>
        /// <returns>���ص��Ӻ��ͼƬ</returns>
        public static Image writeCopyRight(Image img, string CopyRightImgPath)
        {
            System.Drawing.Image CopyRigthimg = System.Drawing.Image.FromFile(CopyRightImgPath);
            return writeCopyRight(img, CopyRigthimg);
        }
        /// <summary>
        /// ��ͼƬ�ϵ�����һ��ͼƬ
        /// </summary>
        /// <param name="img">ԭͼ</param>
        /// <param name="CopyRigthimg">Ҫ���ӵ�ͼƬ(Сͼ)</param>
        /// <returns>���ص��Ӻ��ͼƬ</returns>
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
        /// ���ɶ��΢��ͼ,û�з���ֵ
        /// </summary>
        /// <param name="img">Ҫ����΢��ͼ��ͼƬ(����)</param>
        /// <param name="smallImgInfos">΢��ͼ�Ķ���</param>
        /// <param name="FileName">�����õ��ļ���</param>
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
                //����Ŀ¼�Ƿ����				
                if (!IOOper.FolderOper.FolderExists(smallImgInfo.SaveFolder))
                {
                    IOOper.FolderOper.CreateFolders(smallImgInfo.SaveFolder);
                }
                Image thumimg = getThumbnailImage(img, size.Width, size.Height);
                thumimg.Save(smallImgInfo.SaveFolder + "\\" + FileName, getImgesFormat(FileName));
            }
        }

        /// <summary>
        /// ����΢��ͼ��������С���ͼƬ����
        /// </summary>
        /// <param name="img">Ҫ����΢��ͼ��ͼƬ(����)</param>
        /// <param name="width">��</param>
        /// <param name="height">��</param>
        /// <returns></returns>
        public static Image getThumbnailImage(Image img, int width, int height)
        {
            return getThumbnailImage(img, new Size(width, height));
        }

        /// <summary>
        /// ����΢��ͼ��������С���ͼƬ����
        /// </summary>
        /// <param name="img">Ҫ����΢��ͼ��ͼƬ(����)</param>
        /// <param name="ThumbnailImageSize">΢��ͼ�Ĵ�С</param>
        /// <returns></returns>
        public static Image getThumbnailImage(Image img, Size ThumbnailImageSize)
        {
            Image smallimg = img.GetThumbnailImage(ThumbnailImageSize.Width, ThumbnailImageSize.Height, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
            return smallimg;
        }

        /// <summary>
        /// ����ͼƬ���ű���������΢��ͼ�Ĵ�С
        /// </summary>
        /// <param name="ImageSize">ԭͼ��С</param>
        /// <param name="ThumbnailSize">Ҫ���С</param>
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
        /// ����΢��ͼCallback
        /// </summary>
        /// <returns></returns>
        public static bool ThumbnailCallback()
        {
            return true;
        }
    }
}