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
using CoreCms.Net.IServices;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.WeChatService.WxOpenMessageHandler;
using Microsoft.AspNetCore.Mvc;
using Senparc.CO2NET.AspNet.HttpUtility;
using Senparc.CO2NET.Extensions;
using Senparc.CO2NET.Utilities;
using Senparc.Weixin;
using Senparc.Weixin.Entities.TemplateMessage;
using Senparc.Weixin.MP;
using Senparc.Weixin.WxOpen.AdvancedAPIs.Sns;
using Senparc.Weixin.WxOpen.AdvancedAPIs.WxApp;
using Senparc.Weixin.WxOpen.Containers;
using Senparc.Weixin.WxOpen.Entities;
using Senparc.Weixin.WxOpen.Helpers;

namespace CoreCms.Net.Web.WebApi.Controllers.WeChatOAuth
{
    /// <summary>
    /// 微信小程序请求对接Controller
    /// </summary>
    [Route("[controller]/[action]")]
    public partial class WxOpenMessageController : Controller
    {
        /// <summary>
        /// //与微信小程序后台的Token设置保持一致，区分大小写。
        /// </summary>
        private static readonly string Token = Config.SenparcWeixinSetting.WxOpenToken;
        /// <summary>
        /// //与微信小程序后台的EncodingAESKey设置保持一致，区分大小写。
        /// </summary>
        private static readonly string EncodingAesKey = Config.SenparcWeixinSetting.WxOpenEncodingAESKey;
        /// <summary>
        /// //与微信小程序后台的AppId设置保持一致，区分大小写。
        /// </summary>
        private static readonly string WxOpenAppId = Config.SenparcWeixinSetting.WxOpenAppId;
        /// <summary>
        /// //与微信小程序账号后台的AppId设置保持一致，区分大小写。
        /// </summary>
        private static readonly string WxOpenAppSecret = Config.SenparcWeixinSetting.WxOpenAppSecret;

        readonly Func<string> _getRandomFileName = () => SystemTime.Now.ToString("yyyyMMdd-HHmmss") + Guid.NewGuid().ToString("n").Substring(0, 6);

        private readonly ICoreCmsUserWeChatInfoServices _userWeChatInfoServices;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userWeChatInfoServices"></param>
        public WxOpenMessageController(ICoreCmsUserWeChatInfoServices userWeChatInfoServices)
        {
            _userWeChatInfoServices = userWeChatInfoServices;
        }

        /// <summary>
        /// GET请求用于处理微信小程序后台的URL验证
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("Index")]
        public ActionResult Get(Senparc.Weixin.WxOpen.Entities.Request.PostModel postModel, string echostr)
        {
            if (CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, Token))
            {
                return Content(echostr); //返回随机字符串则表示验证通过
            }
            else
            {
                return Content("failed:" + postModel.Signature + "," + Senparc.Weixin.MP.CheckSignature.GetSignature(postModel.Timestamp, postModel.Nonce, Token) + "。" +
                    "如果你在浏览器中看到这句话，说明此地址可以被作为微信小程序后台的Url，请注意保持Token一致。");
            }
        }

        /// <summary>
        /// 用户发送消息后，微信平台自动Post一个请求到这里，并等待响应XML。
        /// </summary>
        [HttpPost]
        [ActionName("Index")]
        [Obsolete]
        public ActionResult Post(Senparc.Weixin.WxOpen.Entities.Request.PostModel postModel)
        {
            if (!CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, Token))
            {
                return Content("参数错误！");
            }

            postModel.Token = Token;//根据自己后台的设置保持一致
            postModel.EncodingAESKey = EncodingAesKey;//根据自己后台的设置保持一致
            postModel.AppId = WxOpenAppId;//根据自己后台的设置保持一致（必须提供）

            //v4.2.2之后的版本，可以设置每个人上下文消息储存的最大数量，防止内存占用过多，如果该参数小于等于0，则不限制
            var maxRecordCount = 10;

            var logPath = ServerUtility.ContentRootMapPath($"~/App_Data/WxOpen/{SystemTime.Now:yyyy-MM-dd}/");
            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }

            //自定义MessageHandler，对微信请求的详细判断操作都在这里面。
            var messageHandler = new CustomWxOpenMessageHandler(Request.GetRequestMemoryStream(), postModel, maxRecordCount);


            try
            {
                /* 如果需要添加消息去重功能，只需打开OmitRepeatedMessage功能，SDK会自动处理。
                 * 收到重复消息通常是因为微信服务器没有及时收到响应，会持续发送2-5条不等的相同内容的RequestMessage*/
                messageHandler.OmitRepeatedMessage = true;

                //测试时可开启此记录，帮助跟踪数据，使用前请确保App_Data文件夹存在，且有读写权限。
                messageHandler.SaveRequestMessageLog();//记录 Request 日志（可选）

                messageHandler.Execute();//执行微信处理过程（关键）

                messageHandler.SaveResponseMessageLog();//记录 Response 日志（可选）

                ////return Content(messageHandler.ResponseDocument.ToString());//v0.7-
                //return new FixWeixinBugWeixinResult(messageHandler);//为了解决官方微信5.0软件换行bug暂时添加的方法，平时用下面一个方法即可
                //                                                    //return new WeixinResult(messageHandler);//v0.8+

                var str = messageHandler.TextResponseMessage.Replace("\r\n", "\n");
                return Content(str);
            }
            catch (Exception ex)
            {
                using (TextWriter tw = new StreamWriter(ServerUtility.ContentRootMapPath("~/App_Data/Error_WxOpen_" + _getRandomFileName() + ".txt")))
                {
                    tw.WriteLine("ExecptionMessage:" + ex.Message);
                    tw.WriteLine(ex.Source);
                    tw.WriteLine(ex.StackTrace);
                    //tw.WriteLine("InnerExecptionMessage:" + ex.InnerException.Message);

                    if (messageHandler.ResponseDocument != null)
                    {
                        tw.WriteLine(messageHandler.ResponseDocument.ToString());
                    }

                    if (ex.InnerException != null)
                    {
                        tw.WriteLine("========= InnerException =========");
                        tw.WriteLine(ex.InnerException.Message);
                        tw.WriteLine(ex.InnerException.Source);
                        tw.WriteLine(ex.InnerException.StackTrace);
                    }

                    tw.Flush();
                    tw.Close();
                }
                return Content("");
            }
        }

        /// <summary>
        /// 返回数据回调测试
        /// </summary>
        /// <param name="nickName"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RequestData(string nickName)
        {
            var data = new
            {
                msg = $"服务器时间：{SystemTime.Now.LocalDateTime}，昵称：{nickName}"
            };
            return Json(data);
        }

        /// <summary>
        /// wx.login登陆成功之后发送的请求
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult OnLogin(string code)
        {
            try
            {
                var jsonResult = SnsApi.JsCode2Json(WxOpenAppId, WxOpenAppSecret, code);
                if (jsonResult.errcode == ReturnCode.请求成功)
                {
                    //Session["WxOpenUser"] = jsonResult;//使用Session保存登陆信息（不推荐）
                    //使用SessionContainer管理登录信息（推荐）
                    //var unionId = "";
                    var sessionBag = SessionContainer.UpdateSession(null, jsonResult.openid, jsonResult.session_key, jsonResult.unionid);

                    var userInfo = _userWeChatInfoServices.Exists(p => p.openid == jsonResult.openid);
                    if (userInfo == false)
                    {
                        var user = new CoreCmsUserWeChatInfo
                        {
                            openid = jsonResult.openid,
                            type = (int)GlobalEnumVars.UserAccountTypes.微信小程序,
                            sessionKey = sessionBag.SessionKey,
                            gender = 1,
                            createTime = DateTime.Now
                        };
                        var id = _userWeChatInfoServices.Insert(user);
                        if (id > 0)
                        {
                            _userWeChatInfoServices.Update(p => new CoreCmsUserWeChatInfo() { userId = id },
                                p => p.id == id);
                        }
                    }

                    //注意：生产环境下SessionKey属于敏感信息，不能进行传输！
                    //return Json(new { success = true, msg = "OK", sessionId = sessionBag.Key, sessionKey = sessionBag.SessionKey, data = jsonResult, sessionBag = sessionBag });
                    return Json(new { success = true, msg = "OK", sessionId = sessionBag.Key });
                }
                else
                {
                    return Json(new { success = false, msg = jsonResult.errmsg });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, msg = ex.Message });
            }

        }

        /// <summary>
        /// 校验签名
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="rawData"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CheckWxOpenSignature(string sessionId, string rawData, string signature)
        {
            try
            {
                var checkSuccess = Senparc.Weixin.WxOpen.Helpers.EncryptHelper.CheckSignature(sessionId, rawData, signature);
                return Json(new { success = checkSuccess, msg = checkSuccess ? "签名校验成功" : "签名校验失败" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, msg = ex.Message });
            }
        }

        /// <summary>
        /// 解码数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="sessionId"></param>
        /// <param name="encryptedData"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> DecodeEncryptedData(string type, string sessionId, string encryptedData, string iv)
        {
            DecodeEntityBase decodedEntity = null;
            CoreCmsUserWeChatInfo userInfo = null;
            try
            {
                switch (type.ToUpper())
                {
                    case "USERINFO"://wx.getUserInfo()
                        decodedEntity = EncryptHelper.DecodeUserInfoBySessionId(sessionId, encryptedData, iv);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                WeixinTrace.SendCustomLog("EncryptHelper.DecodeUserInfoBySessionId 方法出错",
                    $@"sessionId: {sessionId}encryptedData: {encryptedData}iv: {iv}sessionKey: { (await SessionContainer.CheckRegisteredAsync(sessionId)
                ? (await SessionContainer.GetSessionAsync(sessionId)).SessionKey
                : "未保存sessionId")}异常信息：{ex}");
            }

            //检验水印
            var checkWatermark = false;
            if (decodedEntity != null)
            {
                checkWatermark = decodedEntity.CheckWatermark(WxOpenAppId);

                //保存用户信息（可选）
                if (checkWatermark && decodedEntity is DecodedUserInfo decodedUserInfo)
                {
                    var sessionBag = await SessionContainer.GetSessionAsync(sessionId);
                    if (sessionBag != null)
                    {
                        await SessionContainer.AddDecodedUserInfoAsync(sessionBag, decodedUserInfo);
                    }
                    //更新数据库讯息
                    userInfo = _userWeChatInfoServices.QueryByClause(p => p.openid == decodedUserInfo.openId);
                    if (userInfo == null)
                    {
                        userInfo = new CoreCmsUserWeChatInfo
                        {
                            type = (int)GlobalEnumVars.UserAccountTypes.微信小程序,
                            openid = decodedUserInfo.openId,
                            sessionKey = sessionBag.SessionKey,
                            unionId = decodedUserInfo.unionId,
                            avatar = decodedUserInfo.avatarUrl,
                            nickName = decodedUserInfo.nickName,
                            gender = decodedUserInfo.gender,
                            language = "",
                            city = decodedUserInfo.city,
                            province = decodedUserInfo.province,
                            country = decodedUserInfo.country,
                            mobile = "",
                            createTime = DateTime.Now
                        };
                        var id = _userWeChatInfoServices.Insert(userInfo);
                        if (id > 0)
                        {
                            userInfo.id = id;
                            _userWeChatInfoServices.Update(p => new CoreCmsUserWeChatInfo() { userId = id }, p => p.id == id);
                        }
                    }
                    else
                    {
                        userInfo.gender = decodedUserInfo.gender;
                        userInfo.city = decodedUserInfo.city;
                        userInfo.avatar = decodedUserInfo.avatarUrl;
                        userInfo.country = decodedUserInfo.country;
                        userInfo.nickName = decodedUserInfo.nickName;
                        userInfo.province = decodedUserInfo.province;
                        userInfo.unionId = decodedUserInfo.unionId;
                        userInfo.gender = decodedUserInfo.gender;
                        _userWeChatInfoServices.Update(userInfo);
                    }
                }
            }

            //注意：此处仅为演示，敏感信息请勿传递到客户端！
            return Json(new
            {
                success = checkWatermark,
                userInfo,
                msg = $"水印验证：{(checkWatermark ? "通过" : "不通过")}"
            });
        }


        /// <summary>
        /// 解密电话号码
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="encryptedData"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DecryptPhoneNumber(string sessionId, string encryptedData, string iv)
        {
            SessionContainer.GetSession(sessionId);
            try
            {
                var phoneNumber = Senparc.Weixin.WxOpen.Helpers.EncryptHelper.DecryptPhoneNumber(sessionId, encryptedData, iv);

                //throw new WeixinException("解密PhoneNumber异常测试");//启用这一句，查看客户端返回的异常信息

                return Json(new { success = true, phoneNumber });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, msg = ex.Message });
            }
        }

        /// <summary>
        /// 解密运动步数
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="encryptedData"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        [HttpPost]

        public ActionResult DecryptRunData(string sessionId, string encryptedData, string iv)
        {
            SessionContainer.GetSession(sessionId);
            try
            {
                var runData = Senparc.Weixin.WxOpen.Helpers.EncryptHelper.DecryptRunData(sessionId, encryptedData, iv);

                //throw new WeixinException("解密PhoneNumber异常测试");//启用这一句，查看客户端返回的异常信息

                return Json(new { success = true, runData });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, msg = ex.Message });

            }
        }

        /// <summary>
        /// 获取二维码
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="useBase64"></param>
        /// <param name="codeType"></param>
        /// <returns></returns>
        [HttpPost]

        public async Task<IActionResult> GetQrCode(string sessionId, string useBase64, string codeType = "1")
        {
            var sessionBag = SessionContainer.GetSession(sessionId);
            if (sessionBag == null)
            {
                return Json(new { success = false, msg = "请先登录！" });
            }

            var ms = new MemoryStream();
            var openId = sessionBag.OpenId;
            var page = "pages/QrCode/QrCode";//此接口不可以带参数，如果需要加参数，必须加到scene中
            var scene = $"OpenIdSuffix:{openId.Substring(openId.Length - 10, 10)}#{codeType}";//储存OpenId后缀，以及codeType。scene最多允许32个字符
            LineColor lineColor = null;//线条颜色
            if (codeType == "2")
            {
                lineColor = new LineColor(221, 51, 238);
            }

            await Senparc.Weixin.WxOpen.AdvancedAPIs.WxApp.WxAppApi.GetWxaCodeUnlimitAsync(WxOpenAppId, ms, scene, page, lineColor: lineColor);
            ms.Position = 0;

            if (!useBase64.IsNullOrEmpty())
            {
                //转base64
                var imgBase64 = Convert.ToBase64String(ms.GetBuffer());
                return Json(new { success = true, msg = imgBase64, page });
            }
            else
            {
                //返回文件流
                return File(ms, "image/jpeg");
            }
        }

        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="templateId"></param>
        /// <returns></returns>
        [HttpPost]

        public async Task<IActionResult> SubscribeMessage(string sessionId, string templateId = "xWclWkOqDrxEgWF4DExmb9yUe10pfmSSt2KM6pY7ZlU")
        {
            var sessionBag = SessionContainer.GetSession(sessionId);
            if (sessionBag == null)
            {
                return Json(new { success = false, msg = "请先登录！" });
            }

            await System.Threading.Tasks.Task.Delay(1000);//停1秒钟，实际开发过程中可以将权限存入数据库，任意时间发送。

            var templateMessageData = new TemplateMessageData
            {
                ["thing1"] = new TemplateMessageDataValue("微信公众号+小程序快速开发"),
                ["time5"] = new TemplateMessageDataValue(SystemTime.Now.ToString("yyyy年MM月dd日 HH:mm")),
                ["thing6"] = new TemplateMessageDataValue("盛派网络研究院"),
                ["thing7"] = new TemplateMessageDataValue("第二部分课程正在准备中，尽情期待")
            };

            var page = "pages/index/index";
            //templateId也可以由后端指定

            try
            {
                var result = await Senparc.Weixin.WxOpen.AdvancedAPIs.MessageApi.SendSubscribeAsync(WxOpenAppId, sessionBag.OpenId, templateId, templateMessageData, page);
                if (result.errcode == ReturnCode.请求成功)
                {
                    return Json(new { success = true, msg = "消息已发送，请注意查收" });
                }
                else
                {
                    return Json(new { success = false, msg = result.errmsg });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, msg = ex.Message });
            }
        }


    }
}