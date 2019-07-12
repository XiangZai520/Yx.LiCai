using System;
using System.IO;

namespace YxLiCai.Tools.IOOper
{
    /// <summary>
    /// 对文件操作，包括删除、创建、打开等。
    /// </summary>
    public class FileOper
    {
        /// <summary>
        /// 删除一个文件
        /// </summary>
        /// <param name="Folder">所在文件夹，必须是物理路径如d:\abc\sobo</param>
        /// <param name="FileName">文件名</param>
        /// <returns>是否删除成功</returns>
        public static bool DeleteFile(string Folder, string FileName)
        {
            return DeleteFile(Folder + "\\" + FileName);
        }
        /// <summary>
        /// 删除一个文件
        /// </summary>
        /// <param name="path">文件的完整路径如:d:\abc\123.gif</param>
        /// <returns>是否删除成功</returns>
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
        /// 打开一个文件，返回该文件的内容(字符型)
        /// </summary>
        /// <param name="Folder">所在文件夹</param>
        /// <param name="FileName">文件名</param>
        /// <returns>返回该文件的内容</returns>
        public static string OpenFile(string Folder, string FileName)
        {
            return OpenFile(Folder + "\\" + FileName);
        }
        /// <summary>
        /// 打开一个文件，返回该文件的内容(字符型)
        /// </summary>
        /// <param name="path">文件的完整路径</param>
        /// <returns>返回该文件的内容</returns>
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
        /// 创建文件，并将内容写入文件
        /// </summary>
        /// <param name="Folder">所在文件夹</param>
        /// <param name="FileName">文件名</param>
        /// <param name="txt">要写入文件的内容</param>
        /// <param name="err">是否复盖同名文件</param>
        /// <returns>是否成功</returns>

        public static bool WriteFile(string Folder, string FileName, string txt, bool err)
        {
            if (!IOOper.FolderOper.FolderExists(Folder))
            {
                IOOper.FolderOper.CreateFolders(Folder);
            }
            return WriteFile(Folder + "\\" + FileName, txt, err);
        }
        /// <summary>
        /// 创建文件，并将内容写入文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="txt">要写入文件的内容</param>
        /// <param name="err">是否复盖同名文件</param>
        /// <returns>是否成功</returns>
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
        /// 创建文件
        /// </summary>
        /// <param name="Folder">所在文件夹</param>
        /// <param name="FileName">文件名</param>
        /// <param name="err">是否复盖同名文件</param>
        /// <returns>是否成功</returns>
        public static bool CreateFile(string Folder, string FileName, bool err)
        {
            if (!IOOper.FolderOper.FolderExists(Folder))
            {
                IOOper.FolderOper.CreateFolders(Folder);
            }
            return CreateFile(Folder + "\\" + FileName, err);
        }

        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="err">是否复盖同名文件</param>
        /// <returns>是否成功</returns>
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
        /// 复制文件，返回复制后的文件名或报错信息(“错误:文件已经存在”)
        /// </summary>
        /// <param name="Folder_Source">源文件所在目录</param>
        /// <param name="Folder_Aim">目的文件所在目录</param>
        /// <param name="FileName">文件名</param>
        /// <param name="err">报错方式:0,直接复盖;1,自动更名;2,报错</param>
        /// <returns></returns>
        public static string CopyFile(string Folder_Source, string Folder_Aim, string FileName, int err)
        {
            bool b = true;
            string FileName_Aim = FileName;
            if (File.Exists(Folder_Source + "\\" + FileName))//源
            {
                if (File.Exists(Folder_Aim + "\\" + FileName_Aim))//目的
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
                FileName_Aim = "错误:文件已经存在";
            }
            return FileName_Aim;
        }


        /// <summary>
        /// 复制文件指定新文件名，返回复制后的文件名或报错信息(“错误:文件已经存在”)
        /// </summary>
        /// <param name="Folder_Source">源文件夹</param>
        /// <param name="Folder_Aim">目的文件夹</param>
        /// <param name="FileName">源文件名</param>
        /// <param name="NewFileName">目的文件名</param>
        /// <param name="err">报错方式:0,直接复盖;1,自动更名;2,报错</param>
        /// <returns></returns>
        public static string CopyFile(string Folder_Source, string Folder_Aim, string FileName, string NewFileName, int err)
        {
            bool b = true;
            string FileName_Aim = FileName;
            if (File.Exists(Folder_Source + "\\" + FileName))//源
            {
                if (File.Exists(Folder_Aim + "\\" + FileName_Aim))//目的
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
                FileName_Aim = "错误:文件已经存在";
            }
            return FileName_Aim;
        }

        /// <summary>
        /// 自动更名，返回自动生成的文件名
        /// </summary>
        /// <param name="Folder">所在文件夹</param>
        /// <param name="FileName">文件名</param>
        /// <returns>返回文件名</returns>
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