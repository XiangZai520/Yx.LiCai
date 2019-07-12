using System;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace YxLiCai.Tools
{
    /// <summary>
    /// 对字符串的操作
    /// </summary>
    public class StringOper
    {
        /// <summary>
        /// 检查一个字符串中是否只包括允许的字符,注意：字符区分大小写
        /// </summary>
        /// <param name="str">要检查的字符</param>
        /// <param name="str_allowable">允许的字符，如何1234567890,只允许数字</param>
        /// <returns></returns>
        public static bool String_IsAllowable(string str, string str_allowable)
        {
            bool b = true;
            if (str == null || str == "")
            {
                b = true;
                return b;
            }

            if (str_allowable == null || str_allowable == "")
            {
                b = false;
                return b;
            }

            for (int i = 0; i < str.Length; i++)
            {
                char str_a = str[i];
                int j = 0;
                for (j = 0; j < str_allowable.Length; j++)
                {
                    if (str_a == str_allowable[j])
                    {
                        break;
                    }
                }

                if (j >= str_allowable.Length)
                {
                    b = false;
                    break;
                }
            }
            return b;
        }

        /// <summary>
        /// 将字符转为sql安全字符。
        /// </summary>
        /// <param name="String">将要转换的字符</param>
        /// <param name="IsDel">发现不安全的字符是删除还是替换，true则去除不安全的字符，false则用相应的字符替换不安全的字符。如：把单引号替换成＆#39;</param>
        /// <returns></returns>
        public static string SqlSafeString(string String, bool IsDel)
        {
            if (String != null && String != "")
            {
                if (IsDel)
                {
                    String = String.Replace("'", "");
                    String = String.Replace("\"", "");
                }
                else
                {
                    String = String.Replace("'", "&#39;");
                    String = String.Replace("\"", "&#34;");
                }
            }
            return String;
        }
        /// <summary>
        /// 检查一个数组中是否包括指定的字符串
        /// </summary>
        /// <param name="objs">字符数组</param>
        /// <param name="obj">字符</param>
        /// <returns></returns>
        public static bool Compare(string[] objs, string obj)
        {
            bool b = false;
            try
            {
                for (int i = 0; i < objs.Length; i++)
                {
                    if (objs[i] == obj)
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
        /// 分割字符串,返回分割后的数组
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="splitstr">分隔符</param>
        /// <returns></returns>
        public static string[] SplitMulti(string str, string splitstr)
        {
            string[] strarr = null;
            if (str != null && str != "")
            {
                Regex regex1 = new Regex(splitstr);
                strarr = regex1.Split(str);
            }
            return strarr;
        }

        /// <summary>
        /// 获取文件大小
        /// </summary>
        /// <param name="filesize"></param>
        /// <returns></returns>
        public static string GetFileSize(float filesize)
        {
            float filesizeFloat = filesize / 1024;
            if (filesizeFloat < 1024)
            {
                return Math.Ceiling(filesizeFloat) + "K";
            }
            else
            {
                filesizeFloat = filesizeFloat / 1024;
                return Math.Round(filesizeFloat, 2) + "M";
            }

        }
        /// <summary>
        /// 文本框内容输出成html
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToHtmlText(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            StringBuilder builder1 = new StringBuilder();
            builder1.Append(str);
            builder1.Replace("&", "&amp;");
            builder1.Replace("<", "&lt;");
            builder1.Replace(">", "&gt;");
            builder1.Replace("\"", "&quot;");
            builder1.Replace("\r", "<br>");
            builder1.Replace(" ", "&nbsp;");
            return builder1.ToString();
        }


        /// <summary>
        /// 去除html标签
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveHtml(string str)
        {
            string text1 = "<.*?>";
            Regex regex1 = new Regex(text1);
            str = regex1.Replace(str, "");
            str = str.Replace("&nbsp;", " ");
            return str;
        }


        /// <summary>
        /// 将html代码转换为文本，如空格转为“nbsp;”
        /// </summary>
        /// <param name="txt">要转换的HTML代码</param>
        /// <returns></returns>
        public static string HtmlToText(string txt)
        {
            txt = txt.Replace("&", "&amp;");
            txt = txt.Replace("  ", "&nbsp; "); 
            txt = txt.Replace("\n", "<br/>");
            txt = txt.Replace("<", "&lt;");
            txt = txt.Replace(">", "&gt;");
            txt = txt.Replace("\"", "&quot;");
           
            return txt;
        }

        /// <summary>
        /// 将文本转换为html代码
        /// </summary>
        /// <param name="txt">要转换的文本</param>
        /// <returns></returns>
        public static string TextToHtml(string txt)
        {
            txt = txt.Replace("&lt;", "<");
            txt = txt.Replace("&gt;", ">");
            txt = txt.Replace("&quot;", "\"");
            txt = txt.Replace("&amp;", "&");
            txt = txt.Replace("  ", "&nbsp; ");
            return txt;
        }

        /// <summary>
        /// 将文本转换为HTML代码，用于显示，如用户在多行文本栏输入的回车可转为br,
        /// </summary>
        /// <param name="txt">要转换的文本</param>
        /// <returns></returns>
        public static string TextToHtml_Show(string txt)
        {
            try
            {
                txt = txt.Replace("&", "&amp;");
                txt = txt.Replace("  ", "&nbsp; ");
                //txt=txt.Replace("<","&lt;");
                //txt=txt.Replace(">","&gt;");
                txt = txt.Replace("\"", "&quot;");
                txt = txt.Replace("\n", "<br/>");
            }
            catch { }
            return txt;
        }

        /// <summary>
        /// 将字符串转换为安全的HTML值
        /// </summary>
        /// <param name="str">源串</param>
        /// <returns>结果</returns>
        public static string SafeHtmlValue(string str)
        {
            return string.IsNullOrEmpty(str) ? str : str.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;").Replace("=", "&3D;");
        }

        /// <summary>
        /// 将字符串转换为安全的JSON值，json中遇到\r\n特殊字符时会报错，所以这里特殊处理一下
        /// </summary>
        /// <param name="str">源串</param>
        /// <returns>结果</returns>
        public static string SafeJsonValue(string str)
        {
            //转换&符号的目的是保留源串中包含&apos;符号的字符
            //\r转换为&0D;是自定义的一个实体引用;0D值是((int)'\r').ToString("X2")所得,
            //而前面的&符号和其后的;结尾符号是自己加的，原本可以使用%0D代替，但是还要多转换一个%所以就没有这么做
            //同理\n也做了转换
            return string.IsNullOrEmpty(str) ? str : str.Replace("&", "&amp;").Replace("'", "&apos;").Replace("\"", "&quot;").Replace("\r", "&0D;").Replace("\n", "&0A;");
        }

        /// <summary>
        /// web请求传递XML文本会有安全错误，所以转换一下
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string SafeXmlParameter(string str)
        {
            return string.IsNullOrEmpty(str) ? str : str.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
        }

        /// <summary>
        /// 解析web请求传递XML文本会有安全错误，所以转换一下
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string TranslateXmlParameter(string str)
        {
            return string.IsNullOrEmpty(str) ? str : str.Replace("&lt;", "<").Replace("&gt;", ">").Replace("&amp;", "&");
        }

        /// <summary>
        /// 检查文本中是否全部是英文字符
        /// </summary>
        /// <param name="txt">要检查的文本</param>
        /// <returns></returns>
        public static bool IsStringChinese(string txt)
        {
            bool b = false;
            txt = txt.Trim();
            if (txt.Length != string_len(txt))
            {
                b = true;
            }
            return b;
        }

        /// <summary>
        /// 计算文本长度，一个中文算2个，一个英文算1个
        /// </summary>
        /// <param name="txt">要的计算文本</param>
        /// <returns></returns>
        public static int string_len(string txt)
        {
            byte[] mybyte = System.Text.Encoding.Default.GetBytes(txt.Trim());
            return mybyte.Length;
        }


        /// <summary>
        /// 返回指定长度的文本，一个中文算2个，一个英文算1个
        /// </summary>
        /// <param name="text">要截取的文本</param>
        /// <param name="len">长度，一个中文算2个，一个英文算1个</param>
        /// <returns></returns>
        public static string string_len_get(string text, int len)
        {
            string re_txt = "";
            if (len == -1)
            {
                return text;
            }
            if (string_len(text) > len)
            {
                int y = 0;
                len -= 2;
                for (int i = 0; i < text.Length; i++)
                {
                    re_txt += text[i];
                    if ((int)text[i] < 0 || (int)text[i] > 255)
                    {
                        y += 2;
                    }
                    else
                    {
                        y += 1;
                    }
                    if (y >= len)
                    {
                        break;
                    }
                }
                re_txt += "..";
            }
            else
            {
                re_txt = text;
            }
            return re_txt;
        }

        /// <summary>
        /// 去除HTML标签,如＜p＞中国＜/p＞，返回中国
        /// </summary>
        /// <param name="Htmlstr">HTML文本</param>
        /// <returns></returns>
        public static string HtmlTakeOutLabel(string Htmlstr)
        {
            string Textstr = Htmlstr;
            int L1 = 0, L2 = 0, L3 = 0;
            bool b = true;

            while (b)
            {
                L1 = Textstr.IndexOf("<", L1);
                if (L1 >= 0)
                {
                    L2 = Textstr.IndexOf("<", L1);
                    L3 = Textstr.IndexOf(">", L1);
                    if (L2 >= 0 || L3 >= 0)
                    {
                        if (L2 > L1 && L2 < L3)
                        {
                            Textstr = Textstr.Remove(L2, (L3 - L2) + 1);
                        }
                        else if (L1 >= L2 && L1 < L3)
                        {
                            Textstr = Textstr.Remove(L1, (L3 - L1) + 1);
                        }
                    }
                    else
                    {
                        b = false;
                    }
                }
                else { b = false; }

            }
            Textstr = Textstr.Replace("&nbsp;", "");
            return Textstr;
        }

        /// <summary>
        /// 去除HTML标签,如＜p＞中国＜/p＞，返回中国
        /// 只返回前3900个字符
        /// </summary>
        /// <param name="Htmlstr">HTML文本</param>
        /// <returns></returns>
        public static string HtmlTakeOutLabelSmall(string Htmlstr)
        {
            Htmlstr = HtmlTakeOutLabel(Htmlstr);
            if (Htmlstr.Length > 3900)
            {
                Htmlstr = Htmlstr.Substring(0, 3900);
            }
            return Htmlstr;
        }

        public static byte[] ByteCopyTo(byte[] original, int index, int len)
        {
            byte[] dest = null;
            if (original != null)
            {
                dest = new byte[len];
                if (original.Length < len + index)
                {
                    len = original.Length - index;
                }
                for (int i = 0; i < len; i++)
                {
                    dest[i] = original[i + index];
                }
            }
            return dest;
        }
    }
}