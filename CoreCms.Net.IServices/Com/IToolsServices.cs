/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using System;
using System.IO;
using System.Threading.Tasks;
using CoreCms.Net.Configuration;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.Options;
using CoreCms.Net.Model.ViewModels.UI;
using Microsoft.AspNetCore.Http;

namespace CoreCms.Net.IServices
{
    /// <summary>
    ///     标签表 服务工厂接口
    /// </summary>
    public interface IToolsServices
    {


        /// <summary>
        /// 查询是否存在违规内容并进行替换
        /// </summary>
        /// <returns></returns>
        Task<String> IllegalWordsReplace(string oldString, char symbol = '*');


        /// <summary>
        /// 查询是否存在违规内容
        /// </summary>
        /// <returns></returns>
        Task<bool> IllegalWordsContainsAny(string oldString);

        #region FIle文件上传处理
        /// <summary>
        /// 本地上传（File）
        /// </summary>
        /// <returns></returns>
        Task<string> UpLoadFileForLocalStorage(FilesStorageOptions options, string fileExt, IFormFile file, int filesStorageLocation = (int)GlobalEnumVars.FilesStorageLocation.Admin);

        /// <summary>
        /// AliYunOSS-阿里云上传方法（File）
        /// </summary>
        /// <returns></returns>
        Task<string> UpLoadFileForAliYunOSS(FilesStorageOptions options, string fileExt, IFormFile file);

        /// <summary>
        /// QCloudOSS-腾讯云存储上传方法（File）
        /// </summary>
        /// <returns></returns>
        Task<string> UpLoadFileForQCloudOSS(FilesStorageOptions options, string fileExt, IFormFile file);
        /// <summary>
        /// QiNiuKoDo-七牛云存储上传方法（File）
        /// </summary>
        /// <returns></returns>
        Task<string> UpLoadFileForQiNiuKoDo(FilesStorageOptions options, string fileExt, IFormFile file);
        #endregion



        #region Base64文件上传处理
        /// <summary>
        /// 本地上传（Base64）
        /// </summary>
        /// <returns></returns>
        string UpLoadBase64ForLocalStorage(FilesStorageOptions options, MemoryStream memStream, int filesStorageLocation = (int)GlobalEnumVars.FilesStorageLocation.Admin);

        /// <summary>
        /// AliYunOSS-阿里云上传方法（Base64）
        /// </summary>
        /// <returns></returns>
        Task<string> UpLoadBase64ForAliYunOSS(FilesStorageOptions options, MemoryStream memStream);

        /// <summary>
        /// QCloudOSS-腾讯云存储上传方法（Base64）
        /// </summary>
        /// <returns></returns>
        string UpLoadBase64ForQCloudOSS(FilesStorageOptions options, byte[] bytes);

        /// <summary>
        /// QiNiuKoDo-七牛云存储上传方法（Base64）
        /// </summary>
        /// <returns></returns>
        string UpLoadBase64ForQiNiuKoDo(FilesStorageOptions options, byte[] bytes);

        #endregion




    }
}