using System;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace YxLiCai.Tools
{
    /// <summary>
    /// ���ַ����Ĳ���
    /// </summary>
    public class StringOper
    {
        /// <summary>
        /// ���һ���ַ������Ƿ�ֻ����������ַ�,ע�⣺�ַ����ִ�Сд
        /// </summary>
        /// <param name="str">Ҫ�����ַ�</param>
        /// <param name="str_allowable">������ַ������1234567890,ֻ��������</param>
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
        /// ���ַ�תΪsql��ȫ�ַ���
        /// </summary>
        /// <param name="String">��Ҫת�����ַ�</param>
        /// <param name="IsDel">���ֲ���ȫ���ַ���ɾ�������滻��true��ȥ������ȫ���ַ���false������Ӧ���ַ��滻����ȫ���ַ����磺�ѵ������滻�ɣ�#39;</param>
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
        /// ���һ���������Ƿ����ָ�����ַ���
        /// </summary>
        /// <param name="objs">�ַ�����</param>
        /// <param name="obj">�ַ�</param>
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
        /// �ָ��ַ���,���طָ�������
        /// </summary>
        /// <param name="str">�ַ���</param>
        /// <param name="splitstr">�ָ���</param>
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
        /// ��ȡ�ļ���С
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
        /// �ı������������html
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
        /// ȥ��html��ǩ
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
        /// ��html����ת��Ϊ�ı�����ո�תΪ��nbsp;��
        /// </summary>
        /// <param name="txt">Ҫת����HTML����</param>
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
        /// ���ı�ת��Ϊhtml����
        /// </summary>
        /// <param name="txt">Ҫת�����ı�</param>
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
        /// ���ı�ת��ΪHTML���룬������ʾ�����û��ڶ����ı�������Ļس���תΪbr,
        /// </summary>
        /// <param name="txt">Ҫת�����ı�</param>
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
        /// ���ַ���ת��Ϊ��ȫ��HTMLֵ
        /// </summary>
        /// <param name="str">Դ��</param>
        /// <returns>���</returns>
        public static string SafeHtmlValue(string str)
        {
            return string.IsNullOrEmpty(str) ? str : str.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;").Replace("=", "&3D;");
        }

        /// <summary>
        /// ���ַ���ת��Ϊ��ȫ��JSONֵ��json������\r\n�����ַ�ʱ�ᱨ�������������⴦��һ��
        /// </summary>
        /// <param name="str">Դ��</param>
        /// <returns>���</returns>
        public static string SafeJsonValue(string str)
        {
            //ת��&���ŵ�Ŀ���Ǳ���Դ���а���&apos;���ŵ��ַ�
            //\rת��Ϊ&0D;���Զ����һ��ʵ������;0Dֵ��((int)'\r').ToString("X2")����,
            //��ǰ���&���ź�����;��β�������Լ��ӵģ�ԭ������ʹ��%0D���棬���ǻ�Ҫ��ת��һ��%���Ծ�û����ô��
            //ͬ��\nҲ����ת��
            return string.IsNullOrEmpty(str) ? str : str.Replace("&", "&amp;").Replace("'", "&apos;").Replace("\"", "&quot;").Replace("\r", "&0D;").Replace("\n", "&0A;");
        }

        /// <summary>
        /// web���󴫵�XML�ı����а�ȫ��������ת��һ��
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string SafeXmlParameter(string str)
        {
            return string.IsNullOrEmpty(str) ? str : str.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
        }

        /// <summary>
        /// ����web���󴫵�XML�ı����а�ȫ��������ת��һ��
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string TranslateXmlParameter(string str)
        {
            return string.IsNullOrEmpty(str) ? str : str.Replace("&lt;", "<").Replace("&gt;", ">").Replace("&amp;", "&");
        }

        /// <summary>
        /// ����ı����Ƿ�ȫ����Ӣ���ַ�
        /// </summary>
        /// <param name="txt">Ҫ�����ı�</param>
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
        /// �����ı����ȣ�һ��������2����һ��Ӣ����1��
        /// </summary>
        /// <param name="txt">Ҫ�ļ����ı�</param>
        /// <returns></returns>
        public static int string_len(string txt)
        {
            byte[] mybyte = System.Text.Encoding.Default.GetBytes(txt.Trim());
            return mybyte.Length;
        }


        /// <summary>
        /// ����ָ�����ȵ��ı���һ��������2����һ��Ӣ����1��
        /// </summary>
        /// <param name="text">Ҫ��ȡ���ı�</param>
        /// <param name="len">���ȣ�һ��������2����һ��Ӣ����1��</param>
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
        /// ȥ��HTML��ǩ,�磼p���й���/p���������й�
        /// </summary>
        /// <param name="Htmlstr">HTML�ı�</param>
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
        /// ȥ��HTML��ǩ,�磼p���й���/p���������й�
        /// ֻ����ǰ3900���ַ�
        /// </summary>
        /// <param name="Htmlstr">HTML�ı�</param>
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