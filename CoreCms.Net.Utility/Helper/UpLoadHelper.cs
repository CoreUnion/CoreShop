/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/8/16 12:44:16
 *        Description: 暂无
 ***********************************************************************/


using CoreCms.Net.Configuration;

namespace CoreCms.Net.Utility.Helper
{
    /// <summary>
    /// 上传帮助类
    /// </summary>
    public static class UpLoadHelper
    {
        /// <summary>
        /// 上传路径格式化操作，防止不同类型下上传路径写入失败问题。
        /// </summary>
        /// <param name="storageType">上传类型</param>
        /// <param name="oldFilePath">原始路径</param>
        /// <returns></returns>
        public static string PathFormat(string storageType, string oldFilePath)
        {
            string newPath;
            switch (storageType)
            {
                case "LocalStorage":
                    newPath = oldFilePath.StartsWith("/") ? oldFilePath : "/" + oldFilePath;
                    break;
                case "AliYunOSS":
                    newPath = oldFilePath.StartsWith("/") ? oldFilePath.Substring(1) : oldFilePath;
                    break;
                case "QCloudOSS":
                    newPath = oldFilePath.StartsWith("/") ? oldFilePath.Substring(1) : oldFilePath;
                    break;
                default:
                    newPath = "/upload/";
                    break;
            }
            newPath = newPath.EndsWith("/") ? newPath : newPath + "/";
            return newPath;
        }
    }
}
