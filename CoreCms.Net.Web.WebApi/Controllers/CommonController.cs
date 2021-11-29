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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Aliyun.OSS;
using Aliyun.OSS.Util;
using CoreCms.Net.Configuration;
using CoreCms.Net.IServices;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.FromBody;
using CoreCms.Net.Model.ViewModels.Options;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Utility.Extensions;
using CoreCms.Net.Utility.Helper;
using COSXML;
using COSXML.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SqlSugar;

namespace CoreCms.Net.Web.WebApi.Controllers
{
    /// <summary>
    /// 通用调用接口数据
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICoreCmsSettingServices _settingServices;
        private readonly ICoreCmsAreaServices _areaServices;
        private readonly ICoreCmsServiceDescriptionServices _serviceDescriptionServices;

        private readonly ICoreCmsSettingServices _coreCmsSettingServices;
        private readonly IToolsServices _toolsServices;


        /// <summary>
        /// 构造函数
        /// </summary>
        public CommonController(ICoreCmsSettingServices settingServices
            , ICoreCmsAreaServices areaServices
            , IWebHostEnvironment webHostEnvironment, ICoreCmsServiceDescriptionServices serviceDescriptionServices, ICoreCmsSettingServices coreCmsSettingServices, IToolsServices toolsServices)
        {
            _webHostEnvironment = webHostEnvironment;
            _serviceDescriptionServices = serviceDescriptionServices;
            _coreCmsSettingServices = coreCmsSettingServices;
            _toolsServices = toolsServices;
            _settingServices = settingServices;
            _areaServices = areaServices;

        }

        //公共接口====================================================================================================

        #region 接口测试反馈=============================================================
        /// <summary>
        /// 接口测试反馈
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public WebApiCallBack InterFaceTest()
        {
            var jm = new WebApiCallBack { status = true, msg = "接口访问正常", data = DateTime.Now };
            return jm;
        }

        #endregion

        #region 返回配置数据文件V2.0===============================================================
        /// <summary>
        /// 返回配置数据文件V2.0
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<WebApiCallBack> GetConfigV2()
        {
            var jm = new WebApiCallBack { status = true, msg = "接口访问正常", data = DateTime.Now };
            var allConfigs = await _settingServices.GetConfigDictionaries();

            var shopLogo = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.ShopLogo); //店铺logo
            var shopName = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.ShopName);  //店铺名称
            var shopBeiAn = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.ShopBeiAn);  //店铺备案
            var shopDesc = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.ShopDesc);  //店铺描述
            var showStoresSwitch = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.ShowStoresSwitch).ObjectToInt(2);   //显示门店列表
            var showStoreBalanceRechargeSwitch = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.ShowStoreBalanceRechargeSwitch).ObjectToInt(2); //显示充值功能

            var imageMax = 5; //前端上传图片最多几张
            var storeSwitch = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.StoreSwitch).ObjectToInt();    //开启门店自提状态
            var cateStyle = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.CateStyle).ObjectToInt();    //分类样式
            var cateType = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.CateType).ObjectToInt();  //H5分类类型
            var toCashMoneyLow = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.TocashMoneyLow);    //最低提现
            var toCashMoneyRate = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.TocashMoneyRate);  //服务费
            var pointSwitch = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.PointSwitch).ObjectToInt();    //是否开启积分功能
            var statistics = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.StatisticsCode);   //获取统计代码
            var recommendKeysStr = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.RecommendKeys);
            var recommendKeys = !string.IsNullOrEmpty(recommendKeysStr) ? recommendKeysStr.Split("|") : new string[] { };    //搜索推荐关键字
            var invoiceSwitch = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.InvoiceSwitch).ObjectToInt();    //发票功能开关
            var goodsStocksWarn = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.GoodsStocksWarn).ObjectToInt();  //库存报警数量
            var shopDefaultImage = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.ShopDefaultImage);   //获取默认图片
            var shopMobile = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.ShopMobile);  //店铺联系电话
            var openDistribution = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.OpenDistribution).ObjectToInt();   //是否开启分销
            var distributionNotes = string.Empty;
            var distributionAgreement = string.Empty;
            var distributionStore = 2;
            if (openDistribution == 1)
            {
                distributionNotes = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.DistributionNotes);    //用户须知
                distributionAgreement = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.DistributionAgreement);    //分销协议
                distributionStore = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.DistributionStore).ObjectToInt(2);    //是否开启店铺
            }
            var showInviter = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.ShowInviterInfo).ObjectToInt();    //是否显示邀请人信息
            var shareTitle = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.ShareTitle);  //分享标题
            var shareDesc = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.ShareDesc);    //分享描述
            var shareImage = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.ShareImage); //分享图片
            var aboutArticleId = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.AboutArticleId).ObjectToInt(2);    //关于我们文章
            var entId = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.EntId);    //客服ID
            var userAgreementId = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.UserAgreementId).ObjectToInt(3);  //用户协议
            var privacyPolicyId = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.PrivacyPolicyId).ObjectToInt(4); //隐私政策

            var reshipName = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.ReshipName); //退货联系人
            var reshipMobile = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.ReshipMobile);  //退货联系方式
            var reshipAreaId = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.ReshipAreaId);  //退货区域
            var reshipAddress = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.ReshipAddress);  //退货联系方式
            var reshipCoordinate = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.ReshipCoordinate);  //退货坐标

            var orderCancelTime = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.OrderCancelTime).ObjectToInt(60); //订单取消时间

            //代理
            var isOpenAgent = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.IsOpenAgent).ObjectToInt();    //是否开启代理模块
            var isShowAgentPortal = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.IsShowAgentPortal).ObjectToInt();    //是否显示代理模块入口

            var agentNotes = string.Empty;
            var agentAgreement = string.Empty;
            if (isOpenAgent == 1 && isShowAgentPortal == 1)
            {
                agentNotes = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.AgentNotes);    //用户须知
                agentAgreement = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.AgentAgreement);    //分销协议
            }


            var model = new
            {
                shopLogo,
                shopName,
                shopBeiAn = shopBeiAn,
                shopDesc,
                imageMax,
                storeSwitch,
                showStoresSwitch,
                showStoreBalanceRechargeSwitch,
                cateStyle,
                cateType,
                toCashMoneyLow,
                toCashMoneyRate,
                pointSwitch,
                statistics,
                recommendKeys,
                invoiceSwitch,
                goodsStocksWarn,
                shopDefaultImage,
                shopMobile,
                openDistribution,
                distributionNotes,
                distributionAgreement,
                distributionStore,
                showInviter,
                shareTitle,
                shareDesc,
                shareImage,
                aboutArticleId,
                entId,
                userAgreementId,
                privacyPolicyId,
                reshipName,
                reshipMobile,
                reshipAreaId,
                reshipAddress,
                reshipCoordinate,
                orderCancelTime,
                isOpenAgent,
                isShowAgentPortal,
                agentNotes,
                agentAgreement
            };
            jm.data = model;
            return jm;
        }
        #endregion

        #region 获取区域配置=============================================================================

        /// <summary>
        /// 获取层级分配后的区域信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<WebApiCallBack> GetAreas()
        {
            var jm = new WebApiCallBack();

            var areas = await _areaServices.GetCaChe();
            jm.status = true;
            jm.data = AreaHelper.GetList(areas);

            return jm;
        }

        #endregion

        #region 获取商城关键词说明列表

        /// <summary>
        /// 获取商城关键词说明列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<WebApiCallBack> GetServiceDescription()
        {
            var jm = new WebApiCallBack();

            var caCheList = await _serviceDescriptionServices.GetCaChe();
            jm.status = true;
            jm.data = new
            {
                commonQuestion = caCheList.Where(p => p.type == (int)GlobalEnumVars.ShopServiceNoteType.CommonQuestion && p.isShow == true).OrderBy(p => p.sortId).ToList(),
                service = caCheList.Where(p => p.type == (int)GlobalEnumVars.ShopServiceNoteType.Service && p.isShow == true).OrderBy(p => p.sortId).ToList(),
                delivery = caCheList.Where(p => p.type == (int)GlobalEnumVars.ShopServiceNoteType.Delivery && p.isShow == true).OrderBy(p => p.sortId).ToList()
            };
            return jm;
        }

        #endregion

        //验证接口====================================================================================================

        #region 上传附件通用接口====================================================

        /// <summary>
        /// 上传附件通用接口
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<WebApiCallBack> UploadImages()
        {
            var jm = new WebApiCallBack();

            var filesStorageOptions = await _coreCmsSettingServices.GetFilesStorageOptions();

            //初始化上传参数
            var maxSize = 1024 * 1024 * filesStorageOptions.MaxSize; //上传大小5M

            var file = Request.Form.Files["file"];
            if (file == null)
            {
                jm.msg = "请选择文件";
                return jm;
            }
            string fileName = file.FileName;
            string fileExt = Path.GetExtension(fileName).ToLowerInvariant();

            //检查大小
            if (file.Length > maxSize)
            {
                jm.msg = "上传文件大小超过限制，最大允许上传" + filesStorageOptions.MaxSize + "M";
                return jm;
            }

            //检查文件扩展名
            if (string.IsNullOrEmpty(fileExt) ||
                Array.IndexOf(filesStorageOptions.FileTypes.Split(','), fileExt.Substring(1).ToLower()) == -1)
            {
                jm.msg = "上传文件扩展名是不允许的扩展名,请上传后缀名为：" + filesStorageOptions.FileTypes;
                return jm;
            }

            string url = string.Empty;
            if (filesStorageOptions.StorageType == GlobalEnumVars.FilesStorageOptionsType.LocalStorage.ToString())
            {
                url = await _toolsServices.UpLoadFileForLocalStorage(filesStorageOptions, fileExt, file, (int)GlobalEnumVars.FilesStorageLocation.API);
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
            jm.status = bl;
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? "上传成功!" : "上传失败";
            jm.data = new
            {
                fileUrl = url,
                src = url,
                imageId = url
            };

            return jm;
        }
        #endregion
    }
}