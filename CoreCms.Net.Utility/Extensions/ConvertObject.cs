using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CoreCms.Net.Utility.Extensions
{
    public class ConvertObjectExtensions
    {

        #region Method1

        /// <summary>
        /// 将object对象转换为实体对象
        /// </summary>
        /// <typeparam name="T">实体对象类名</typeparam>
        /// <param name="asObject">object对象</param>
        /// <returns></returns>
        private T ConvertObject<T>(object asObject) where T : new()
        {
            //创建实体对象实例
            var t = Activator.CreateInstance<T>();
            if (asObject != null)
            {
                Type type = asObject.GetType();
                //遍历实体对象属性
                foreach (var info in typeof(T).GetProperties())
                {
                    object obj = null;
                    //取得object对象中此属性的值
                    var val = type.GetProperty(info.Name)?.GetValue(asObject);
                    if (val != null)
                    {
                        //非泛型
                        if (!info.PropertyType.IsGenericType)
                            obj = Convert.ChangeType(val, info.PropertyType);
                        else//泛型Nullable<>
                        {
                            Type genericTypeDefinition = info.PropertyType.GetGenericTypeDefinition();
                            if (genericTypeDefinition == typeof(Nullable<>))
                            {
                                obj = Convert.ChangeType(val, Nullable.GetUnderlyingType(info.PropertyType));
                            }
                            else
                            {
                                obj = Convert.ChangeType(val, info.PropertyType);
                            }
                        }
                        info.SetValue(t, obj, null);
                    }
                }
            }
            return t;
        }

        #endregion

        #region Method2

        /// <summary>
        /// 将object对象转换为实体对象
        /// </summary>
        /// <typeparam name="T">实体对象类名</typeparam>
        /// <param name="asObject">object对象</param>
        /// <returns></returns>
        public static T ConvertObjectByJson<T>(object asObject) where T : new()
        {
            //将object对象转换为json字符
            var json = JsonConvert.SerializeObject(asObject);
            //将json字符转换为实体对象
            var t = JsonConvert.DeserializeObject<T>(json);
            return t;
        }
        #endregion


        #region Method3
        /// <summary>
        /// 将object尝试转为指定对象
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T ConvertObjToModel<T>(object data) where T : new()
        {
            if (data == null) return new T();
            // 定义集合
            T result = new T();

            // 获得此模型的类型
            Type type = typeof(T);
            string tempName = "";

            // 获得此模型的公共属性
            PropertyInfo[] propertys = result.GetType().GetProperties();
            foreach (PropertyInfo pi in propertys)
            {
                tempName = pi.Name;  // 检查object是否包含此列

                // 判断此属性是否有Setter
                if (!pi.CanWrite) continue;

                try
                {
                    object value = GetPropertyValue(data, tempName);
                    if (value != DBNull.Value)
                    {
                        Type tempType = pi.PropertyType;
                        pi.SetValue(result, GetDataByType(value, tempType), null);

                    }
                }
                catch
                { }

            }

            return result;
        }

        /// <summary>
        /// 获取一个类指定的属性值
        /// </summary>
        /// <param name="info">object对象</param>
        /// <param name="field">属性名称</param>
        /// <returns></returns>
        public static object GetPropertyValue(object info, string field)
        {
            if (info == null) return null;
            Type t = info.GetType();
            IEnumerable<System.Reflection.PropertyInfo> property = from pi in t.GetProperties() where pi.Name.ToLower() == field.ToLower() select pi;
            return property.First().GetValue(info, null);
        }

        /// <summary>
        /// 将数据转为制定类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data1"></param>
        /// <returns></returns>
        public static object GetDataByType(object data1, Type itype, params object[] myparams)
        {
            object result = new object();
            try
            {
                if (itype == typeof(decimal))
                {
                    result = Convert.ToDecimal(data1);
                    if (myparams.Length > 0)
                    {
                        result = Convert.ToDecimal(Math.Round(Convert.ToDecimal(data1), Convert.ToInt32(myparams[0])));
                    }
                }
                else if (itype == typeof(double))
                {

                    if (myparams.Length > 0)
                    {
                        result = Convert.ToDouble(Math.Round(Convert.ToDouble(data1), Convert.ToInt32(myparams[0])));
                    }
                    else
                    {
                        result = double.Parse(Convert.ToDecimal(data1).ToString("0.00"));
                    }
                }
                else if (itype == typeof(Int32))
                {
                    result = Convert.ToInt32(data1);
                }
                else if (itype == typeof(DateTime))
                {
                    result = Convert.ToDateTime(data1);
                }
                else if (itype == typeof(Guid))
                {
                    result = new Guid(data1.ToString());
                }
                else if (itype == typeof(string))
                {
                    result = data1.ToString();
                }
            }
            catch
            {
                if (itype == typeof(decimal))
                {
                    result = 0;
                }
                else if (itype == typeof(double))
                {
                    result = 0;
                }
                else if (itype == typeof(Int32))
                {
                    result = 0;
                }
                else if (itype == typeof(DateTime))
                {
                    result = null;
                }
                else if (itype == typeof(Guid))
                {
                    result = Guid.Empty;
                }
                else if (itype == typeof(string))
                {
                    result = "";
                }
            }
            return result;
        }
        #endregion

    }
}
