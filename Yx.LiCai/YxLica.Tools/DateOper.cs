using System;

namespace YxLiCai.Tools
{
    /// <summary>
    /// �����ڵĲ���
    /// </summary>
    public class DateOper
    {

        /// <summary>
        /// ������תΪ�����ڣ�������ʱ�䣩
        /// </summary>
        /// <param name="date">DateTime����</param>
        /// <returns></returns>
        public static string LongToShort(DateTime date)
        {
            return date.Year + "-" + DateOnlyMonthDay(date);
        }

        /// <summary>
        /// ֻ�������գ���2005-10-14������10-14
        /// </summary>
        /// <param name="date">����</param>
        /// <returns></returns>
        public static string DateOnlyMonthDay(DateTime date)
        {
            return date.Month + "-" + date.Day;
        }


        /// <summary>
        /// ����2��ʱ������Ӽ��
        /// </summary>
        /// <param name="bigDate">����ʱ��</param>
        /// <param name="smallDate">��ʼʱ��</param>
        /// <returns></returns>
        public static int DateDays(DateTime bigDate, DateTime smallDate)
        {
            System.TimeSpan timespan = bigDate - smallDate;
            if (timespan.TotalDays < 1 && timespan.TotalDays > 0) return 1;
            if (timespan.TotalDays < 0) return -1;
            return timespan.Days+1;
        }

        /// <summary>
        /// ����2��ʱ������Ӽ��
        /// </summary>
        /// <param name="EndDate">����ʱ��</param>
        /// <param name="StartDate">��ʼʱ��</param>
        /// <returns></returns>
        public static int DateDayss(DateTime EndDate, DateTime StartDate)
        {
            System.TimeSpan timespan = EndDate - StartDate;
            int d = timespan.Days;
            return d;


        }

        /// <summary>
        /// ����2��ʱ��ļ������ʽ��3d1h4m,��ʾ3��1Сʱ4��
        /// </summary>
        /// <param name="bigDate">����ʱ��</param>
        /// <param name="smallDate">��ʼʱ��</param>
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
        /// �Ƿ�Ϊ����
        /// </summary>
        /// <param name="year">���</param>
        /// <returns></returns>
        public static bool IsLeapYear(int year)
        {
            return year % 400 == 0 || (year % 4 == 0 && year % 100 != 0);
        }

        /// <summary>
        /// ��ȡ����������
        /// </summary>
        /// <param name="year">���</param>
        /// <param name="month">�·�</param>
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