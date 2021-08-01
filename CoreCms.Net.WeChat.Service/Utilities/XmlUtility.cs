/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/7/29 23:21:06
 *        Description: 暂无
 ***********************************************************************/


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CoreCms.Net.WeChat.Service.Utilities
{
    /// <summary>XML 工具类</summary>
    public static class XmlUtility
    {
        /// <summary>反序列化</summary>
        /// <param name="xml">XML字符串</param>
        /// <returns></returns>
        public static object Deserialize<T>(string xml)
        {
            try
            {
                using (StringReader stringReader = new StringReader(xml))
                    return new XmlSerializer(typeof(T)).Deserialize((TextReader)stringReader);
            }
            catch (Exception ex)
            {
                Console.WriteLine((object)ex);
                return (object)null;
            }
        }

        /// <summary>反序列化</summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static object Deserialize<T>(Stream stream) => new XmlSerializer(typeof(T)).Deserialize(stream);

        /// <summary>
        /// 序列化
        /// 说明：此方法序列化复杂类，如果没有声明XmlInclude等特性，可能会引发“使用 XmlInclude 或 SoapInclude 特性静态指定非已知的类型。”的错误。
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static string Serializer<T>(T obj)
        {
            MemoryStream memoryStream = new MemoryStream();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            try
            {
                xmlSerializer.Serialize((Stream)memoryStream, (object)obj);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            memoryStream.Position = 0L;
            StreamReader streamReader = new StreamReader((Stream)memoryStream);
            string end = streamReader.ReadToEnd();
            streamReader.Dispose();
            memoryStream.Dispose();
            return end;
        }

        /// <summary>序列化将流转成XML字符串</summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static XDocument Convert(Stream stream)
        {
            if (stream.CanSeek)
                stream.Seek(0L, SeekOrigin.Begin);
            using (XmlReader reader = XmlReader.Create(stream))
                return XDocument.Load(reader);
        }

        /// <summary>序列化将流转成XML字符串</summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string ConvertToString(Stream stream)
        {
            StreamReader reader = new StreamReader(stream);
            string sHtml = reader.ReadToEnd();
            return sHtml;
        }

    }
}
