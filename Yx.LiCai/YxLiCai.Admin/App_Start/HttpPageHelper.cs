/*
 *  页面帮助类
 * 作者：平扬
 * 时间：2014-1-16
 * 版本：1.0.0.0
 * 
 */
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;

using YxLiCai.Model.UploadConfig;
using System.Text;

namespace YxLiCai.Admin
{

    #region 站点枚举 SystemSiteEnum

    /// <summary>
    /// 商城站点枚举
    /// </summary>
    public enum SystemSiteEnum
    {
        /// <summary>
        /// 商城主站点
        /// </summary>
        MallWeb = 1, 
        /// <summary>
        /// 静态资源站点
        /// </summary>
        StaticResourceMallWeb = 2,
        /// <summary>
        /// Admin
        /// </summary>
        AdminSiteURL=3
    }
    #endregion

    public class HttpPageHelper
    { 
        #region  获取站点资源
        /// <summary>
        /// 获取站点资源
       /// </summary>
        /// <param name="site">分站站点 如资源站点</param>
        /// <param name="str">文件路径 如upload/abc.jpg</param>
        /// <returns>返回绝地路径及文件名(http://img.xxx.com/upload/abc.jpg)</returns>
        public static string GetRemoteNetURL(SystemSiteEnum site, string str)
        {
            switch (site)
            {
                case SystemSiteEnum.MallWeb:
                    return "http://m."+System.Configuration.ConfigurationManager.AppSettings["COOKIE_HOST"] +"/"+ str; 
                case SystemSiteEnum.StaticResourceMallWeb:
                    return "http://static." + System.Configuration.ConfigurationManager.AppSettings["COOKIE_HOST"] + "/" + str;
                case SystemSiteEnum.AdminSiteURL:
                    return "http://admin." + System.Configuration.ConfigurationManager.AppSettings["COOKIE_HOST"] + "/" + str; 
                default:
                    return string.Empty;
            }
        }
        #endregion 

        #region 文件操作
        
        /// <summary>
        /// 更具配置删除文件
        /// </summary>
        /// <param name="uptype">文件配置</param>
        /// <param name="fileName">文件地址</param>
        public static void DeleteFile(string uptype, string fileName)
        {
            YxLiCai.Tools.IOOper.DToF DToF = new YxLiCai.Tools.IOOper.DToF(); 
            if (UploadProvice.Instance().Settings[uptype] == null)
            {
                return;
            }
            //删除大图
            string deleteFile = DToF.DummyToFact(UploadProvice.Instance().Settings[uptype].FilePath + fileName);
            if (File.Exists(deleteFile))
            {
                File.Delete(deleteFile);
            }

            //删除小图            
            if (UploadProvice.Instance().Settings[uptype].CreateSmallPic)
            {
                string[] smalls = UploadProvice.Instance().Settings[uptype].SmallPicSize.Split(',');
                string fileExt = Path.GetExtension(fileName).ToLower();
                string fileNameP = fileName.Replace(fileExt, "");
                foreach (string small in smalls)
                {
                    string deleteSmallFile = fileNameP + small + fileExt;
                    if (File.Exists(deleteSmallFile))
                    {
                        File.Delete(deleteSmallFile);
                    }
                }
            }
        }
        #endregion

        #region Request方法
        /// <summary>
        /// Request一个数字
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static int RequestInt(string name)
        { 
            string v = HttpContext.Current.Request[name];
            if (v == null)
            {
                return 0;
            }
            else
            {
                if (IsNumber(v))
                {
                    return int.Parse(v);
                }
                return 0;
            }
        }
        /// <summary>
        /// Request一个布尔值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool RequestBool(string name)
        {
            string v = System.Web.HttpContext.Current.Request[name];
            if (v == null)
            {
                return false;
            }
            else
            {
                try
                {
                    return bool.Parse(v);
                }
                catch
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// 请求一个字符串，返回null时返回空字符串
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string RequestString(string name)
        {
            string v = System.Web.HttpContext.Current.Request[name];
            if (v == null)
            {
                v = string.Empty;
            }
            return v;
        }


        public static bool IsNumber(string checkStr)
        {
            if (string.IsNullOrEmpty(checkStr)) { return false; }
            return Regex.IsMatch(checkStr, @"^[-]{0,1}\d+$");

        }


        /// <summary>
        /// 请求字符串类型
        /// </summary>
        /// <param name="requestStr">请求的Request值</param>
        /// <returns>返回字符串，null则返回""</returns>
        public static string ConvertString(string requestStr)
        {
            if (string.IsNullOrEmpty(requestStr))
            {
                return "";
            }
            else
            {
                return requestStr;
            }
        }

        /// <summary>
        ///   数字类型值
        /// </summary>
        /// <param name="requestStr">获取request值</param>
        /// <returns>返回数字类型,空则返回0</returns>
        public static int ConvertInt(string requestStr)
        {
            if (IsNumber(requestStr))
            {
                return Int32.Parse(requestStr);
            }
            else
            {
                return 0;
            }
        }
        #endregion

        #region MessageBox 对话框
        /// <summary>
        /// 弹出对话框并且返回前一个页面
        /// </summary>
        /// <param name="message">对话框内容</param>
        public static void MessageBoxAndReturn(string message)
        {
            string script = string.Format("<script type=\"text/javascript\">alert('{0}');window.history.back(-1);</script>", message.Replace("'", @"\'"));
            HttpContext.Current.Response.Write(script);
            HttpContext.Current.Response.End();
        }

        public static void MessageBoxAndJump(string message, string jumpPage)
        {
            string script = string.Format("<script type=\"text/javascript\">alert('{0}');window.location.href='{1}';</script>", message.Replace("'", @"\'"), jumpPage.Replace("'", @"\'"));
            HttpContext.Current.Response.Write(script);
            HttpContext.Current.Response.End();
        }


        /// <summary>
        /// 弹出对话框
        /// </summary>
        /// <param name="message">对话框内容</param>
        public static void MessageBox(string message, System.Web.UI.Page page)
        {
            string script = string.Format("<script type=\"text/javascript\">alert('{0}');</script>", message.Replace("'", @"\'"));
            if (page.Form != null)
            {
                page.ClientScript.RegisterClientScriptBlock(typeof(System.Web.UI.Page), "messagebox", script);
            }
            else
            {
                HttpContext.Current.Response.Write(script);
            }
        }

        /// <summary>
        /// 弹出对话框并关闭页面。
        /// </summary>
        /// <param name="message">对话框内容</param>
        public static void MessageBoxAndClose(string message)
        {
            string script = string.Format("<script type=\"text/javascript\">alert('{0}');window.close();</script>", message.Replace("'", @"\'"));
            HttpContext.Current.Response.Write(script);
            HttpContext.Current.Response.End();

        }
        #endregion

        #region 分页辅助
        /// <summary>
        /// 显示分页信息
        /// </summary>
        /// <param name="totleNum">总记录数</param>
        /// <param name="numPerPage">每页条数</param>
        /// <returns>分页HTML代码</returns>
        public static string ShowPage(int totleNum, int numPerPage, int thisPage)
        {
            return ShowPage(totleNum, numPerPage, thisPage, 1, "");

        }

        /// <summary>
        /// 显示分页信息
        /// </summary>
        /// <param name="totleNum">总记录数</param>
        /// <param name="numPerPage">每页条数</param>
        /// <param name="thisPage">当前页数</param>
        /// <param name="showJump">是否显示跳转</param>
        /// <param name="pageStyle">分页样式</param>
        /// <param name="postStr">URL附加参数</param>
        /// <returns>分页HTML代码</returns>
        public static string ShowPage(int totleNum, int numPerPage, int thisPage, int pageStyle, string postStr)
        {
            if (totleNum <= numPerPage) { return ""; }

            int AllPage = (int)Math.Ceiling((float)totleNum / numPerPage); //求总页数
            string thisurl = GetUrl(postStr); //获取当前页URL地址

            string PageHtml = "", PageHtml1 = "";
            PageHtml += thisPage.ToString() + "/" + AllPage.ToString() + "页 ";
            PageHtml += numPerPage + "条/页 ";
            PageHtml += "共" + totleNum + "条 &nbsp;";

            switch (pageStyle)
            {
                case 1:
                    if (thisPage > 1)
                    {
                        PageHtml += " <a href=\"" + thisurl + (thisPage - 1).ToString() + "\">[上一页]</a>";
                    }

                    int shownum = 8;     //显示相邻页数量
                    int haveshownum = 1; //已经显示相邻页数量



                    int prenum = shownum / 2; //默认前面显示4页
                    if (AllPage - thisPage < prenum) //如果后面多余
                    {
                        prenum = shownum - (AllPage - thisPage); //重新计算前面显示页数
                    }


                    if ((thisPage - prenum) > 1)
                    {
                        PageHtml += " <a href=\"" + thisurl + "1\">[1]</a>";
                    }


                    if ((thisPage - prenum) > 2)
                    {
                        PageHtml += "...";
                    }



                    int realPrePage = 0;
                    //显示前相邻页
                    for (int loop = thisPage - 1; loop >= 1 && haveshownum <= prenum; loop--)
                    {
                        realPrePage++;
                        haveshownum++;
                        PageHtml1 = " <a href=\"" + thisurl + loop + "\">[" + loop + "]</a> " + PageHtml1;
                    }

                    PageHtml += PageHtml1 + " " + thisPage + " ";//本页

                    //显示后相邻页
                    for (int loop = thisPage + 1; loop <= AllPage && haveshownum <= shownum; loop++)
                    {
                        haveshownum++;
                        PageHtml += " <a href=\"" + thisurl + loop + "\">[" + loop + "]</a> ";
                    }

                    if (thisPage + (shownum - realPrePage) < AllPage)
                    {
                        PageHtml += "...";
                    }
                    if (thisPage < AllPage)
                    {
                        PageHtml += " <a href=\"" + thisurl + (thisPage + 1).ToString() + "\">[下一页]</a>";
                    }


                    break;
            }
            return PageHtml;
        }
        /// <summary>
        /// 显示分页信息
        /// </summary>
        /// <param name="totleNum">总记录数</param>
        /// <param name="numPerPage">每页条数</param>
        /// <param name="thisPage">当前页数</param>
        /// <param name="showJump">是否显示跳转</param>
        /// <param name="pageStyle">分页样式</param>
        /// <param name="postStr">URL附加参数</param>
        /// <returns>分页HTML代码</returns>
        public static string ShowPage(int totleNum, int numPerPage, int thisPage, string urlFormat)
        {
            if (totleNum <= numPerPage) { return ""; }

            int AllPage = (int)Math.Ceiling((float)totleNum / numPerPage); //求总页数
            string thisurl = urlFormat == "" ? GetUrl("") + "{0}" : urlFormat; //获取当前页URL地址

            string PageHtml = "", PageHtml1 = "";

            if (thisPage > 1)
            {
                PageHtml += " <a href=\"" + string.Format(thisurl, thisPage - 1) + "\">[上一页]</a>";
            } 
            int shownum = 8;     //显示相邻页数量
            int haveshownum = 1; //已经显示相邻页数量 
            int prenum = shownum / 2; //默认前面显示4页
            if (AllPage - thisPage < prenum) //如果后面多余
            {
                prenum = shownum - (AllPage - thisPage); //重新计算前面显示页数
            } 

            if ((thisPage - prenum) > 1)
            {
                PageHtml += " <a href=\"" + string.Format(thisurl, 1) + "\">[1]</a>";
            } 

            if ((thisPage - prenum) > 2)
            {
                PageHtml += "...";
            } 
            int realPrePage = 0;
            //显示前相邻页
            for (int loop = thisPage - 1; loop >= 1 && haveshownum <= prenum; loop--)
            {
                realPrePage++;
                haveshownum++;
                PageHtml1 = " <a href=\"" + string.Format(thisurl, loop) + "\">[" + loop + "]</a> " + PageHtml1;
            } 
            PageHtml += PageHtml1 + " <span class=\"pageNow\">[" + thisPage + "]</span> ";//本页

            //显示后相邻页
            for (int loop = thisPage + 1; loop <= AllPage && haveshownum <= shownum; loop++)
            {
                haveshownum++;
                PageHtml += " <a href=\"" + string.Format(thisurl, loop) + "\">[" + loop + "]</a> ";
            } 
            if (thisPage + (shownum - realPrePage) < AllPage)
            {
                PageHtml += "...";
            } 
            if (thisPage < AllPage)
            {
                PageHtml += " <a href=\"" + string.Format(thisurl, thisPage + 1) + "\">[下一页]</a>";
            }
            return PageHtml;
        }
        /// <summary>
        /// 图片分页信息
        /// </summary>
        /// <param name="totleNum">总记录数</param>
        /// <param name="numPerPage">每页条数</param>
        /// <param name="thisPage">当前页数</param>
        /// <param name="showJump">是否显示跳转</param>
        /// <param name="pageStyle">分页样式</param>
        /// <param name="postStr">URL附加参数</param>
        /// <returns>分页HTML代码</returns>
        public static string ShowPageImg(int totleNum, int numPerPage, int thisPage, string urlFormat)
        {

            if (totleNum <= numPerPage) { return ""; }

            int AllPage = (int)Math.Ceiling((float)totleNum / numPerPage); //求总页数
            string thisurl = urlFormat == "" ? GetUrl("") + "{0}" : urlFormat; //获取当前页URL地址

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<span class=\"text\">共{0}条记录</span> <span class=\"text\">共{1}页</span> ", totleNum, AllPage);
            string PageHtml1 = "";

            if (thisPage > 1)
            {
                sb.AppendFormat("<a href=\"{0}\" class=\"prev\">上一页<b></b></a><a class=\"prev\" href=\"{1}\">首页</a>", string.Format(thisurl, thisPage - 1), string.Format(thisurl, 1));
            }
            else
            {
                sb.Append("<span class=\"prev-disabled\">上一页<b></b></span><span class=\"prev-disabled\">首页</span>");
            }

            int shownum = 8;     //显示相邻页数量
            int haveshownum = 1; //已经显示相邻页数量 

            int prenum = shownum / 2; //默认前面显示4页
            if (AllPage - thisPage < prenum) //如果后面多余
            {
                prenum = shownum - (AllPage - thisPage); //重新计算前面显示页数
            }
            if ((thisPage - prenum) > 1)
            {
                sb.AppendFormat("<a href=\"{0}\">1</a>", string.Format(thisurl, thisPage - 1));
            }
            if ((thisPage - prenum) > 2)
            {
                sb.Append("<span  class=\"text\">...</span>");
            }
            int realPrePage = 0;
            //显示前相邻页
            for (int loop = thisPage - 1; loop >= 1 && haveshownum <= prenum; loop--)
            {
                realPrePage++;
                haveshownum++;
                PageHtml1 = " <a href=\"" + string.Format(thisurl, loop) + "\">" + loop + "</a> " + PageHtml1;
            }
            sb.Append(PageHtml1 + " <a class=\"current\">" + thisPage + "</a> ");//本页 
            //显示后相邻页
            for (int loop = thisPage + 1; loop <= AllPage && haveshownum <= shownum; loop++)
            {
                haveshownum++;
                sb.Append(" <a href=\"" + string.Format(thisurl, loop) + "\">" + loop + "</a> ");
            }

            if (thisPage + (shownum - realPrePage) < AllPage)
            {
                sb.Append("<span  class=\"text\">...</span>");
            }

            if (thisPage < AllPage)
            {
                sb.AppendFormat("<a class=\"next\" href=\"{1}\">末页</a><a href=\"{0}\" class=\"next\">下一页<b></b></a>", string.Format(thisurl, thisPage + 1), string.Format(thisurl, AllPage));
            }
            else
            {
                sb.Append("<span class=\"prev-disabled\">末页<b></b></span><span class=\"prev-disabled\">下一页</span>");
            }
            return sb.ToString(); ;
        }

        /// <summary>
        /// 店铺列表页商品分页条
        /// </summary>
        /// <param name="totleNum">总记录数</param>
        /// <param name="numPerPage">每页条数</param>
        /// <param name="thisPage">当前页数</param>
        /// <param name="showJump">是否显示跳转</param>
        /// <param name="pageStyle">分页样式</param>
        /// <param name="postStr">URL附加参数</param>
        /// <returns>分页HTML代码</returns>
        public static string ShowPageProduct(int totleNum, int numPerPage, int thisPage, string urlFormat)
        { 
            if (totleNum <= numPerPage) { return ""; } 
            int AllPage = (int)Math.Ceiling((float)totleNum / numPerPage); //求总页数
            string thisurl = urlFormat == "" ? GetUrl("") + "{0}" : urlFormat; //获取当前页URL地址 
            StringBuilder sb = new StringBuilder(); 
            string PageHtml1 = ""; 
            if (thisPage > 1)
            {
                sb.AppendFormat("<a href=\"{0}\" class=\"prev\"><i class=\"act_left\"></i>上一页</a>", string.Format(thisurl, thisPage - 1));
            }
            else
            {
                sb.Append("<a class=\"prev\"><i class=\"act_left\"></i><i>上一页</i></a>");
            } 
            int shownum = 8;     //显示相邻页数量
            int haveshownum = 1; //已经显示相邻页数量 

            int prenum = shownum / 2; //默认前面显示4页
            if (AllPage - thisPage < prenum) //如果后面多余
            {
                prenum = shownum - (AllPage - thisPage); //重新计算前面显示页数
            }
            if ((thisPage - prenum) > 1)
            {
                sb.AppendFormat("<a  href=\"{0}\">1</a>", string.Format(thisurl, thisPage - 1));
            }
            if ((thisPage - prenum) > 2)
            {
                sb.Append("<a >...</a>");
            }
            int realPrePage = 0;
            //显示前相邻页
            for (int loop = thisPage - 1; loop >= 1 && haveshownum <= prenum; loop--)
            {
                realPrePage++;
                haveshownum++;
                PageHtml1 = " <a href=\"" + string.Format(thisurl, loop) + "\">" + loop + "</a> " + PageHtml1;
            }
            sb.Append(PageHtml1 + " <a class=\"active\">" + thisPage + "</a> ");//本页 
            //显示后相邻页
            for (int loop = thisPage + 1; loop <= AllPage && haveshownum <= shownum; loop++)
            {
                haveshownum++;
                sb.Append(" <a href=\"" + string.Format(thisurl, loop) + "\">" + loop + "</a> ");
            }

            if (thisPage + (shownum - realPrePage) < AllPage)
            {
                sb.Append("<a >...</a>");
            }

            if (thisPage < AllPage)
            {
                sb.AppendFormat("<a  href=\"{0}\"><i>下一页</i><i class=\"act_right\"></i></a>", string.Format(thisurl, thisPage + 1));
            }
            else
            {
                sb.Append("<a><i>下一页</i><i class=\"act_right\"></i></a>");
            }
            return sb.ToString(); ;
        }

        /// <summary>
        /// Ajax显示分页信息
        /// </summary>
        /// <param name="totleNum">总记录数</param>
        /// <param name="numPerPage">每页条数</param>
        /// <param name="thisPage">当前页数</param>
        /// <param name="pageShow">调用方法名</param>
        /// <returns>分页HTML代码</returns>
        public static string ShowPageAjax(int totleNum, int numPerPage, int thisPage,string pageShow)
        {
            if (totleNum <= numPerPage) { return ""; }

            int AllPage = (int)Math.Ceiling((float)totleNum / numPerPage); //求总页数
            string thisurl = "javascript:"+pageShow+"({0});";//获取当前页URL地址

            string PageHtml = "", PageHtml1 = "";

            if (thisPage > 1)
            {
                PageHtml += " <a href=\"" + string.Format(thisurl, thisPage - 1) + "\">[上一页]</a>";
            }




            int shownum = 8;     //显示相邻页数量
            int haveshownum = 1; //已经显示相邻页数量



            int prenum = shownum / 2; //默认前面显示4页
            if (AllPage - thisPage < prenum) //如果后面多余
            {
                prenum = shownum - (AllPage - thisPage); //重新计算前面显示页数
            }


            if ((thisPage - prenum) > 1)
            {
                PageHtml += " <a href=\"" + string.Format(thisurl, 1) + "\">[1]</a>";
            }


            if ((thisPage - prenum) > 2)
            {
                PageHtml += "...";
            }



            int realPrePage = 0;
            //显示前相邻页
            for (int loop = thisPage - 1; loop >= 1 && haveshownum <= prenum; loop--)
            {
                realPrePage++;
                haveshownum++;
                PageHtml1 = " <a href=\"" + string.Format(thisurl, loop) + "\">[" + loop + "]</a> " + PageHtml1;
            }

            PageHtml += PageHtml1 + " <span class=\"pageNow\">[" + thisPage + "]</span> ";//本页

            //显示后相邻页
            for (int loop = thisPage + 1; loop <= AllPage && haveshownum <= shownum; loop++)
            {
                haveshownum++;
                PageHtml += " <a href=\"" + string.Format(thisurl, loop) + "\">[" + loop + "]</a> ";
            }

            if (thisPage + (shownum - realPrePage) < AllPage)
            {
                PageHtml += "...";
            }

            if (thisPage < AllPage)
            {
                PageHtml += " <a href=\"" + string.Format(thisurl, thisPage + 1) + "\">[下一页]</a>";
            }
            return PageHtml;
        }

        public static int GetThisPage()
        {
            int thisPage = 1;
            string thisPageStr = HttpContext.Current.Request.QueryString["page"] + "";
            try
            {
                thisPage = Convert.ToInt32(thisPageStr);
            }
            catch
            {
                thisPage = 1;
            }
            return thisPage;

        }
        public static string GetUrl(string postStr)
        {
            string Addressurl = HttpContext.Current.Request.ServerVariables["SCRIPT_NAME"].ToString();
            Addressurl += "?";
            string ItemUrls = "";
            foreach (string queryStr in HttpContext.Current.Request.QueryString)
            {
                if (postStr == "")
                {
                    if (queryStr != "page")
                    {
                        ItemUrls += queryStr + "=" + HttpContext.Current.Request.QueryString[queryStr] + "&";
                    }
                }
                else
                {
                    if (queryStr != "page" && queryStr != "records")
                    {
                        ItemUrls += queryStr + "=" + HttpContext.Current.Request.QueryString[queryStr] + "&";
                    }
                }

            }
            if (postStr != "")
            {
                Addressurl += postStr + "&";

            }
            return Addressurl + ItemUrls + "page=";
        }
        #endregion
    }
}
