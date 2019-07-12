using System;
using System.IO;

namespace YxLiCai.Tools.IOOper
{
    /// <summary>
    /// ���ļ�����������ɾ�����������򿪵ȡ�
    /// </summary>
    public class FileOper
    {
        /// <summary>
        /// ɾ��һ���ļ�
        /// </summary>
        /// <param name="Folder">�����ļ��У�����������·����d:\abc\sobo</param>
        /// <param name="FileName">�ļ���</param>
        /// <returns>�Ƿ�ɾ���ɹ�</returns>
        public static bool DeleteFile(string Folder, string FileName)
        {
            return DeleteFile(Folder + "\\" + FileName);
        }
        /// <summary>
        /// ɾ��һ���ļ�
        /// </summary>
        /// <param name="path">�ļ�������·����:d:\abc\123.gif</param>
        /// <returns>�Ƿ�ɾ���ɹ�</returns>
        public static bool DeleteFile(string path)
        {
            bool b = true;
            try
            {
                System.IO.File.Delete(path);
            }
            catch
            {
                b = false;
            }
            return b;
        }
        /// <summary>
        /// ��һ���ļ������ظ��ļ�������(�ַ���)
        /// </summary>
        /// <param name="Folder">�����ļ���</param>
        /// <param name="FileName">�ļ���</param>
        /// <returns>���ظ��ļ�������</returns>
        public static string OpenFile(string Folder, string FileName)
        {
            return OpenFile(Folder + "\\" + FileName);
        }
        /// <summary>
        /// ��һ���ļ������ظ��ļ�������(�ַ���)
        /// </summary>
        /// <param name="path">�ļ�������·��</param>
        /// <returns>���ظ��ļ�������</returns>
        public static string OpenFile(string path)
        {
            string txt = "";
            try
            {
                System.IO.StreamReader SReader = new StreamReader(path, System.Text.Encoding.Default);
                txt = SReader.ReadToEnd();
                SReader.Close();
            }
            catch { }
            return txt;
        }


        /// <summary>
        /// �����ļ�����������д���ļ�
        /// </summary>
        /// <param name="Folder">�����ļ���</param>
        /// <param name="FileName">�ļ���</param>
        /// <param name="txt">Ҫд���ļ�������</param>
        /// <param name="err">�Ƿ񸴸�ͬ���ļ�</param>
        /// <returns>�Ƿ�ɹ�</returns>

        public static bool WriteFile(string Folder, string FileName, string txt, bool err)
        {
            if (!IOOper.FolderOper.FolderExists(Folder))
            {
                IOOper.FolderOper.CreateFolders(Folder);
            }
            return WriteFile(Folder + "\\" + FileName, txt, err);
        }
        /// <summary>
        /// �����ļ�����������д���ļ�
        /// </summary>
        /// <param name="path">�ļ�·��</param>
        /// <param name="txt">Ҫд���ļ�������</param>
        /// <param name="err">�Ƿ񸴸�ͬ���ļ�</param>
        /// <returns>�Ƿ�ɹ�</returns>
        public static bool WriteFile(string path, string txt, bool err)
        {
            bool b = true;
            if (CreateFile(path, err))
            {
                System.IO.StreamWriter SWrite = new StreamWriter(path, false, System.Text.Encoding.Default);
                SWrite.Write(txt);
                SWrite.Close();
            }
            else
            {
                b = false;
            }
            return b;
        }

        /// <summary>
        /// �����ļ�
        /// </summary>
        /// <param name="Folder">�����ļ���</param>
        /// <param name="FileName">�ļ���</param>
        /// <param name="err">�Ƿ񸴸�ͬ���ļ�</param>
        /// <returns>�Ƿ�ɹ�</returns>
        public static bool CreateFile(string Folder, string FileName, bool err)
        {
            if (!IOOper.FolderOper.FolderExists(Folder))
            {
                IOOper.FolderOper.CreateFolders(Folder);
            }
            return CreateFile(Folder + "\\" + FileName, err);
        }

        /// <summary>
        /// �����ļ�
        /// </summary>
        /// <param name="path">�ļ�·��</param>
        /// <param name="err">�Ƿ񸴸�ͬ���ļ�</param>
        /// <returns>�Ƿ�ɹ�</returns>
        public static bool CreateFile(string path, bool err)
        {
            bool b = true;
            if (File.Exists(path))
            {
                if (err)
                {
                    b = DeleteFile(path);
                }
                else
                {
                    b = false;
                }
            }

            if (b)
            {
                File.Create(path).Close();
            }
            return b;
        }

        //err 

        /// <summary>
        /// �����ļ������ظ��ƺ���ļ����򱨴���Ϣ(������:�ļ��Ѿ����ڡ�)
        /// </summary>
        /// <param name="Folder_Source">Դ�ļ�����Ŀ¼</param>
        /// <param name="Folder_Aim">Ŀ���ļ�����Ŀ¼</param>
        /// <param name="FileName">�ļ���</param>
        /// <param name="err">����ʽ:0,ֱ�Ӹ���;1,�Զ�����;2,����</param>
        /// <returns></returns>
        public static string CopyFile(string Folder_Source, string Folder_Aim, string FileName, int err)
        {
            bool b = true;
            string FileName_Aim = FileName;
            if (File.Exists(Folder_Source + "\\" + FileName))//Դ
            {
                if (File.Exists(Folder_Aim + "\\" + FileName_Aim))//Ŀ��
                {
                    switch (err)
                    {
                        case 0:
                            break;
                        case 1:
                            FileName_Aim = FileAutoReName(Folder_Aim, FileName_Aim);
                            break;
                        case 2:
                            b = false;
                            break;
                    }
                }
            }
            else
            {
                b = false;
            }
            if (b)
            {
                try
                {
                    File.Copy(Folder_Source + "\\" + FileName, Folder_Aim + "\\" + FileName_Aim, true);
                }
                catch { }
            }
            else
            {
                FileName_Aim = "����:�ļ��Ѿ�����";
            }
            return FileName_Aim;
        }


        /// <summary>
        /// �����ļ�ָ�����ļ��������ظ��ƺ���ļ����򱨴���Ϣ(������:�ļ��Ѿ����ڡ�)
        /// </summary>
        /// <param name="Folder_Source">Դ�ļ���</param>
        /// <param name="Folder_Aim">Ŀ���ļ���</param>
        /// <param name="FileName">Դ�ļ���</param>
        /// <param name="NewFileName">Ŀ���ļ���</param>
        /// <param name="err">����ʽ:0,ֱ�Ӹ���;1,�Զ�����;2,����</param>
        /// <returns></returns>
        public static string CopyFile(string Folder_Source, string Folder_Aim, string FileName, string NewFileName, int err)
        {
            bool b = true;
            string FileName_Aim = FileName;
            if (File.Exists(Folder_Source + "\\" + FileName))//Դ
            {
                if (File.Exists(Folder_Aim + "\\" + FileName_Aim))//Ŀ��
                {
                    switch (err)
                    {
                        case 0:
                            break;
                        case 1:
                            FileName_Aim = FileAutoReName(Folder_Aim, FileName_Aim);
                            break;
                        case 2:
                            b = false;
                            break;
                    }
                }
            }
            else
            {
                b = false;
            }
            if (b)
            {
                try
                {
                    File.Copy(Folder_Source + "\\" + FileName, Folder_Aim + "\\" + NewFileName, true);
                }
                catch { }
            }
            else
            {
                FileName_Aim = "����:�ļ��Ѿ�����";
            }
            return FileName_Aim;
        }

        /// <summary>
        /// �Զ������������Զ����ɵ��ļ���
        /// </summary>
        /// <param name="Folder">�����ļ���</param>
        /// <param name="FileName">�ļ���</param>
        /// <returns>�����ļ���</returns>
        public static string FileAutoReName(string Folder, string FileName)
        {
            string rfilename = FileName;
            if (File.Exists(Folder + "\\" + FileName))
            {
                string[] F_Arr = FileName.Split(new char[] { '.' });
                int i = 0;
                bool b = true;
                while (b)
                {
                    if (!(File.Exists(Folder + "\\" + F_Arr[0] + i + "." + F_Arr[1])))
                    {
                        b = false;
                        rfilename = F_Arr[0] + i + "." + F_Arr[1];
                        break;
                    }
                    i += 1;
                }
            }
            return rfilename;
        }
    }
}