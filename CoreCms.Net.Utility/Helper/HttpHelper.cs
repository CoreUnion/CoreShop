/***********************************************************************
 *            Project: CoreCms.Net                                     *
 *                Web: https://CoreCms.Net                             *
 *        ProjectName: 核心内容管理系统                                *
 *             Author: 大灰灰                                          *
 *              Email: JianWeie@163.com                                *
 *         CreateTime: 2020-05-12 0:54:53
 *        Description: 暂无
 ***********************************************************************/


using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace CoreCms.Net.Utility.Helper
{
    /// <summary>
    /// 模拟标准表单Post提交
    /// </summary>
    public static class HttpHelper
    {
        /// <summary>
        /// 模拟标准表单Post提交
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postdate"></param>
        /// <returns></returns>
        public static string PostSend(string url, string postdate)
        {
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            myHttpWebRequest.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            myHttpWebRequest.Method = "POST";
            Stream myRequestStream = myHttpWebRequest.GetRequestStream();
            StreamWriter myStreamWriter = new StreamWriter(myRequestStream);
            myStreamWriter.Write(postdate);
            myStreamWriter.Flush();
            myStreamWriter.Close();
            myRequestStream.Close();

            HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
            Stream myResponseStream = myHttpWebResponse.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            String outdata = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            return outdata;
        }

    }
}
