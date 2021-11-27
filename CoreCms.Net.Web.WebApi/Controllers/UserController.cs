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
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using CoreCms.Net.Auth.HttpContextUser;
using CoreCms.Net.Auth.Policys;
using CoreCms.Net.Caching.AccressToken;
using CoreCms.Net.Configuration;
using CoreCms.Net.IServices;
using CoreCms.Net.Loging;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.Entities.Expression;
using CoreCms.Net.Model.FromBody;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Model.ViewModels.DTO;
using CoreCms.Net.Utility.Extensions;
using CoreCms.Net.Utility.Helper;
using CoreCms.Net.WeChat.Service.Enums;
using CoreCms.Net.WeChat.Service.HttpClients;
using CoreCms.Net.WeChat.Service.Models;
using CoreCms.Net.WeChat.Service.Options;
using CoreCms.Net.WeChat.Service.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Nito.AsyncEx;
using NLog;
using SKIT.FlurlHttpClient.Wechat.Api;
using SKIT.FlurlHttpClient.Wechat.Api.Models;
using SqlSugar;

namespace CoreCms.Net.Web.WebApi.Controllers
{
    /// <summary>
    /// 用户操作事件
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly ICoreCmsUserWeChatInfoServices _userWeChatInfoServices;
        private readonly ICoreCmsUserServices _userServices;
        private readonly PermissionRequirement _permissionRequirement;
        private readonly ICoreCmsSmsServices _smsServices;
        private readonly ICoreCmsUserGradeServices _userGradeServices;
        private readonly IHttpContextUser _user;
        private readonly ICoreCmsUserLogServices _userLogServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICoreCmsGoodsServices _goodsServices;
        private readonly ICoreCmsGoodsBrowsingServices _goodsBrowsingServices;
        private readonly ICoreCmsCartServices _cartServices;
        private readonly ICoreCmsGoodsCollectionServices _goodsCollectionServices;
        private readonly ICoreCmsUserShipServices _userShipServices;
        private readonly ICoreCmsAreaServices _areaServices;
        private readonly ICoreCmsBillPaymentsServices _billPaymentsServices;
        private readonly ICoreCmsGoodsCommentServices _goodsCommentServices;
        private readonly ICoreCmsUserBankCardServices _userBankCardServices;
        private readonly ICoreCmsUserTocashServices _userTocashServices;
        private readonly ICoreCmsUserBalanceServices _userBalanceServices;
        private readonly ICoreCmsInvoiceServices _invoiceServices;
        private readonly ICoreCmsUserPointLogServices _userPointLogServices;
        private readonly ICoreCmsShareServices _shareServices;
        private readonly ICoreCmsSettingServices _settingServices;
        private readonly ICoreCmsServicesServices _servicesServices;
        private readonly ICoreCmsUserServicesOrderServices _userServicesOrderServices;
        private readonly ICoreCmsUserServicesTicketServices _userServicesTicketServices;
        private readonly ICoreCmsStoreServices _storeServices;
        private readonly ICoreCmsCouponServices _couponServices;
        private readonly ICoreCmsOrderServices _orderServices;

        private readonly IWeChatApiHttpClientFactory _weChatApiHttpClientFactory;
        private readonly WeChatOptions _weChatOptions;

        private readonly AsyncLock _mutex = new AsyncLock();

        /// <summary>
        /// 构造函数
        /// </summary>
        public UserController(
            IHttpContextUser user
            , ICoreCmsUserWeChatInfoServices userWeChatInfoServices
            , ICoreCmsUserServices userServices
            , PermissionRequirement permissionRequirement
            , ICoreCmsSmsServices smsServices
            , ICoreCmsUserGradeServices userGradeServices
            , ICoreCmsUserLogServices userLogServices
            , IHttpContextAccessor httpContextAccessor
            , ICoreCmsGoodsServices goodsServices
            , ICoreCmsGoodsBrowsingServices goodsBrowsingServices
            , ICoreCmsCartServices cartServices
            , ICoreCmsGoodsCollectionServices goodsCollectionServices
            , ICoreCmsUserShipServices userShipServices
            , ICoreCmsAreaServices areaServices
            , ICoreCmsBillPaymentsServices billPaymentsServices
            , ICoreCmsGoodsCommentServices goodsCommentServices
            , ICoreCmsUserBankCardServices userBankCardServices
            , ICoreCmsUserTocashServices userTocashServices
            , ICoreCmsUserBalanceServices userBalanceServices
            , ICoreCmsInvoiceServices invoiceServices
            , ICoreCmsUserPointLogServices userPointLogServices
            , ICoreCmsShareServices shareServices
            , ICoreCmsSettingServices settingServices
            , ICoreCmsServicesServices servicesServices
            , IOptions<WeChatOptions> weChatOptions
            , ICoreCmsUserServicesOrderServices userServicesOrderServices, ICoreCmsUserServicesTicketServices userServicesTicketServices, ICoreCmsStoreServices storeServices, ICoreCmsCouponServices couponServices, ICoreCmsOrderServices orderServices, IWeChatApiHttpClientFactory weChatApiHttpClientFactory)
        {
            _user = user;
            _userWeChatInfoServices = userWeChatInfoServices;
            _userServices = userServices;
            _permissionRequirement = permissionRequirement;
            _smsServices = smsServices;
            _userGradeServices = userGradeServices;
            _userLogServices = userLogServices;
            _httpContextAccessor = httpContextAccessor;
            _goodsServices = goodsServices;
            _goodsBrowsingServices = goodsBrowsingServices;
            _cartServices = cartServices;
            _goodsCollectionServices = goodsCollectionServices;
            _userShipServices = userShipServices;
            _areaServices = areaServices;
            _billPaymentsServices = billPaymentsServices;
            _goodsCommentServices = goodsCommentServices;
            _userBankCardServices = userBankCardServices;
            _userTocashServices = userTocashServices;
            _userBalanceServices = userBalanceServices;
            _invoiceServices = invoiceServices;
            _userPointLogServices = userPointLogServices;
            _shareServices = shareServices;
            _settingServices = settingServices;
            _servicesServices = servicesServices;
            _userServicesOrderServices = userServicesOrderServices;
            _userServicesTicketServices = userServicesTicketServices;
            _storeServices = storeServices;
            _couponServices = couponServices;
            _orderServices = orderServices;
            _weChatApiHttpClientFactory = weChatApiHttpClientFactory;
            _weChatOptions = weChatOptions.Value;

        }

        #region wx.login登陆成功之后发送的请求=========================================================

        /// <summary>
        /// wx.login登陆成功之后发送的请求
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<WebApiCallBack> OnLogin([FromBody] FMWxPost entity)
        {
            var jm = new WebApiCallBack();

            var client = _weChatApiHttpClientFactory.CreateWxOpenClient();
            var accessToken = WeChatCacheAccessTokenHelper.GetWxOpenAccessToken();
            var request = new SnsJsCode2SessionRequest();
            request.JsCode = entity.code;
            request.AccessToken = accessToken;

            var response = await client.ExecuteSnsJsCode2SessionAsync(request, HttpContext.RequestAborted);
            if (response.ErrorCode == (int)WeChatReturnCode.ReturnCode.请求成功)
            {
                using (await _mutex.LockAsync())
                {
                    var userInfo = await _userWeChatInfoServices.QueryByClauseAsync(p => p.openid == response.OpenId);
                    if (userInfo == null)
                    {
                        userInfo = new CoreCmsUserWeChatInfo();
                        userInfo.openid = response.OpenId;
                        userInfo.type = (int)GlobalEnumVars.UserAccountTypes.微信小程序;
                        userInfo.sessionKey = response.SessionKey;
                        userInfo.gender = 1;
                        userInfo.createTime = DateTime.Now;

                        await _userWeChatInfoServices.InsertAsync(userInfo);
                    }
                    else
                    {
                        if (userInfo.sessionKey != response.SessionKey)
                        {
                            await _userWeChatInfoServices.UpdateAsync(
                                p => new CoreCmsUserWeChatInfo() { sessionKey = response.SessionKey, updateTime = DateTime.Now },
                                p => p.openid == userInfo.openid);
                        }
                    }

                    if (userInfo is { userId: > 0 })
                    {
                        var user = await _userServices.QueryByClauseAsync(p => p.id == userInfo.userId);
                        if (user != null)
                        {
                            var claims = new List<Claim> {
                                new Claim(ClaimTypes.Name, user.nickName),
                                new Claim(JwtRegisteredClaimNames.Jti, user.id.ToString()),
                                new Claim(ClaimTypes.Expiration, DateTime.Now.AddSeconds(_permissionRequirement.Expiration.TotalSeconds).ToString(CultureInfo.InvariantCulture)) };

                            //用户标识
                            var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);
                            identity.AddClaims(claims);
                            jm.status = true;
                            jm.data = new
                            {
                                auth = JwtToken.BuildJwtToken(claims.ToArray(), _permissionRequirement),
                                user
                            };
                            jm.otherData = response.OpenId;

                            //录入登录日志
                            var log = new CoreCmsUserLog();
                            log.userId = user.id;
                            log.state = (int)GlobalEnumVars.UserLogTypes.登录;
                            log.ip = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress != null ? _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString() : "127.0.0.1";
                            log.createTime = DateTime.Now;
                            log.parameters = GlobalEnumVars.UserLogTypes.登录.ToString();
                            await _userLogServices.InsertAsync(log);

                            return jm;
                        }
                    }
                }

                //注意：生产环境下SessionKey属于敏感信息，不能进行传输！
                //return new JsonResult(new { success = true, msg = "OK", sessionAuthId = sessionBag.Key, sessionKey = sessionBag.SessionKey, data = jsonResult, sessionBag = sessionBag });
                jm.status = true;
                jm.data = response.OpenId;
                jm.otherData = response.OpenId;
                //jm.methodDescription = JsonConvert.SerializeObject(sessionBag);
                jm.msg = "OK";
            }
            else
            {
                jm.msg = response.ErrorMessage;
            }

            return jm;
        }
        #endregion

        #region 微信核验数据并获取用户详细资料=====================================================
        /// <summary>
        /// 核验数据并获取用户详细资料
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<WebApiCallBack> DecodeEncryptedData([FromBody] FMWxLoginDecodeEncryptedData entity)
        {
            var jm = new WebApiCallBack();

            var userInfo = await _userWeChatInfoServices.QueryByClauseAsync(p => p.openid == entity.sessionAuthId);
            if (userInfo == null)
            {
                jm.status = false;
                jm.msg = "用户信息获取失败";
                return jm;
            }
            var decodedEntity = EncryptHelper.DecodeUserInfoBySessionId(userInfo.sessionKey, entity.encryptedData, entity.iv);
            var token = string.Empty;
            var userWxId = entity.sessionAuthId;
            //检验水印
            if (decodedEntity != null)
            {
                var checkWatermark = decodedEntity.CheckWatermark(_weChatOptions.WxOpenAppId);
                jm.status = checkWatermark;

                //保存用户信息（可选）
                if (checkWatermark && decodedEntity is { } decodedUserInfo)
                {
                    //更新数据库讯息
                    userInfo.gender = decodedUserInfo.gender;
                    userInfo.city = decodedUserInfo.city;
                    userInfo.avatar = decodedUserInfo.avatarUrl;
                    userInfo.country = decodedUserInfo.country;
                    userInfo.nickName = decodedUserInfo.nickName;
                    userInfo.province = decodedUserInfo.province;
                    userInfo.unionId = decodedUserInfo.unionId;
                    userInfo.updateTime = DateTime.Now;

                    await _userWeChatInfoServices.UpdateAsync(userInfo);

                    if (userInfo.userId > 0)
                    {
                        var user = await _userServices.QueryByClauseAsync(p => p.id == userInfo.userId);
                        if (user != null)
                        {
                            var claims = new List<Claim> {
                                new Claim(ClaimTypes.Name, user.nickName),
                                new Claim(JwtRegisteredClaimNames.Jti, user.id.ToString()),
                                new Claim(ClaimTypes.Expiration, DateTime.Now.AddSeconds(_permissionRequirement.Expiration.TotalSeconds).ToString(CultureInfo.InvariantCulture)) };

                            //用户标识
                            var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);
                            identity.AddClaims(claims);
                            jm.status = true;
                            jm.data = JwtToken.BuildJwtToken(claims.ToArray(), _permissionRequirement);

                            //录入登录日志
                            var log = new CoreCmsUserLog();
                            log.userId = user.id;
                            log.state = (int)GlobalEnumVars.UserLogTypes.登录;
                            log.ip = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress != null ? _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString() : "127.0.0.1";
                            log.createTime = DateTime.Now;
                            log.parameters = GlobalEnumVars.UserLogTypes.登录.ToString();
                            await _userLogServices.InsertAsync(log);

                            //更新手机号码标识
                            if (!string.IsNullOrEmpty(userInfo.mobile))
                            {
                                await _userWeChatInfoServices.UpdateAsync(p => new CoreCmsUserWeChatInfo() { mobile = user.mobile }, p => p.id == userInfo.id);
                            }

                            return jm;
                        }
                    }
                }
            }
            jm.data = new
            {
                token,
                sessionAuthId = userWxId
            };
            return jm;
        }
        #endregion

        #region 用户短信发送===================================================================
        /// <summary>
        /// 用户短信发送
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<WebApiCallBack> SendSms([FromBody] FMWxSendSMS entity)
        {
            var jm = new WebApiCallBack();
            if (!CommonHelper.IsMobile(entity.mobile))
            {
                jm.msg = "请输入合法的手机号码";
                return jm;
            }
            if (entity.code == "login")
            {
                var shave = await _userServices.ExistsAsync(p => p.mobile == entity.mobile && p.userWx > 0);
                if (shave)
                {
                    jm.msg = "手机号码已被绑定,请更换";
                    return jm;
                }
            }
            jm = await _smsServices.DoSendSms(entity.code, entity.mobile);
            return jm;
        }
        #endregion

        #region 手机短信验证码登陆，同时兼有手机短信注册的功能，还有第三方账户绑定的功能======================================================
        /// <summary>
        /// 手机短信验证码登陆，同时兼有手机短信注册的功能，还有第三方账户绑定的功能
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<WebApiCallBack> SmsLogin([FromBody] FMWxAccountCreate entity)
        {
            var jm = await _userServices.SmsLogin(entity, 2, entity.platform);
            return jm;
        }

        #endregion

        #region 微信小程序授权拉取手机号码

        /// <summary>
        /// 微信小程序授权拉取手机号码
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<WebApiCallBack> DecryptPhoneNumber([FromBody] FMWxLoginDecryptPhoneNumber entity)
        {
            var jm = new WebApiCallBack();

            var userInfo = await _userWeChatInfoServices.QueryByClauseAsync(p => p.openid == entity.sessionAuthId);
            if (userInfo == null)
            {
                jm.status = false;
                jm.msg = "用户信息获取失败";
                return jm;
            }
            DecodedPhoneNumber phoneNumber;
            try
            {
                phoneNumber = EncryptHelper.DecryptPhoneNumber(userInfo.sessionKey, entity.encryptedData, entity.iv);
            }
            catch (Exception ex)
            {
                jm.status = false;
                jm.code = 500;
                NLogUtil.WriteAll(LogLevel.Error, LogType.Web, "小程序接口", "微信小程序授权拉取手机号码", ex);
                return jm;
            }

            var data = new FMWxAccountCreate
            {
                mobile = phoneNumber.phoneNumber,
                invitecode = entity.invitecode,
                sessionAuthId = entity.sessionAuthId
            };

            jm = await _userServices.SmsLogin(data);

            return jm;
        }


        #endregion

        #region 用户短信注册并返回jwt token(弃用)======================================================
        /// <summary>
        /// 用户短信注册并返回jwt token(弃用)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Obsolete]
        [HttpPost]
        public async Task<WebApiCallBack> SmsLogin2([FromBody] FMWxAccountCreate entity)
        {
            var jm = new WebApiCallBack();
            if (!CommonHelper.IsMobile(entity.mobile))
            {
                jm.msg = "请输入合法的手机号码";
                return jm;
            }

            var user = await _userServices.QueryByClauseAsync(p => p.mobile == entity.mobile);
            if (user != null)
            {
                jm.msg = "此号码已经绑定,请更换";
                return jm;
            }
            var wxUserInfo = new CoreCmsUserWeChatInfo();
            //1就是h5登陆（h5端和微信公众号端），2就是微信小程序登陆，3是支付宝小程序，4是app，5是pc
            if (entity.platform == 2)
            {
                if (string.IsNullOrEmpty(entity.sessionAuthId))
                {
                    jm.msg = "用户未正确登陆";
                    return jm;
                }
                wxUserInfo = await _userWeChatInfoServices.QueryByClauseAsync(p => p.openid == entity.sessionAuthId);
            }
            var sms = await _smsServices.QueryByClauseAsync(p => p.parameters == entity.code && p.mobile == entity.mobile);
            if (sms == null)
            {
                jm.msg = "验证码核验失败";
                return jm;
            }
            if (sms.isUsed)
            {
                jm.msg = "验证码已被使用";
                return jm;
            }
            var dt = DateTime.Now;
            var endDt = sms.createTime.AddMinutes(10);
            if (dt > endDt)
            {
                jm.msg = "验证码已过期，请重新获取";
                return jm;
            }
            user = new CoreCmsUser();
            user.mobile = entity.mobile;
            user.sex = wxUserInfo?.gender ?? 3;
            user.avatarImage = wxUserInfo != null ? wxUserInfo.avatar : "";
            user.nickName = wxUserInfo != null ? wxUserInfo.nickName : entity.mobile;
            user.balance = 0;
            user.parentId = 0;
            user.point = 0;
            //获取用户等级
            var userGrade = await _userGradeServices.QueryByClauseAsync(p => p.isDefault);
            user.grade = userGrade?.id ?? 0;
            user.createTime = DateTime.Now;
            user.status = 1;
            user.userWx = wxUserInfo?.id ?? 0;
            user.isDelete = false;

            if (entity.invitecode > 0)
            {
                var parentId = UserHelper.GetUserIdByShareCode(entity.invitecode);
                if (parentId > 0 && await _userServices.ExistsAsync(p => p.id == parentId))
                {
                    user.parentId = parentId;
                }
            }

            var id = await _userServices.InsertAsync(user);
            if (id > 0)
            {
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, user.nickName),
                    new Claim(JwtRegisteredClaimNames.Jti, id.ToString()),
                    new Claim(ClaimTypes.Expiration, DateTime.Now.AddSeconds(_permissionRequirement.Expiration.TotalSeconds).ToString(CultureInfo.InvariantCulture)) };
                //用户标识
                var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);
                identity.AddClaims(claims);
                jm.status = true;
                jm.msg = "注册成功";
                jm.data = JwtToken.BuildJwtToken(claims.ToArray(), _permissionRequirement);
                //录入登录日志
                var log = new CoreCmsUserLog();
                log.userId = id;
                log.state = (int)GlobalEnumVars.UserLogTypes.注册;
                log.ip = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress != null ?
                    _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString() : "127.0.0.1";
                log.createTime = DateTime.Now;
                log.parameters = GlobalEnumVars.UserLogTypes.注册.ToString();
                await _userLogServices.InsertAsync(log);
                //标识短信是否可用
                sms.isUsed = true;
                await _smsServices.UpdateAsync(sms);
            }
            else
            {
                jm.msg = "注册失败";
            }

            return jm;
        }

        #endregion

        #region 获取区域ID
        /// <summary>
        /// 获取区域ID
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<WebApiCallBack> GetAreaId([FromBody] GetAreaIdPost entity)
        {
            var jm = await _areaServices.GetAreaId(entity.provinceName, entity.cityName, entity.countyName, entity.postalCode);

            return jm;
        }
        #endregion

        #region 注销登录
        /// <summary>
        /// 注销登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public WebApiCallBack LogOut()
        {
            var jm = new WebApiCallBack
            {
                status = true,
                data = new
                {
                    token = "", //直接前端删除token-无为而治
                }
            };

            return jm;
        }
        #endregion

        #region 判断是否开启积分
        /// <summary>
        /// 判断是否开启积分
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<WebApiCallBack> IsPoint()
        {
            var jm = new WebApiCallBack { status = true, msg = "获取成功" };

            var allConfigs = await _settingServices.GetConfigDictionaries();
            jm.data = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.PointSwitch).ObjectToInt(2);

            return jm;
        }
        #endregion

        #region 统一分享url处理

        /// <summary>
        /// 统一分享url处理
        /// 新的分享，不管是二维码，还是地址，都走这个
        /// page	场景值		1店铺首页，2商品详情页，3拼团详情页,4邀请好友（店铺页面,params里需要传store）,5文章页面,6参团页面，7自定义页面，8智能表单，9团购，10秒杀，11代理
        /// url：前端地址
        /// params：参数，根据场景值不一样而内容不一样
        /// 1
        /// 2 goodsId:商品ID
        /// 3 goodsId:商品ID，teamId:拼团ID
        /// 4 store:店铺code
        /// 5 articleId:文章ID，articleType:文章类型
        /// 6 goodsId:商品ID，groupId:参团ID，teamId:拼团ID
        /// 7 pageCode:自定义页面code
        /// 8 id：智能表单ID
        /// 9 goodsId:商品ID，groupId:团购秒杀ID
        ///     type	类型，1url，2二维码，3海报
        ///     token	可以保存推荐人的信息
        ///     client	终端，1普通h5，2微信小程序，3微信公众号（h5），4头条系小程序,5pc，6阿里小程序
        /// 10 store:店铺code
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>array</returns>
        [HttpPost]
        public async Task<WebApiCallBack> Share([FromBody] FMShare entity)
        {
            var jm = new WebApiCallBack();

            var userShareCode = 0;
            if (_user != null && _user.ID > 0)
            {
                userShareCode = UserHelper.GetShareCodeByUserId(_user.ID);
            }
            if (entity.type == (int)GlobalEnumVars.ShareType.Url) //链接分享
            {
                jm = _shareServices.UrlShare(entity.client, entity.page, userShareCode, entity.url, entity.@params);
            }
            else if (entity.type == (int)GlobalEnumVars.ShareType.QrCode)  //二维码
            {
                jm = await _shareServices.QrShare(entity.client, entity.page, userShareCode, entity.url, entity.@params);
            }
            else if (entity.type == (int)GlobalEnumVars.ShareType.Poster) //海报
            {
                jm = await _shareServices.PosterShare(entity.client, entity.page, userShareCode, entity.url, entity.@params);
            }

            return jm;

        }
        #endregion

        #region 统一分享解码
        /// <summary>
        /// 统一分享解码
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public WebApiCallBack DeShare([FromBody] FMDeShare entity)
        {
            var jm = new WebApiCallBack();

            if (string.IsNullOrEmpty(entity.code))
            {
                jm.msg = GlobalErrorCodeVars.Code10000;
            }
            jm = _shareServices.de_url(entity.code);

            return jm;
        }

        #endregion

        //验证接口====================================================================================================

        #region 同步微信用户数据=====================================================
        /// <summary>
        /// 同步微信用户数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<WebApiCallBack> SyncWeChatInfo([FromBody] FMWxSync entity)
        {
            var jm = new WebApiCallBack();

            var user = await _userServices.QueryByClauseAsync(p => p.id == _user.ID);
            if (user != null)
            {
                user.avatarImage = entity.avatarUrl;
                user.nickName = entity.nickName;
                user.sex = entity.gender;

                //更新
                await _userServices.UpdateAsync(p => new CoreCmsUser()
                {
                    avatarImage = entity.avatarUrl,
                    nickName = entity.nickName,
                    sex = entity.gender,
                }, p => p.id == user.id);
            }
            else
            {
                jm.msg = "用户信息获取失败";
            }

            if (user is { userWx: > 0 })
            {
                var userInfo = await _userWeChatInfoServices.QueryByClauseAsync(p => p.id == user.userWx);
                if (userInfo != null)
                {
                    userInfo.avatar = entity.avatarUrl;
                    userInfo.city = entity.city;
                    userInfo.country = entity.country;
                    userInfo.gender = entity.gender;
                    userInfo.nickName = entity.nickName;
                    userInfo.province = entity.province;
                    userInfo.updateTime = DateTime.Now;
                    await _userWeChatInfoServices.UpdateAsync(userInfo);
                }
            }

            jm.status = true;
            jm.data = user;
            return jm;
        }
        #endregion


        #region 获取用户信息
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> GetUserInfo()
        {
            var jm = new WebApiCallBack() { status = true };
            var user = await _userServices.QueryByIdAsync(_user.ID);
            if (user == null)
            {
                jm.status = false;
                jm.msg = "用户信息获取失败";
                jm.code = 14007;
                return jm;
            }
            //获取用户等级
            var userGrade = await _userGradeServices.QueryByClauseAsync(p => p.id == user.grade);
            //获取优惠券
            var userCouponCount = await _couponServices.GetMyCouponCount(user.id);
            //订单数量
            var orderCount = await _orderServices.OrderCount(0, user.id);
            //足迹
            var footPrintCount = await _goodsBrowsingServices.GetCountAsync(p => p.userId == user.id);
            //收藏
            var collectionCount = await _goodsCollectionServices.GetCountAsync(p => p.userId == user.id);


            jm.data = new
            {
                user.id,
                user.userName,
                user.mobile,
                user.sex,
                user.birthday,
                user.avatarImage,
                user.nickName,
                user.balance,
                user.point,
                user.grade,
                user.createTime,
                user.updataTime,
                user.status,
                user.parentId,
                user.passWord,
                gradeName = userGrade != null ? userGrade.title : "",
                userCouponCount,
                orderCount,
                footPrintCount,
                collectionCount
            };
            return jm;
        }
        #endregion

        #region 获取购物车商品数量
        /// <summary>
        /// 获取购物车商品数量
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> GetCartNumber()
        {
            var jm = new WebApiCallBack();

            var count = await _cartServices.GetCountAsync(_user.ID);
            jm.status = true;
            jm.msg = jm.status ? GlobalConstVars.GetDataSuccess : GlobalConstVars.GetDataFailure;
            jm.data = count;

            return jm;
        }
        #endregion

        #region 商品取消/添加收藏
        /// <summary>
        /// 商品取消/添加收藏
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> GoodsCollectionCreateOrDelete([FromBody] FMIntId entity)
        {
            var jm = new WebApiCallBack();

            var collection = await _goodsCollectionServices.QueryByClauseAsync(p => p.goodsId == entity.id && p.userId == _user.ID);
            if (collection == null)
            {
                var goods = await _goodsServices.QueryByIdAsync(entity.id);
                if (goods == null)
                {
                    jm.msg = GlobalErrorCodeVars.Code17001;
                    return jm;
                }

                collection = new CoreCmsGoodsCollection()
                {
                    goodsId = goods.id,
                    userId = _user.ID,
                    goodsName = goods.name,
                    createTime = DateTime.Now,
                };
                await _goodsCollectionServices.InsertAsync(collection);
                jm.msg = GlobalErrorCodeVars.Code17002;
            }
            else
            {
                await _goodsCollectionServices.DeleteAsync(collection);
                jm.msg = GlobalErrorCodeVars.Code17003;
            }
            jm.status = true;

            return jm;
        }


        #endregion

        #region 获取用户获取用户默认收货地址
        /// <summary>
        /// 获取用户获取用户默认收货地址
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> GetUserDefaultShip()
        {
            var jm = new WebApiCallBack();

            var ship = await _userShipServices.QueryByClauseAsync(p => p.isDefault && p.userId == _user.ID) ?? await _userShipServices.QueryByClauseAsync(p => p.userId == _user.ID, p => p.id, OrderByType.Desc);

            if (ship != null)
            {
                var fullName = await _areaServices.GetAreaFullName(ship.areaId);
                if (fullName.status)
                {
                    ship.areaName = fullName.data.ToString();
                }
            }

            jm.status = true;
            jm.data = ship;

            return jm;
        }
        #endregion

        #region 设置默认地址
        /// <summary>
        /// 设置默认地址
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> SetDefShip([FromBody] FMIntId entity)
        {
            var jm = new WebApiCallBack();

            var ship = await _userShipServices.QueryByClauseAsync(p => p.id == entity.id && p.userId == _user.ID);
            if (ship != null)
            {
                //没有默认的直接设置为默认
                ship.isDefault = true;
                var result = await _userShipServices.UpdateAsync(ship);
                jm.status = result.code == 0;
                jm.msg = jm.status ? "保存成功" : "保存失败";
            }
            else
            {
                jm.msg = "该地址不存在";
            }

            return jm;
        }

        #endregion

        #region 判断用户下单可以使用多少积分
        /// <summary>
        /// 判断用户下单可以使用多少积分
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> GetUserPoint([FromBody] GetUserPointPost entity)
        {
            var jm = new WebApiCallBack();

            var ship = await _userServices.GetUserPoint(_user.ID, entity.orderMoney);
            jm.status = true;
            jm.data = ship;

            return jm;
        }
        #endregion

        #region 获取用户的收货地址列表
        /// <summary>
        /// 获取用户的收货地址列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> GetUserShip()
        {
            var jm = new WebApiCallBack();

            var ship = await _userShipServices.QueryListByClauseAsync(p => p.userId == _user.ID, p => p.isDefault, OrderByType.Desc);
            if (ship.Any())
            {
                ship.ForEach(Action);
            }
            jm.status = true;
            jm.data = ship;

            return jm;
        }

        private async void Action(CoreCmsUserShip p)
        {
            var fullName = await _areaServices.GetAreaFullName(p.areaId);
            if (fullName.status)
            {
                p.areaName = fullName.data.ToString();
            }
        }

        #endregion

        #region 保存用户地址
        /// <summary>
        /// 保存用户地址
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> SaveUserShip([FromBody] SaveUserShipPost entity)
        {
            var jm = new WebApiCallBack();

            if (entity.id > 0)
            {
                //判断是否存在默认数据
                if (entity.isDefault != 1)
                {
                    if (await _userShipServices.ExistsAsync(p => p.userId == _user.ID && p.isDefault == true && p.id != entity.id) == false) entity.isDefault = 1;
                }
                var userShip = new CoreCmsUserShip();
                userShip.id = entity.id;
                userShip.userId = _user.ID;
                userShip.areaId = entity.areaId;
                userShip.isDefault = entity.isDefault == 1;
                userShip.name = entity.name;
                userShip.address = entity.address;
                userShip.mobile = entity.mobile;
                userShip.updateTime = DateTime.Now;
                var ship = await _userShipServices.UpdateAsync(userShip);
                jm.status = true;
                jm.data = ship;
                jm.msg = "地址保存成功";
            }
            else
            {
                //判断是否存在默认数据
                if (entity.isDefault != 1)
                {
                    if (await _userShipServices.ExistsAsync(p => p.userId == _user.ID && p.isDefault == true) == false) entity.isDefault = 1;
                }
                var userShip = new CoreCmsUserShip();
                userShip.userId = _user.ID;
                userShip.areaId = entity.areaId;
                userShip.isDefault = entity.isDefault == 1;
                userShip.name = entity.name;
                userShip.address = entity.address;
                userShip.mobile = entity.mobile;
                userShip.createTime = DateTime.Now;
                var ship = await _userShipServices.InsertAsync(userShip);
                jm.status = true;
                jm.data = ship;
                jm.msg = "地址保存成功";
            }

            return jm;
        }

        #endregion

        #region 获取用户单个地址详情
        /// <summary>
        /// 获取用户单个地址详情
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> GetShipDetail([FromBody] FMIntId entity)
        {
            var jm = new WebApiCallBack();

            var ship = await _userShipServices.QueryByClauseAsync(p => p.userId == _user.ID && p.id == entity.id);
            if (ship != null)
            {
                //var areas = _areaServices.FindListAsync();
                var fullName = await _areaServices.GetAreaFullName(ship.areaId);
                if (fullName.status)
                {
                    ship.areaName = fullName.data.ToString();
                }
            }
            jm.status = true;
            jm.data = ship;

            return jm;
        }
        #endregion

        #region 收货地址删除
        /// <summary>
        /// 收货地址删除
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> RemoveShip([FromBody] FMIntId entity)
        {
            var jm = new WebApiCallBack();

            jm.status = await _userShipServices.DeleteAsync(p => p.userId == _user.ID && p.id == entity.id);
            jm.msg = jm.status ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;

            if (jm.status)
            {
                //如果只有一个地址了，默认将最后一个剩余的地址设置为默认。
                var anySum = await _userShipServices.GetCountAsync(p => p.userId == _user.ID);
                if (anySum == 1)
                {
                    await _userShipServices.UpdateAsync(p => new CoreCmsUserShip() { isDefault = true }, p => p.userId == _user.ID);
                }
            }
            return jm;
        }
        #endregion

        #region 支付

        /// <summary>
        /// 支付
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> Pay([FromBody] PayPost entity)
        {
            var jm = new WebApiCallBack();

            if (string.IsNullOrEmpty(entity.ids))
            {
                jm.code = 13100;
                jm.msg = GlobalErrorCodeVars.Code13100;
            }
            else if (string.IsNullOrEmpty(entity.payment_code))
            {
                jm.code = 10055;
                jm.msg = GlobalErrorCodeVars.Code10055;
            }
            else if (entity.payment_type == 0)
            {
                jm.code = 10051;
                jm.msg = GlobalErrorCodeVars.Code10051;
            }
            //生成支付单,并发起支付
            jm = await _billPaymentsServices.Pay(entity.ids, entity.payment_code, _user.ID, entity.payment_type,
                entity.@params);

            return jm;
        }

        #endregion

        #region 订单评价

        /// <summary>
        /// 订单评价
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> OrderEvaluate([FromBody] OrderEvaluatePost entity)
        {
            var jm = new WebApiCallBack();

            if (string.IsNullOrEmpty(entity.orderId))
            {
                jm.code = 13100;
                jm.msg = GlobalErrorCodeVars.Code13100;
            }
            else if (entity.items == null || entity.items.Count == 0)
            {
                jm.code = 10051;
                jm.msg = GlobalErrorCodeVars.Code10051;
            }
            jm = await _goodsCommentServices.AddComment(entity.orderId, entity.items, _user.ID);
            jm.otherData = entity;
            return jm;
        }

        #endregion

        #region 我的银行卡列表
        /// <summary>
        /// 我的银行卡列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> GetMyBankcardsList()
        {
            var jm = await _userBankCardServices.GetMyBankcardsList(_user.ID);
            return jm;
        }

        #endregion

        #region 添加银行卡
        /// <summary>
        /// 添加银行卡
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> AddBankCards([FromBody] CoreCmsUserBankCard entity)
        {
            entity.userId = _user.ID;
            var jm = await _userBankCardServices.AddBankCards(entity);
            return jm;

        }

        #endregion

        #region 设置默认银行卡
        /// <summary>
        /// 设置默认银行卡
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> SetDefaultBankCard([FromBody] FMIntId entity)
        {
            var jm = new WebApiCallBack();

            if (entity.id == 0)
            {
                jm.msg = GlobalErrorCodeVars.Code10051;
                return jm;
            }

            jm = await _userBankCardServices.SetDefault(_user.ID, entity.id);
            return jm;

        }

        #endregion

        #region 获取银行卡信息
        /// <summary>
        /// 获取银行卡信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> GetBankCardInfo([FromBody] FMIntId entity)
        {
            var jm = new WebApiCallBack();

            if (entity.id == 0)
            {
                jm.msg = GlobalErrorCodeVars.Code10051;
                return jm;
            }

            jm = await _userBankCardServices.GetBankcardInfo(_user.ID, entity.id);
            return jm;

        }

        #endregion

        #region 获取用户默认银行卡信息
        /// <summary>
        /// 获取用户默认银行卡信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> GetDefaultBankCard()
        {
            var jm = await _userBankCardServices.GetDefaultBankCard(_user.ID);
            return jm;

        }

        #endregion

        #region 删除银行卡信息
        /// <summary>
        /// 删除银行卡信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> Removebankcard([FromBody] FMIntId entity)
        {
            var jm = await _userBankCardServices.Removebankcard(entity.id, _user.ID);
            return jm;
        }

        #endregion

        #region 获取银行卡组织信息
        /// <summary>
        /// 获取银行卡组织信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public WebApiCallBack GetBankCardsOrganization([FromBody] FMStringId entity)
        {
            var jm = _userBankCardServices.BankCardsOrganization(entity.id);
            return jm;
        }

        #endregion

        #region 提现申请
        /// <summary>
        /// 提现申请
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> Cash([FromBody] FMIntId entity)
        {
            var money = entity.data.ObjectToDecimal(0);
            var jm = await _userTocashServices.Tocash(_user.ID, money, entity.id);
            return jm;
        }

        #endregion

        #region 提现记录列表
        /// <summary>
        /// 提现记录列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> CashList([FromBody] FMPageByIntId entity)
        {
            var jm = await _userTocashServices.UserToCashList(_user.ID, entity.page, entity.limit, entity.id);
            return jm;
        }

        #endregion

        #region 获取我的余额明细列表
        /// <summary>
        /// 获取我的余额明细列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> UserBalance([FromBody] FMGetBalancePost entity)
        {
            var jm = new WebApiCallBack();

            var where = PredicateBuilder.True<CoreCmsUserBalance>();
            where = where.And(p => p.userId == _user.ID);
            if (entity.id > 0)
            {
                where = where.And(p => p.type == entity.id);
            }

            if (!string.IsNullOrEmpty(entity.propsDate))
            {
                if (entity.propsDate.Contains("-"))
                {
                    var dts = entity.propsDate.Split("-");
                    if (dts.Length == 2)
                    {
                        var dt = dts[0].ObjectToDate(DateTime.Now);
                        var startTime = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
                        var dt2 = dts[1].ObjectToDate(DateTime.Now);
                        var endTime = new DateTime(dt2.Year, dt2.Month, dt2.Day, 23, 59, 59);
                        where = where.And(p => p.createTime > startTime && p.createTime < endTime);
                    }
                }
                else
                {
                    var dt = entity.propsDate.ObjectToDate(DateTime.Now);
                    var startTime = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
                    var endTime = new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59);
                    where = where.And(p => p.createTime > startTime && p.createTime < endTime);
                }
            }

            var data = await _userBalanceServices.QueryPageAsync(where, p => p.createTime, OrderByType.Desc,
                entity.page, entity.limit);
            if (data.Any())
            {
                foreach (var item in data)
                {
                    item.typeName = EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.UserBalanceSourceTypes>(item.type);
                }
            }

            var sunMoney = await _userBalanceServices.GetSumAsync(where, p => p.money);

            jm.status = true;
            jm.data = data;
            jm.otherData = new
            {
                data.TotalPages,
                sunMoney
            };
            return jm;

        }

        #endregion

        #region 我的发票列表
        /// <summary>
        /// 我的发票列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> UserInvoiceList([FromBody] FMPageByIntId entity)
        {
            var jm = new WebApiCallBack();

            var where = PredicateBuilder.True<CoreCmsInvoice>();
            where = where.And(p => p.userId == _user.ID);
            if (entity.id > 0)
            {
                where = where.And(p => p.id == entity.id);
            }
            var status = entity.otherData.ObjectToInt(0);
            if (status > 0)
            {
                where = where.And(p => p.status == status);

            }
            var data = await _invoiceServices.QueryPageAsync(where, p => p.createTime, OrderByType.Desc,
                entity.page, entity.limit);
            if (data.Any())
            {
                foreach (var item in data)
                {
                    item.categoryName = EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.OrderTaxCategory>(item.category);
                    item.typeName = EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.OrderTaxType>(item.type);
                    item.statusName = EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.OrderTaxStatus>(item.status);
                }
            }
            jm.status = true;
            jm.data = data;
            jm.otherData = new
            {
                data.TotalCount,
                data.TotalPages
            };
            return jm;

        }

        #endregion

        #region 我的积分列表
        /// <summary>
        /// 我的积分列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> UserPointLog([FromBody] FMPageByIntId entity)
        {
            var jm = new WebApiCallBack();

            var where = PredicateBuilder.True<CoreCmsUserPointLog>();
            where = where.And(p => p.userId == _user.ID);

            var data = await _userPointLogServices.QueryPageAsync(where, p => p.createTime, OrderByType.Desc, entity.page, entity.limit);
            if (data.Any())
            {
                foreach (var item in data)
                {
                    item.typeName = EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.UserPointSourceTypes>(item.type);
                }
            }
            jm.status = true;
            jm.data = data;
            jm.otherData = new
            {
                data.TotalCount,
                data.TotalPages
            };
            return jm;

        }

        #endregion

        #region 取得商品收藏记录（关注）
        /// <summary>
        /// 取得商品收藏记录（关注）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> GoodsCollectionList([FromBody] FMPageByIntId entity)
        {
            var jm = new WebApiCallBack();

            var data = await _goodsCollectionServices.QueryPageAsync(p => p.userId == _user.ID, p => p.createTime, OrderByType.Desc, entity.page, entity.limit);

            jm.status = true;
            jm.data = new
            {
                list = data,
                count = data.TotalCount,

            };
            return jm;

        }

        #endregion

        #region 添加商品收藏（关注）
        /// <summary>
        /// 添加商品收藏（关注）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> GoodsCollection([FromBody] FMIntId entity)
        {
            var jm = await _goodsCollectionServices.ToAdd(_user.ID, entity.id);
            return jm;
        }

        #endregion

        #region 取得商品浏览足迹
        /// <summary>
        /// 取得商品浏览足迹
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> Goodsbrowsing([FromBody] FMPageByIntId entity)
        {
            var jm = new WebApiCallBack();

            var data = await _goodsBrowsingServices.QueryPageAsync(p => p.userId == _user.ID, p => p.createTime, OrderByType.Desc, entity.page, entity.limit);

            jm.status = true;
            jm.data = new
            {
                list = data,
                count = data.TotalCount,

            };
            return jm;

        }

        #endregion

        #region 添加商品浏览足迹
        /// <summary>
        /// 添加商品浏览足迹
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> AddGoodsBrowsing([FromBody] FMIntId entity)
        {
            var jm = new WebApiCallBack();


            //获取数据
            var goods = await _goodsServices.QueryByIdAsync(entity.id);
            if (goods == null)
            {
                jm.msg = GlobalConstVars.DataisNo;
                return jm;
            }
            var goodsBrowsing = new CoreCmsGoodsBrowsing
            {
                goodsId = goods.id,
                userId = _user.ID,
                goodsName = goods.name,
                createTime = DateTime.Now,
                isdel = false
            };
            jm.status = await _goodsBrowsingServices.InsertAsync(goodsBrowsing) > 0;
            jm.msg = jm.status ? GlobalConstVars.InsertSuccess : GlobalConstVars.InsertFailure;

            return jm;
        }


        #endregion

        #region 删除商品浏览足迹
        /// <summary>
        /// 删除商品浏览足迹
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> DelGoodsBrowsing([FromBody] FMIntId entity)
        {
            var jm = new WebApiCallBack();

            jm.status = await _goodsBrowsingServices.DeleteAsync(p => p.userId == _user.ID && p.goodsId == entity.id);
            jm.msg = jm.status ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;

            return jm;
        }
        #endregion

        #region 更换头像
        /// <summary>
        /// 更换头像
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> ChangeAvatar([FromBody] FMStringId entity)
        {
            var jm = new WebApiCallBack();

            if (string.IsNullOrEmpty(entity.id))
            {
                jm.data = GlobalErrorCodeVars.Code11003;
                return jm;
            }

            var up = await _userServices.UpdateAsync(p => new CoreCmsUser() { avatarImage = entity.id },
                p => p.id == _user.ID);

            jm.status = up;
            jm.msg = jm.status ? "设置头像成功" : "设置头像失败";
            jm.data = entity.id;

            return jm;
        }
        #endregion

        #region 编辑用户信息
        /// <summary>
        /// 编辑用户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> EditInfo([FromBody] EditInfoPost entity)
        {
            var jm = new WebApiCallBack();

            if (entity.birthday == null)
            {
                jm.msg = GlobalErrorCodeVars.Code11027;
                return jm;
            }

            if (string.IsNullOrEmpty(entity.nickname))
            {
                jm.msg = GlobalErrorCodeVars.Code11028;
                return jm;
            }

            if (entity.sex <= 0)
            {
                jm.msg = GlobalErrorCodeVars.Code11029;
                return jm;
            }

            var up = await _userServices.UpdateAsync(p => new CoreCmsUser() { birthday = entity.birthday, nickName = entity.nickname, sex = entity.sex },
                p => p.id == _user.ID);

            jm.status = up;
            jm.msg = jm.status ? "资料保存成功" : "资料保存失败";

            return jm;
        }


        #endregion

        #region 修改用户密码
        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> EditPwd([FromBody] EditPwdPost entity)
        {
            var jm = new WebApiCallBack();

            if (string.IsNullOrEmpty(entity.repwd))
            {
                jm.msg = GlobalErrorCodeVars.Code11014;
                return jm;
            }
            if (string.IsNullOrEmpty(entity.newpwd))
            {
                jm.msg = GlobalErrorCodeVars.Code11013;
                return jm;
            }
            if (entity.repwd != entity.newpwd)
            {
                jm.msg = GlobalErrorCodeVars.Code11025;
                return jm;
            }
            jm = await _userServices.ChangePassword(_user.ID, entity.newpwd, entity.pwd);

            return jm;
        }
        #endregion

        #region 邀请好友(获取我的要求相关信息)
        /// <summary>
        /// 邀请好友(获取我的要求相关信息)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> MyInvite()
        {
            var jm = new WebApiCallBack();

            jm.status = true;

            //我的邀请码
            var code = UserHelper.GetShareCodeByUserId(_user.ID);
            //我邀请的人数
            var number = await _userServices.GetCountAsync(p => p.parentId == _user.ID);
            //邀请赚的佣金
            var money = await _userBalanceServices.GetInviteCommission(_user.ID);
            //是否有上级
            var userInfo = await _userServices.QueryByIdAsync(_user.ID);
            bool isSuperior = userInfo != null && userInfo.parentId > 0;

            jm.data = new
            {
                code,
                number,
                money,
                isSuperior
            };
            return jm;
        }

        #endregion

        #region 设置我的上级邀请人
        /// <summary>
        /// 设置我的上级邀请人
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> SetMyInvite([FromBody] FMIntId entity)
        {
            var jm = new WebApiCallBack();

            if (entity.id == 0)
            {
                jm.msg = "请输入推荐人邀请码！";
                return jm;
            }
            var code = UserHelper.GetUserIdByShareCode(entity.id);
            jm = await _userServices.SetMyInvite(code, _user.ID);

            return jm;
        }

        #endregion


        #region 获取我的上级邀请人
        /// <summary>
        /// 获取我的上级邀请人
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> GetMyInvite()
        {
            return await _userServices.GetMyInvite(_user.ID);
        }

        #endregion



        #region 获取我的下级用户数量
        /// <summary>
        /// 获取我的下级用户数量
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> GetMyChildSum()
        {
            var jm = new WebApiCallBack();

            var first = await _userServices.QueryChildCountAsync(_user.ID);
            var second = await _userServices.QueryChildCountAsync(_user.ID, 2);

            var monthFirst = await _userServices.QueryChildCountAsync(_user.ID, 1, true);

            var monthSecond = await _userServices.QueryChildCountAsync(_user.ID, 2, true);

            jm.status = true;
            jm.data = new
            {
                count = first + second,
                first,
                second,
                monthCount = monthFirst + monthSecond,
                monthFirst,
                monthSecond
            };

            return jm;
        }

        #endregion



        #region 获取用户推荐列表
        /// <summary>
        /// 获取用户推荐列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> Recommend([FromBody] FMPageByIntId entity)
        {
            var jm = new WebApiCallBack();

            var where = PredicateBuilder.True<CoreCmsUser>();
            where = where.And(p => p.parentId == _user.ID);

            var data = await _userServices.QueryPageAsync(where, p => p.createTime, OrderByType.Desc, entity.page, entity.limit);

            jm.status = true;
            jm.data = data.Select(p => new
            {
                p.nickName,
                p.avatarImage,
                mobile = UserHelper.FormatMobile(p.mobile),
                p.createTime,
                p.childNum
            });
            jm.otherData = new
            {
                data.TotalCount,
                data.TotalPages
            };

            return jm;

        }
        #endregion

        #region 获取用户邀请码
        /// <summary>
        /// 获取用户邀请码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public WebApiCallBack ShareCode()
        {
            var jm = new WebApiCallBack();

            jm.status = true;
            jm.data = UserHelper.GetShareCodeByUserId(_user.ID);

            return jm;

        }
        #endregion

        #region 判断是否签到
        /// <summary>
        /// 判断是否签到
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> IsSign()
        {
            var jm = await _userPointLogServices.IsSign(_user.ID);
            return jm;
        }
        #endregion

        #region 用户签到
        /// <summary>
        /// 用户签到
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> Sign()
        {
            var jm = await _userPointLogServices.Sign(_user.ID);
            return jm;
        }
        #endregion

        #region 用户找回密码
        /// <summary>
        /// 用户找回密码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> ForgetPwd([FromBody] FMForgetPwdPost entity)
        {
            var jm = new WebApiCallBack();
            if (string.IsNullOrEmpty(entity.mobile))
            {
                jm.msg = GlobalErrorCodeVars.Code10051;
                return jm;
            }
            if (string.IsNullOrEmpty(entity.code))
            {
                jm.msg = GlobalErrorCodeVars.Code10013;
                return jm;
            }
            if (string.IsNullOrEmpty(entity.newpwd))
            {
                jm.msg = GlobalErrorCodeVars.Code11013;
                return jm;
            }
            if (string.IsNullOrEmpty(entity.repwd))
            {
                jm.msg = GlobalErrorCodeVars.Code11014;
                return jm;
            }
            if (entity.newpwd != entity.repwd)
            {
                jm.msg = GlobalErrorCodeVars.Code11025;
                return jm;
            }
            jm = await _userServices.ForgetPassword(entity.mobile, entity.code, entity.newpwd);

            return jm;
        }
        #endregion

        #region 取得服务卡列表信息
        /// <summary>
        /// 取得服务卡列表信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> GetServicesPageList([FromBody] FMPageByIntId entity)
        {
            var jm = new WebApiCallBack();

            var where = PredicateBuilder.True<CoreCmsUserServicesOrder>();
            //where = where.And(p => p.status == (int)GlobalEnumVars.ServicesStatus.Shelve);
            where = where.And(p => p.userId == _user.ID);
            where = where.And(p => p.isPay == true);

            var orders = await _userServicesOrderServices.QueryPageAsync(where, p => p.payTime, OrderByType.Desc, entity.page, entity.limit);

            if (orders.Any())
            {
                var services = await _servicesServices.QueryAsync();
                foreach (var item in orders)
                {
                    item.service = services.Find(p => p.id == item.servicesId);
                    item.statusStr =
                        EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.ServicesOrderStatus>(item.status);
                }
            }

            jm.status = true;
            jm.data = new
            {
                list = orders,
                count = orders.TotalCount,
            };
            return jm;

        }

        #endregion

        #region 取得服务卡列表信息
        /// <summary>
        /// 取得服务卡列表信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> GetServicesTickets([FromBody] FMPageByStringId entity)
        {
            var jm = new WebApiCallBack();


            var order = await _userServicesOrderServices.QueryByClauseAsync(p => p.serviceOrderId == entity.id);
            if (order == null)
            {
                jm.msg = "订单信息获取失败";
                return jm;
            }
            var model = await _servicesServices.QueryByClauseAsync(p => p.id == order.servicesId);
            if (model != null)
            {
                var dt = DateTime.Now;
                TimeSpan ts = model.endTime.Subtract(dt);
                model.timestamp = (int)ts.TotalSeconds;

                if (!string.IsNullOrEmpty(model.consumableStore))
                {
                    var consumableStoreStr = CommonHelper.GetCaptureInterceptedText(model.consumableStore, ",");
                    var consumableStoreIds = CommonHelper.StringToIntArray(consumableStoreStr);
                    if (consumableStoreIds.Any())
                    {
                        var stores = await _storeServices.QueryListByClauseAsync(p => consumableStoreIds.Contains(p.id));
                        model.consumableStores = stores.Select(p => p.storeName).ToList();
                    }
                }
                if (!string.IsNullOrEmpty(model.allowedMembership))
                {
                    var allowedMembershipStr = CommonHelper.GetCaptureInterceptedText(model.allowedMembership, ",");
                    var allowedMembershipIds = CommonHelper.StringToIntArray(allowedMembershipStr);
                    if (allowedMembershipIds.Any())
                    {
                        var userGrades = await _userGradeServices.QueryListByClauseAsync(p => allowedMembershipIds.Contains(p.id));
                        model.allowedMemberships = userGrades.Select(p => p.title).ToList();
                    }
                }
            }

            var orders = await _userServicesTicketServices.QueryPageAsync(p => p.serviceOrderId == entity.id, p => p.createTime, OrderByType.Asc, entity.page, entity.limit);
            if (orders.Any())
            {
                foreach (var item in orders)
                {
                    item.statusStr =
                        EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.ServicesTicketStatus>(item.status);
                }
            }

            jm.status = true;
            jm.data = new
            {
                model,
                list = orders,
                count = orders.TotalCount,
            };
            return jm;

        }

        #endregion

    }
}