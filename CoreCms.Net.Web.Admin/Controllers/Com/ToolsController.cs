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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Aliyun.OSS;
using Aliyun.OSS.Util;
using CoreCms.Net.Auth.HttpContextUser;
using CoreCms.Net.Configuration;
using CoreCms.Net.Filter;
using CoreCms.Net.IServices;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.Entities.Expression;
using CoreCms.Net.Model.FromBody;
using CoreCms.Net.Model.ViewModels.Echarts;
using CoreCms.Net.Model.ViewModels.Options;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Model.ViewModels.View;
using CoreCms.Net.Utility.Extensions;
using CoreCms.Net.Utility.Helper;
using COSXML;
using COSXML.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Senparc.Weixin;
using Senparc.Weixin.WxOpen.AdvancedAPIs.WxApp;
using SqlSugar;

namespace CoreCms.Net.Web.Admin.Controllers
{
    /// <summary>
    ///     后端常用方法
    /// </summary>
    [Description("后端常用方法")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ToolsController : ControllerBase
    {
        private readonly ICoreCmsAreaServices _areaServices;
        private readonly ICodeGeneratorServices _codeGeneratorServices;
        private readonly ICoreCmsArticleServices _coreCmsArticleServices;
        private readonly ICoreCmsArticleTypeServices _coreCmsArticleTypeServices;
        private readonly ICoreCmsFormServices _coreCmsFormServices;


        private readonly ICoreCmsGoodsServices _coreCmsGoodsServices;
        private readonly ICoreCmsNoticeServices _coreCmsNoticeServices;
        private readonly ICoreCmsPinTuanRuleServices _coreCmsPinTuanRuleServices;
        private readonly ICoreCmsPromotionServices _coreCmsPromotionServices;
        private readonly ICoreCmsSettingServices _coreCmsSettingServices;
        private readonly ICoreCmsLogisticsServices _logisticsServices;
        private readonly ISysMenuServices _sysMenuServices;
        private readonly ISysOrganizationServices _sysOrganizationServices;
        private readonly ISysRoleServices _sysRoleServices;
        private readonly ISysUserRoleServices _sysUserRoleServices;

        private readonly ISysRoleMenuServices _sysRoleMenuServices;


        private readonly ISysUserServices _sysUserServices;
        private readonly IHttpContextUser _user;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ISysLoginRecordServices _sysLoginRecordServices;
        private readonly ISysNLogRecordsServices _sysNLogRecordsServices;
        private readonly ICoreCmsBillPaymentsServices _paymentsServices;
        private readonly ICoreCmsBillDeliveryServices _billDeliveryServices;
        private readonly ICoreCmsBillAftersalesServices _aftersalesServices;
        private readonly ICoreCmsUserServices _userServices;
        private readonly ICoreCmsOrderServices _orderServices;
        private readonly ICoreCmsSettingServices _settingServices;
        private readonly ICoreCmsProductsServices _productsServices;
        private readonly ICoreCmsServicesServices _servicesServices;


        private static readonly string WxOpenAppId = Config.SenparcWeixinSetting.WxOpenAppId;//与微信小程序后台的AppId设置保持一致，区分大小写。
        private static readonly string WxOpenAppSecret = Config.SenparcWeixinSetting.WxOpenAppSecret;//与微信小程序账号后台的AppId设置保持一致，区分大小写。

        /// <summary>
        ///     构造函数
        /// </summary>
        public ToolsController(
            IHttpContextUser user
            , IWebHostEnvironment webHostEnvironment
            , ICoreCmsGoodsServices coreCmsGoodsServices
            , ICoreCmsSettingServices coreCmsSettingServices
            , ICoreCmsArticleServices coreCmsArticleServices
            , ICoreCmsFormServices coreCmsFormServices
            , ICoreCmsArticleTypeServices coreCmsArticleTypeServices
            , ICoreCmsNoticeServices coreCmsNoticeServices
            , ICoreCmsPinTuanRuleServices coreCmsPinTuanRuleServices
            , ICoreCmsPromotionServices coreCmsPromotionServices
            , ICoreCmsAreaServices areaServices
            , ISysUserServices sysUserServices
            , ISysRoleServices sysRoleServices
            , ISysMenuServices sysMenuServices
            , ISysUserRoleServices sysUserRoleServices
            , ISysOrganizationServices sysOrganizationServices, ICodeGeneratorServices codeGeneratorServices,
            ICoreCmsLogisticsServices logisticsServices, ISysLoginRecordServices sysLoginRecordServices, ISysNLogRecordsServices sysNLogRecordsServices, ICoreCmsBillPaymentsServices paymentsServices, ICoreCmsBillDeliveryServices billDeliveryServices, ICoreCmsUserServices userServices, ICoreCmsOrderServices orderServices, ICoreCmsBillAftersalesServices aftersalesServices, ICoreCmsSettingServices settingServices, ICoreCmsProductsServices productsServices, ICoreCmsServicesServices servicesServices, IOptions<FilesStorageOptions> filesStorageOptions, ISysRoleMenuServices sysRoleMenuServices)
        {
            _user = user;
            _webHostEnvironment = webHostEnvironment;

            _coreCmsGoodsServices = coreCmsGoodsServices;
            _coreCmsSettingServices = coreCmsSettingServices;
            _coreCmsArticleServices = coreCmsArticleServices;
            _coreCmsFormServices = coreCmsFormServices;
            _coreCmsArticleTypeServices = coreCmsArticleTypeServices;
            _coreCmsNoticeServices = coreCmsNoticeServices;
            _coreCmsPinTuanRuleServices = coreCmsPinTuanRuleServices;
            _coreCmsPromotionServices = coreCmsPromotionServices;
            _areaServices = areaServices;
            _sysUserServices = sysUserServices;
            _sysRoleServices = sysRoleServices;
            _sysMenuServices = sysMenuServices;
            _sysUserRoleServices = sysUserRoleServices;
            _sysOrganizationServices = sysOrganizationServices;
            _codeGeneratorServices = codeGeneratorServices;
            _logisticsServices = logisticsServices;
            _sysLoginRecordServices = sysLoginRecordServices;
            _sysNLogRecordsServices = sysNLogRecordsServices;
            _paymentsServices = paymentsServices;
            _billDeliveryServices = billDeliveryServices;
            _userServices = userServices;
            _orderServices = orderServices;
            _aftersalesServices = aftersalesServices;
            _settingServices = settingServices;
            _productsServices = productsServices;
            _servicesServices = servicesServices;
            _sysRoleMenuServices = sysRoleMenuServices;
        }

        #region 获取登录用户用户信息(用于面板展示)====================================================

        /// <summary>
        ///     获取登录用户用户信息(用于面板展示)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetUserInfo()
        {
            var jm = new AdminUiCallBack();
            var userModel = await _sysUserServices.QueryByIdAsync(_user.ID);
            jm.code = 0;
            jm.msg = "数据获取正常";
            jm.data = new
            {
                userModel.userName,
                userModel.nickName,
                userModel.createTime
            };
            return new JsonResult(jm);
        }

        #endregion

        #region 获取登录用户用户全部信息(用于编辑个人信息)====================================================

        /// <summary>
        ///     获取登录用户用户全部信息(用于编辑个人信息)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetEditUserInfo()
        {
            var jm = new AdminUiCallBack();
            var userModel = await _sysUserServices.QueryByIdAsync(_user.ID);

            if (userModel != null)
            {
                var roles = await _sysUserRoleServices.QueryListByClauseAsync(p => p.userId == userModel.id);
                if (roles.Any())
                {
                    var roleIds = roles.Select(p => p.roleId).ToList();
                    userModel.roles = await _sysRoleServices.QueryListByClauseAsync(p => roleIds.Contains(p.id));
                }

                if (userModel.organizationId != null && userModel.organizationId > 0)
                {
                    var organization = await _sysOrganizationServices.QueryByIdAsync(userModel.organizationId);
                    if (organization != null) userModel.organizationName = organization.organizationName;
                }
            }

            jm.code = 0;
            jm.msg = "数据获取正常";
            jm.data = userModel;
            return new JsonResult(jm);
        }

        #endregion

        #region 获取角色列表信息====================================================

        /// <summary>
        ///     获取角色列表信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetManagerRoles()
        {
            var jm = new AdminUiCallBack();
            var roles = await _sysRoleServices.QueryAsync();
            jm.code = 0;
            jm.msg = "数据获取正常";
            jm.data = roles.Select(p => new { title = p.roleName, value = p.id });
            return new JsonResult(jm);
        }

        #endregion

        #region 用户编辑个人登录账户密码====================================================

        /// <summary>
        ///     获取登录用户用户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> EditLoginUserPassWord([FromBody] FMEditLoginUserPassWord entity)
        {
            var jm = new AdminUiCallBack();

            if (string.IsNullOrEmpty(entity.oldPassword))
            {
                jm.msg = "请键入旧密码";
                return new JsonResult(jm);
            }

            if (string.IsNullOrEmpty(entity.password))
            {
                jm.msg = "请键入新密码";
                return new JsonResult(jm);
            }

            if (string.IsNullOrEmpty(entity.repassword))
            {
                jm.msg = "请键入新密码确认密码";
                return new JsonResult(jm);
            }

            if (entity.password != entity.repassword)
            {
                jm.msg = "新密码与确认密码不相符";
                return new JsonResult(jm);
            }

            if (CommonHelper.Md5For32(entity.oldPassword) == CommonHelper.Md5For32(entity.password))
            {
                jm.msg = "新密码与旧密码相同,无需修改";
                return new JsonResult(jm);
            }

            var userModel = await _sysUserServices.QueryByIdAsync(_user.ID);

            if (userModel.passWord != CommonHelper.Md5For32(entity.oldPassword))
            {
                jm.msg = "旧密码输入错误";
                return new JsonResult(jm);
            }

            userModel.passWord = CommonHelper.Md5For32(entity.password);
            var bl = await _sysUserServices.UpdateAsync(userModel);

            jm.code = bl ? 0 : 1;
            jm.msg = bl ? "修改成功" : "修改失败";
            return new JsonResult(jm);
        }

        #endregion

        #region 用户编辑个人非安全隐私数据====================================================

        /// <summary>
        ///     用户编辑个人非安全隐私数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> EditLoginUserInfo([FromBody] EditLoginUserInfo entity)
        {
            var jm = new AdminUiCallBack();

            var userModel = await _sysUserServices.QueryByIdAsync(_user.ID);

            if (!string.IsNullOrEmpty(entity.nickName)) userModel.nickName = entity.nickName;
            if (!string.IsNullOrEmpty(entity.avatar)) userModel.avatar = entity.avatar;
            if (entity.sex > 0) userModel.sex = entity.sex;
            if (!string.IsNullOrEmpty(entity.phone)) userModel.phone = entity.phone;
            if (!string.IsNullOrEmpty(entity.email)) userModel.email = entity.email;
            if (!string.IsNullOrEmpty(entity.introduction)) userModel.introduction = entity.introduction;

            if (!string.IsNullOrEmpty(entity.trueName)) userModel.trueName = entity.trueName;
            if (!string.IsNullOrEmpty(entity.idCard)) userModel.idCard = entity.idCard;
            if (entity.birthday != null) userModel.birthday = entity.birthday;

            userModel.updateTime = DateTime.Now;
            var bl = await _sysUserServices.UpdateAsync(userModel);

            jm.code = bl ? 0 : 1;
            jm.msg = bl ? "修改成功" : "修改失败";
            return new JsonResult(jm);
        }

        #endregion

        #region 反射获取后端Api的controller和action

        /// <summary>
        ///     反射获取后端Api的controller和action
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetAllControllerAndActionByAssembly()
        {
            var jm = new AdminUiCallBack();
            var data = AdminsControllerPermission.GetAllControllerAndActionByAssembly();
            jm.data = data.OrderBy(u => u.name).ToList();
            jm.code = 0;
            jm.msg = "获取成功";
            return new JsonResult(jm);
        }

        #endregion

        //通用操作=========================================================================

        #region 通用上传接口====================================================

        /// <summary>
        ///     通用上传接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> UploadFiles()
        {
            var jm = new AdminUiCallBack();

            var _filesStorageOptions = await _coreCmsSettingServices.GetFilesStorageOptions();

            //初始化上传参数
            var maxSize = 1024 * 1024 * _filesStorageOptions.MaxSize; //上传大小5M

            var file = Request.Form.Files["file"];
            if (file == null)
            {
                jm.msg = "请选择文件";
                return new JsonResult(jm);
            }

            var fileName = file.FileName;
            var fileExt = Path.GetExtension(fileName).ToLowerInvariant();

            //检查大小
            if (file.Length > maxSize)
            {
                jm.msg = "上传文件大小超过限制，最大允许上传" + _filesStorageOptions.MaxSize + "M";
                return new JsonResult(jm);
            }

            //检查文件扩展名
            if (string.IsNullOrEmpty(fileExt) ||
                Array.IndexOf(_filesStorageOptions.FileTypes.Split(','), fileExt.Substring(1).ToLower()) == -1)
            {
                jm.msg = "上传文件扩展名是不允许的扩展名,请上传后缀名为：" + _filesStorageOptions.FileTypes;
                return new JsonResult(jm);
            }

            var newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + fileExt;
            var today = DateTime.Now.ToString("yyyyMMdd");

            if (_filesStorageOptions.StorageType == GlobalEnumVars.FilesStorageOptionsType.LocalStorage.ToString())
            {
                var saveUrl = "/Upload/" + today + "/";
                var dirPath = _webHostEnvironment.WebRootPath + saveUrl;
                if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
                var filePath = dirPath + newFileName;
                var fileUrl = saveUrl + newFileName;

                string bucketBindDomain = AppSettingsConstVars.AppConfigAppUrl;

                await using (var fs = System.IO.File.Create(filePath))
                {
                    await file.CopyToAsync(fs);
                    fs.Flush();
                }

                jm.code = 0;
                jm.msg = "上传成功!";
                jm.data = new
                {
                    fileUrl,
                    src = bucketBindDomain + fileUrl
                };
            }
            else if (_filesStorageOptions.StorageType == GlobalEnumVars.FilesStorageOptionsType.AliYunOSS.ToString())
            {

                //上传到阿里云
                await using (var fileStream = file.OpenReadStream()) //转成Stream流
                {
                    var md5 = OssUtils.ComputeContentMd5(fileStream, file.Length);

                    var filePath = "Upload/" + today + "/" + newFileName; //云文件保存路径
                                                                          //初始化阿里云配置--外网Endpoint、访问ID、访问password
                    var aliyun = new OssClient(_filesStorageOptions.Endpoint, _filesStorageOptions.AccessKeyId, _filesStorageOptions.AccessKeySecret);
                    //将文件md5值赋值给meat头信息，服务器验证文件MD5
                    var objectMeta = new ObjectMetadata
                    {
                        ContentMd5 = md5
                    };
                    //文件上传--空间名、文件保存路径、文件流、meta头信息(文件md5) //返回meta头信息(文件md5)
                    aliyun.PutObject(_filesStorageOptions.BucketName, filePath, fileStream, objectMeta);
                    //返回给UEditor的插入编辑器的图片的src
                    jm.code = 0;
                    jm.msg = "上传成功";
                    jm.data = new
                    {
                        fileUrl = _filesStorageOptions.BucketBindUrl + filePath,
                        src = _filesStorageOptions.BucketBindUrl + filePath
                    };
                }
            }
            else if (_filesStorageOptions.StorageType == GlobalEnumVars.FilesStorageOptionsType.QCloudOSS.ToString())
            {
                var filePath = "Upload/" + today + "/" + newFileName; //云文件保存路径

                //上传到腾讯云OSS
                //初始化 CosXmlConfig
                string appid = _filesStorageOptions.AccountId;//设置腾讯云账户的账户标识 APPID
                string region = _filesStorageOptions.CosRegion; //设置一个默认的存储桶地域
                CosXmlConfig config = new CosXmlConfig.Builder()
                    .SetAppid(appid)
                    .IsHttps(true)  //设置默认 HTTPS 请求
                    .SetRegion(region)  //设置一个默认的存储桶地域
                    .SetDebugLog(true)  //显示日志
                    .Build();  //创建 CosXmlConfig 对象

                long durationSecond = 600;  //每次请求签名有效时长，单位为秒
                QCloudCredentialProvider qCloudCredentialProvider = new DefaultQCloudCredentialProvider(_filesStorageOptions.AccessKeyId, _filesStorageOptions.AccessKeySecret, durationSecond);


                byte[] bytes;
                await using (var ms = new MemoryStream())
                {
                    await file.CopyToAsync(ms);
                    bytes = ms.ToArray();
                }

                var cosXml = new CosXmlServer(config, qCloudCredentialProvider);
                COSXML.Model.Object.PutObjectRequest putObjectRequest = new COSXML.Model.Object.PutObjectRequest(_filesStorageOptions.BucketName, filePath, bytes);
                cosXml.PutObject(putObjectRequest);

                jm.code = 0;
                jm.msg = "上传成功";
                jm.data = new
                {
                    fileUrl = _filesStorageOptions.BucketBindUrl + filePath,
                    src = _filesStorageOptions.BucketBindUrl + filePath
                };

            }
            return new JsonResult(jm);
        }

        #endregion

        #region 裁剪Base64上传====================================================

        /// <summary>
        ///     裁剪Base64上传
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> UploadFilesFByBase64([FromBody] FMBase64Post entity)
        {
            var jm = new AdminUiCallBack();

            var _filesStorageOptions = await _coreCmsSettingServices.GetFilesStorageOptions();



            if (string.IsNullOrEmpty(entity.base64))
            {
                jm.msg = "请上传合法内容";
                return new JsonResult(jm);
            }

            entity.base64 = entity.base64.Replace("data:image/png;base64,", "").Replace("data:image/jgp;base64,", "").Replace("data:image/jpg;base64,", "").Replace("data:image/jpeg;base64,", "");//将base64头部信息替换
            byte[] bytes = Convert.FromBase64String(entity.base64);
            MemoryStream memStream = new MemoryStream(bytes);

            Image mImage = Image.FromStream(memStream);
            Bitmap bp = new Bitmap(mImage);


            var newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + ".jpg";
            var today = DateTime.Now.ToString("yyyyMMdd");


            if (_filesStorageOptions.StorageType == GlobalEnumVars.FilesStorageOptionsType.LocalStorage.ToString())
            {
                var saveUrl = "/Upload/" + today + "/";
                var dirPath = _webHostEnvironment.WebRootPath + saveUrl;
                string bucketBindDomain = AppSettingsConstVars.AppConfigAppUrl;

                if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
                var filePath = dirPath + newFileName;
                var fileUrl = saveUrl + newFileName;

                bp.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);//注意保存路径

                jm.code = 0;
                jm.msg = "上传成功!";
                jm.data = new
                {
                    fileUrl,
                    src = bucketBindDomain + fileUrl
                };
                jm.otherData = GlobalEnumVars.FilesStorageOptionsType.LocalStorage.ToString();

            }
            else if (_filesStorageOptions.StorageType == GlobalEnumVars.FilesStorageOptionsType.AliYunOSS.ToString())
            {
                //上传到阿里云

                // 设置当前流的位置为流的开始
                memStream.Seek(0, SeekOrigin.Begin);

                await using (var fileStream = memStream) //转成Stream流
                {
                    var md5 = OssUtils.ComputeContentMd5(fileStream, memStream.Length);

                    var filePath = "Upload/" + today + "/" + newFileName; //云文件保存路径
                                                                          //初始化阿里云配置--外网Endpoint、访问ID、访问password
                    var aliyun = new OssClient(_filesStorageOptions.Endpoint, _filesStorageOptions.AccessKeyId, _filesStorageOptions.AccessKeySecret);
                    //将文件md5值赋值给meat头信息，服务器验证文件MD5
                    var objectMeta = new ObjectMetadata
                    {
                        ContentMd5 = md5
                    };
                    //文件上传--空间名、文件保存路径、文件流、meta头信息(文件md5) //返回meta头信息(文件md5)
                    aliyun.PutObject(_filesStorageOptions.BucketName, filePath, fileStream, objectMeta);
                    //返回给UEditor的插入编辑器的图片的src
                    jm.code = 0;
                    jm.msg = "上传成功";
                    jm.data = new
                    {
                        fileUrl = _filesStorageOptions.BucketBindUrl + filePath,
                        src = _filesStorageOptions.BucketBindUrl + filePath
                    };
                }
                jm.otherData = GlobalEnumVars.FilesStorageOptionsType.AliYunOSS.ToString();

            }
            else if (_filesStorageOptions.StorageType == GlobalEnumVars.FilesStorageOptionsType.QCloudOSS.ToString())
            {
                //上传到腾讯云OSS
                //初始化 CosXmlConfig
                string appid = _filesStorageOptions.AccountId;//设置腾讯云账户的账户标识 APPID
                string region = _filesStorageOptions.CosRegion; //设置一个默认的存储桶地域
                CosXmlConfig config = new CosXmlConfig.Builder()
                    .SetAppid(appid)
                    .IsHttps(true)  //设置默认 HTTPS 请求
                    .SetRegion(region)  //设置一个默认的存储桶地域
                    .SetDebugLog(true)  //显示日志
                    .Build();  //创建 CosXmlConfig 对象

                long durationSecond = 600;  //每次请求签名有效时长，单位为秒
                QCloudCredentialProvider qCloudCredentialProvider = new DefaultQCloudCredentialProvider(
                    _filesStorageOptions.AccessKeyId, _filesStorageOptions.AccessKeySecret, durationSecond);


                var cosXml = new CosXmlServer(config, qCloudCredentialProvider);

                var filePath = "Upload/" + today + "/" + newFileName; //云文件保存路径
                COSXML.Model.Object.PutObjectRequest putObjectRequest = new COSXML.Model.Object.PutObjectRequest(_filesStorageOptions.BucketName, filePath, bytes);

                cosXml.PutObject(putObjectRequest);

                jm.code = 0;
                jm.msg = "上传成功";
                jm.data = new
                {
                    fileUrl = _filesStorageOptions.BucketBindUrl + filePath,
                    src = _filesStorageOptions.BucketBindUrl + filePath
                };
            }


            return new JsonResult(jm);
        }

        #endregion

        #region CKEditor编辑上传接口====================================================

        /// <summary>
        ///     CKEditor编辑上传接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CkEditorUploadFiles()
        {
            var jm = new CKEditorUploadedResult();

            var _filesStorageOptions = await _coreCmsSettingServices.GetFilesStorageOptions();

            //初始化上传参数
            var maxSize = 1024 * 1024 * _filesStorageOptions.MaxSize; //上传大小5M

            var file = Request.Form.Files["upload"];
            if (file == null)
            {
                jm.error.message = "请选择文件";
                return new JsonResult(jm);
            }

            var fileName = file.FileName;
            var fileExt = Path.GetExtension(fileName).ToLowerInvariant();

            //检查大小
            if (file.Length > maxSize)
            {
                jm.error.message = "上传文件大小超过限制，最大允许上传" + _filesStorageOptions.MaxSize + "M";
                return new JsonResult(jm);
            }

            //检查文件扩展名
            if (string.IsNullOrEmpty(fileExt) ||
                Array.IndexOf(_filesStorageOptions.FileTypes.Split(','), fileExt.Substring(1).ToLower()) == -1)
            {
                jm.error.message = "上传文件扩展名是不允许的扩展名,请上传后缀名为：" + _filesStorageOptions.FileTypes;
                return new JsonResult(jm);
            }

            var newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + fileExt;
            var today = DateTime.Now.ToString("yyyyMMdd");


            if (_filesStorageOptions.StorageType == GlobalEnumVars.FilesStorageOptionsType.LocalStorage.ToString())
            {
                var saveUrl = "/Upload/" + today + "/";
                var dirPath = _webHostEnvironment.WebRootPath + saveUrl;
                if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
                var filePath = dirPath + newFileName;
                var fileUrl = saveUrl + newFileName;

                string bucketBindDomain = AppSettingsConstVars.AppConfigAppUrl;

                await using (var fs = System.IO.File.Create(filePath))
                {
                    await file.CopyToAsync(fs);
                    fs.Flush();
                }

                jm.uploaded = 1;
                jm.fileName = fileName;
                jm.url = bucketBindDomain + fileUrl;
            }
            else if (_filesStorageOptions.StorageType == GlobalEnumVars.FilesStorageOptionsType.AliYunOSS.ToString())
            {
                //上传到阿里云
                await using (var fileStream = file.OpenReadStream()) //转成Stream流
                {
                    var md5 = OssUtils.ComputeContentMd5(fileStream, file.Length);

                    var filePath = "Upload/" + today + "/" + newFileName; //云文件保存路径
                                                                          //初始化阿里云配置--外网Endpoint、访问ID、访问password
                    var aliyun = new OssClient(_filesStorageOptions.Endpoint, _filesStorageOptions.AccessKeyId, _filesStorageOptions.AccessKeySecret);
                    //将文件md5值赋值给meat头信息，服务器验证文件MD5
                    var objectMeta = new ObjectMetadata
                    {
                        ContentMd5 = md5
                    };
                    //文件上传--空间名、文件保存路径、文件流、meta头信息(文件md5) //返回meta头信息(文件md5)
                    aliyun.PutObject(_filesStorageOptions.BucketName, filePath, fileStream, objectMeta);
                    //返回给UEditor的插入编辑器的图片的src

                    jm.uploaded = 1;
                    jm.fileName = fileName;
                    jm.url = _filesStorageOptions.BucketBindUrl + filePath;

                }
            }
            else if (_filesStorageOptions.StorageType == GlobalEnumVars.FilesStorageOptionsType.QCloudOSS.ToString())
            {
                var filePath = "Upload/" + today + "/" + newFileName; //云文件保存路径

                //上传到腾讯云OSS
                //初始化 CosXmlConfig
                string appid = _filesStorageOptions.AccountId;//设置腾讯云账户的账户标识 APPID
                string region = _filesStorageOptions.CosRegion; //设置一个默认的存储桶地域
                CosXmlConfig config = new CosXmlConfig.Builder()
                    .SetAppid(appid)
                    .IsHttps(true)  //设置默认 HTTPS 请求
                    .SetRegion(region)  //设置一个默认的存储桶地域
                    .SetDebugLog(true)  //显示日志
                    .Build();  //创建 CosXmlConfig 对象

                long durationSecond = 600;  //每次请求签名有效时长，单位为秒
                QCloudCredentialProvider qCloudCredentialProvider = new DefaultQCloudCredentialProvider(_filesStorageOptions.AccessKeyId, _filesStorageOptions.AccessKeySecret, durationSecond);


                byte[] bytes;
                await using (var ms = new MemoryStream())
                {
                    await file.CopyToAsync(ms);
                    bytes = ms.ToArray();
                }

                var cosXml = new CosXmlServer(config, qCloudCredentialProvider);
                COSXML.Model.Object.PutObjectRequest putObjectRequest = new COSXML.Model.Object.PutObjectRequest(_filesStorageOptions.BucketName, filePath, bytes);
                cosXml.PutObject(putObjectRequest);

                jm.uploaded = 1;
                jm.fileName = fileName;
                jm.url = _filesStorageOptions.BucketBindUrl + filePath;

            }

            return new JsonResult(jm);
        }

        #endregion

        #region 根据id获取商品信息====================================================

        // POST: Api/Tools/GetGoodsByIds
        /// <summary>
        ///     根据id获取商品信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("根据id获取商品信息")]
        public async Task<JsonResult> GetGoodsByIds([FromBody] FMArrayIntIds entity)
        {
            var jm = new AdminUiCallBack();

            var list = await _coreCmsGoodsServices.QueryByIDsAsync(entity.id);
            jm.code = 0;
            jm.data = list;

            return new JsonResult(jm);
        }

        #endregion

        #region 后台生成小程序码============================================================

        // POST: Api/CoreCmsForm/GetCreate
        /// <summary>
        ///     后台生成小程序码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("后台生成小程序码")]
        public async Task<JsonResult> GetFormWxCode([FromBody] FMIntId entity)
        {
            //返回数据
            var jm = new AdminUiCallBack { code = 0 };

            var _filesStorageOptions = await _coreCmsSettingServices.GetFilesStorageOptions();


            var formModel = _coreCmsFormServices.QueryById(entity.id);
            if (formModel == null)
            {
                jm.msg = "不存在此信息";
                return new JsonResult(jm);
            }

            var path = "pages/form/details/details?id=" + entity.id;

            using (var memStream = new MemoryStream())
            {
                var result = await WxAppApi.CreateWxQrCodeAsync(WxOpenAppId, memStream, path);

                memStream.Seek(0, SeekOrigin.Begin);

                var newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + ".jpg";
                var today = DateTime.Now.ToString("yyyyMMdd");

                if (_filesStorageOptions.StorageType == GlobalEnumVars.FilesStorageOptionsType.LocalStorage.ToString())
                {
                    var saveUrl = "/Upload/QrCode/" + today + "/";
                    var dirPath = _webHostEnvironment.WebRootPath + saveUrl;
                    string bucketBindDomain = AppSettingsConstVars.AppConfigAppUrl;

                    if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
                    var filePath = dirPath + newFileName;
                    var fileUrl = saveUrl + newFileName;

                    //储存图片
                    System.IO.File.Delete(filePath);
                    await using (var fs = new FileStream(filePath, FileMode.CreateNew))
                    {
                        await memStream.CopyToAsync(fs).ConfigureAwait(false);
                        await fs.FlushAsync().ConfigureAwait(false);
                    }

                    jm.code = 0;
                    jm.msg = "上传成功!";
                    jm.data = new
                    {
                        fileUrl,
                        src = bucketBindDomain + fileUrl
                    };
                }
                else if (_filesStorageOptions.StorageType == GlobalEnumVars.FilesStorageOptionsType.AliYunOSS.ToString())
                {
                    //上传到阿里云

                    // 设置当前流的位置为流的开始
                    memStream.Seek(0, SeekOrigin.Begin);

                    await using var fileStream = memStream;

                    var md5 = OssUtils.ComputeContentMd5(fileStream, memStream.Length);

                    var filePath = "Upload/QrCode/" + today + "/" + newFileName; //云文件保存路径
                    //初始化阿里云配置--外网Endpoint、访问ID、访问password
                    var aliyun = new OssClient(_filesStorageOptions.Endpoint, _filesStorageOptions.AccessKeyId, _filesStorageOptions.AccessKeySecret);
                    //将文件md5值赋值给meat头信息，服务器验证文件MD5
                    var objectMeta = new ObjectMetadata
                    {
                        ContentMd5 = md5
                    };
                    //文件上传--空间名、文件保存路径、文件流、meta头信息(文件md5) //返回meta头信息(文件md5)
                    aliyun.PutObject(_filesStorageOptions.BucketName, filePath, fileStream, objectMeta);
                    //返回给UEditor的插入编辑器的图片的src
                    jm.code = 0;
                    jm.msg = "上传成功";
                    jm.data = new
                    {
                        fileUrl = _filesStorageOptions.BucketBindUrl + filePath,
                        src = _filesStorageOptions.BucketBindUrl + filePath
                    };
                }
                else if (_filesStorageOptions.StorageType == GlobalEnumVars.FilesStorageOptionsType.QCloudOSS.ToString())
                {
                    //上传到腾讯云OSS
                    //初始化 CosXmlConfig
                    string appid = _filesStorageOptions.AccountId;//设置腾讯云账户的账户标识 APPID
                    string region = _filesStorageOptions.CosRegion; //设置一个默认的存储桶地域
                    CosXmlConfig config = new CosXmlConfig.Builder()
                        .SetAppid(appid)
                        .IsHttps(true)  //设置默认 HTTPS 请求
                        .SetRegion(region)  //设置一个默认的存储桶地域
                        .SetDebugLog(true)  //显示日志
                        .Build();  //创建 CosXmlConfig 对象

                    long durationSecond = 600;  //每次请求签名有效时长，单位为秒
                    QCloudCredentialProvider qCloudCredentialProvider = new DefaultQCloudCredentialProvider(
                        _filesStorageOptions.AccessKeyId, _filesStorageOptions.AccessKeySecret, durationSecond);


                    var cosXml = new CosXmlServer(config, qCloudCredentialProvider);

                    byte[] bytes = memStream.ToArray();

                    var filePath = "Upload/QrCode/" + today + "/" + newFileName; //云文件保存路径
                    COSXML.Model.Object.PutObjectRequest putObjectRequest = new COSXML.Model.Object.PutObjectRequest(_filesStorageOptions.BucketName, filePath, bytes);

                    cosXml.PutObject(putObjectRequest);

                    jm.code = 0;
                    jm.msg = "上传成功";
                    jm.data = new
                    {
                        fileUrl = _filesStorageOptions.BucketBindUrl + filePath,
                        src = _filesStorageOptions.BucketBindUrl + filePath
                    };
                }

                jm.otherData = result;
            }

            return new JsonResult(jm);
        }

        #endregion

        //通用页面获取=========================================================================

        #region 获取商品列表====================================================

        // POST: Api/Tools/GetGoods
        /// <summary>
        ///     获取商品列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("获取商品列表")]
        public async Task<JsonResult> GetGoods()
        {
            var jm = new AdminUiCallBack();
            var pageCurrent = Request.Form["page"].FirstOrDefault().ObjectToInt(1);
            var pageSize = Request.Form["limit"].FirstOrDefault().ObjectToInt(30);
            var where = PredicateBuilder.True<CoreCmsGoods>();
            where = where.And(p => p.isMarketable);
            //商品编码 nvarchar
            var bn = Request.Form["bn"].FirstOrDefault();
            if (!string.IsNullOrEmpty(bn)) where = where.And(p => p.bn.Contains(bn));
            //商品名称 nvarchar
            var name = Request.Form["name"].FirstOrDefault();
            if (!string.IsNullOrEmpty(name)) where = where.And(p => p.name.Contains(name));
            where = where.And(p => p.isDel == false);
            //获取数据
            var list = await _coreCmsGoodsServices.QueryPageAsync(where, p => p.createTime, OrderByType.Desc,
                pageCurrent, pageSize);
            //返回数据

            var newObj = list.Select(p => new
            {
                p.id,
                image = !string.IsNullOrEmpty(p.images) ? p.images.Split(",")[0] : "/static/images/common/empty.png",
                p.images,
                p.price,
                p.name,
                p.stock
            }).ToList();


            jm.data = newObj;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return new JsonResult(jm);
        }

        #endregion

        #region 根据商品序列获取货品数据====================================================

        // POST: Api/Tools/GetProducts
        /// <summary>
        ///     根据商品序列获取货品数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("获取商品列表")]
        public async Task<JsonResult> GetProducts(FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var list = await _productsServices.GetProducts(entity.id);

            jm.code = 0;
            jm.data = list;
            jm.msg = "数据调用成功!";
            return new JsonResult(jm);
        }

        #endregion


        #region 获取文章列表============================================================

        // POST: Api/Tools/GetArticles
        /// <summary>
        ///     获取文章列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("获取文章列表")]
        public async Task<JsonResult> GetArticles()
        {
            var jm = new AdminUiCallBack();
            var pageCurrent = Request.Form["page"].FirstOrDefault().ObjectToInt(1);
            var pageSize = Request.Form["limit"].FirstOrDefault().ObjectToInt(30);
            var where = PredicateBuilder.True<CoreCmsArticle>();

            //标题 nvarchar
            var title = Request.Form["title"].FirstOrDefault();
            if (!string.IsNullOrEmpty(title)) where = where.And(p => p.title.Contains(title));

            //获取数据
            var list = await _coreCmsArticleServices.QueryPageAsync(where, p => p.id, OrderByType.Asc, pageCurrent,
                pageSize);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return new JsonResult(jm);
        }

        #endregion

        #region 获取智能表单列表============================================================

        // POST: Api/Tools/GetForms
        /// <summary>
        ///     获取智能表单列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("获取智能表单列表")]
        public async Task<JsonResult> GetForms()
        {
            var jm = new AdminUiCallBack();
            var pageCurrent = Request.Form["page"].FirstOrDefault().ObjectToInt(1);
            var pageSize = Request.Form["limit"].FirstOrDefault().ObjectToInt(30);
            var where = PredicateBuilder.True<CoreCmsForm>();

            //表单名称 nvarchar
            var name = Request.Form["name"].FirstOrDefault();
            if (!string.IsNullOrEmpty(name)) where = where.And(p => p.name.Contains(name));

            //获取数据
            var list = await _coreCmsFormServices.QueryPageAsync(where, p => p.id, OrderByType.Desc, pageCurrent,
                pageSize);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return new JsonResult(jm);
        }

        #endregion

        #region 获取文章分类列表============================================================

        // POST: Api/Tools/GetArticleTypes
        /// <summary>
        ///     获取文章分类列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("获取文章分类列表")]
        public async Task<JsonResult> GetArticleTypes()
        {
            var jm = new AdminUiCallBack();

            //获取数据
            var list = await _coreCmsArticleTypeServices.QueryAsync();
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.msg = "数据调用成功!";
            return new JsonResult(jm);
        }

        #endregion

        #region 获取公告列表============================================================

        // POST: Api/Tools/GetNotices
        /// <summary>
        ///     获取公告列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("获取公告列表")]
        public async Task<JsonResult> GetNotices()
        {
            var jm = new AdminUiCallBack();
            var pageCurrent = Request.Form["page"].FirstOrDefault().ObjectToInt(1);
            var pageSize = Request.Form["limit"].FirstOrDefault().ObjectToInt(30);
            var where = PredicateBuilder.True<CoreCmsNotice>();

            //公告标题 nvarchar
            var title = Request.Form["title"].FirstOrDefault();
            if (!string.IsNullOrEmpty(title)) where = where.And(p => p.title.Contains(title));
            //获取数据
            var list = await _coreCmsNoticeServices.QueryPageAsync(where, p => p.id, OrderByType.Desc, pageCurrent,
                pageSize);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return new JsonResult(jm);
        }

        #endregion

        #region 供tag标签选择拼团商品的时候使用============================================================

        // POST: Api/Tools/GetPingTuans
        /// <summary>
        ///     供tag标签选择拼团商品的时候使用
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("获取列表")]
        public async Task<JsonResult> TagPinTuan()
        {
            var jm = new AdminUiCallBack();
            var pageCurrent = Request.Form["page"].FirstOrDefault().ObjectToInt(1);
            var pageSize = Request.Form["limit"].FirstOrDefault().ObjectToInt(30);
            var where = PredicateBuilder.True<TagPinTuanResult>();

            var dt = DateTime.Now;
            where = where.And(p => p.isStatusOpen == true && p.startTime < dt && p.endTime > dt);


            //活动名称 nvarchar
            var name = Request.Form["name"].FirstOrDefault();
            if (!string.IsNullOrEmpty(name)) where = where.And(p => p.name.Contains(name));

            //获取数据
            var list = await _coreCmsPinTuanRuleServices.QueryTagPinTuanPageAsync(where, p => p.id, OrderByType.Desc, pageCurrent, pageSize);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return new JsonResult(jm);
        }

        #endregion

        #region 供tag标签选择合法团购秒杀的时候使用============================================================

        // POST: Api/Tools/TagPromotions
        /// <summary>
        ///     供tag标签选择合法团购秒杀的时候使用
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("供tag标签选择合法团购秒杀的时候使用")]
        public async Task<JsonResult> TagPromotions()
        {
            var jm = new AdminUiCallBack();
            var pageCurrent = Request.Form["page"].FirstOrDefault().ObjectToInt(1);
            var pageSize = Request.Form["limit"].FirstOrDefault().ObjectToInt(30);
            var where = PredicateBuilder.True<CoreCmsPromotion>();

            var dt = DateTime.Now;
            where = where.And(p => p.isEnable == true && p.isDel == false && p.startTime < dt && p.endTime > dt && p.type == (int)GlobalEnumVars.PromotionType.Seckill);

            //促销名称 nvarchar
            var name = Request.Form["name"].FirstOrDefault();
            if (!string.IsNullOrEmpty(name)) where = where.And(p => p.name.Contains(name));

            where = where.And(p => p.isDel == false);
            //获取数据
            var list = await _coreCmsPromotionServices.QueryPageAsync(where, p => p.id, OrderByType.Desc, pageCurrent,
                pageSize);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return new JsonResult(jm);
        }

        #endregion

        #region 供tag标签选择服务卡的时候使用============================================================

        // POST: Api/Tools/TagServices
        /// <summary>
        ///     供tag标签选择服务卡的时候使用
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("获取列表")]
        public async Task<JsonResult> TagServices()
        {
            var jm = new AdminUiCallBack();
            var pageCurrent = Request.Form["page"].FirstOrDefault().ObjectToInt(1);
            var pageSize = Request.Form["limit"].FirstOrDefault().ObjectToInt(30);

            var dt = DateTime.Now;
            var where = PredicateBuilder.True<CoreCmsServices>();

            where = where.And(p => p.status == (int)GlobalEnumVars.ServicesStatus.Shelve);
            where = where.And(p => p.amount > 0);
            where = where.And(p => p.startTime < dt && p.endTime > dt);

            //服务名称 nvarchar
            var title = Request.Form["title"].FirstOrDefault();
            if (!string.IsNullOrEmpty(title)) where = where.And(p => p.title.Contains(title));

            //获取数据
            var list = await _servicesServices.TagQueryPageAsync(where, p => p.createTime, OrderByType.Desc, pageCurrent, pageSize);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return new JsonResult(jm);
        }

        #endregion

        #region 获取促销列表============================================================

        // POST: Api/Tools/GetPromotions
        /// <summary>
        ///     获取促销列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("获取促销列表")]
        public async Task<JsonResult> GetPromotions()
        {
            var jm = new AdminUiCallBack();
            var pageCurrent = Request.Form["page"].FirstOrDefault().ObjectToInt(1);
            var pageSize = Request.Form["limit"].FirstOrDefault().ObjectToInt(30);
            var where = PredicateBuilder.True<CoreCmsPromotion>();

            //序列 int
            var types = Request.Form["types"].FirstOrDefault().ObjectToInt(0);
            if (types == 1)
                where = where.And(p => p.type == 1);
            else if (types == 2)
                where = where.And(p => p.type == 2);
            else if (types == 3) where = where.And(p => p.type == 3 || p.type == 4);
            //促销名称 nvarchar
            var name = Request.Form["name"].FirstOrDefault();
            if (!string.IsNullOrEmpty(name)) where = where.And(p => p.name.Contains(name));

            where = where.And(p => p.isDel == false);
            //获取数据
            var list = await _coreCmsPromotionServices.QueryPageAsync(where, p => p.id, OrderByType.Desc, pageCurrent,
                pageSize);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return new JsonResult(jm);
        }

        #endregion

        #region 获取区域信息=======================================================

        /// <summary>
        ///     获取区域信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetArea()
        {
            var jm = new WebApiCallBack();

            var ischecked = Request.Form["ischecked"].FirstOrDefault().ObjectToInt(0);
            var nodeId = Request.Form["nodeId"].FirstOrDefault().ObjectToInt(0);
            var idsStr = Request.Form["ids"].FirstOrDefault();

            var ids = new List<PostAreasTreeNode>();
            if (!string.IsNullOrEmpty(idsStr)) ids = JsonConvert.DeserializeObject<List<PostAreasTreeNode>>(idsStr);
            var areaTrees = _areaServices.GetTreeArea(ids, nodeId, ischecked);

            jm.status = true;
            jm.data = areaTrees;
            jm.msg = ids.Count.ToString();

            return new JsonResult(jm);
        }

        #endregion

        #region 物流查询接口=======================================================

        /// <summary>
        ///     物流查询接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<JsonResult> LogisticsByApi([FromBody] FMApiLogisticsByApiPost entity)
        {
            var jm = new WebApiCallBack();

            if (string.IsNullOrEmpty(entity.code) || string.IsNullOrEmpty(entity.no))
            {
                jm.code = 1;
                jm.msg = GlobalErrorCodeVars.Code13225;
                return new JsonResult(jm);
            }

            if (entity.code == "benditongcheng")
            {
                jm.code = 1;
                jm.msg = "本地同城配送不支持轨迹查询";
                return new JsonResult(jm);
            }

            jm = await _logisticsServices.ExpressPoll(entity.code, entity.no, entity.mobile);
            return new JsonResult(jm);

        }

        #endregion

        //用户相关=========================================================================

        #region 根据用户权限获取对应左侧菜单列表====================================================

        /// <summary>
        ///     根据用户权限获取对应左侧菜单列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetNavs()
        {
            var jm = new AdminUiCallBack();

            //先获取用户关联角色
            var roles = await _sysUserRoleServices.QueryListByClauseAsync(p => p.userId == _user.ID);
            if (roles.Any())
            {
                var roleIds = roles.Select(p => p.roleId).ToList();
                var sysRoleMenu = await _sysRoleMenuServices.QueryListByClauseAsync(p => roleIds.Contains(p.roleId));


                var menuIds = sysRoleMenu.Select(p => p.menuId).ToList();


                var where = PredicateBuilder.True<SysMenu>();
                where = where.And(p => p.deleted == false && p.hide == false && p.menuType == 0);
                where = where.And(p => menuIds.Contains(p.id));

                var navs = await _sysMenuServices.QueryListByClauseAsync(where, p => p.sortNumber, OrderByType.Asc);
                var menus = GetMenus(navs, 0);

                jm.data = menus;
            }

            jm.msg = "数据获取正常";
            jm.code = 0;

            return new JsonResult(jm);
        }

        /// <summary>
        ///     迭代方法
        /// </summary>
        /// <param name="oldNavs"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        private static List<AdminUiMenu> GetMenus(List<SysMenu> oldNavs, int parentId)
        {
            var childTree = new List<AdminUiMenu>();
            if (parentId == 0)
            {
                var topMenu = new AdminUiMenu { title = "主页", icon = "layui-icon-home", name = "HomePanel" };
                var list = new List<AdminUiMenu>
                {
                    new AdminUiMenu
                        {title = "控制台", jump = "/", name = "controllerPanel", list = new List<AdminUiMenu>()}
                };
                topMenu.list = list;
                childTree.Add(topMenu);
            }

            var model = oldNavs.Where(p => p.parentId == parentId).ToList();
            foreach (var item in model)
            {
                var menu = new AdminUiMenu
                {
                    name = item.identificationCode,
                    title = item.menuName,
                    icon = item.menuIcon,
                    jump = !string.IsNullOrEmpty(item.path) ? item.path : null
                };
                childTree.Add(menu);
                menu.list = GetMenus(oldNavs, item.id);
            }

            return childTree;
        }

        #endregion

        #region 后台Select三级下拉联动配合

        /// <summary>
        ///     获取大类列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetAreaCheckedList([FromBody] FMIntId entity)
        {
            var res = new List<AreasDtoForAdminEdit>();

            if (entity.id != 0)
            {
                var model3 = new AreasDtoForAdminEdit();
                model3.info = await _areaServices.QueryByIdAsync(entity.id);
                if (model3.info != null && model3.info.parentId != 0)
                {
                    model3.list = await _areaServices.QueryListByClauseAsync(p => p.parentId == model3.info.parentId);

                    var model2 = new AreasDtoForAdminEdit();
                    model2.info = await _areaServices.QueryByIdAsync(model3.info.parentId);
                    if (model2.info != null && model2.info.parentId != 0)
                    {
                        model2.list =
                            await _areaServices.QueryListByClauseAsync(p => p.parentId == model2.info.parentId);

                        var model = new AreasDtoForAdminEdit();
                        model.info = await _areaServices.QueryByIdAsync(model2.info.parentId);
                        if (model.info != null)
                            model.list =
                                await _areaServices.QueryListByClauseAsync(p => p.parentId == model.info.parentId);
                        res.Add(model);
                    }

                    res.Add(model2);
                }

                res.Add(model3);
            }
            else
            {
                var model4 = new AreasDtoForAdminEdit();
                model4.list = await _areaServices.QueryListByClauseAsync(p => p.parentId == 0);
                model4.info = model4.list.FirstOrDefault();
                res.Add(model4);
            }

            return new JsonResult(res);
        }

        /// <summary>
        ///     取地区的下级列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetAreaChildren([FromBody] FMIntId entity)
        {
            var list = await _areaServices.QueryListByClauseAsync(p => p.parentId == entity.id);
            return new JsonResult(list);
        }

        #endregion

        //后端首页使用数据

        #region 获取最近登录日志============================================================

        // POST: Api/Tools/GetSysLoginRecord
        /// <summary>
        ///     获取最近登录日志
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("获取最近登录日志")]
        public async Task<JsonResult> GetSysLoginRecord()
        {
            var jm = new AdminUiCallBack();

            //获取数据
            var list = await _sysLoginRecordServices.QueryPageAsync(p => p.id > 0, p => p.createTime, OrderByType.Desc, 1, 10);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return new JsonResult(jm);
        }

        #endregion

        #region 获取全局Nlog日志============================================================

        // POST: Api/Tools/GetSysNLogRecords
        /// <summary>
        ///     获取全局Nlog日志
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("获取全局Nlog日志")]
        public async Task<JsonResult> GetSysNLogRecords()
        {
            var jm = new AdminUiCallBack();
            //获取数据
            var list = await _sysNLogRecordsServices.QueryPageAsync(p => p.id > 0, p => p.id, OrderByType.Desc, 1, 10);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return new JsonResult(jm);
        }

        #endregion

        #region 获取7天订单情况数据统计============================================================

        // POST: Api/Tools/GetOrdersStatistics
        /// <summary>
        ///     获取7天订单情况数据统计
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("获取7天订单情况数据统计")]
        public async Task<JsonResult> GetOrdersStatistics()
        {
            var jm = new AdminUiCallBack();


            var data = new List<string>() { "已支付", "已发货" };
            var legend = new Legend();
            legend.data = data;

            var paymentsStatistics = await _paymentsServices.Statistics();
            var billDeliveryStatistics = await _billDeliveryServices.Statistics();


            var xAxis = new List<XAxis>();
            var xItem = new XAxis();
            xItem.type = "category";
            xItem.data = paymentsStatistics.Select(p => p.day).ToList();

            for (int i = 0; i < xItem.data.Count; i++)
            {
                xItem.data[i] = Convert.ToDateTime(xItem.data[i]).ToString("d日");
            }
            xAxis.Add(xItem);

            var series = new List<SeriesDataIntItem>
            {
                new SeriesDataIntItem()
                {
                    name = "已支付", type = "line", data = paymentsStatistics.Select(p => p.nums).ToList()
                },
                new SeriesDataIntItem()
                {
                    name = "已发货", type = "line", data = billDeliveryStatistics.Select(p => p.nums).ToList()
                }
            };

            jm.code = 0;
            jm.data = new
            {
                legend,
                xAxis,
                series
            };

            //返回数据
            jm.msg = "数据调用成功!";
            return new JsonResult(jm);
        }

        #endregion

        #region 获取用户最新统计数据============================================================

        // POST: Api/Tools/GetUsersStatistics
        /// <summary>
        ///     获取用户最新统计数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("获取用户最新统计数据")]
        public async Task<JsonResult> GetUsersStatistics()
        {
            var jm = new AdminUiCallBack();


            var data = new List<string>() { "新增记录", "活跃记录" };
            var legend = new Legend();
            legend.data = data;

            var regs = await _userServices.Statistics(7);
            var orders = await _userServices.StatisticsOrder(7);


            var xAxis = new List<XAxis>();
            var xItem = new XAxis();
            xItem.type = "category";
            xItem.data = orders.Select(p => p.day).ToList();

            for (int i = 0; i < xItem.data.Count; i++)
            {
                xItem.data[i] = Convert.ToDateTime(xItem.data[i]).ToString("d日");
            }
            xAxis.Add(xItem);



            var series = new List<SeriesDataIntItem>
            {
                new SeriesDataIntItem() {name = "新增记录", type = "line", data = regs.Select(p => p.nums).ToList()},
                new SeriesDataIntItem() {name = "活跃记录", type = "line", data = orders.Select(p => p.nums).ToList()}
            };



            jm.code = 0;
            jm.data = new
            {
                legend,
                xAxis,
                series
            };

            //返回数据
            jm.msg = "数据调用成功!";
            return new JsonResult(jm);
        }

        #endregion

        #region 获取代办事宜数据============================================================

        // POST: Api/Tools/GetBackLog
        /// <summary>
        ///     获取代办事宜数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("获取用户最新统计数据")]
        public async Task<JsonResult> GetBackLog()
        {
            var jm = new AdminUiCallBack();
            //待支付
            var unpaidCount = await _orderServices.GetCountAsync(p =>
                p.status == (int)GlobalEnumVars.OrderStatus.Normal &&
                p.payStatus == (int)GlobalEnumVars.OrderPayStatus.No &&
                p.shipStatus == (int)GlobalEnumVars.ShipStatus.Yes);

            //待发货
            var unshipCount = await _orderServices.GetCountAsync(p =>
                p.status == (int)GlobalEnumVars.OrderStatus.Normal &&
                p.payStatus == (int)GlobalEnumVars.OrderPayStatus.No &&
                (p.shipStatus == (int)GlobalEnumVars.ShipStatus.Yes || p.shipStatus == (int)GlobalEnumVars.ShipStatus.No));

            //待售后
            var aftersalesCount = await _aftersalesServices.GetCountAsync(p => p.status == (int)GlobalEnumVars.BillAftersalesStatus.WaitAudit);

            var allConfigs = await _settingServices.GetConfigDictionaries();
            var goodsStocksWarn = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.GoodsStocksWarn).ObjectToInt(10);

            //库存报警
            var goodsStaticsTotalWarn = await _productsServices.GoodsStaticsTotalWarn(goodsStocksWarn);

            //返回数据
            jm.code = 0;
            jm.msg = "数据调用成功!";
            jm.data = new
            {
                unpaidCount,
                unshipCount,
                aftersalesCount,
                goodsStaticsTotalWarn
            };



            return new JsonResult(jm);
        }

        #endregion

    }
}