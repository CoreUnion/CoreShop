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
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Aliyun.OSS;
using Aliyun.OSS.Util;
using CoreCms.Net.Caching.Manual;
using CoreCms.Net.Configuration;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.IServices;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.Options;
using CoreCms.Net.Model.ViewModels.UI;
using COSXML;
using COSXML.Auth;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Qiniu.Storage;
using Qiniu.Util;
using SixLabors.ImageSharp;
using ToolGood.Words;

namespace CoreCms.Net.Services
{
    /// <summary>
    ///     标签表 接口实现
    /// </summary>
    public class ToolsServices : IToolsServices
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IWebHostEnvironment _webHostEnvironment;



        public ToolsServices(IWebHostEnvironment hostEnvironment, IWebHostEnvironment webHostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
            _webHostEnvironment = webHostEnvironment;
        }


        /// <summary>
        /// 查询是否存在违规内容并进行替换
        /// </summary>
        /// <returns></returns>
        public async Task<String> IllegalWordsReplace(string oldString, char symbol = '*')
        {
            var cache = ManualDataCache.Instance.Get<string>(ToolsVars.IllegalWordsCahceName);
            if (string.IsNullOrEmpty(cache))
            {
                IFileProvider fileProvider = this._hostEnvironment.ContentRootFileProvider;
                IFileInfo fileInfo = fileProvider.GetFileInfo("illegalWord/IllegalKeywords.txt");

                string fileContent = null;

                using (StreamReader readSteam = new StreamReader(fileInfo.CreateReadStream()))
                {
                    fileContent = await readSteam.ReadToEndAsync();
                }
                cache = fileContent;
                ManualDataCache.Instance.Set(ToolsVars.IllegalWordsCahceName, cache);
            }

            WordsMatch wordsSearch = new WordsMatch();
            wordsSearch.SetKeywords(cache.Split('|', StringSplitOptions.RemoveEmptyEntries));

            var t = wordsSearch.Replace(oldString, symbol);
            return t;
        }


        /// <summary>
        /// 查询是否存在违规内容
        /// </summary>
        /// <returns></returns>
        public async Task<bool> IllegalWordsContainsAny(string oldString)
        {
            var cache = ManualDataCache.Instance.Get<string>(ToolsVars.IllegalWordsCahceName);
            if (string.IsNullOrEmpty(cache))
            {
                IFileProvider fileProvider = this._hostEnvironment.ContentRootFileProvider;
                IFileInfo fileInfo = fileProvider.GetFileInfo("illegalWord/IllegalKeywords.txt");

                string fileContent = null;

                using (StreamReader readSteam = new StreamReader(fileInfo.CreateReadStream()))
                {
                    fileContent = await readSteam.ReadToEndAsync();
                }
                cache = fileContent;
                ManualDataCache.Instance.Set(ToolsVars.IllegalWordsCahceName, cache);
            }

            WordsMatch wordsSearch = new WordsMatch();
            wordsSearch.SetKeywords(cache.Split('|', StringSplitOptions.RemoveEmptyEntries));

            var bl = wordsSearch.ContainsAny(oldString);

            return bl;
        }


        #region 本地上传方法(File)

        /// <summary>
        /// 本地上传方法(File)
        /// </summary>
        /// <param name="options"></param>
        /// <param name="fileExt"></param>
        /// <param name="file"></param>
        /// <param name="filesStorageLocation"></param>
        /// <returns></returns>
        public async Task<string> UpLoadFileForLocalStorage(FilesStorageOptions options, string fileExt, IFormFile file, int filesStorageLocation = (int)GlobalEnumVars.FilesStorageLocation.Admin)
        {

            var newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + fileExt;
            var today = DateTime.Now.ToString("yyyyMMdd");

            var saveUrl = options.Path + today + "/";
            var dirPath = _webHostEnvironment.WebRootPath + saveUrl;
            if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
            var filePath = dirPath + newFileName;
            var fileUrl = saveUrl + newFileName;

            string bucketBindDomain = string.Empty;
            if (filesStorageLocation == (int)GlobalEnumVars.FilesStorageLocation.Admin)
            {
                bucketBindDomain = AppSettingsConstVars.AppConfigAppUrl;
            }
            else if (filesStorageLocation == (int)GlobalEnumVars.FilesStorageLocation.API)
            {
                bucketBindDomain = AppSettingsConstVars.AppConfigAppInterFaceUrl;
            }

            await using (var fs = System.IO.File.Create(filePath))
            {
                await file.CopyToAsync(fs);
                fs.Flush();
            }

            return bucketBindDomain + fileUrl;
        }
        #endregion

        #region 阿里云上传方法（File）
        /// <summary>
        /// 阿里云上传方法（File）
        /// </summary>
        /// <param name="options"></param>
        /// <param name="fileExt"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<string> UpLoadFileForAliYunOSS(FilesStorageOptions options, string fileExt, IFormFile file)
        {
            var jm = new AdminUiCallBack();

            var newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + fileExt;
            var today = DateTime.Now.ToString("yyyyMMdd");

            //上传到阿里云
            await using var fileStream = file.OpenReadStream();
            var md5 = OssUtils.ComputeContentMd5(fileStream, file.Length);

            var filePath = options.Path + today + "/" + newFileName; //云文件保存路径
            //初始化阿里云配置--外网Endpoint、访问ID、访问password
            var aliYun = new OssClient(options.Endpoint, options.AccessKeyId, options.AccessKeySecret);
            //将文件md5值赋值给meat头信息，服务器验证文件MD5
            var objectMeta = new ObjectMetadata
            {
                ContentMd5 = md5
            };
            //文件上传--空间名、文件保存路径、文件流、meta头信息(文件md5) //返回meta头信息(文件md5)
            aliYun.PutObject(options.BucketName, filePath, fileStream, objectMeta);
            //返回给UEditor的插入编辑器的图片的src

            return options.BucketBindUrl + filePath;
        }

        #endregion

        #region 腾讯云存储上传方法（File）
        /// <summary>
        /// 腾讯云存储上传方法（File）
        /// </summary>
        /// <param name="options"></param>
        /// <param name="fileExt"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<string> UpLoadFileForQCloudOSS(FilesStorageOptions options, string fileExt, IFormFile file)
        {
            var jm = new AdminUiCallBack();

            var newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + fileExt;
            var today = DateTime.Now.ToString("yyyyMMdd");

            var filePath = options.Path + today + "/" + newFileName; //云文件保存路径

            //上传到腾讯云OSS
            //初始化 CosXmlConfig
            string appid = options.AccountId;//设置腾讯云账户的账户标识 APPID
            string region = options.CosRegion; //设置一个默认的存储桶地域
            CosXmlConfig config = new CosXmlConfig.Builder()
                //.SetAppid(appid)
                .IsHttps(true)  //设置默认 HTTPS 请求
                .SetRegion(region)  //设置一个默认的存储桶地域
                .SetDebugLog(true)  //显示日志
                .Build();  //创建 CosXmlConfig 对象

            long durationSecond = 600;  //每次请求签名有效时长，单位为秒
            QCloudCredentialProvider qCloudCredentialProvider = new DefaultQCloudCredentialProvider(options.AccessKeyId, options.AccessKeySecret, durationSecond);


            byte[] bytes;
            await using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                bytes = ms.ToArray();
            }

            var cosXml = new CosXmlServer(config, qCloudCredentialProvider);
            COSXML.Model.Object.PutObjectRequest putObjectRequest = new COSXML.Model.Object.PutObjectRequest(options.TencentBucketName, filePath, bytes);
            cosXml.PutObject(putObjectRequest);

            return options.BucketBindUrl + filePath;
        }
        #endregion

        #region 七牛云存储上传（File）
        /// <summary>
        /// 七牛云存储上传（File）
        /// </summary>
        /// <param name="options"></param>
        /// <param name="fileExt"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<string> UpLoadFileForQiNiuKoDo(FilesStorageOptions options, string fileExt, IFormFile file)
        {
            var jm = new AdminUiCallBack();

            var newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_fffff", DateTimeFormatInfo.InvariantInfo) + fileExt;

            Mac mac = new Mac(options.AccessKeyId, options.AccessKeySecret);

            byte[] bytes;
            await using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                bytes = ms.ToArray();
            }

            // 设置上传策略
            PutPolicy putPolicy = new PutPolicy();
            // 设置要上传的目标空间
            putPolicy.Scope = options.QiNiuBucketName;
            // 上传策略的过期时间(单位:秒)
            putPolicy.SetExpires(3600);
            // 文件上传完毕后，在多少天后自动被删除
            //putPolicy.DeleteAfterDays = 1;
            // 生成上传token
            string token = Qiniu.Util.Auth.CreateUploadToken(mac, putPolicy.ToJsonString());

            Config config = new Config();
            // 设置 http 或者 https 上传
            config.UseHttps = true;
            config.UseCdnDomains = true;
            config.ChunkSize = ChunkUnit.U512K;

            UploadManager um = new UploadManager(config);
            var outData = um.UploadData(bytes, newFileName, token, null);

            return options.BucketBindUrl + newFileName;
        }
        #endregion


        #region 本地上传方法(Base64)

        /// <summary>
        /// 本地上传方法(Base64)
        /// </summary>
        /// <param name="options"></param>
        /// <param name="memStream"></param>
        /// <param name="filesStorageLocation"></param>
        /// <returns></returns>
        public string UpLoadBase64ForLocalStorage(FilesStorageOptions options, MemoryStream memStream, int filesStorageLocation = (int)GlobalEnumVars.FilesStorageLocation.Admin)
        {
            var jm = new AdminUiCallBack();

            var newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + ".jpg";
            var today = DateTime.Now.ToString("yyyyMMdd");

            byte[] data = new byte[memStream.Length];
            memStream.Seek(0, SeekOrigin.Begin);
            memStream.Read(data, 0, Convert.ToInt32(memStream.Length));
            SixLabors.ImageSharp.Image image = SixLabors.ImageSharp.Image.Load(new MemoryStream(data));


            var saveUrl = options.Path + today + "/";
            var dirPath = _webHostEnvironment.WebRootPath + saveUrl;

            string bucketBindDomain = string.Empty;
            if (filesStorageLocation == (int)GlobalEnumVars.FilesStorageLocation.Admin)
            {
                //bucketBindDomain = AppSettingsConstVars.AppConfigAppUrl;
                bucketBindDomain = !string.IsNullOrEmpty(options.BucketBindUrl) ? options.BucketBindUrl : AppSettingsConstVars.AppConfigAppUrl;
                ;
            }
            else if (filesStorageLocation == (int)GlobalEnumVars.FilesStorageLocation.API)
            {
                bucketBindDomain = AppSettingsConstVars.AppConfigAppInterFaceUrl;
            }

            if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
            var filePath = dirPath + newFileName;
            var fileUrl = saveUrl + newFileName;

            //保存到图片
            image.SaveAsync(filePath);

            return bucketBindDomain + fileUrl;
        }
        #endregion

        #region 阿里云上传方法（Base64）
        /// <summary>
        /// 阿里云上传方法（Base64）
        /// </summary>
        /// <param name="options"></param>
        /// <param name="memStream"></param>
        /// <returns></returns>
        public async Task<string> UpLoadBase64ForAliYunOSS(FilesStorageOptions options, MemoryStream memStream)
        {
            var jm = new AdminUiCallBack();

            var newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + ".jpg";
            var today = DateTime.Now.ToString("yyyyMMdd");

            // 设置当前流的位置为流的开始
            memStream.Seek(0, SeekOrigin.Begin);

            await using var fileStream = memStream;
            var md5 = OssUtils.ComputeContentMd5(fileStream, memStream.Length);

            var filePath = options.Path + today + "/" + newFileName; //云文件保存路径
            //初始化阿里云配置--外网Endpoint、访问ID、访问password
            var aliyun = new OssClient(options.Endpoint, options.AccessKeyId, options.AccessKeySecret);
            //将文件md5值赋值给meat头信息，服务器验证文件MD5
            var objectMeta = new ObjectMetadata
            {
                ContentMd5 = md5
            };
            //文件上传--空间名、文件保存路径、文件流、meta头信息(文件md5) //返回meta头信息(文件md5)
            aliyun.PutObject(options.BucketName, filePath, fileStream, objectMeta);
            //返回给UEditor的插入编辑器的图片的src

            return options.BucketBindUrl + filePath;

        }

        #endregion

        #region 腾讯云存储上传方法（Base64）

        /// <summary>
        /// 腾讯云存储上传方法（Base64）
        /// </summary>
        /// <param name="options"></param>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public string UpLoadBase64ForQCloudOSS(FilesStorageOptions options, byte[] bytes)
        {
            var jm = new AdminUiCallBack();

            var newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + ".jpg";
            var today = DateTime.Now.ToString("yyyyMMdd");

            //初始化 CosXmlConfig
            string appid = options.AccountId;//设置腾讯云账户的账户标识 APPID
            string region = options.CosRegion; //设置一个默认的存储桶地域
            CosXmlConfig config = new CosXmlConfig.Builder()
                //.SetAppid(appid)
                .IsHttps(true)  //设置默认 HTTPS 请求
                .SetRegion(region)  //设置一个默认的存储桶地域
                .SetDebugLog(true)  //显示日志
                .Build();  //创建 CosXmlConfig 对象

            long durationSecond = 600;  //每次请求签名有效时长，单位为秒
            QCloudCredentialProvider qCloudCredentialProvider = new DefaultQCloudCredentialProvider(options.AccessKeyId, options.AccessKeySecret, durationSecond);

            var cosXml = new CosXmlServer(config, qCloudCredentialProvider);

            var filePath = options.Path + today + "/" + newFileName; //云文件保存路径
            COSXML.Model.Object.PutObjectRequest putObjectRequest = new COSXML.Model.Object.PutObjectRequest(options.TencentBucketName, filePath, bytes);

            cosXml.PutObject(putObjectRequest);

            return options.BucketBindUrl + filePath;
        }
        #endregion

        #region 牛云上传方法（Base64）

        /// <summary>
        /// 七牛云上传方法（Base64）
        /// </summary>
        /// <param name="options"></param>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public string UpLoadBase64ForQiNiuKoDo(FilesStorageOptions options, byte[] bytes)
        {
            var newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_fffff", DateTimeFormatInfo.InvariantInfo) + ".jpg";

            Mac mac = new Mac(options.AccessKeyId, options.AccessKeySecret);

            // 设置上传策略
            PutPolicy putPolicy = new PutPolicy();
            // 设置要上传的目标空间
            putPolicy.Scope = options.QiNiuBucketName;
            // 上传策略的过期时间(单位:秒)
            putPolicy.SetExpires(3600);
            // 文件上传完毕后，在多少天后自动被删除
            //putPolicy.DeleteAfterDays = 1;
            // 生成上传token
            string token = Qiniu.Util.Auth.CreateUploadToken(mac, putPolicy.ToJsonString());

            Config config = new Config();
            // 设置 http 或者 https 上传
            config.UseHttps = true;
            config.UseCdnDomains = true;
            config.ChunkSize = ChunkUnit.U512K;

            UploadManager um = new UploadManager(config);
            var outData = um.UploadData(bytes, newFileName, token, null);

            return options.BucketBindUrl + newFileName;
        }
        #endregion


    }
}