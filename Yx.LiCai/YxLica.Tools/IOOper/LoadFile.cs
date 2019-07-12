using System;
using System.IO;
using System.Drawing;

namespace YxLiCai.Tools.IOOper
{
    /// <summary>
    /// �ϴ��ļ��ࡣ
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
        /// ���췽��
        /// </summary>
        public LoadFile()
        {
            FileError = 0;//û�з�������
            this.LoadFileFolder = "";//�ϴ���·��
            this.LoadFileFolderSmall = "";
            this.LoadFileFolderTemp = "";
            this.LoadFileFolder_Users = "";
        }

        /// <summary>
        /// �Զ�����,�����ļ�������_��_��_ʱ_��_��_�����.��չ��
        /// </summary>
        /// <param name="Ext">��չ��</param>
        /// <returns>�����ļ����磺2005_10_1_12_10_10_2345.gif</returns>
        public string RandomFileName(string Ext)
        {
            return IOPublic.getRandomFileName(Ext);
        }

        /// <summary>
        /// ɾ���ϴ���ͼƬ�Լ���΢��ͼ
        /// </summary>
        /// <param name="FileName">�ļ���</param>
        public void Delete_LoadFile(string FileName)
        {
            IOOper.FileOper.DeleteFile(IOOper.FolderOper.DummyToFact(LoadFileFolder + "/" + FileName));
            IOOper.FileOper.DeleteFile(IOOper.FolderOper.DummyToFact(LoadFileFolderSmall + "/" + FileName));
        }

        /// <summary>
        /// �ϴ��ļ�����isproductΪtrue��������΢��ͼ��д���Ȩ��Ϣ,���ļ����浽��ʱĿ¼
        /// </summary>
        /// <param name="UpFile">HtmlInputFile����</param>
        /// <param name="isproduct">�Ƿ�����΢��ͼ</param>
        /// <param name="FileCopyRight">����ͼƬ�ĵ�ַ</param>
        /// <returns></returns>
        public string UpLoadFile(System.Web.UI.HtmlControls.HtmlInputFile UpFile, bool isproduct, string FileCopyRight)
        {
            string Filename = "";
            if (UpFile.PostedFile != null && UpFile.PostedFile.ContentLength > 0)
            {
                Filename = this.RandomFileName(this.GetFileExt(UpFile.PostedFile.FileName));
                if (this.UploadFileFormat(Filename))//�Ϸ����ļ�����
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
        /// �ϴ�������΢��ͼ�������ļ���
        /// </summary>
        /// <param name="UpFile">HtmlInputFile����</param>
        /// <param name="width">΢��ͼ���</param>
        /// <param name="height">΢��ͼ���</param>
        /// <returns></returns>
        public string UpLoadFile(System.Web.UI.HtmlControls.HtmlInputFile UpFile, int width, int height)
        {
            string Filename = "";
            if (UpFile.PostedFile != null && UpFile.PostedFile.ContentLength > 0)
            {
                Filename = this.RandomFileName(this.GetFileExt(UpFile.PostedFile.FileName));
                if (this.UploadFileFormat(Filename))//�Ϸ����ļ�����
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
        /// ���ɴ���Ȩ��΢��ͼ,������Ȩд��ԭͼƬ
        /// </summary>
        /// <param name="imgpath">Ҫ���ɵ�ԭͼ·��</param>
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
        /// ����΢��ͼ������ԭͼ
        /// </summary>
        /// <param name="imgpath">ԭͼ��ַ</param>
        /// <param name="width">��</param>
        /// <param name="heigth">��</param>
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
            FileOper.DeleteFile(imgpath);//ɾ��ԭͼ
            img.Dispose();
            imgsmall.Save(imgpath, this.GetImgesForm(Filename));
            imgsmall.Dispose();
        }


        bool ThumbnailCallback()
        {
            return true;
        }



        /// <summary>
        /// �ϴ����ļ������Ƿ�Ϸ�
        /// </summary>
        /// <param name="Filename">�ļ���</param>
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

        //�Ƿ���ͼƬ
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

        //����ͼƬ�ı���
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


        //ȡ��һ��URL��Ŀ¼����
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

        //ȡ��һ��URL����󲿷��ļ���+��չ��
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

        //ȡ��һ��URL����󲿷���չ��
        public string GetFileOnlyName(string Filename)
        {
            string FileName = "";
            if (Filename != "")
            {
                FileName = Filename.Split(new char[] { '.' })[0];
            }
            return FileName;
        }

        //ȡ��һ��URL����󲿷���չ��
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