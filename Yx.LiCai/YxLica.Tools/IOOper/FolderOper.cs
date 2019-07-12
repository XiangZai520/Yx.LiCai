using System;
using System.IO;

namespace YxLiCai.Tools.IOOper
{
    /// <summary>
    /// ���ļ��еĲ�����
    /// </summary>
    public class FolderOper
    {
        /// <summary>
        /// ����·��ת��Ϊ����·��
        /// </summary>
        /// <param name="path">����·����"/web/cn"</param>
        /// <returns></returns>
        public static string DummyToFact(string path)
        {
            DToF dtof = new DToF();
            return dtof.DummyToFact(path);
        }

        /// <summary>
        /// �ļ����Ƿ���ڡ�
        /// </summary>
        /// <param name="Folder">�ļ���,��d:\abc\123</param>
        /// <returns></returns>
        public static bool FolderExists(string Folder)
        {
            return Directory.Exists(Folder);
        }


        /// <summary>
        /// �½��ļ���,�ݹ顣��Ҫ����d:\abc\123���ȼ��d:\abc�Ƿ���ڣ�����������򴴽�d:\abc
        /// </summary>
        /// <param name="Folder">�ļ���·��,��d:\abc\123</param>
        public static void CreateFolders(string Folder)
        {
            string[] paths = Folder.Split(new char[] { '\\' });
            if (paths.Length > 0)
            {
                string temppath = paths[0];
                for (int i = 1; i < paths.Length; i++)
                {
                    temppath += "\\" + paths[i];
                    try
                    {
                        if (!FolderExists(temppath))
                            CreateFolder(temppath);
                    }
                    catch { }
                }
            }
        }

        /// <summary>
        /// �½��ļ���
        /// </summary>
        /// <param name="Folder">�ļ���·��,��d:\abc\123</param>
        /// <returns></returns>
        public static System.IO.DirectoryInfo CreateFolder(string Folder)
        {
            return Directory.CreateDirectory(Folder);
        }

        /// <summary>
        /// ɾ���ļ���
        /// </summary>
        /// <param name="Folder">�ļ���·��,��d:\abc\123</param>
        public static void DeleteFolder(string Folder)
        {
            try
            {
                Directory.Delete(Folder, true);
            }
            catch { }
        }
    }
}