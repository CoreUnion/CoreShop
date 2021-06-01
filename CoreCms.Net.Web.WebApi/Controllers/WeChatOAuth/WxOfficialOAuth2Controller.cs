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
using System.Text;
using CoreCms.Net.Configuration;
using CoreCms.Net.IServices;
using CoreCms.Net.Model.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Senparc.CO2NET.Extensions;
using Senparc.CO2NET.Trace;
using Senparc.Weixin.Exceptions;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;

namespace CoreCms.Net.Web.WebApi.Controllers.WeChatOAuth
{
    /// <summary>
    /// 微信公众号拉取授权
    /// </summary>
    [Route("[controller]/[action]")]
    public class WxOfficialOAuth2Controller : Controller
    {
        //下面换成账号对应的信息，也可以放入web.config等地方方便配置和更换
        /// <summary>
        /// appid与微信公众账号后台的AppId设置保持一致，区分大小写。
        /// </summary>
        private readonly string appId = Senparc.Weixin.Config.SenparcWeixinSetting.WeixinAppId;
        /// <summary>
        /// 与微信公众账号后台的AppId设置保持一致，区分大小写。
        /// </summary>
        private readonly string appSecret = Senparc.Weixin.Config.SenparcWeixinSetting.WeixinAppSecret;


        private ICoreCmsUserWeChatInfoServices _userWeChatInfoServices;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userWeChatInfoServices"></param>
        public WxOfficialOAuth2Controller(ICoreCmsUserWeChatInfoServices userWeChatInfoServices)
        {
            _userWeChatInfoServices = userWeChatInfoServices;
        }

        /// <summary>
        /// OAuthScope.snsapi_userinfo方式回调
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <param name="returnUrl">用户最初尝试进入的页面</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult UserInfoCallback(string code, string state, string returnUrl)
        {
            if (string.IsNullOrEmpty(code))
            {
                return Content("您拒绝了授权！");
            }
            if (!state.StartsWith("FromSenparc") || !state.Contains("|") || state.Length != 30)
            {
                //这里的state其实是会暴露给客户端的，验证能力很弱，这里只是演示一下，
                //建议用完之后就清空，将其一次性使用
                //实际上可以存任何想传递的数据，比如用户ID，并且需要结合例如下面的Session["OAuthAccessToken"]进行验证
                return Content("验证失败！请从正规途径进入！");
            }
            OAuthAccessTokenResult result = null;
            //通过，用code换取access_token
            try
            {
                result = OAuthApi.GetAccessToken(appId, appSecret, code);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
            if (result.errcode != Senparc.Weixin.ReturnCode.请求成功)
            {
                return Content("错误：" + result.errmsg);
            }
            //下面2个数据也可以自己封装成一个类，储存在数据库中（建议结合缓存）
            //如果可以确保安全，可以将access_token存入用户的cookie中，每一个人的access_token是不一样的
            HttpContext.Session.SetString("OAuthAccessTokenStartTime", SystemTime.Now.ToString());
            HttpContext.Session.SetString("OAuthAccessToken", result.ToJson());


            //因为第一步选择的是OAuthScope.snsapi_userinfo，这里可以进一步获取用户详细信息
            try
            {
                OAuthUserInfo userInfo = OAuthApi.GetUserInfo(result.access_token, result.openid);
                var weChatUserInfo = _userWeChatInfoServices.QueryByClause(p => p.openid == userInfo.openid);
                if (weChatUserInfo == null)
                {
                    weChatUserInfo = new CoreCmsUserWeChatInfo();
                    weChatUserInfo.type = (int)GlobalEnumVars.UserAccountTypes.微信公众号;
                    weChatUserInfo.openid = userInfo.openid;
                    weChatUserInfo.sessionKey = result.access_token;
                    weChatUserInfo.unionId = userInfo.unionid;
                    weChatUserInfo.avatar = userInfo.headimgurl;
                    weChatUserInfo.nickName = userInfo.nickname;
                    weChatUserInfo.gender = userInfo.sex;
                    weChatUserInfo.language = "";
                    weChatUserInfo.city = userInfo.city;
                    weChatUserInfo.province = userInfo.province;
                    weChatUserInfo.country = userInfo.country;
                    weChatUserInfo.mobile = "";
                    weChatUserInfo.createTime = DateTime.Now;
                    var id = _userWeChatInfoServices.Insert(weChatUserInfo);
                    if (id > 0)
                    {
                        weChatUserInfo.id = id;
                        _userWeChatInfoServices.Update(p => new CoreCmsUserWeChatInfo() { userId = id }, p => p.id == id);
                    }
                }
                else
                {
                    if (weChatUserInfo.country != userInfo.country || weChatUserInfo.province != userInfo.province || weChatUserInfo.nickName != userInfo.nickname || weChatUserInfo.avatar != userInfo.headimgurl || weChatUserInfo.unionId != userInfo.unionid || weChatUserInfo.gender != userInfo.sex || weChatUserInfo.city != userInfo.city)
                    {
                        weChatUserInfo.city = userInfo.city;
                        weChatUserInfo.country = userInfo.country;
                        weChatUserInfo.province = userInfo.province;
                        weChatUserInfo.nickName = userInfo.nickname;
                        weChatUserInfo.gender = userInfo.sex;
                        weChatUserInfo.avatar = userInfo.headimgurl;
                        weChatUserInfo.unionId = userInfo.unionid;
                        //weChatUserInfo.remark = bkurl;
                        _userWeChatInfoServices.Update(weChatUserInfo);
                    }
                }
                //缓存信息
                Response.Cookies.Append(GlobalConstVars.CookieOpenId, result.openid);
                HttpContext.Session.SetString(GlobalConstVars.SessionOpenId, result.openid);//进行登录
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return View(userInfo);
            }
            catch (ErrorJsonResultException ex)
            {
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// OAuthScope.snsapi_base方式回调
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <param name="returnUrl">用户最初尝试进入的页面</param>
        /// <returns></returns>
        [HttpGet]

        public IActionResult BaseCallback(string code, string state, string returnUrl)
        {
            try
            {
                if (string.IsNullOrEmpty(code))
                {
                    return Content("您拒绝了授权！");
                }

                if (state != HttpContext.Session.GetString("State"))
                {
                    //这里的state其实是会暴露给客户端的，验证能力很弱，这里只是演示一下，
                    //建议用完之后就清空，将其一次性使用
                    //实际上可以存任何想传递的数据，比如用户ID，并且需要结合例如下面的Session["OAuthAccessToken"]进行验证
                    return Content("验证失败！请从正规途径进入！");
                }

                //通过，用code换取access_token
                var result = OAuthApi.GetAccessToken(appId, appSecret, code);
                if (result.errcode != Senparc.Weixin.ReturnCode.请求成功)
                {
                    return Content("错误：" + result.errmsg);
                }

                //下面2个数据也可以自己封装成一个类，储存在数据库中（建议结合缓存）
                //如果可以确保安全，可以将access_token存入用户的cookie中，每一个人的access_token是不一样的
                HttpContext.Session.SetString("OAuthAccessTokenStartTime", SystemTime.Now.ToString());
                HttpContext.Session.SetString("OAuthAccessToken", result.ToJson());

                //因为这里还不确定用户是否关注本微信，所以只能试探性地获取一下
                OAuthUserInfo userInfo = null;
                try
                {
                    //已关注，可以得到详细信息
                    userInfo = OAuthApi.GetUserInfo(result.access_token, result.openid);

                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }


                    ViewData["ByBase"] = true;
                    return View("UserInfoCallback", userInfo);
                }
                catch (ErrorJsonResultException ex)
                {
                    SenparcTrace.SendCustomLog("获取用户讯息失败", ex.ToString());
                    //未关注，只能授权，无法得到详细信息
                    //这里的 ex.JsonResult 可能为："{\"errcode\":40003,\"errmsg\":\"invalid openid\"}"
                    return Content("用户已授权，授权Token：" + result, "text/html", Encoding.UTF8);
                }
            }
            catch (Exception ex)
            {
                SenparcTrace.SendCustomLog("BaseCallback 发生错误", ex.ToString());
                return Content("发生错误：" + ex.ToString());
            }
        }
    }
}