using System;

namespace YxLiCai.Tools
{
    /// <summary>
    /// 对日期的操作
    /// </summary>
    public class DateOper
    {

        /// <summary>
        /// 长日期转为短日期（不包括时间）
        /// </summary>
        /// <param name="date">DateTime对象</param>
        /// <returns></returns>
        public static string LongToShort(DateTime date)
        {
            return date.Year + "-" + DateOnlyMonthDay(date);
        }

        /// <summary>
        /// 只返回月日，如2005-10-14　返回10-14
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns></returns>
        public static string DateOnlyMonthDay(DateTime date)
        {
            return date.Month + "-" + date.Day;
        }


        /// <summary>
        /// 返回2个时间的日子间隔
        /// </summary>
        /// <param name="bigDate">结束时间</param>
        /// <param name="smallDate">开始时间</param>
        /// <returns></returns>
        public static int DateDays(DateTime bigDate, DateTime smallDate)
        {
            System.TimeSpan timespan = bigDate - smallDate;
            if (timespan.TotalDays < 1 && timespan.TotalDays > 0) return 1;
            if (timespan.TotalDays < 0) return -1;
            return timespan.Days+1;
        }

        /// <summary>
        /// 返回2个时间的日子间隔
        /// </summary>
        /// <param name="EndDate">结束时间</param>
        /// <param name="StartDate">开始时间</param>
        /// <returns></returns>
        public static int DateDayss(DateTime EndDate, DateTime StartDate)
        {
            System.TimeSpan timespan = EndDate - StartDate;
            int d = timespan.Days;
            return d;


        }

        /// <summary>
        /// 返回2个时间的间隔。格式：3d1h4m,表示3天1小时4分
        /// </summary>
        /// <param name="bigDate">结束时间</param>
        /// <param name="smallDate">开始时间</param>
        /// <returns></returns>
        public static string DateTimeSpan(DateTime bigDate, DateTime smallDate)
        {
            System.TimeSpan timespan = bigDate - smallDate;
            string txt = "";
            int d = 0;
            int h = 0;
            int m = 0;
            d = timespan.Days;
            h = timespan.Hours;
            m = timespan.Minutes;
            if (d > 0)
            {
                txt += "" + d + "d";
            }
            txt += "" + h + "h";
            txt += "" + m + "m";
            return txt;
        }

        /// <summary>
        /// 是否为润年
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns></returns>
        public static bool IsLeapYear(int year)
        {
            return year % 400 == 0 || (year % 4 == 0 && year % 100 != 0);
        }

        /// <summary>
        /// 获取年月日期数
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="month">月份</param>
        /// <returns></returns>
        public static int GetYearMonthDays(int year, int month)
        {
            int days = 30;
            if (year > 0 && month > 0 && month < 13)
            {
                days = 31;
                if (month == 2)
                {
                    days = 28;
                    if (IsLeapYear(year))
                    {
                        days = 29;
                    }
                }
                else
                {
                    switch (month)
                    {
                        case 4:
                        case 6:
                        case 9:
                        case 11:
                            days = 30;
                            break;
                    }
                }
            }
            return days;
        }
    }
}