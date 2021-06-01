/***********************************************************************
 *            Project: CoreCms.Net                                     *
 *                Web: https://CoreCms.Net                             *
 *        ProjectName: 核心内容管理系统                                *
 *             Author: 大灰灰                                          *
 *              Email: JianWeie@163.com                                *
 *         CreateTime: 2020-03-02 23:15:19
 *        Description: 暂无
 ***********************************************************************/


using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CoreCms.Net.Utility.Helper
{
    /// <summary>
    /// Json文件读写
    /// 引用Newtonsoft.Json
    /// </summary>
    public class JsonFileHelper
    {
        //注意：section为根节点
        private readonly string _path;
        private IConfiguration Configuration { get; set; }
        public JsonFileHelper(string jsonName)
        {
            _path = !jsonName.EndsWith(".json") ? $"{jsonName}.json" : jsonName;
            //ReloadOnChange = true 当*.json文件被修改时重新加载            
            Configuration = new ConfigurationBuilder()
            .Add(new JsonConfigurationSource { Path = _path, ReloadOnChange = true, Optional = true })
            .Build();
        }

        /// <summary>
        /// 读取Json返回实体对象
        /// </summary>
        /// <returns></returns>
        public T Read<T>() => Read<T>("");

        /// <summary>
        /// 根据节点读取Json返回实体对象
        /// </summary>
        /// <returns></returns>
        public T Read<T>(string section)
        {
            using var file = new StreamReader(_path);
            using var reader = new JsonTextReader(file);
            var jObj = (JObject)JToken.ReadFrom(reader);
            if (!string.IsNullOrWhiteSpace(section))
            {
                var secJt = jObj[section];
                if (secJt != null)
                {
                    return JsonConvert.DeserializeObject<T>(secJt.ToString());
                }
            }
            else
            {
                return JsonConvert.DeserializeObject<T>(jObj.ToString());
            }

            return default;
        }

        /// <summary>
        /// 读取Json返回集合
        /// </summary>
        /// <returns></returns>
        public List<T> ReadList<T>() => ReadList<T>("");

        /// <summary>
        /// 根据节点读取Json返回集合
        /// </summary>
        /// <returns></returns>
        public List<T> ReadList<T>(string section)
        {
            using var file = new StreamReader(_path);
            using var reader = new JsonTextReader(file);
            var jObj = (JObject)JToken.ReadFrom(reader);
            if (!string.IsNullOrWhiteSpace(section))
            {
                var secJt = jObj[section];
                if (secJt != null)
                {
                    return JsonConvert.DeserializeObject<List<T>>(secJt.ToString());
                }
            }
            else
            {
                return JsonConvert.DeserializeObject<List<T>>(jObj.ToString());
            }

            return default;
        }

        /// <summary>
        /// 写入文件
        /// </summary>
        /// <typeparam name="T">自定义对象</typeparam>
        /// <param name="t"></param>
        public void Write<T>(T t) => Write("", t);

        /// <summary>
        /// 写入指定section文件
        /// </summary>
        /// <typeparam name="T">自定义对象</typeparam>
        /// <param name="t"></param>
        public void Write<T>(string section, T t)
        {
            JObject jObj;
            using (StreamReader file = new StreamReader(_path))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                jObj = (JObject)JToken.ReadFrom(reader);
                var json = JsonConvert.SerializeObject(t);
                if (string.IsNullOrWhiteSpace(section))
                    jObj = JObject.Parse(json);
                else
                    jObj[section] = JObject.Parse(json);
            }

            using var writer = new StreamWriter(_path);
            using var jsonWriter = new JsonTextWriter(writer);
            jObj.WriteTo(jsonWriter);
        }

        /// <summary>
        /// 删除指定section节点
        /// </summary>
        /// <param name="section"></param>
        /// <exception cref="Exception"></exception>
        public void Remove(string section)
        {
            JObject jObj;
            using (StreamReader file = new StreamReader(_path))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                jObj = (JObject)JToken.ReadFrom(reader);
                jObj.Remove(section);
            }

            using (var writer = new StreamWriter(_path))
            using (var jsonWriter = new JsonTextWriter(writer))
            {
                jObj.WriteTo(jsonWriter);
            }
        }
    }
}
