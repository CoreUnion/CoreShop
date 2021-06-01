using System;
using System.Collections.Generic;
using System.ComponentModel;
using CoreCms.Net.Model.ViewModels.UI;

namespace CoreCms.Net.Utility.Helper
{
    public class EnumHelper
    {
        /// <summary>
        /// 将枚举转成List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<EnumEntity> EnumToList<T>()
        {
            List<EnumEntity> list = new List<EnumEntity>();

            foreach (var e in Enum.GetValues(typeof(T)))
            {
                EnumEntity m = new EnumEntity();
                object[] objArr = e.GetType().GetField(e.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (objArr != null && objArr.Length > 0)
                {
                    DescriptionAttribute da = objArr[0] as DescriptionAttribute;
                    m.description = da.Description;
                }
                m.value = Convert.ToInt32(e);
                m.title = e.ToString();
                list.Add(m);
            }
            return list;
        }

        /// <summary>
        /// 根据枚举值来获取单个枚举实体
        /// </summary>
        /// <typeparam name="T">枚举</typeparam>
        /// <param name="value">value</param>
        /// <returns></returns>
        public static EnumEntity GetEnumberEntity<T>(int value)
        {
            foreach (var e in Enum.GetValues(typeof(T)))
            {
                EnumEntity m = new EnumEntity();
                object[] objArr = e.GetType().GetField(e.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (objArr != null && objArr.Length > 0)
                {
                    DescriptionAttribute da = objArr[0] as DescriptionAttribute;
                    m.description = da.Description;
                }
                m.value = Convert.ToInt32(e);
                m.title = e.ToString();
                if (value == m.value)
                {
                    return m;
                }
            }
            return null;
        }


        /// <summary>
        /// 根据枚举值value来获取单个枚举实体的文字描述内容
        /// </summary>
        /// <typeparam name="T">枚举</typeparam>
        /// <param name="value">value</param>
        /// <returns></returns>
        public static string GetEnumDescriptionByValue<T>(int value)
        {
            foreach (var e in Enum.GetValues(typeof(T)))
            {
                EnumEntity m = new EnumEntity();
                object[] objArr = e.GetType().GetField(e.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (objArr != null && objArr.Length > 0)
                {
                    DescriptionAttribute da = objArr[0] as DescriptionAttribute;
                    m.description = da.Description;
                }
                m.value = Convert.ToInt32(e);
                m.title = e.ToString();
                if (value == m.value)
                {
                    return m.description;
                }
            }
            return "";
        }

        /// <summary>
        /// 根据枚举key来获取单个枚举实体的文字描述内容
        /// </summary>
        /// <typeparam name="T">枚举</typeparam>
        /// <param name="key">value</param>
        /// <returns></returns>
        public static string GetEnumDescriptionByKey<T>(string key)
        {
            foreach (var e in Enum.GetValues(typeof(T)))
            {
                EnumEntity m = new EnumEntity();
                object[] objArr = e.GetType().GetField(e.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (objArr != null && objArr.Length > 0)
                {
                    DescriptionAttribute da = objArr[0] as DescriptionAttribute;
                    m.description = da.Description;
                }
                m.value = Convert.ToInt32(e);
                m.title = e.ToString();
                if (key == m.title)
                {
                    return m.description;
                }
            }
            return "";
        }

    }




}
