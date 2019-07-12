using System;
using System.IO;
using System.Drawing; 
using System.Text;
using System.Configuration;
using System.Collections.Generic;

namespace YxLiCai.Tools.IOOper
{
    /// <summary>
    /// �ļ��ϴ���������΢��ͼ����������֡�ͼƬ��Ȩ��Ϣ��
    /// </summary>
    public class UpLoadFile
    {
        /// <summary>
        /// ������1���ϴ����ļ�̫��
        /// </summary>
        public const int Error_FileLength = 1;
        /// <summary>
        /// ������2���������ϴ����ļ���ʽ
        /// </summary>
        public const int Error_FileFormat = 2;
        /// <summary>
        /// ������3��û��ѡ��Ҫ�ļ�����ѡ����ļ�·������ȷ
        /// </summary>
        public const int Error_NoFile = 3;
        /// <summary>
        /// ������9��ϵͳ����
        /// </summary>
        public const int Error_System = 9;
        /// <summary>
        /// �����ɹ�
        /// </summary>
        public const int Error_Succeed = 0;

        /// <summary>
        /// ���ϴ��ļ�����չ������"swf|doc|rar|zip|xls|mdb|txt|pdf"
        /// </summary>
        public string FileExt = IOPublic.FileExt;
        /// <summary>
        /// �Ƿ���ͼƬ
        /// </summary>
        public bool isImg = true;
        /// <summary>
        /// HtmlInputFile����
        /// </summary>
        public System.Web.UI.HtmlControls.HtmlInputFile InputFile;
        /// <summary>
        /// HttpPostedFile����
        /// </summary>
        public System.Web.HttpPostedFile PostedFile;
        /// <summary>
        /// ����·��(�ļ���)
        /// </summary>
        public string SaveForder;
        /// <summary>
        /// ��������΢��ͼ�Ķ��������
        /// </summary>
        public IOOper.SmallImage[] SmallImg;
        /// <summary>
        /// ��ȨͼƬ�ĵ�ַ
        /// </summary>
        public string CopyRightImgPath;
        /// <summary>
        /// ��ȨͼƬ����
        /// </summary>
        public Image CopyRightImg;

        /// <summary>
        /// �Ƿ���ͼƬ��д�����ְ�Ȩ
        /// </summary>
        public bool isWriteTxt = false;

        /// <summary>
        /// д�����ְ�Ȩ����Ϣ
        /// </summary>
        public string CopyRightTxt = "";

        /// <summary>
        /// �ļ���
        /// </summary>
        private string FileName;
        /// <summary>
        /// ���صĴ�����
        /// </summary>
        private int ErrorValue = Error_Succeed;
        /// <summary>
        /// �ϴ��ļ�������С��Ĭ��Ϊ2M
        /// </summary>
        private int FileLength = 1024 * 1024 * 2;//Ĭ��2MB

        /// <summary>
        /// �ļ��ϴ���С
        /// </summary>
        public int FileSize;
 
        /// <summary>
        /// �ļ��ϴ�,���췽��
        /// </summary>
        /// <param name="InputFile">HtmlInputFile����</param>
        public UpLoadFile(System.Web.UI.HtmlControls.HtmlInputFile InputFile)
        {
            this.InputFile = InputFile;
        }

        /// <summary>
        /// �ļ��ϴ�,���췽��
        /// </summary>
        /// <param name="InputFile">HtmlInputFile����</param>
        /// <param name="SaveForder">����·��</param>
        public UpLoadFile(System.Web.UI.HtmlControls.HtmlInputFile InputFile, string SaveForder)
        {
            this.InputFile = InputFile;
            this.SaveForder = SaveForder;
        }

        /// <summary>
        /// �ļ��ϴ�,���췽��
        /// </summary>
        /// <param name="InputFile">HtmlInputFile����</param>
        /// <param name="SaveForder">����·��</param>
        /// <param name="SmallImg">΢��ͼ��������</param>
        public UpLoadFile(System.Web.UI.HtmlControls.HtmlInputFile InputFile, string SaveForder, IOOper.SmallImage[] SmallImg)
        {
            this.InputFile = InputFile;
            this.SaveForder = SaveForder;
            this.SmallImg = SmallImg;
        }

        /// <summary>
        /// �ļ��ϴ�,���췽��
        /// </summary>
        /// <param name="InputFile">HtmlInputFile����</param>
        /// <param name="SaveForder">����·��</param>
        /// <param name="SmallImg">΢��ͼ��������</param>
        /// <param name="CopyRightImgPath">��ȨͼƬ�ĵ�ַ</param>
        public UpLoadFile(System.Web.UI.HtmlControls.HtmlInputFile InputFile, string SaveForder, IOOper.SmallImage[] SmallImg, string CopyRightImgPath)
        {
            this.InputFile = InputFile;
            this.SaveForder = SaveForder;
            this.CopyRightImgPath = CopyRightImgPath;
            this.CopyRightImg = System.Drawing.Image.FromFile(IOOper.FolderOper.DummyToFact(CopyRightImgPath));
            this.SmallImg = SmallImg;
        }
        /// <summary>
        /// �ļ��ϴ�,���췽��
        /// </summary>
        /// <param name="InputFile">HtmlInputFile����</param>
        /// <param name="SaveForder">����·��</param>
        /// <param name="SmallImg">΢��ͼ��������</param>
        /// <param name="isWriteTxt">�Ƿ���ͼƬ��д�����ְ�Ȩ</param>
        /// <param name="CopyRightTxt">��Ȩ����</param>
        public UpLoadFile(System.Web.UI.HtmlControls.HtmlInputFile InputFile, string SaveForder, IOOper.SmallImage[] SmallImg, bool isWriteTxt, string CopyRightTxt)
        {
            this.InputFile = InputFile;
            this.SaveForder = SaveForder;
            this.SmallImg = SmallImg;
            this.isWriteTxt = isWriteTxt;
            this.CopyRightTxt = CopyRightTxt;
        }

        /// <summary>
        /// �ļ��ϴ�,���췽��
        /// </summary>
        /// <param name="InputFile">HtmlInputFile����</param>
        /// <param name="SaveForder">����·��</param>
        /// <param name="SmallImg">΢��ͼ��������</param>
        /// <param name="CopyRightImg">��ȨͼƬ����</param>
        public UpLoadFile(System.Web.UI.HtmlControls.HtmlInputFile InputFile, string SaveForder, IOOper.SmallImage[] SmallImg, Image CopyRightImg)
        {
            this.InputFile = InputFile;
            this.SaveForder = SaveForder;
            this.CopyRightImg = CopyRightImg;
            this.SmallImg = SmallImg;
        }

        /// <summary>
        /// HtmlInputFile�ϴ��ļ�
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
                        //�Ƿ���ͼƬ
                        if (this.isImg)
                        {
                            if (IOOper.IOPublic.uploadFileIsImg(this.FileName))
                            {
                                Image img = System.Drawing.Image.FromStream(imgStream);
                                if (this.CopyRightImgPath != null && this.CopyRightImgPath != "")
                                {
                                    //��Ӱ�Ȩ
                                    img = IOOper.IOPublic.writeCopyRight(img, CopyRightImg);
                                }

                                //д�����ְ�Ȩ
                                if (this.isWriteTxt)
                                {
                                    img = IOOper.IOPublic.writeCopyRightTxt(img, this.CopyRightTxt);
                                }

                                if (this.SmallImg != null && this.SmallImg.Length > 0)
                                {
                                    //����΢��ͼ
                                    IOOper.IOPublic.writeThumbnailImage(img, this.SmallImg, this.FileName);
                                }
                                //�ļ����Ƿ����
                                if (!IOOper.FolderOper.FolderExists(this.SaveForder)) IOOper.FolderOper.CreateFolders(this.SaveForder);
                                //�����ļ�
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
        /// HttpPostedFile�ϴ��ļ�
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
                        //�Ƿ���ͼƬ
                        if (this.isImg)
                        {
                            if (IOOper.IOPublic.uploadFileIsImg(this.FileName))
                            {
                                Image img = System.Drawing.Image.FromStream(imgStream);
                                if (this.CopyRightImgPath != null && this.CopyRightImgPath != "")
                                {
                                    //��Ӱ�Ȩ
                                    img = IOOper.IOPublic.writeCopyRight(img, CopyRightImg);
                                }

                                //д�����ְ�Ȩ
                                if (this.isWriteTxt)
                                {
                                    img = IOOper.IOPublic.writeCopyRightTxt(img, this.CopyRightTxt);
                                }

                                if (this.SmallImg != null && this.SmallImg.Length > 0)
                                {
                                    //����΢��ͼ
                                    IOOper.IOPublic.writeThumbnailImage(img, this.SmallImg, this.FileName);
                                }
                                //�ļ����Ƿ����
                                if (!IOOper.FolderOper.FolderExists(this.SaveForder)) IOOper.FolderOper.CreateFolders(this.SaveForder);
                                //�����ļ�
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
        /// �ϴ�ָ����С��ͼƬ
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

                        //�Ƿ���ͼƬ
                        if (IOOper.IOPublic.uploadFileIsImg(this.FileName))
                        {
                            Image img = System.Drawing.Image.FromStream(imgStream);
                            if (this.CopyRightImgPath != null && this.CopyRightImgPath != "")
                            {
                                //��Ӱ�Ȩ
                                img = IOOper.IOPublic.writeCopyRight(img, CopyRightImg);
                            }

                            //д�����ְ�Ȩ
                            if (this.isWriteTxt)
                            {
                                img = IOOper.IOPublic.writeCopyRightTxt(img, this.CopyRightTxt);
                            }

                            if (this.SmallImg != null && this.SmallImg.Length > 0)
                            {
                                //����΢��ͼ
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
        /// ���ش�����
        /// </summary>
        /// <returns></returns>
        public int getErrorValue()
        {
            return this.ErrorValue;
        }


        /// <summary>
        /// �����ϴ��ļ�����󳤶�
        /// </summary>
        /// <param name="FileLength">�ļ�����󳤶ȣ����ֽ�Ϊ��λ</param>
        public void setFileLength(int FileLength)
        {
            this.FileLength = FileLength;
        }

        /// <summary>
        /// �ͷ���Դ
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