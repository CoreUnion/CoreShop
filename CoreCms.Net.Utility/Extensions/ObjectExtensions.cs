/***********************************************************************
 *            Project: CoreCms.Net                                     *
 *                Web: https://CoreCms.Net                             *
 *        ProjectName: 核心内容管理系统                                *
 *             Author: 大灰灰                                          *
 *              Email: JianWeie@163.com                                *
 *           Versions: 1.0                                             *
 *         CreateTime: 2020-02-01 17:48:52
 *          NameSpace: CoreCms.Net.Framework.Utility.Extensions
 *           FileName: ConvertExtensions
 *   ClassDescription:
 ***********************************************************************/


using System;
using System.Collections.Generic;
using System.Text;

namespace CoreCms.Net.Utility.Extensions
{
    /// <summary>
    /// 扩展数据转换
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// 数据转换为int类型
        /// </summary>
        /// <param name="thisValue"></param>
        /// <returns></returns>
        public static int ObjectToInt(this object thisValue)
        {
            int result = 0;
            if (thisValue == null)
                return 0;
            return thisValue != null && thisValue != DBNull.Value && int.TryParse(thisValue.ToString(), out result) ? result : result;
        }

        /// <summary>
        /// 数据转换为int类型
        /// </summary>
        /// <param name="thisValue"></param>
        /// <param name="errorValue"></param>
        /// <returns></returns>
        public static int ObjectToInt(this object thisValue, int errorValue)
        {
            int result = 0;
            return thisValue != null && thisValue != DBNull.Value && int.TryParse(thisValue.ToString(), out result) ? result : errorValue;
        }

        /// <summary>
        /// 数据转换为Double类型
        /// </summary>
        /// <param name="thisValue"></param>
        /// <returns></returns>
        public static double ObjectToDouble(this object thisValue)
        {
            double result = 0.0;
            return thisValue != null && thisValue != DBNull.Value && double.TryParse(thisValue.ToString(), out result) ? result : 0.0;
        }

        /// <summary>
        /// 数据转换为Double类型
        /// </summary>
        /// <param name="thisValue"></param>
        /// <param name="errorValue"></param>
        /// <returns></returns>
        public static double ObjectToDouble(this object thisValue, double errorValue)
        {
            double result = 0.0;
            return thisValue != null && thisValue != DBNull.Value && double.TryParse(thisValue.ToString(), out result) ? result : errorValue;
        }

        /// <summary>
        /// 数据转换为Float类型
        /// </summary>
        /// <param name="thisValue"></param>
        /// <returns></returns>
        public static float ObjectToFloat(this object thisValue)
        {
            float result = 0;
            return thisValue != null && thisValue != DBNull.Value && float.TryParse(thisValue.ToString(), out result) ? result : 0;
        }

        /// <summary>
        /// 数据转换为Float类型
        /// </summary>
        /// <param name="thisValue"></param>
        /// <param name="errorValue"></param>
        /// <returns></returns>
        public static float ObjectToFloat(this object thisValue, float errorValue)
        {
            float result = 0;
            return thisValue != null && thisValue != DBNull.Value && float.TryParse(thisValue.ToString(), out result) ? result : errorValue;
        }

        /// <summary>
        /// 数据转换为String类型
        /// </summary>
        /// <param name="thisValue"></param>
        /// <returns></returns>
        public static string ObjectToString(this object thisValue)
        {
            return thisValue != null ? thisValue.ToString().Trim() : "";
        }

        /// <summary>
        /// 数据转换为String类型
        /// </summary>
        /// <param name="thisValue"></param>
        /// <param name="errorValue"></param>
        /// <returns></returns>
        public static string ObjectToString(this object thisValue, string errorValue)
        {
            return thisValue != null ? thisValue.ToString().Trim() : errorValue;
        }

        /// <summary>
        /// 数据转换为Decimal类型
        /// </summary>
        /// <param name="thisValue"></param>
        /// <returns></returns>
        public static Decimal ObjectToDecimal(this object thisValue)
        {
            Decimal result = new Decimal();
            return thisValue != null && thisValue != DBNull.Value && Decimal.TryParse(thisValue.ToString(), out result) ? result : Decimal.Zero;
        }

        /// <summary>
        /// 数据转换为Decimal类型
        /// </summary>
        /// <param name="thisValue"></param>
        /// <param name="errorValue"></param>
        /// <returns></returns>
        public static Decimal ObjectToDecimal(this object thisValue, Decimal errorValue)
        {
            Decimal result = new Decimal();
            return thisValue != null && thisValue != DBNull.Value && Decimal.TryParse(thisValue.ToString(), out result) ? result : errorValue;
        }

        /// <summary>
        /// 数据转换为DateTime类型
        /// </summary>
        /// <param name="thisValue"></param>
        /// <returns></returns>
        public static DateTime ObjectToDate(this object thisValue)
        {
            DateTime result = DateTime.MinValue;
            if (thisValue != null && thisValue != DBNull.Value && DateTime.TryParse(thisValue.ToString(), out result))
                result = Convert.ToDateTime(thisValue);
            return result;
        }

        /// <summary>
        /// 数据转换为DateTime类型
        /// </summary>
        /// <param name="thisValue"></param>
        /// <param name="errorValue"></param>
        /// <returns></returns>
        public static DateTime ObjectToDate(this object thisValue, DateTime errorValue)
        {
            DateTime result = DateTime.MinValue;
            return thisValue != null && thisValue != DBNull.Value && DateTime.TryParse(thisValue.ToString(), out result) ? result : errorValue;
        }

        /// <summary>
        /// 数据转换为bool类型
        /// </summary>
        /// <param name="thisValue"></param>
        /// <returns></returns>
        public static bool ObjectToBool(this object thisValue)
        {
            bool result = false;
            return thisValue != null && thisValue != DBNull.Value && bool.TryParse(thisValue.ToString(), out result) ? result : result;
        }




    }
}
