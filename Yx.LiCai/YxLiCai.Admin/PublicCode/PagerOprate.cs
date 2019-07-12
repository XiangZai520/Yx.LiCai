using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace YxLiCai.Admin.PublicCode
{
    /// <summary>
    /// 分页
    /// </summary>
    public class PagerOprate
    {
        /// <summary>
        /// 创建分页html
        /// </summary>
        /// <param name="TotalCount">总数量</param>
        /// <param name="CurrentPage">当前页</param>
        /// <param name="PageSize">页容量</param>
        /// <returns></returns>
        public static string CreatePagerHTML(int TotalCount, int CurrentPage, int PageSize = 10)
        {
            if (TotalCount == 0)
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();

            int TotalPage =(int)Math.Ceiling((1.0m * TotalCount) / PageSize);
            bool q_s = false;
            bool h_s = false;

            sb.Append("<a class='" + (CurrentPage == 1 ? "gx-pager-disabled" : "") + "' onclick='Search(1);' href='javascript:void(0);'><i class='gx-icon'>首页</i></a>");
            sb.Append("<a class='" + (CurrentPage == 1 ? "gx-pager-disabled" : "") + "' onclick='Search(" + ((CurrentPage <= 1) ? 1 : (CurrentPage - 1)) + ");' href='javascript:void(0);'><i class='gx-icon'>上一页</i></a>");

            for (int i = 1; i <= TotalPage; i++)
            {
                if (i == 1 || i == TotalPage)
                {
                    sb.Append("<a class='" + (CurrentPage == i ? "gx-pager-actived" : "") + "' " + (CurrentPage == i ? "" : "onclick='Search(" + i.ToString() + ")'") + " href='javascript:void(0);'>" + i.ToString() + "</a>");
                }
                else if (Math.Abs(i - CurrentPage) <= 2)
                {
                    sb.Append("<a class='" + (CurrentPage == i ? "gx-pager-actived" : "") + "' " + (CurrentPage == i ? "" : "onclick='Search(" + i.ToString() + ")'") + " href='javascript:void(0);'>" + i.ToString() + "</a>");
                }
                else
                {
                    if (q_s == false && (CurrentPage - i) > 2)
                    {
                        q_s = true;
                        sb.Append("<span style='font-size:30px'>…</span>");
                    }
                    if (h_s == false && (i - CurrentPage) > 2)
                    {
                        h_s = true;
                        sb.Append("<span style='font-size:30px'>…</span>");
                    }
                }
            }


            sb.Append("<a class='" + (CurrentPage == TotalPage ? "gx-pager-disabled" : "") + "' onclick='Search(" + ((CurrentPage >= TotalPage) ? TotalPage : (CurrentPage + 1)) + ");' href='javascript:void(0);'><i class='gx-icon'>下一页</i></a>");
            sb.Append("<a class='" + (CurrentPage == TotalPage ? "gx-pager-disabled" : "") + "' onclick='Search(" + TotalPage.ToString() + ");' href='javascript:void(0);'><i class='gx-icon'>末页</i></a>");
            sb.Append("<span><input type='text' style='width:30px;margin:10px;' id='go_pageindex' /><input class='gx-button gx-button-info' type='button' onclick=\"var go_pageindex=$('#go_pageindex').val(); if(isNaN(parseInt(go_pageindex))){go_pageindex=1;}else{go_pageindex=parseInt(go_pageindex);} Search(go_pageindex);\" value='GO' style='padding:3px;' /></span>");
            sb.Append("<span style='margin:10px;color:blue;'>共" + TotalPage.ToString() + "页/" + TotalCount.ToString() + "条</span>");


            return sb.ToString();
        }
    }
}