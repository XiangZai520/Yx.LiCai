﻿#region

using System;

#endregion

namespace YxLiCai.Tools.Util
{
    public class ParseHelper
    {
        #region ToInt

        /// <summary>
        /// 将参数中的值转换成int类型
        /// </summary>
        /// <param name="o">要转换的值</param>
        /// <returns></returns>
        public static int ToInt(object o)
        {
            return ToInt(o, 0);
        }

        /// <summary>
        /// 将参数中的值转换成int类型
        /// </summary>
        /// <param name="o">要转换的值</param>
        /// <returns></returns>
        public static int DecimalToInt(decimal o)
        {
            int result = 0;
            try
            {
                result = Decimal.ToInt32(o);
            }
            catch
            {
                result = 0;
            }
            return result;
        }

        /// <summary>
        /// 将参数中的值转换成int类型
        /// </summary>
        /// <param name="o">要转换的值</param>
        /// <param name="DefaultValue">转换失败后返回的默认值</param>
        /// <returns></returns>
        public static int ToInt(object o, int DefaultValue)
        {
            int result = 0;
            if (!int.TryParse(Convert.ToString(o), out result))
                result = DefaultValue;
            return result;
        }

        #endregion

        #region ToLong

        /// <summary>
        /// 将参数中的值转换成long类型
        /// </summary>
        /// <param name="o">要转换的值</param>
        /// <returns></returns>
        public static long ToLong(object o)
        {
            return ToLong(o, 0);
        }

        /// <summary>
        /// 将参数中的值转换成long类型
        /// </summary>
        /// <param name="o">要转换的值</param>
        /// <param name="DefaultValue">转换失败后返回的默认值</param>
        /// <returns></returns>
        public static long ToLong(object o, long DefaultValue)
        {
            long result = 0;
            if (!long.TryParse(Convert.ToString(o), out result))
                result = DefaultValue;
            return result;
        }

        #endregion

        #region ToDecimal

        /// <summary>
        /// 将参数中的值转换成decimal类型
        /// </summary>
        /// <param name="o">要转换的值</param>
        /// <returns></returns>
        public static decimal ToDecimal(object o)
        {
            return ToDecimal(o, 0);
        }

        /// <summary>
        /// 将参数中的值转换成decimal类型
        /// </summary>
        /// <param name="o">要转换的值</param>
        /// <param name="DefaultValue">转换失败后返回的默认值</param>
        /// <returns></returns>
        public static decimal ToDecimal(object o, decimal DefaultValue)
        {
            decimal result = 0;
            if (!decimal.TryParse(Convert.ToString(o), out result))
                result = DefaultValue;
            return result;
        }

        #endregion

        #region ToDouble

        /// <summary>
        /// 将参数中的值转换成double类型
        /// </summary>
        /// <param name="o">要转换的值</param>
        /// <returns></returns>
        public static double ToDouble(object o)
        {
            return ToDouble(o, 0);
        }

        /// <summary>
        /// 将参数中的值转换成double类型
        /// </summary>
        /// <param name="o">要转换的值</param>
        /// <param name="DefaultValue">转换失败后返回的默认值</param>
        /// <returns></returns>
        public static double ToDouble(object o, double DefaultValue)
        {
            double result = 0;
            if (!double.TryParse(Convert.ToString(o), out result))
                result = DefaultValue;
            return result;
        }

        #endregion

        #region ToBool

        /// <summary>
        /// 将参数中的值转换成bool类型
        /// </summary>
        /// <param name="o">要转换的值</param>
        /// <returns></returns>
        public static bool ToBool(object o)
        {
            return ToBool(o, false);
        }

        /// <summary>
        /// 将参数中的值转换成bool类型
        /// </summary>
        /// <param name="o">要转换的值</param>
        /// <param name="DefaultValue">转换失败后返回的默认值</param>
        /// <returns></returns>
        public static bool ToBool(object o, bool DefaultValue)
        {
            bool result = false;
            if (!bool.TryParse(Convert.ToString(o), out result))
                result = DefaultValue;
            return result;
        }

        #endregion

        #region ToDatetime

        /// <summary>
        /// 将参数中的值转换成DateTime类型
        /// </summary>
        /// <param name="o">要转换的值</param>
        /// <returns></returns>
        public static DateTime ToDatetime(object o)
        {
            return ToDatetime(o, DateTime.MinValue);
        }

        /// <summary>
        /// 将参数中的值转换成DateTime类型
        /// </summary>
        /// <param name="o">要转换的值</param>
        /// <param name="DefaultValue">转换失败后返回的默认值</param>
        /// <returns></returns>
        public static DateTime ToDatetime(object o, DateTime DefaultValue)
        {
            DateTime result = DateTime.MinValue;
            if (!DateTime.TryParse(Convert.ToString(o), out result))
                result = DefaultValue;
            return result;
        }

        #endregion

        #region ToString

        /// <summary>
        /// 将参数中的值转换成string类型
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string ToString(object o)
        {
            return Convert.ToString(o);
        }

        #endregion

        #region Division
        /// <summary>
        /// 两数相除
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <returns></returns>
        public static double ToDivision(double num1, double num2)
        {
            if (num2 <= 0)
            {
                return 0;
            }
            double numtmp = num1 / num2;
            return Math.Round(numtmp, 2);
        }

        //public static decimal ToDivision(decimal num1, decimal num2)
        //{
        //    if (num2 <= 0)
        //    {
        //        return 0;
        //    }
        //    decimal numtmp = num1 / num2;
        //    return Math.Round(numtmp, 2);
        //}
        #endregion

        public static string ToSplitByPercentile(bool IsInt, object obj)
        {
            if (IsInt)
            {
                string temp = String.Format("{0:N}", obj).ToString();
                if (temp.Contains(".")) {
                    return String.Format("{0:N}", obj).ToString().Substring(0,temp.IndexOf('.'));
                } 
              
            }
            return String.Format("{0:N}", obj);
        }
    }
}