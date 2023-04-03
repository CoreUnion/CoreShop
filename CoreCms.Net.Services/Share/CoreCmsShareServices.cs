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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using CoreCms.Net.Caching.AccressToken;
using CoreCms.Net.Configuration;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.IServices;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Utility.Extensions;
using CoreCms.Net.Utility.Helper;
using CoreCms.Net.WeChat.Service.Enums;
using CoreCms.Net.WeChat.Service.HttpClients;
using CoreCms.Net.WeChat.Service.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

using SKIT.FlurlHttpClient.Wechat.Api;
using SKIT.FlurlHttpClient.Wechat.Api.Models;


namespace CoreCms.Net.Services
{
    /// <summary>
    /// 表单 接口实现
    /// </summary>
    public class CoreCmsShareServices : BaseServices<CoreCmsSetting>, ICoreCmsShareServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly string _appInterFaceUrl = AppSettingsConstVars.AppConfigAppInterFaceUrl;

        private readonly ICoreCmsGoodsServices _goodsServices;
        private readonly ICoreCmsUserServices _userServices;
        private readonly WeChatOptions _weChatOptions;
        private readonly WeChat.Service.HttpClients.IWeChatApiHttpClientFactory _weChatApiHttpClientFactory;



        public CoreCmsShareServices(IUnitOfWork unitOfWork
            , IWebHostEnvironment webHostEnvironment
            , ICoreCmsGoodsServices goodsServices
            , IOptions<WeChatOptions> weChatOptions, IWeChatApiHttpClientFactory weChatApiHttpClientFactory,ICoreCmsUserServices userServices)

        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _goodsServices = goodsServices;
            _weChatApiHttpClientFactory = weChatApiHttpClientFactory;
            _userServices = userServices;
            _weChatOptions = weChatOptions.Value;

        }


        #region 二维码分享

        /// <summary>
        /// 二维码分享
        /// </summary>
        /// <param name="client"></param>
        /// <param name="page"></param>
        /// <param name="userShareCode"></param>
        /// <param name="url"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> QrShare(int client, int page, int userShareCode, string url, JObject parameter)
        {
            var res = GetCode(client, page, userShareCode, url, parameter);
            if (!res.status)
            {
                return res;
            }
            return await getQr(url, res.otherData.ToString(), client);
        }

        private async Task<WebApiCallBack> getQr(string url, string code, int client)
        {
            var jm = new WebApiCallBack() { status = true };

            switch (client)
            {
                case (int)GlobalEnumVars.UrlShareClentType.Wxmnapp:
                    jm = await GetQrCode(code, url);
                    break;
                default:
                    var urlStr = GetUrl(url, code);
                    urlStr = HttpUtility.UrlEncode(urlStr);
                    break;
            }
            return jm;
        }

        #endregion

        #region 海报分享
        /// <summary>
        /// 海报分享
        /// </summary>
        /// <param name="client"></param>
        /// <param name="page"></param>
        /// <param name="userShareCode"></param>
        /// <param name="url"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> PosterShare(int client, int page, int userShareCode, string url, JObject parameter)
        {
            var res = GetCode(client, page, userShareCode, url, parameter);
            if (!res.status)
            {
                return res;
            }
            //如果不是商品和拼团
            if (page != (int)GlobalEnumVars.UrlSharePageType.PinTuan && page != (int)GlobalEnumVars.UrlSharePageType.Goods && page != (int)GlobalEnumVars.UrlSharePageType.Seckill && page != (int)GlobalEnumVars.UrlSharePageType.Group)
            {
                return await getQr(url, res.otherData.ToString(), client);
            }
            //生成海报图片

            var userId = UserHelper.GetUserIdByShareCode(userShareCode);
            var user = await _userServices.QueryByClauseAsync(p => p.id == userId);


            var result = await Poster(url, res.otherData.ToString(), client, user);
            return result;
        }
        #endregion

        #region 页面分享UrlShare
        /// <summary>
        /// 页面分享
        /// </summary>
        /// <param name="client"></param>
        /// <param name="page"></param>
        /// <param name="userShareCode"></param>
        /// <param name="url"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public WebApiCallBack UrlShare(int client, int page, int userShareCode, string url, JObject parameter)
        {
            var res = GetCode(client, page, userShareCode, url, parameter);
            if (!res.status)
            {
                return res;
            }
            res.data = GetUrl(url, res.otherData.ToString());
            return res;
        }

        #endregion

        #region 二维码生成
        /// <summary>
        /// 二维码生成
        /// </summary>
        /// <param name="invite"></param>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <param name="groupId"></param>
        /// <param name="teamId"></param>
        /// <param name="style"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> GetParameterQrCode(string invite = "", int type = 1, string id = "", string groupId = "", string teamId = "", List<string> style = null, string page = "pages/share/jump")
        {
            var jm = new WebApiCallBack() { status = false, msg = "获取失败" };

            var styles = style != null && style.Any() ? string.Join("-", style.ToArray()) : "";
            var nameStr = CommonHelper.Md5For32(page + invite + type + id + groupId + teamId + _weChatOptions.WxOpenAppId + styles);

            //QrCode 根目录
            var dir = "/static/qrCode/weChat/";
            //文件虚拟目录
            var fileName = dir + nameStr + ".jpg";
            //文件硬地址
            var qrCodeDir = _webHostEnvironment.WebRootPath + dir;
            //QrCode 根目录
            var pathFileName = qrCodeDir + nameStr + ".jpg";

            //QrCode 根目录
            if (File.Exists(pathFileName))
            {
                //有这个二维码了
                jm.status = true;
                jm.msg = "二维码获取成功";
                jm.data = _appInterFaceUrl + fileName;
            }
            else
            {
                //没有去官方请求生成
                var scene = string.Empty;
                if (type == 1)
                {
                    //商品详情页
                    if (!string.IsNullOrEmpty(invite))
                    {
                        scene = ShareHelper.share_parameter_encode("2", invite, id, "");
                    }
                    else
                    {
                        scene = ShareHelper.share_parameter_encode("2", "", id, "");
                    }
                }
                else if (type == 2)
                {
                    //首页
                    if (!string.IsNullOrEmpty(invite))
                    {
                        scene = ShareHelper.share_parameter_encode("3", invite, "", "");
                    }
                    else
                    {
                        scene = ShareHelper.share_parameter_encode("3", "", "", "");
                    }
                }
                else if (type == 3)
                {
                    //拼团
                    if (!string.IsNullOrEmpty(invite))
                    {
                        if (!string.IsNullOrEmpty(teamId))
                        {
                            scene = ShareHelper.share_parameter_encode("5", invite, id, teamId);
                        }
                        else
                        {
                            scene = ShareHelper.share_parameter_encode("5", invite, id, "");
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(teamId))
                        {
                            scene = ShareHelper.share_parameter_encode("5", "", id, teamId);
                        }
                        else
                        {
                            scene = ShareHelper.share_parameter_encode("5", "", id, "");
                        }
                    }
                }
                else if (type == 4)
                {
                    //店铺首页
                    if (!string.IsNullOrEmpty(invite))
                    {
                        scene = ShareHelper.share_parameter_encode("9", invite, id, "");
                    }
                    else
                    {
                        scene = ShareHelper.share_parameter_encode("9", "", id, "");
                    }
                }
                else
                {
                    //默认首页
                    if (!string.IsNullOrEmpty(invite))
                    {
                        scene = ShareHelper.share_parameter_encode("3", invite, "", "");
                    }
                    else
                    {
                        scene = ShareHelper.share_parameter_encode("3", "", "", "");
                    }
                }

                //没有去官方请求生成
                var ms = new MemoryStream();

                var accessToken = WeChatCacheAccessTokenHelper.GetWxOpenAccessToken();
                var client = _weChatApiHttpClientFactory.CreateWxOpenClient();
                var request = new WxaGetWxaCodeUnlimitRequest();
                request.AccessToken = accessToken;
                request.Scene = scene;
                request.PagePath = page;
                request.LineColor = new WxaGetWxaCodeUnlimitRequest.Types.Color() { Red = 221, Blue = 51, Green = 238 };

                var response = await client.ExecuteWxaGetWxaCodeUnlimitAsync(request);
                if (response.IsSuccessful())
                {
                    ms = new MemoryStream(response.RawBytes);
                }
                else
                {
                    if (response.ErrorCode == (int)WeChatReturnCode.ReturnCode.page不正确)
                    {
                        jm.msg = "后台小程序配置的APPID和APPSECRET对应的小程序未发布上线,或者page没有发布，无法生成海报";
                        return jm;
                    }
                    else if (response.IsSuccessful() && response.ErrorCode == (int)WeChatReturnCode.ReturnCode.获取access_token时AppSecret错误或者access_token无效)
                    {
                        jm.msg = "微信小程序access_token已过期，无法为你生成海报";
                        return jm;
                    }
                    else
                    {
                        var enumType = EnumHelper.GetEnumberEntity<WeChatReturnCode.ReturnCode>(response.ErrorCode);
                        if (enumType != null)
                        {
                            jm.msg = response.ErrorCode + enumType.title;
                        }
                        return jm;
                    }
                }

                //QrCode 根目录
                if (!Directory.Exists(qrCodeDir))
                {
                    Directory.CreateDirectory(qrCodeDir);
                }

                await using FileStream fs = System.IO.File.Create(pathFileName);
                await ms.CopyToAsync(fs);
                fs.Flush();

                jm.status = true;
                jm.msg = "二维码生成成功";
                jm.data = AppSettingsConstVars.AppConfigAppInterFaceUrl + fileName;
                jm.otherData = response;
            }
            return jm;
        }
        #endregion

        #region 小程序二维码，和业务没关系【GetQrCode】
        /// <summary>
        /// 小程序二维码，和业务没关系
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> GetQrCode(string scene, string page = "pages/share/jump")
        {
            var jm = new WebApiCallBack();

            jm.otherData = scene;

            var fileNameStr = CommonHelper.Md5For32(scene) + CommonHelper.Msectime();
            //QrCode 根目录
            var dir = "/static/qrCode/weChat/";
            //文件虚拟目录
            var fileName = dir + fileNameStr + ".jpg";
            //文件硬地址
            var qrCodeDir = _webHostEnvironment.WebRootPath + dir;
            //QrCode 文件全路径
            var pathFileName = qrCodeDir + fileNameStr + ".jpg";
            var fileNameMin = fileNameStr + ".jpg";

            if (File.Exists(pathFileName))
            {
                jm.status = true;
                jm.msg = "二维码获取成功";
                jm.data = _appInterFaceUrl + fileName;
                jm.otherData = fileNameMin;
            }
            else
            {
                //没有去官方请求生成
                var ms = new MemoryStream();

                var accessToken = WeChatCacheAccessTokenHelper.GetWxOpenAccessToken();
                var client = _weChatApiHttpClientFactory.CreateWxOpenClient();
                var request = new WxaGetWxaCodeUnlimitRequest();
                request.AccessToken = accessToken;
                request.Scene = scene;
                request.PagePath = page;
                request.LineColor = new WxaGetWxaCodeUnlimitRequest.Types.Color() { Red = 221, Blue = 51, Green = 238 };

                var response = await client.ExecuteWxaGetWxaCodeUnlimitAsync(request);
                if (!response.IsSuccessful() && response.ErrorCode == (int)WeChatReturnCode.ReturnCode.page不正确)
                {
                    jm.msg = "后台小程序配置的APPID和APPSECRET对应的小程序未发布上线,或者page没有发布，无法生成海报";
                    return jm;
                }
                else if (!response.IsSuccessful() && response.ErrorCode == (int)WeChatReturnCode.ReturnCode.获取access_token时AppSecret错误或者access_token无效)
                {
                    jm.msg = "微信小程序access_token已过期，无法为你生成海报";
                    return jm;
                }
                else if (!response.IsSuccessful() && response.ErrorCode > 0)
                {
                    var enumType = EnumHelper.GetEnumberEntity<WeChatReturnCode.ReturnCode>(response.ErrorCode);
                    if (enumType != null)
                    {
                        jm.msg = response.ErrorCode + enumType.title;
                    }
                    return jm;
                }
                else
                {
                    ms = new MemoryStream(response.RawBytes);
                }

                //QrCode 根目录
                if (!Directory.Exists(qrCodeDir))
                {
                    Directory.CreateDirectory(qrCodeDir);
                }

                FileStream fs = new FileStream(pathFileName, FileMode.OpenOrCreate);
                BinaryWriter w = new BinaryWriter(fs);
                w.Write(ms.ToArray());
                fs.Close();
                ms.Close();

                jm.status = true;
                jm.msg = "GetQrCode";
                jm.data = AppSettingsConstVars.AppConfigAppInterFaceUrl + fileName;
                jm.otherData = fileNameMin;
            }
            return jm;
        }

        #endregion

        #region 获得分享的Code【GetCode】
        /// <summary>
        /// 获得分享的code
        /// </summary>
        /// <param name="client"></param>
        /// <param name="page"></param>
        /// <param name="userShareCode"></param>
        /// <param name="url"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private static WebApiCallBack GetCode(int client, int page, int userShareCode, string url, JObject parameter)
        {
            //检查params参数是否正确
            WebApiCallBack result = en_params(page, parameter);
            if (!result.status)
            {
                return result;
            }
            var code = en_url(page, userShareCode, result.data.ToString());
            result.otherData = code;
            return result;
        }

        #endregion

        #region 根据获得的code，拼接url【GetUrl】
        /// <summary>
        /// 根据获得的code，拼接url
        /// </summary>
        /// <param name="url"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        private static string GetUrl(string url, string code)
        {
            return url + "?scene=" + code;
        }
        #endregion

        #region url参数加密【en_url】
        /// <summary>
        /// url参数加密
        /// </summary>
        /// <param name="page"></param>
        /// <param name="userShareCode"></param>
        /// <param name="paramsStr"></param>
        /// <returns></returns>
        private static string en_url(int page, int userShareCode, string paramsStr)
        {
            return page + "-" + userShareCode + "-" + paramsStr;
        }
        #endregion

        #region url参数解密【de_url】
        /// <summary>
        /// url参数解密
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public WebApiCallBack de_url(string code)
        {
            //3-3702-2032_0
            var jm = new WebApiCallBack();

            var arr = code.Split("-");
            if (arr.Length != 3)
            {
                return jm;
            }

            var page = arr[0];
            var userShareCode = arr[1];


            var paramsResult = de_params(Convert.ToInt16(arr[0]), arr[2]);
            if (paramsResult.status)
            {
                jm.status = true;
                jm.data = new
                {
                    page,
                    userShareCode,
                    @params = paramsResult.data
                };
            }
            else
            {
                paramsResult.otherData = code;
                return paramsResult;
            }
            return jm;
        }
        #endregion

        #region 检查参数，拼接参数【en_params】
        /// <summary>
        /// 检查参数，拼接参数
        /// </summary>
        /// <param name="page"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private static WebApiCallBack en_params(int page, JObject parameter)
        {
            var jm = new WebApiCallBack();
            var str = "";

            switch (page)
            {
                case (int)GlobalEnumVars.UrlSharePageType.Index:

                    break;
                case (int)GlobalEnumVars.UrlSharePageType.Goods:
                    if (parameter.ContainsKey("goodsId"))
                    {
                        str = parameter["goodsId"]?.ToString();
                    }
                    else
                    {
                        jm.msg = "参数必须传goodsId";
                        return jm;
                    }
                    break;
                case (int)GlobalEnumVars.UrlSharePageType.PinTuan:
                    if (parameter.ContainsKey("goodsId") || parameter.ContainsKey("teamId"))
                    {
                        str = parameter["goodsId"] + "_" + parameter["teamId"];
                    }
                    else
                    {
                        jm.msg = "参数必须传goodsId,teamId";
                        return jm;
                    }
                    break;
                case (int)GlobalEnumVars.UrlSharePageType.Inv:
                    if (parameter.ContainsKey("store"))
                    {
                        str = parameter["store"]?.ToString();
                    }
                    else
                    {
                        jm.msg = "参数必须传store";
                        return jm;
                    }
                    break;
                case (int)GlobalEnumVars.UrlSharePageType.Article:
                    if (parameter.ContainsKey("articleId") || parameter.ContainsKey("articleType"))
                    {
                        str = parameter["articleId"] + "_" + parameter["articleType"];
                    }
                    else
                    {
                        jm.msg = "参数必须传articleId,articleType";
                        return jm;
                    }
                    break;
                case (int)GlobalEnumVars.UrlSharePageType.AddPinTuan:
                    if (parameter.ContainsKey("goodsId") || parameter.ContainsKey("groupId") || parameter.ContainsKey("teamId"))
                    {
                        str = parameter["goodsId"] + "_" + parameter["groupId"] + "_" + parameter["teamId"];
                    }
                    else
                    {
                        jm.msg = "参数必须传goodsId,groupId,teamId";
                        return jm;
                    }
                    break;
                case (int)GlobalEnumVars.UrlSharePageType.Page:
                    if (parameter.ContainsKey("pageCode"))
                    {
                        str = parameter["pageCode"]?.ToString();
                    }
                    else
                    {
                        jm.msg = "参数必须传pageCode";
                        return jm;
                    }
                    break;
                case (int)GlobalEnumVars.UrlSharePageType.Form:
                    if (parameter.ContainsKey("id"))
                    {
                        str = parameter["id"]?.ToString();
                    }
                    else
                    {
                        jm.msg = "参数必须传id";
                        return jm;
                    }
                    break;
                case (int)GlobalEnumVars.UrlSharePageType.Group:
                    if (parameter.ContainsKey("goodsId") || parameter.ContainsKey("groupId"))
                    {
                        str = parameter["goodsId"] + "_" + parameter["groupId"];
                    }
                    else
                    {
                        jm.msg = "参数必须传goodsId,groupId";
                        return jm;
                    }
                    break;
                case (int)GlobalEnumVars.UrlSharePageType.Seckill:
                    if (parameter.ContainsKey("goodsId") || parameter.ContainsKey("groupId"))
                    {
                        str = parameter["goodsId"] + "_" + parameter["groupId"];
                    }
                    else
                    {
                        jm.msg = "参数必须传goodsId,groupId";
                        return jm;
                    }
                    break;
                case (int)GlobalEnumVars.UrlSharePageType.Agent:
                    if (parameter.ContainsKey("store"))
                    {
                        str = parameter["store"]?.ToString();
                    }
                    else
                    {
                        jm.msg = "参数必须传store";
                        return jm;
                    }
                    break;
                default:
                    jm.msg = GlobalErrorCodeVars.Code10000;
                    return jm;
            }

            jm.status = true;
            jm.data = str;
            return jm;
        }

        #endregion

        #region 解码参数【de_params】
        /// <summary>
        /// 解码参数
        /// </summary>
        /// <param name="page"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private static WebApiCallBack de_params(int page, string parameter)
        {
            var jm = new WebApiCallBack();
            var arr = parameter.Split("_");

            switch (page)
            {
                case (int)GlobalEnumVars.UrlSharePageType.Index:
                    jm.status = true;
                    break;
                case (int)GlobalEnumVars.UrlSharePageType.Goods:
                    if (arr.Length == 1)
                    {
                        jm.data = new { goodsId = arr[0] };
                        jm.status = true;
                    }
                    break;
                case (int)GlobalEnumVars.UrlSharePageType.PinTuan:
                    //if (arr.Length == 3)
                    //{
                    //    jm.data = new
                    //    {
                    //        goodsId = arr[0],
                    //        groupId = arr[1],
                    //        teamId = arr[2]
                    //    };
                    //    jm.status = true;
                    //}

                    if (arr.Length == 2)
                    {
                        jm.data = new
                        {
                            goodsId = arr[0],
                            teamId = arr[1]
                        };
                        jm.status = true;
                    }

                    break;
                case (int)GlobalEnumVars.UrlSharePageType.Inv:
                    if (arr.Length == 1)
                    {
                        jm.data = new { store = arr[0] };
                        jm.status = true;
                    }
                    break;
                case (int)GlobalEnumVars.UrlSharePageType.Article:
                    if (arr.Length == 2)
                    {
                        jm.data = new
                        {
                            articleId = arr[0],
                            articleType = arr[1],
                        };
                        jm.status = true;
                    }
                    break;
                case (int)GlobalEnumVars.UrlSharePageType.AddPinTuan:
                    if (arr.Length == 3)
                    {
                        jm.data = new
                        {
                            goodsId = arr[0],
                            groupId = arr[1],
                            teamId = arr[2]
                        };
                        jm.status = true;
                    }
                    break;
                case (int)GlobalEnumVars.UrlSharePageType.Page:
                    if (arr.Length == 1)
                    {
                        jm.data = new { pageCode = arr[0] };
                        jm.status = true;
                    }
                    break;
                case (int)GlobalEnumVars.UrlSharePageType.Form:
                    if (arr.Length == 1)
                    {
                        jm.data = new { goodsId = arr[0] };
                        jm.status = true;
                    }
                    break;
                case (int)GlobalEnumVars.UrlSharePageType.Group:
                    if (arr.Length == 2)
                    {
                        jm.data = new
                        {
                            goodsId = arr[0],
                            groupId = arr[1],
                        };
                        jm.status = true;
                    }
                    break;
                case (int)GlobalEnumVars.UrlSharePageType.Seckill:
                    if (arr.Length == 2)
                    {
                        jm.data = new
                        {
                            goodsId = arr[0],
                            groupId = arr[1],
                        };
                        jm.status = true;
                    }
                    break;
                case (int)GlobalEnumVars.UrlSharePageType.Agent:
                    if (arr.Length == 1)
                    {
                        jm.data = new { store = arr[0] };
                        jm.status = true;
                    }
                    break;
                default:
                    jm.msg = GlobalErrorCodeVars.Code10000;
                    return jm;
            }
            return jm;
        }
        #endregion

        /// <summary>
        /// 渲染并生成海报图片
        /// </summary>
        /// <param name="url"></param>
        /// <param name="code"></param>
        /// <param name="client"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> Poster(string url, string code, int client, CoreCmsUser user)
        {
            var jm = new WebApiCallBack() { status = false, msg = "海报生成失败0" };


            var fileNameStr = CommonHelper.Md5For32(url + code + client) + CommonHelper.Msectime();
            //QrCode 根目录
            var dir = "/static/poster/";
            //文件虚拟目录
            var fileName = dir + fileNameStr + ".jpg";
            //文件硬地址
            var qrCodeDir = _webHostEnvironment.WebRootPath + dir;
            //QrCode 根目录
            var pathFileName = qrCodeDir + fileNameStr + ".jpg";

            //QrCode 根目录
            if (File.Exists(pathFileName))
            {
                //有这个二维码了
                jm.status = true;
                jm.msg = "海报获取成功";
                jm.data = _appInterFaceUrl + fileName;
            }
            else
            {
                //去生成
                var res = de_url(code);
                if (!res.status)
                {
                    jm.msg = "海报生成失败1";
                    return res;
                }

                var qrResult = await getQr(url, code, client);
                if (!qrResult.status)
                {
                    jm.msg = "海报生成失败2";
                    return qrResult;
                }

                var mkResult = await DoMark(res.data, qrResult.otherData.ToString(), fileName, user);
                if (mkResult)
                {
                    jm.status = true;
                    jm.msg = "海报生成成功";
                    jm.data = _appInterFaceUrl + fileName;
                }
                jm.otherData = mkResult;
            }
            return jm;
        }

        /// <summary>
        /// 绘制图片
        /// </summary>
        /// <param name="data"></param>
        /// <param name="otherData">生成的二维码图片地址</param>
        /// <param name="fileNameStr">海报文件名称</param>
        /// <param name="user">用户信息</param>
        /// <returns></returns>
        public async Task<bool> DoMark(object data, string otherData, string fileNameStr, CoreCmsUser user)
        {
            var fileName = fileNameStr;

            //文件硬地址
            var savePath = _webHostEnvironment.WebRootPath + "/static/poster/";
            var qrCodeDir = _webHostEnvironment.WebRootPath + "/static/qrCode/weChat/" + otherData;
            //如果文件夹不存在，则创建文件夹
            if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);

            //获取数据来源
            var dataObj = JObject.FromObject(data);
            if (!dataObj.ContainsKey("page") || dataObj["page"].ObjectToInt(0) <= 0)
            {
                return false;
            }
            //2商品详情页，3拼团详情页
            var page = dataObj["page"].ObjectToInt(0);
            if (page is (int)GlobalEnumVars.UrlSharePageType.Goods or (int)GlobalEnumVars.UrlSharePageType.PinTuan or (int)GlobalEnumVars.UrlSharePageType.Seckill)
            {
                if (dataObj.ContainsKey("params"))
                {
                    var paramsObj = JObject.FromObject(dataObj["params"]);
                    if (!paramsObj.ContainsKey("goodsId"))
                    {
                        return false;
                    }
                    var goodId = paramsObj["goodsId"].ObjectToInt();
                    var goodModel = await _goodsServices.GetGoodsDetial(goodId);
                    if (goodModel != null)
                    {
                        var images = goodModel.images.Split(",");
                        if (images.Any())
                        {
                            var image = images[0];
                            //创建一个背景宽度为400X600的底图
                            using var imageTemple = new SixLabors.ImageSharp.Image<Rgba32>(400, 600);
                            //设置底图的背景色为白色
                            imageTemple.Mutate(x => x.BackgroundColor(SixLabors.ImageSharp.Color.White));
                            //绘制商品图片（网络下载图片）
                            HttpClient client = new HttpClient();
                            HttpResponseMessage response = await client.GetAsync(image);
                            response.EnsureSuccessStatusCode();
                            var stream = await response.Content.ReadAsStreamAsync();
                            //载入下载的图片流2
                            var imageThumbnail = await SixLabors.ImageSharp.Image.LoadAsync(stream);
                            //将下载的图片压缩至400X400
                            imageThumbnail.Mutate(x =>
                            {
                                x.Resize(400, 400);
                            });
                            //将商品大图合并到背景图上
                            imageTemple.Mutate(x => x.DrawImage(imageThumbnail, new SixLabors.ImageSharp.Point(0, 0), 1));
                            //将用户的分享二维码合并大背景图上
                            var imageQrcode = await SixLabors.ImageSharp.Image.LoadAsync(qrCodeDir);
                            //将二维码缩略至120X120
                            imageQrcode.Mutate(x =>
                            {
                                x.Resize(120, 120);
                            });
                            //将二维码图片合并到背景图上
                            imageTemple.Mutate(x => x.DrawImage(imageQrcode, new SixLabors.ImageSharp.Point(275, 420), 1));
                            //构建字体//装载字体(ttf)（而且字体一定要支持简体中文的）
                            var fonts = new FontCollection();
                            SixLabors.Fonts.FontFamily fontFamily = fonts.Install(_webHostEnvironment.WebRootPath + "/fonts/SourceHanSansK-Normal.ttf");
                            //商品名称可能较长,设置为多行文本输出
                            SixLabors.Fonts.Font titleFont = new SixLabors.Fonts.Font(fontFamily, 20, SixLabors.Fonts.FontStyle.Regular);
                            //多行文本输出
                            var textOptions = new TextOptions()
                            {
                                ApplyKerning = true,
                                VerticalAlignment = VerticalAlignment.Top,
                                HorizontalAlignment = HorizontalAlignment.Left,
                                WrapTextWidth = 230
                            };
                            var graphicsOptions = new GraphicsOptions()
                            {
                                Antialias = true
                            };
                            //沿着行尾的绕行路径绘制文本  
                            var options = new SixLabors.ImageSharp.Drawing.Processing.DrawingOptions
                            {
                                GraphicsOptions = graphicsOptions,
                                TextOptions = textOptions
                            };
                            //开始绘制商品名称
                            imageTemple.Mutate(ctx => ctx.DrawText(options, goodModel.name, titleFont, SixLabors.ImageSharp.Color.Red, new SixLabors.ImageSharp.PointF(10, 450)));
                            //绘制商品金额
                            SixLabors.Fonts.Font moneyFont = new SixLabors.Fonts.Font(fontFamily, 18);
                            //获取该文件绘制所需的大小
                            imageTemple.Mutate(ctx => ctx.DrawText("￥" + goodModel.price, moneyFont, SixLabors.ImageSharp.Color.Crimson, new SixLabors.ImageSharp.PointF(10, 410)));
                            //绘制提示语
                            SixLabors.Fonts.Font tipsFont = new SixLabors.Fonts.Font(fontFamily, 10);
                            imageTemple.Mutate(ctx => ctx.DrawText("扫描或长按识别二维码", tipsFont, SixLabors.ImageSharp.Color.Black, new SixLabors.ImageSharp.PointF(283, 555)));
                            //载入流存储在到文件
                            await imageTemple.SaveAsync(_webHostEnvironment.WebRootPath + fileName);

                            return true;
                        }

                    }
                }
            }
            return false;
        }

    }
}
