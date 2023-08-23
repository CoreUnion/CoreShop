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
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using CoreCms.Net.Auth.Policys;
using CoreCms.Net.Configuration;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.IServices;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.FromBody;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Model.ViewModels.DTO;
using CoreCms.Net.Utility.Extensions;
using CoreCms.Net.Utility.Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using SqlSugar;


namespace CoreCms.Net.Services
{
    /// <summary>
    /// 用户表 接口实现
    /// </summary>
    public class CoreCmsUserServices : BaseServices<CoreCmsUser>, ICoreCmsUserServices
    {
        private readonly ICoreCmsUserRepository _dal;
        private readonly ICoreCmsUserBalanceServices _userBalanceServices;
        private readonly ICoreCmsSettingServices _settingServices;
        private readonly ICoreCmsUserPointLogServices _userPointLogServices;
        private readonly ICoreCmsSmsServices _smsServices;
        private readonly ICoreCmsUserWeChatInfoServices _userWeChatInfoServices;
        private readonly ICoreCmsUserGradeServices _userGradeServices;
        private readonly ICoreCmsUserLogServices _userLogServices;

        private readonly IUnitOfWork _unitOfWork;
        private readonly PermissionRequirement _permissionRequirement;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public CoreCmsUserServices(IUnitOfWork unitOfWork
            , ICoreCmsUserRepository dal
            , ICoreCmsUserBalanceServices userBalanceServices
            , ICoreCmsSettingServices settingServices
            , ICoreCmsUserPointLogServices userPointLogServices, ICoreCmsSmsServices smsServices, ICoreCmsUserWeChatInfoServices userWeChatInfoServices, ICoreCmsUserGradeServices userGradeServices, PermissionRequirement permissionRequirement, IHttpContextAccessor httpContextAccessor, ICoreCmsUserLogServices userLogServices)
        {
            this._dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
            _userBalanceServices = userBalanceServices;
            _settingServices = settingServices;
            _userPointLogServices = userPointLogServices;
            _smsServices = smsServices;
            _userWeChatInfoServices = userWeChatInfoServices;
            _userGradeServices = userGradeServices;
            _permissionRequirement = permissionRequirement;
            _httpContextAccessor = httpContextAccessor;
            _userLogServices = userLogServices;
        }


        #region 更新余额

        /// <summary>
        /// 更新余额
        /// </summary>
        /// <param name="id"></param>
        /// <param name="money"></param>
        /// <returns></returns>
        public async Task<AdminUiCallBack> UpdateBalance(int id, decimal money)
        {
            var jm = new AdminUiCallBack();

            var model = await _dal.QueryByIdAsync(id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            var newMoney = model.balance + money;
            var up = await _dal.UpdateAsync(p => new CoreCmsUser() { balance = newMoney }, p => p.id == id);
            if (up)
            {
                var balance = new CoreCmsUserBalance();
                balance.type = (int)GlobalEnumVars.UserBalanceSourceTypes.Admin;
                balance.userId = model.id;
                balance.balance = newMoney;
                balance.createTime = DateTime.Now;
                balance.memo = UserHelper.GetMemo(balance.type, money);
                balance.money = money;
                balance.sourceId = GlobalEnumVars.UserBalanceSourceTypes.Admin.ToString();

                jm.code = await _userBalanceServices.InsertAsync(balance) > 0 ? 0 : 1;
            }

            jm.msg = jm.code == 0 ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return jm;
        }

        #endregion

        #region 更新积分
        /// <summary>
        /// 更新积分
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<AdminUiCallBack> UpdatePiont(FMUpdateUserPoint entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _dal.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            var newPoint = model.point + entity.point;
            var up = await _dal.UpdateAsync(p => new CoreCmsUser() { point = newPoint }, p => p.id == entity.id);
            if (up)
            {
                var point = new CoreCmsUserPointLog();
                point.userId = model.id;
                point.type = (int)GlobalEnumVars.UserPointSourceTypes.PointTypeAdminEdit;
                point.num = entity.point;
                point.balance = newPoint;
                point.remarks = entity.memo;
                point.createTime = DateTime.Now;

                jm.code = await _userPointLogServices.InsertAsync(point) > 0 ? 0 : 1;
            }

            jm.msg = jm.code == 0 ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;


            return jm;
        }
        #endregion

        #region 获取用户的积分

        /// <summary>
        /// 获取用户的积分
        /// </summary>
        /// <param name="userId">用户序列</param>
        /// <param name="orderMoney">订单金额</param>
        /// <returns></returns>
        public async Task<GetUserPointResult> GetUserPoint(int userId, decimal orderMoney)
        {
            GetUserPointResult dto = new GetUserPointResult();
            //1是2否
            var allConfigs = await _settingServices.GetConfigDictionaries();

            var pointSwitch = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.PointSwitch).ObjectToInt();    //是否开启积分功能
            if (pointSwitch == 2)
            {
                dto.@switch = 2;
                return dto;
            }
            var user = await _dal.QueryByClauseAsync(p => p.id == userId);
            if (user != null)
            {
                dto.point = user.point;
                if (orderMoney != 0)
                {
                    //计算可用积分
                    var ordersPointProportion = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.OrdersPointProportion).ObjectToInt(10);//订单积分使用比例
                    //最多可以抵扣的金额
                    var maxPointDeductedMoney = Math.Round((orderMoney * ordersPointProportion) / 100, 2);
                    //订单积分折现比例(多少积分可以折现1块钱)
                    var pointDiscountedProportion = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.PointDiscountedProportion).ObjectToInt(100);
                    //计算需要多少积分
                    var needsPoint = maxPointDeductedMoney * pointDiscountedProportion;
                    //确定是否有那么多积分去抵扣比例计算出的能抵扣的钱
                    dto.availablePoint = Convert.ToInt32(needsPoint > user.point ? user.point : needsPoint);
                    dto.pointExchangeMoney = dto.availablePoint / pointDiscountedProportion;
                }
            }
            return dto;
        }
        #endregion

        #region 修改用户密码，如果用户之前没有密码，那么就不校验原密码
        /// <summary>
        ///   修改用户密码，如果用户之前没有密码，那么就不校验原密码
        /// </summary>
        public async Task<WebApiCallBack> ChangePassword(int userId, string newPwd, string password = "")
        {
            var jm = new WebApiCallBack();

            //修改密码验证原密码

            var user = await _dal.QueryByIdAsync(userId);
            if (user == null)
            {
                jm.msg = GlobalErrorCodeVars.Code10000;
                return jm;
            }

            if (!string.IsNullOrEmpty(user.passWord))
            {
                if (string.IsNullOrEmpty(password))
                {
                    jm.msg = "请输入原密码!";
                    return jm;
                }

                if (user.passWord != CommonHelper.EnPassword(password, user.createTime))
                {
                    jm.msg = "原密码不正确!";
                    return jm;
                }
            }

            if (string.IsNullOrEmpty(newPwd) || newPwd.Length < 6 || newPwd.Length > 16)
            {
                jm.msg = GlobalErrorCodeVars.Code11009;
                return jm;
            }

            var md5Pwd = CommonHelper.EnPassword(newPwd, user.createTime);

            if (!string.IsNullOrEmpty(user.passWord) && user.passWord == md5Pwd)
            {
                jm.msg = "原密码和新密码一样!";
                return jm;
            }

            var bl = await _dal.UpdateAsync(p => new CoreCmsUser() { passWord = md5Pwd, updataTime = DateTime.Now }, p => p.id == userId);
            jm.status = bl;
            jm.msg = bl ? "密码修改成功!" : "密码修改失败!";
            return jm;
        }
        #endregion

        #region 绑定上级
        /// <summary>
        /// 绑定上级
        /// </summary>
        /// <param name="superiorId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> SetMyInvite(int superiorId, int userId)
        {
            var jm = new WebApiCallBack() { msg = "填写邀请码失败" };

            jm.otherData = superiorId;

            //自己不能邀请自己
            if (userId == superiorId)
            {
                jm.msg = "自己不能邀请自己";
                return jm;
            }
            var user = await _dal.QueryByIdAsync(userId);
            if (user == null)
            {
                jm.msg = GlobalErrorCodeVars.Code10000;
                return jm;
            }
            if (user.parentId != 0)
            {
                jm.msg = "已有上级邀请，不能绑定其他的邀请";
                return jm;
            }

            var superior = await _dal.QueryByIdAsync(superiorId);
            if (superior == null)
            {
                jm.msg = "不存在这个邀请码";
                return jm;
            }

            if (superior.parentId == user.id)
            {
                jm.msg = "不允许双方互相设置为上级";
                return jm;
            }

            var flag = IsInvited(userId, superiorId);
            if (flag)
            {
                jm.msg = "不允许填写下级的邀请码";
                return jm;
            }

            var bl = await _dal.UpdateAsync(p => new CoreCmsUser() { parentId = superiorId }, p => p.id == userId);
            jm.status = bl;
            jm.msg = bl ? "填写邀请码成功!" : "填写邀请码失败!";
            return jm;
        }
        #endregion

        #region 获取我的上级邀请人
        /// <summary>
        /// 获取我的上级邀请人
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> GetMyInvite(int userId)
        {
            var jm = new WebApiCallBack() { status = true };

            var user = await _dal.QueryByIdAsync(userId);
            if (user == null)
            {
                jm.msg = GlobalErrorCodeVars.Code10000;
                return jm;
            }
            if (user.parentId == 0)
            {
                jm.msg = "无上级邀请人";
                return jm;
            }
            var parentUser = await _dal.QueryByClauseAsync(p => p.id == user.parentId);
            if (parentUser != null)
            {
                jm.data = new
                {
                    nickname = parentUser.nickName,
                    avatar = parentUser.avatarImage,
                    mobile = UserHelper.FormatMobile(parentUser.mobile),
                    ctime = parentUser.createTime
                };
            }
            return jm;
        }


        /// <summary>
        ///     获取下级推广用户数量
        /// </summary>
        /// <param name="parentId">父类序列</param>
        /// <param name="type">1获取1级，其他为2级</param>
        /// <param name="thisMonth">当月</param>
        /// <returns></returns>
        public async Task<int> QueryChildCountAsync(int parentId, int type = 1, bool thisMonth = false)
        {
            return await _dal.QueryChildCountAsync(parentId, type, thisMonth);
        }

        /// <summary>
        /// 判断userId是否是pid的父节点或者祖父节点,如果是，就返回true，如果不是就返回false
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        private bool IsInvited(int userId, int pid)
        {
            var info = _dal.QueryById(pid);
            if (info == null || info.parentId == 0)
            {
                return false;
            }
            else
            {
                if (info.parentId == userId)
                {
                    return true;
                }
                else
                {
                    return IsInvited(userId, info.parentId);
                }
            }
        }
        #endregion

        #region  忘记密码，找回密码
        /// <summary>
        /// 忘记密码，找回密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newPwd"></param>
        /// <param name="cTime"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> ForgetPassword(string mobile, string code, string newPwd)
        {
            var jm = new WebApiCallBack();

            if (string.IsNullOrEmpty(code))
            {
                jm.msg = GlobalErrorCodeVars.Code10013;
                return jm;
            }
            var smsBool = await _smsServices.Check(mobile, code, "veri");
            if (!smsBool)
            {
                jm.msg = GlobalErrorCodeVars.Code10012;
                return jm;
            }
            var userInfo = await _dal.QueryByClauseAsync(p => p.mobile == mobile);
            if (userInfo == null)
            {
                jm.msg = "没有此手机号码";
                return jm;
            }
            return await EditPwd(userInfo.id, newPwd, userInfo.createTime);
        }

        #endregion

        #region 修改密码
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userId">用户序列</param>
        /// <param name="newPwd">新密码</param>
        /// <param name="createTime">创建时间</param>
        /// <returns></returns>
        private async Task<WebApiCallBack> EditPwd(int userId, string newPwd, DateTime createTime)
        {
            var jm = new WebApiCallBack();

            if (string.IsNullOrEmpty(newPwd) || newPwd.Length < 6 || newPwd.Length > 16)
            {
                jm.msg = GlobalErrorCodeVars.Code11009;
                return jm;
            }
            var md5Pwd = CommonHelper.EnPassword(newPwd, createTime);
            var up = await _dal.UpdateAsync(p => new CoreCmsUser() { passWord = md5Pwd }, p => p.id == userId);
            if (!up)
            {
                jm.status = false;
                jm.msg = "密码修改失败";
                return jm;
            }
            jm.status = true;
            jm.msg = "密码修改成功";
            return jm;
        }
        #endregion



        #region 手机短信验证码登陆，同时兼有手机短信注册的功能，还有第三方账户绑定的功能

        /// <summary>
        /// 手机短信验证码登陆，同时兼有手机短信注册的功能，还有第三方账户绑定的功能
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <param name="loginType">登录方式(1普通,2短信,3微信小程序拉取手机号)</param>
        /// <param name="platform"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> SmsLogin(FMWxAccountCreate entity, int loginType = (int)GlobalEnumVars.LoginType.WeChatPhoneNumber, int platform = 1)
        {
            var jm = new WebApiCallBack();

            if (string.IsNullOrEmpty(entity.mobile))
            {
                jm.msg = "请输入手机号码";
                return jm;
            }

            if (!CommonHelper.IsMobile(entity.mobile))
            {
                jm.msg = "请输入合法的手机号码";
                return jm;
            }

            if (loginType == (int)GlobalEnumVars.LoginType.Sms)
            {
                if (string.IsNullOrEmpty(entity.code))
                {
                    jm.msg = "请输入验证码";
                    return jm;
                }

                if (!await _smsServices.Check(entity.mobile, entity.code, "login"))
                {
                    jm.msg = "短信验证码错误";
                    return jm;
                }
            }

            var isReg = false;
            var userInfo = await _dal.QueryByClauseAsync(p => p.mobile == entity.mobile);
            if (userInfo == null)
            {
                isReg = true;
                userInfo = new CoreCmsUser();
                userInfo.userName = entity.mobile;
                userInfo.mobile = entity.mobile;
                userInfo.sex = 3;
                userInfo.isDelete = false;
                userInfo.balance = 0;
                userInfo.point = 0;
                userInfo.userWx = 0;
                userInfo.status = (int)GlobalEnumVars.UserStatus.正常;
                userInfo.createTime = DateTime.Now;

                //没有此用户，创建此用户
                if (!string.IsNullOrEmpty(entity.sessionAuthId))
                {
                    var wxUserInfo = await _userWeChatInfoServices.QueryByClauseAsync(p => p.openid == entity.sessionAuthId);
                    if (wxUserInfo != null)
                    {
                        if (string.IsNullOrEmpty(entity.avatar))
                        {
                            entity.avatar = wxUserInfo.avatar;
                        }
                        if (string.IsNullOrEmpty(entity.nickname))
                        {
                            entity.nickname = wxUserInfo.nickName;
                        }
                        userInfo.sex = wxUserInfo?.gender ?? 3;
                        userInfo.userWx = wxUserInfo?.id ?? 0;
                    }
                }
                //如果没有头像和昵称，那么就取系统头像和昵称吧
                if (!string.IsNullOrEmpty(entity.avatar))
                {
                    userInfo.avatarImage = entity.avatar;
                }
                else
                {
                    var allConfigs = await _settingServices.GetConfigDictionaries();
                    var defaultImage = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.ShopDefaultImage);
                    userInfo.avatarImage = defaultImage;
                }

                userInfo.nickName = !string.IsNullOrEmpty(entity.nickname) ? entity.nickname : UserHelper.FormatMobile(entity.mobile);

                if (entity.invitecode > 0)
                {
                    var pid = UserHelper.GetUserIdByShareCode(entity.invitecode);
                    var pInfo = await _dal.QueryByClauseAsync(p => p.id == pid);
                    if (pInfo != null)
                    {
                        userInfo.parentId = pid;
                    }
                    else
                    {
                        jm.msg = GlobalErrorCodeVars.Code10014;
                        return jm;
                    }
                }

                if (!string.IsNullOrEmpty(entity.password))
                {
                    //判断密码是否符合要求
                    if (entity.password.Length < 5 || entity.password.Length > 16)
                    {
                        jm.msg = GlobalErrorCodeVars.Code11009;
                        return jm;
                    }
                    userInfo.passWord = CommonHelper.EnPassword(entity.password, userInfo.createTime);
                }
                else
                {
                    userInfo.passWord = "";
                }

                //取默认的用户等级
                var userGradeInfo = await _userGradeServices.QueryByClauseAsync(p => p.isDefault == true);
                userInfo.grade = userGradeInfo?.id ?? 0;

                var userId = await _dal.InsertAsync(userInfo);
                if (userId == 0)
                {
                    jm.msg = GlobalErrorCodeVars.Code10000;
                    return jm;
                }
                userInfo = await _dal.QueryByIdAsync(userId);
            }
            else
            {
                //如果有这个账号的话，判断一下是不是传密码了，如果传密码了，就是注册，这里就有问题，因为已经注册过
                if (!string.IsNullOrEmpty(entity.password))
                {
                    jm.msg = GlobalErrorCodeVars.Code11019;
                    return jm;
                }
            }
            //判断是否是小程序里的微信登陆，如果是，就给他绑定微信账号
            if (!string.IsNullOrEmpty(entity.sessionAuthId))
            {
                var updateAsync = await _userWeChatInfoServices.UpdateAsync(p => new CoreCmsUserWeChatInfo() { userId = userInfo.id }, p => p.openid == entity.sessionAuthId);
                if (updateAsync)
                {
                    //多个微信可能同时授权一个号码登录。
                    //如果已经存在微信用户(A)数据绑定了手机号码。
                    //使用新微信(B)登录，同时又授权此手机号码绑定。
                    //小程序内微信支付时候，因为登录的微信（B）与拉取手机号码绑定后获取到数据是（A）。
                    //会导致微信数据报错（）
                    await _userWeChatInfoServices.UpdateAsync(p => new CoreCmsUserWeChatInfo() { userId = 0 }, p => p.openid != entity.sessionAuthId && p.userId == userInfo.id);
                }
                //如果是别的未绑定微信用户进来，则反向直接关联。
                var wxUserInfo = await _userWeChatInfoServices.QueryByClauseAsync(p => p.openid == entity.sessionAuthId);
                if (wxUserInfo != null)
                {
                    await _dal.UpdateAsync(p => new CoreCmsUser() { userWx = wxUserInfo.id }, p => p.id == userInfo.id);
                }
            }

            if (userInfo.status == (int)GlobalEnumVars.UserStatus.正常)
            {
                var claims = new List<Claim> {
                        new Claim(ClaimTypes.Name, userInfo.nickName),
                        new Claim(JwtRegisteredClaimNames.Jti, userInfo.id.ToString()),
                        new Claim(ClaimTypes.Expiration, DateTime.Now.AddSeconds(_permissionRequirement.Expiration.TotalSeconds).ToString()) };
                //用户标识
                var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);
                identity.AddClaims(claims);
                jm.status = true;
                jm.msg = "注册成功";
                jm.data = JwtToken.BuildJwtToken(claims.ToArray(), _permissionRequirement);
                //录入登录日志
                var log = new CoreCmsUserLog();
                log.userId = userInfo.id;
                log.state = isReg ? (int)GlobalEnumVars.UserLogTypes.注册 : (int)GlobalEnumVars.UserLogTypes.登录;
                log.ip = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress != null ?
                    _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString() : "127.0.0.1";
                log.createTime = DateTime.Now;
                log.parameters = isReg ? GlobalEnumVars.UserLogTypes.注册.ToString() : GlobalEnumVars.UserLogTypes.登录.ToString();
                await _userLogServices.InsertAsync(log);
            }
            else
            {
                jm.msg = GlobalErrorCodeVars.Code11022;
                return jm;
            }
            return jm;
        }

        #endregion


        /// <summary>
        ///     根据条件查询分页数据
        /// </summary>
        /// <param name="predicate">判断集合</param>
        /// <param name="orderByType">排序方式</param>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <param name="orderByExpression"></param>
        /// <returns></returns>
        public async Task<IPageList<CoreCmsUser>> QueryPageAsync(Expression<Func<CoreCmsUser, bool>> predicate,
            Expression<Func<CoreCmsUser, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20)
        {
            return await _dal.QueryPageAsync(predicate, orderByExpression, orderByType, pageIndex, pageSize);
        }



        /// <summary>
        /// 按天统计新会员
        /// </summary>
        /// <returns></returns>
        public async Task<List<StatisticsOut>> Statistics(int day)
        {
            return await _dal.Statistics(day);
        }

        /// <summary>
        /// 按天统计当天下单活跃会员
        /// </summary>
        /// <returns></returns>
        public async Task<List<StatisticsOut>> StatisticsOrder(int day)
        {

            return await _dal.StatisticsOrder(day);
        }


    }
}
