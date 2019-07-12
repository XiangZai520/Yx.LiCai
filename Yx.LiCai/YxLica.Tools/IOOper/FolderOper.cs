using System;
using System.IO;

namespace YxLiCai.Tools.IOOper
{
    /// <summary>
    /// 对文件夹的操作。
    /// </summary>
    public class FolderOper
    {
        /// <summary>
        /// 虚拟路径转换为物理路径
        /// </summary>
        /// <param name="path">虚拟路径如"/web/cn"</param>
        /// <returns></returns>
        public static string DummyToFact(string path)
        {
            DToF dtof = new DToF();
            return dtof.DummyToFact(path);
        }

        /// <summary>
        /// 文件夹是否存在。
        /// </summary>
        /// <param name="Folder">文件夹,如d:\abc\123</param>
        /// <returns></returns>
        public static bool FolderExists(string Folder)
        {
            return Directory.Exists(Folder);
        }


        /// <summary>
        /// 新建文件夹,递归。如要创建d:\abc\123会先检查d:\abc是否存在，如果不存在则创建d:\abc
        /// </summary>
        /// <param name="Folder">文件夹路径,如d:\abc\123</param>
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
        /// 新建文件夹
        /// </summary>
        /// <param name="Folder">文件夹路径,如d:\abc\123</param>
        /// <returns></returns>
        public static System.IO.DirectoryInfo CreateFolder(string Folder)
        {
            return Directory.CreateDirectory(Folder);
        }

        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="Folder">文件夹路径,如d:\abc\123</param>
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