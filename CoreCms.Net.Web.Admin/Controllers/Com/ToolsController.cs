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
using CoreCms.Net.Caching.AccressToken;
using CoreCms.Net.Configuration;
using CoreCms.Net.Filter;
using CoreCms.Net.IServices;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.Entities.Expression;
using CoreCms.Net.Model.FromBody;
using CoreCms.Net.Model.ViewModels.Echarts;
using CoreCms.Net.Model.ViewModels.Options;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Model.ViewModels.DTO;
using CoreCms.Net.Utility.Extensions;
using CoreCms.Net.Utility.Helper;
using CoreCms.Net.WeChat.Service.HttpClients;
using COSXML;
using COSXML.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SKIT.FlurlHttpClient.Wechat.Api;
using SKIT.FlurlHttpClient.Wechat.Api.Models;
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
        private readonly ICoreCmsPagesServices _pagesServices;
        private readonly IToolsServices _toolsServices;
        private readonly ICoreCmsReportsServices _reportsServices;



        private readonly WeChat.Service.HttpClients.IWeChatApiHttpClientFactory _weChatApiHttpClientFactory;



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
            ICoreCmsLogisticsServices logisticsServices, ISysLoginRecordServices sysLoginRecordServices, ISysNLogRecordsServices sysNLogRecordsServices, ICoreCmsBillPaymentsServices paymentsServices, ICoreCmsBillDeliveryServices billDeliveryServices, ICoreCmsUserServices userServices, ICoreCmsOrderServices orderServices, ICoreCmsBillAftersalesServices aftersalesServices, ICoreCmsSettingServices settingServices, ICoreCmsProductsServices productsServices, ICoreCmsServicesServices servicesServices, IOptions<FilesStorageOptions> filesStorageOptions, ISysRoleMenuServices sysRoleMenuServices, IWeChatApiHttpClientFactory weChatApiHttpClientFactory, ICoreCmsPagesServices pagesServices, IToolsServices toolsServices, ICoreCmsReportsServices reportsServices)
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
            _weChatApiHttpClientFactory = weChatApiHttpClientFactory;
            _pagesServices = pagesServices;
            _toolsServices = toolsServices;
            _reportsServices = reportsServices;
        }

        #region 获取登录用户用户信息(用于面板展示)====================================================

        /// <summary>
        ///     获取登录用户用户信息(用于面板展示)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<AdminUiCallBack> GetUserInfo()
        {
            var jm = new AdminUiCallBack();
            var userModel = await _sysUserServices.QueryByIdAsync(_user.ID);
            jm.code = userModel == null ? 401 : 0;
            jm.msg = userModel == null ? "注销登录" : "数据获取正常";
            jm.data = userModel == null ? null : new
            {
                userModel.userName,
                userModel.nickName,
                userModel.createTime
            };
            return jm;
        }

        #endregion

        #region 获取登录用户用户全部信息(用于编辑个人信息)====================================================

        /// <summary>
        ///     获取登录用户用户全部信息(用于编辑个人信息)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<AdminUiCallBack> GetEditUserInfo()
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
            return jm;
        }

        #endregion

        #region 获取角色列表信息====================================================

        /// <summary>
        ///     获取角色列表信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<AdminUiCallBack> GetManagerRoles()
        {
            var jm = new AdminUiCallBack();
            var roles = await _sysRoleServices.QueryAsync();
            jm.code = 0;
            jm.msg = "数据获取正常";
            jm.data = roles.Select(p => new { title = p.roleName, value = p.id });
            return jm;
        }

        #endregion

        #region 用户编辑个人登录账户密码====================================================

        /// <summary>
        ///     获取登录用户用户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<AdminUiCallBack> EditLoginUserPassWord([FromBody] FMEditLoginUserPassWord entity)
        {
            var jm = new AdminUiCallBack();

            if (string.IsNullOrEmpty(entity.oldPassword))
            {
                jm.msg = "请键入旧密码";
                return jm;
            }

            if (string.IsNullOrEmpty(entity.password))
            {
                jm.msg = "请键入新密码";
                return jm;
            }

            if (string.IsNullOrEmpty(entity.repassword))
            {
                jm.msg = "请键入新密码确认密码";
                return jm;
            }

            if (entity.password != entity.repassword)
            {
                jm.msg = "新密码与确认密码不相符";
                return jm;
            }

            if (entity.password == entity.oldPassword)
            {
                jm.msg = "请设置与旧密码不同的新密码";
                return jm;
            }

            var oldPassWord = CommonHelper.Md5For32(entity.oldPassword);
            var newPassWord = CommonHelper.Md5For32(entity.password);

            var userModel = await _sysUserServices.QueryByIdAsync(_user.ID);
            if (userModel.passWord.ToUpperInvariant() != oldPassWord)
            {
                jm.msg = "旧密码输入错误";
                return jm;
            }
            else if (userModel.passWord.ToUpperInvariant() == newPassWord)
            {
                jm.msg = "新旧密码一致，无需修改，请设置与旧密码不同的新密码";
                return jm;
            }

            userModel.passWord = newPassWord;

            var bl = await _sysUserServices.UpdateAsync(userModel);

            jm.code = bl ? 0 : 1;
            jm.msg = bl ? "修改成功" : "修改失败";
            return jm;
        }

        #endregion

        #region 用户编辑个人非安全隐私数据====================================================

        /// <summary>
        ///     用户编辑个人非安全隐私数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<AdminUiCallBack> EditLoginUserInfo([FromBody] EditLoginUserInfo entity)
        {
            var jm = new AdminUiCallBack();

            if (entity.trueName.Length > 4)
            {
                jm.msg = "用户真实姓名不能大于4个字符。";
                return jm;
            }



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
            return jm;
        }

        #endregion

        #region 反射获取后端Api的controller和action

        /// <summary>
        ///     反射获取后端Api的controller和action
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public AdminUiCallBack GetAllControllerAndActionByAssembly()
        {
            var jm = new AdminUiCallBack();
            var data = AdminsControllerPermission.GetAllControllerAndActionByAssembly();
            jm.data = data.OrderBy(u => u.name).ToList();
            jm.code = 0;
            jm.msg = "获取成功";
            return jm;
        }

        #endregion

        //通用操作=========================================================================

        #region 通用上传接口====================================================

        /// <summary>
        ///     通用上传接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<AdminUiCallBack> UploadFiles()
        {
            var jm = new AdminUiCallBack();

            var filesStorageOptions = await _coreCmsSettingServices.GetFilesStorageOptions();

            //初始化上传参数
            var maxSize = 1024 * 1024 * filesStorageOptions.MaxSize; //上传大小5M

            var file = Request.Form.Files["file"];
            if (file == null)
            {
                jm.msg = "请选择文件";
                return jm;
            }

            var fileName = file.FileName;
            var fileExt = Path.GetExtension(fileName).ToLowerInvariant();

            //检查大小
            if (file.Length > maxSize)
            {
                jm.msg = "上传文件大小超过限制，最大允许上传" + filesStorageOptions.MaxSize + "M";
                return jm;
            }

            //检查文件扩展名
            if (string.IsNullOrEmpty(fileExt) || Array.IndexOf(filesStorageOptions.FileTypes.Split(','), fileExt.Substring(1).ToLower()) == -1)
            {
                jm.msg = "上传文件扩展名是不允许的扩展名,请上传后缀名为：" + filesStorageOptions.FileTypes;
                return jm;
            }

            string url = string.Empty;
            if (filesStorageOptions.StorageType == GlobalEnumVars.FilesStorageOptionsType.LocalStorage.ToString())
            {
                url = await _toolsServices.UpLoadFileForLocalStorage(filesStorageOptions, fileExt, file);
            }
            else if (filesStorageOptions.StorageType == GlobalEnumVars.FilesStorageOptionsType.AliYunOSS.ToString())
            {
                url = await _toolsServices.UpLoadFileForAliYunOSS(filesStorageOptions, fileExt, file);
            }
            else if (filesStorageOptions.StorageType == GlobalEnumVars.FilesStorageOptionsType.QCloudOSS.ToString())
            {
                url = await _toolsServices.UpLoadFileForQCloudOSS(filesStorageOptions, fileExt, file);
            }
            else if (filesStorageOptions.StorageType == GlobalEnumVars.FilesStorageOptionsType.QiNiuKoDo.ToString())
            {
                url = await _toolsServices.UpLoadFileForQiNiuKoDo(filesStorageOptions, fileExt, file);
            }

            var bl = !string.IsNullOrEmpty(url);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? "上传成功!" : "上传失败";
            jm.data = new
            {
                fileUrl = url,
                src = url
            };

            return jm;
        }

        #endregion

        #region 裁剪Base64上传====================================================

        /// <summary>
        ///     裁剪Base64上传
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<AdminUiCallBack> UploadFilesFByBase64([FromBody] FMBase64Post entity)
        {
            var jm = new AdminUiCallBack();

            var filesStorageOptions = await _coreCmsSettingServices.GetFilesStorageOptions();

            if (string.IsNullOrEmpty(entity.base64))
            {
                jm.msg = "请上传合法内容";
                return jm;
            }

            //检查上传大小
            if (!CommonHelper.CheckBase64Size(entity.base64, filesStorageOptions.MaxSize))
            {
                jm.msg = "上传文件大小超过限制，最大允许上传" + filesStorageOptions.MaxSize + "M";
                return jm;
            }


            entity.base64 = entity.base64.Replace("data:image/png;base64,", "").Replace("data:image/jgp;base64,", "").Replace("data:image/jpg;base64,", "").Replace("data:image/jpeg;base64,", "");//将base64头部信息替换
            byte[] bytes = Convert.FromBase64String(entity.base64);
            MemoryStream memStream = new MemoryStream(bytes);

            string url = string.Empty;
            if (filesStorageOptions.StorageType == GlobalEnumVars.FilesStorageOptionsType.LocalStorage.ToString())
            {
                url = _toolsServices.UpLoadBase64ForLocalStorage(filesStorageOptions, memStream);
            }
            else if (filesStorageOptions.StorageType == GlobalEnumVars.FilesStorageOptionsType.AliYunOSS.ToString())
            {
                //上传到阿里云
                url = await _toolsServices.UpLoadBase64ForAliYunOSS(filesStorageOptions, memStream);

            }
            else if (filesStorageOptions.StorageType == GlobalEnumVars.FilesStorageOptionsType.QCloudOSS.ToString())
            {
                //上传到腾讯云OSS
                url = _toolsServices.UpLoadBase64ForQCloudOSS(filesStorageOptions, bytes);
            }
            else if (filesStorageOptions.StorageType == GlobalEnumVars.FilesStorageOptionsType.QiNiuKoDo.ToString())
            {
                //上传到七牛云kodo
                url = _toolsServices.UpLoadBase64ForQiNiuKoDo(filesStorageOptions, bytes);
            }
            var bl = !string.IsNullOrEmpty(url);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? "上传成功!" : "上传失败";
            jm.data = new
            {
                fileUrl = url,
                src = url
            };

            return jm;
        }

        #endregion

        #region CKEditor编辑上传接口====================================================

        /// <summary>
        ///     CKEditor编辑上传接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<CKEditorUploadedResult> CkEditorUploadFiles()
        {
            var jm = new CKEditorUploadedResult();

            var filesStorageOptions = await _coreCmsSettingServices.GetFilesStorageOptions();

            //初始化上传参数
            var maxSize = 1024 * 1024 * filesStorageOptions.MaxSize; //上传大小5M

            var file = Request.Form.Files["upload"];
            if (file == null)
            {
                jm.error.message = "请选择文件";
                return jm;
            }

            var fileName = file.FileName;
            var fileExt = Path.GetExtension(fileName).ToLowerInvariant();

            //检查大小
            if (file.Length > maxSize)
            {
                jm.error.message = "上传文件大小超过限制，最大允许上传" + filesStorageOptions.MaxSize + "M";
                return jm;
            }

            //检查文件扩展名
            if (string.IsNullOrEmpty(fileExt) || Array.IndexOf(filesStorageOptions.FileTypes.Split(','), fileExt.Substring(1).ToLower()) == -1)
            {
                jm.error.message = "上传文件扩展名是不允许的扩展名,请上传后缀名为：" + filesStorageOptions.FileTypes;
                return jm;
            }

            string url = string.Empty;
            if (filesStorageOptions.StorageType == GlobalEnumVars.FilesStorageOptionsType.LocalStorage.ToString())
            {
                url = await _toolsServices.UpLoadFileForLocalStorage(filesStorageOptions, fileExt, file);
            }
            else if (filesStorageOptions.StorageType == GlobalEnumVars.FilesStorageOptionsType.AliYunOSS.ToString())
            {
                //上传到阿里云
                url = await _toolsServices.UpLoadFileForAliYunOSS(filesStorageOptions, fileExt, file);
            }
            else if (filesStorageOptions.StorageType == GlobalEnumVars.FilesStorageOptionsType.QCloudOSS.ToString())
            {
                url = await _toolsServices.UpLoadFileForQCloudOSS(filesStorageOptions, fileExt, file);
            }
            else if (filesStorageOptions.StorageType == GlobalEnumVars.FilesStorageOptionsType.QiNiuKoDo.ToString())
            {
                url = await _toolsServices.UpLoadFileForQiNiuKoDo(filesStorageOptions, fileExt, file);
            }
            jm.uploaded = !string.IsNullOrEmpty(url) ? 1 : 0;
            jm.fileName = fileName;
            jm.url = url;
            return jm;
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
        public async Task<AdminUiCallBack> GetGoodsByIds([FromBody] FMArrayIntIds entity)
        {
            var jm = new AdminUiCallBack();

            var list = await _coreCmsGoodsServices.QueryByIDsAsync(entity.id);
            jm.code = 0;
            jm.data = list;

            return jm;
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
        public async Task<AdminUiCallBack> GetFormWxCode([FromBody] FMIntId entity)
        {
            //返回数据
            var jm = new AdminUiCallBack { code = 0 };

            var filesStorageOptions = await _coreCmsSettingServices.GetFilesStorageOptions();


            var formModel = await _coreCmsFormServices.QueryByIdAsync(entity.id);
            if (formModel == null)
            {
                jm.code = 1;
                jm.msg = "不存在此信息";
                return jm;
            }

            var path = "pages/form/details/details?id=" + entity.id;

            var accessToken = WeChatCacheAccessTokenHelper.GetWxOpenAccessToken();
            var client = _weChatApiHttpClientFactory.CreateWxOpenClient();
            var request = new CgibinWxaappCreateWxaQrcodeRequest();
            request.AccessToken = accessToken;
            request.Path = path;

            var response = await client.ExecuteCgibinWxaappCreateWxaQrcodeAsync(request);
            if (response.IsSuccessful())
            {
                var memStream = new MemoryStream(response.RawBytes);

                string url = string.Empty;
                if (filesStorageOptions.StorageType == GlobalEnumVars.FilesStorageOptionsType.LocalStorage.ToString())
                {
                    url = _toolsServices.UpLoadBase64ForLocalStorage(filesStorageOptions, memStream);

                }
                else if (filesStorageOptions.StorageType == GlobalEnumVars.FilesStorageOptionsType.AliYunOSS.ToString())
                {
                    //上传到阿里云
                    url = await _toolsServices.UpLoadBase64ForAliYunOSS(filesStorageOptions, memStream);
                }
                else if (filesStorageOptions.StorageType == GlobalEnumVars.FilesStorageOptionsType.QCloudOSS.ToString())
                {
                    //上传到腾讯云OSS
                    url = _toolsServices.UpLoadBase64ForQCloudOSS(filesStorageOptions, response.RawBytes);
                }
                else if (filesStorageOptions.StorageType == GlobalEnumVars.FilesStorageOptionsType.QiNiuKoDo.ToString())
                {
                    //上传到七牛云kodo
                    url = _toolsServices.UpLoadBase64ForQiNiuKoDo(filesStorageOptions, response.RawBytes);
                }

                var bl = !string.IsNullOrEmpty(url);
                jm.code = bl ? 0 : 1;
                jm.msg = bl ? "上传成功!" : "上传失败";
                jm.data = new
                {
                    fileUrl = url,
                    src = url
                };

            }
            else
            {
                jm.code = 1;
                jm.msg = response.ErrorMessage;
            }
            jm.otherData = response;


            return jm;
        }

        #endregion

        #region 后台生成预览页面设计小程序码============================================================

        // POST: Api/Tools/GetPageWxCode
        /// <summary>
        ///     后台生成预览页面设计小程序码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("后台生成小程序码")]
        public async Task<AdminUiCallBack> GetPageWxCode([FromBody] FMStringId entity)
        {
            //返回数据
            var jm = new AdminUiCallBack { code = 0 };

            var filesStorageOptions = await _coreCmsSettingServices.GetFilesStorageOptions();


            var pageModel = await _pagesServices.QueryByClauseAsync(p => p.code == entity.id);
            if (pageModel == null)
            {
                jm.code = 1;
                jm.msg = "不存在此信息";
                return jm;
            }

            var path = "pages/index/custom/custom?pageCode=" + entity.id;

            var accessToken = WeChatCacheAccessTokenHelper.GetWxOpenAccessToken();
            var client = _weChatApiHttpClientFactory.CreateWxOpenClient();
            var request = new CgibinWxaappCreateWxaQrcodeRequest();
            request.AccessToken = accessToken;
            request.Path = path;

            var response = await client.ExecuteCgibinWxaappCreateWxaQrcodeAsync(request);
            if (response.IsSuccessful())
            {
                var memStream = new MemoryStream(response.RawBytes);

                string url = string.Empty;
                if (filesStorageOptions.StorageType == GlobalEnumVars.FilesStorageOptionsType.LocalStorage.ToString())
                {
                    url = _toolsServices.UpLoadBase64ForLocalStorage(filesStorageOptions, memStream);

                }
                else if (filesStorageOptions.StorageType == GlobalEnumVars.FilesStorageOptionsType.AliYunOSS.ToString())
                {
                    //上传到阿里云
                    url = await _toolsServices.UpLoadBase64ForAliYunOSS(filesStorageOptions, memStream);
                }
                else if (filesStorageOptions.StorageType == GlobalEnumVars.FilesStorageOptionsType.QCloudOSS.ToString())
                {
                    //上传到腾讯云OSS
                    url = _toolsServices.UpLoadBase64ForQCloudOSS(filesStorageOptions, response.RawBytes);
                }
                else if (filesStorageOptions.StorageType == GlobalEnumVars.FilesStorageOptionsType.QiNiuKoDo.ToString())
                {
                    //上传到七牛云kodo
                    url = _toolsServices.UpLoadBase64ForQiNiuKoDo(filesStorageOptions, response.RawBytes);
                }

                var bl = !string.IsNullOrEmpty(url);
                jm.code = bl ? 0 : 1;
                jm.msg = bl ? "上传成功!" : "上传失败";
                jm.data = new
                {
                    fileUrl = url,
                    src = url
                };

            }
            else
            {
                jm.code = 1;
                jm.msg = response.ErrorMessage;
            }
            jm.otherData = response;


            return jm;
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
        public async Task<AdminUiCallBack> GetGoods()
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
            return jm;
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
        public async Task<AdminUiCallBack> GetProducts(FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var list = await _productsServices.GetProducts(entity.id);

            jm.code = 0;
            jm.data = list;
            jm.msg = "数据调用成功!";
            return jm;
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
        public async Task<AdminUiCallBack> GetArticles()
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
            return jm;
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
        public async Task<AdminUiCallBack> GetForms()
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
            return jm;
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
        public async Task<AdminUiCallBack> GetArticleTypes()
        {
            var jm = new AdminUiCallBack();

            //获取数据
            var list = await _coreCmsArticleTypeServices.QueryAsync();
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.msg = "数据调用成功!";
            return jm;
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
        public async Task<AdminUiCallBack> GetNotices()
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
            return jm;
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
        public async Task<AdminUiCallBack> TagPinTuan()
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
            return jm;
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
        public async Task<AdminUiCallBack> TagPromotions()
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
            return jm;
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
        public async Task<AdminUiCallBack> TagServices()
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
            return jm;
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
        public async Task<AdminUiCallBack> GetPromotions()
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
            return jm;
        }

        #endregion

        #region 获取区域信息=======================================================

        /// <summary>
        ///     获取区域信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<WebApiCallBack> GetArea()
        {
            var jm = new WebApiCallBack();

            var ischecked = Request.Form["ischecked"].FirstOrDefault().ObjectToInt(0);
            var nodeId = Request.Form["nodeId"].FirstOrDefault().ObjectToInt(0);
            var idsStr = Request.Form["ids"].FirstOrDefault();

            var ids = new List<PostAreasTreeNode>();
            if (!string.IsNullOrEmpty(idsStr)) ids = JsonConvert.DeserializeObject<List<PostAreasTreeNode>>(idsStr);
            var areaTrees = await _areaServices.GetTreeArea(ids, nodeId, ischecked);

            jm.status = true;
            jm.data = areaTrees;
            jm.msg = ids.Count.ToString();

            return jm;
        }

        #endregion

        #region 物流查询接口=======================================================

        /// <summary>
        ///     物流查询接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<WebApiCallBack> LogisticsByApi([FromBody] FMApiLogisticsByApiPost entity)
        {
            var jm = new WebApiCallBack();

            if (string.IsNullOrEmpty(entity.code) || string.IsNullOrEmpty(entity.no))
            {
                jm.code = 1;
                jm.msg = GlobalErrorCodeVars.Code13225;
                return jm;
            }

            var systemLogistics = SystemSettingDictionary.GetSystemLogistics();
            foreach (var p in systemLogistics)
            {
                if (entity.code == p.sKey)
                {
                    jm.msg = p.sDescription + "不支持轨迹查询";
                    return jm;
                }
            }

            jm = await _logisticsServices.ExpressPoll(entity.code, entity.no, entity.mobile);
            return jm;

        }

        #endregion

        //用户相关=========================================================================

        #region 根据用户权限获取对应左侧菜单列表====================================================

        /// <summary>
        ///     根据用户权限获取对应左侧菜单列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<AdminUiCallBack> GetNavs()
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

            return jm;
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
        public async Task<List<AreasDtoForAdminEdit>> GetAreaCheckedList([FromBody] FMIntId entity)
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

            return res;
        }

        /// <summary>
        ///     取地区的下级列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<CoreCmsArea>> GetAreaChildren([FromBody] FMIntId entity)
        {
            var list = await _areaServices.QueryListByClauseAsync(p => p.parentId == entity.id);
            return list;
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
        public async Task<AdminUiCallBack> GetSysLoginRecord()
        {
            var jm = new AdminUiCallBack();

            //获取数据
            var list = await _sysLoginRecordServices.QueryPageAsync(p => p.id > 0, p => p.createTime, OrderByType.Desc, 1, 10);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return jm;
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
        public async Task<AdminUiCallBack> GetSysNLogRecords()
        {
            var jm = new AdminUiCallBack();
            //获取数据
            var list = await _sysNLogRecordsServices.QueryPageAsync(p => p.id > 0, p => p.id, OrderByType.Desc, 1, 10);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return jm;
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
        public Task<AdminUiCallBack> GetOrdersStatistics()
        {
            var jm = new AdminUiCallBack();

            var dtEnd = DateTime.Now;
            var dtStart = dtEnd.AddDays(-7);
            var dtStr = dtStart.ToString("yyyy-MM-dd") + " 到 " + dtEnd.ToString("yyyy-MM-dd");

            var dataRes = ReportsHelper.GetDate(dtStr, 2);
            if (!dataRes.status)
            {
                jm.msg = dataRes.msg;
                return System.Threading.Tasks.Task.FromResult(jm);
            }

            var echartsOption = new EchartsOption();

            echartsOption.title.text = "最近7天订单统计";
            var legend = new List<string>() { "全部", "待付款", "已付款" };
            echartsOption.legend.data = legend;

            var getDate = dataRes.data as ReportsBackForGetDate;

            var xData = ReportsHelper.GetXdata(getDate);
            if (!xData.status)
            {
                jm.msg = dataRes.msg;
                return System.Threading.Tasks.Task.FromResult(jm);
            }
            echartsOption.xAxis.data = xData.data as List<string>;

            var whereSql = string.Empty;
            var data = new List<GetOrdersReportsDbSelectOut>();
            var data1 = new List<GetOrdersReportsDbSelectOut>();
            var data2 = new List<GetOrdersReportsDbSelectOut>();
            var data3 = new List<GetOrdersReportsDbSelectOut>();
            foreach (var item in legend)
            {
                switch (item)
                {
                    case "全部":
                        whereSql = string.Empty;
                        whereSql += " and createTime > '" + getDate.start.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                        whereSql += " and createTime < '" + getDate.end.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                        data = _reportsServices.GetOrderMark(getDate.num, whereSql, getDate.section, getDate.start, "createTime");
                        data1 = data;
                        break;
                    case "待付款":
                        whereSql = string.Empty;
                        whereSql += " and createTime > '" + getDate.start.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                        whereSql += " and createTime < '" + getDate.end.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                        whereSql += " and payStatus=1 ";
                        data = _reportsServices.GetOrderMark(getDate.num, whereSql, getDate.section, getDate.start, "createTime");
                        data2 = data;
                        break;
                    case "已付款":
                        whereSql = string.Empty;
                        whereSql += " and paymentTime > '" + getDate.start.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                        whereSql += " and paymentTime < '" + getDate.end.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                        whereSql += " and payStatus>1 ";
                        data = _reportsServices.GetOrderMark(getDate.num, whereSql, getDate.section, getDate.start, "paymentTime");
                        data3 = data;
                        break;
                }

                if (data != null && data.Any())
                {
                    var vals = data.Select(p => p.val).ToList();
                    echartsOption.series.Add(new SeriesItem()
                    {
                        name = item,
                        type = "line",
                        data = vals.ConvertAll<string>(x => x.ToString(CultureInfo.InvariantCulture))
                    });
                }
                else
                {
                    echartsOption.series.Add(new SeriesItem()
                    {
                        name = item,
                        type = "line",
                        data = new List<string>()
                    });
                }
            }
            //组装数据列表用于table里使用
            var tableData = new List<OrderTableItem>();
            for (int i = 0; i < getDate.num; i++)
            {
                var item = new OrderTableItem();
                if (echartsOption.xAxis.data != null) item.x = echartsOption.xAxis.data[i];
                item.order_all_val = data1[i].val.ToString(CultureInfo.InvariantCulture);
                item.order_all_num = data1[i].num;
                item.order_nopay_val = data2[i].val.ToString(CultureInfo.InvariantCulture);
                item.order_nopay_num = data2[i].num;
                item.order_payed_val = data3[i].val.ToString(CultureInfo.InvariantCulture);
                item.order_payed_num = data3[i].num;
                tableData.Add(item);
            }

            jm.code = 0;
            jm.data = new
            {
                option = echartsOption,
                table = tableData
            };

            return System.Threading.Tasks.Task.FromResult(jm);
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
        public async Task<AdminUiCallBack> GetUsersStatistics()
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
            return jm;
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
        public async Task<AdminUiCallBack> GetBackLog()
        {
            var jm = new AdminUiCallBack();
            //待支付
            var paymentWhere = _orderServices.GetReverseStatus((int)GlobalEnumVars.OrderAllStatusType.ALL_PENDING_PAYMENT);
            var unpaidCount = await _orderServices.GetCountAsync(paymentWhere);


            //待发货
            var deliveredWhere = _orderServices.GetReverseStatus((int)GlobalEnumVars.OrderAllStatusType.ALL_PENDING_DELIVERY);
            var unshipCount = await _orderServices.GetCountAsync(deliveredWhere);

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

            return jm;
        }

        #endregion

    }
}