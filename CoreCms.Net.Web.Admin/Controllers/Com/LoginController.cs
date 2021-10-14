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
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Threading.Tasks;
using CoreCms.Net.Auth.OverWrite;
using CoreCms.Net.Auth.Policys;
using CoreCms.Net.Configuration;
using CoreCms.Net.IRepository;
using CoreCms.Net.IServices;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.FromBody;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Utility.Extensions;
using CoreCms.Net.Utility.Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace CoreCms.Net.Web.Admin.Controllers
{
    /// <summary>
    /// 用户授权登录
    /// </summary>
    [Route("api/[controller]/[action]")]
    [AllowAnonymous]
    public class LoginController : ControllerBase
    {
        private readonly PermissionRequirement _permissionRequirement;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly ISysUserServices _sysUserServices;
        private readonly ISysRoleMenuServices _sysRoleMenuServices;
        private readonly ISysLoginRecordRepository _sysLoginRecordRepository;


        /// <summary>
        /// 构造函数注入
        /// </summary>
        public LoginController(
            PermissionRequirement permissionRequirement
            , ISysUserServices sysUserServices
            , ISysRoleMenuServices sysRoleMenuServices
            , IHttpContextAccessor httpContextAccessor
            , ISysLoginRecordRepository sysLoginRecordRepository
            )
        {
            _permissionRequirement = permissionRequirement;
            _sysUserServices = sysUserServices;
            _sysRoleMenuServices = sysRoleMenuServices;
            _httpContextAccessor = httpContextAccessor;
            _sysLoginRecordRepository = sysLoginRecordRepository;
        }

        /// <summary>
        /// 获取JWT的授权
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<AdminUiCallBack> GetJwtToken([FromBody] FMLogin model)
        {
            var jm = new AdminUiCallBack();

            if (string.IsNullOrEmpty(model.userName) || string.IsNullOrEmpty(model.password))
            {
                jm.msg = "用户名或密码不能为空";
                return jm;
            }

            model.password = CommonHelper.Md5For32(model.password);

            var user = await _sysUserServices.QueryByClauseAsync(p => p.userName == model.userName && p.passWord == model.password);
            if (user != null)
            {
                if (user.state == 1)
                {
                    jm.msg = "您的账户已经被冻结,请联系管理员解锁";
                    return jm;
                }
                var userRoles = await _sysUserServices.GetUserRoleNameStr(model.userName, model.password);
                //如果是基于用户的授权策略，这里要添加用户;如果是基于角色的授权策略，这里要添加角色
                var claims = new List<Claim> {
                        new Claim(ClaimTypes.GivenName, user.nickName),
                        new Claim(ClaimTypes.Name, user.userName),
                        new Claim(JwtRegisteredClaimNames.Jti, user.id.ToString()),
                        new Claim(ClaimTypes.Expiration, DateTime.Now.AddSeconds(_permissionRequirement.Expiration.TotalSeconds).ToString()) };
                claims.AddRange(userRoles.Split(',').Select(s => new Claim(ClaimTypes.Role, s)));

                // ids4和jwt切换
                // jwt
                if (!Permissions.IsUseIds4)
                {
                    var data = await _sysRoleMenuServices.RoleModuleMaps();
                    var list = (from item in data
                                orderby item.id
                                select new PermissionItem
                                {
                                    Url = item.menu?.component,
                                    RouteUrl = item.menu?.path,
                                    Authority = item.menu?.authority,
                                    Role = item.role?.roleCode,
                                }).ToList();

                    _permissionRequirement.Permissions = list;
                }

                //用户标识
                var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);
                identity.AddClaims(claims);

                var token = JwtToken.BuildJwtToken(claims.ToArray(), _permissionRequirement);

                jm.code = 0;
                jm.msg = "认证成功";
                jm.data = new
                {
                    token,
                    loginUrl = "Panel.html"
                };

                //插入登录日志
                var log = new SysLoginRecord();
                log.username = model.userName;
                log.ip = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress != null ?
                    _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString() : "127.0.0.1";

                log.os = RuntimeInformation.OSDescription;
                if (_httpContextAccessor.HttpContext != null)
                    log.browser = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.UserAgent];
                log.operType = (int)GlobalEnumVars.LoginRecordType.登录成功;
                log.createTime = DateTime.Now;
                await _sysLoginRecordRepository.InsertAsync(log);

                return jm;
            }
            else
            {
                //插入登录日志
                var log = new SysLoginRecord();
                log.username = model.userName;
                log.ip = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress != null ?
                    _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString() : "127.0.0.1";

                log.os = RuntimeInformation.OSDescription;
                if (_httpContextAccessor.HttpContext != null)
                    log.browser = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.UserAgent];
                log.operType = (int)GlobalEnumVars.LoginRecordType.登录失败;
                log.createTime = DateTime.Now;
                await _sysLoginRecordRepository.InsertAsync(log);

                jm.msg = "账户密码错误";
                return jm;
            }

        }

        /// <summary>
        /// 请求刷新Token（以旧换新）
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("RefreshToken")]
        public async Task<AdminUiCallBack> RefreshToken(string token = "")
        {
            var jm = new AdminUiCallBack();
            if (string.IsNullOrEmpty(token))
            {
                jm.code = 1001;
                jm.msg = "token无效，请重新登录！";
                return jm;
            }
            var tokenModel = JwtHelper.SerializeJwt(token);
            if (tokenModel != null && tokenModel.Uid > 0)
            {
                var user = await _sysUserServices.QueryByIdAsync(tokenModel.Uid);
                if (user != null)
                {
                    var userRoles = await _sysUserServices.GetUserRoleNameStr(user.userName, user.passWord);
                    //如果是基于用户的授权策略，这里要添加用户;如果是基于角色的授权策略，这里要添加角色
                    var claims = new List<Claim> {
                        new Claim(ClaimTypes.Name, user.userName),
                        new Claim(JwtRegisteredClaimNames.Jti, tokenModel.Uid.ObjectToString()),
                        new Claim(ClaimTypes.Expiration, DateTime.Now.AddSeconds(_permissionRequirement.Expiration.TotalSeconds).ToString()) };
                    claims.AddRange(userRoles.Split(',').Select(s => new Claim(ClaimTypes.Role, s)));

                    //用户标识
                    var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);
                    identity.AddClaims(claims);

                    var refreshToken = JwtToken.BuildJwtToken(claims.ToArray(), _permissionRequirement);
                    jm.code = 0;
                    jm.msg = "认证成功";
                    jm.data = refreshToken;


                    //插入登录日志
                    var log = new SysLoginRecord();
                    log.username = user.userName;
                    if (_httpContextAccessor.HttpContext != null)
                    {
                        if (_httpContextAccessor.HttpContext.Connection.RemoteIpAddress != null)
                        {
                            log.ip = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                        }
                        log.os = RuntimeInformation.OSDescription;
                        log.browser = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.UserAgent];
                    }

                    log.operType = (int)GlobalEnumVars.LoginRecordType.刷新Token;
                    log.createTime = DateTime.Now;
                    await _sysLoginRecordRepository.InsertAsync(log);

                    return jm;
                }
            }
            jm.code = 1001;
            jm.msg = "token无效，请重新登录！";
            return jm;
        }

    }
}
