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
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoreCms.Net.Configuration;
using CoreCms.Net.Filter;
using CoreCms.Net.IServices;
using CoreCms.Net.Loging;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.Entities.Expression;
using CoreCms.Net.Model.FromBody;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Utility.Extensions;
using CoreCms.Net.Utility.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace CoreCms.Net.Web.Admin.Controllers
{
    /// <summary>
    ///     用户表
    /// </summary>
    [Description("用户表")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize(Permissions.Name)]
    public class SysUserController : ControllerBase
    {
        private readonly ISysOrganizationServices _sysOrganizationServices;
        private readonly ISysRoleServices _sysRoleServices;
        private readonly ISysUserRoleServices _sysUserRoleServices;
        private readonly ISysUserServices _sysUserServices;
        private readonly IWebHostEnvironment _webHostEnvironment;

        /// <summary>
        ///     构造函数
        /// </summary>
        public SysUserController(IWebHostEnvironment webHostEnvironment
            , ISysUserServices sysUserServices
            , ISysRoleServices sysRoleServices
            , ISysUserRoleServices sysUserRoleServices
            , ISysOrganizationServices sysOrganizationServices
        )
        {
            _webHostEnvironment = webHostEnvironment;
            _sysUserServices = sysUserServices;
            _sysRoleServices = sysRoleServices;
            _sysUserRoleServices = sysUserRoleServices;
            _sysOrganizationServices = sysOrganizationServices;
        }

        #region 获取列表============================================================

        // POST: Api/SysUser/GetPageList
        /// <summary>
        ///     获取列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("获取列表")]
        public async Task<AdminUiCallBack> GetPageList()
        {
            var jm = new AdminUiCallBack();
            var pageCurrent = Request.Form["page"].FirstOrDefault().ObjectToInt(1);
            var pageSize = Request.Form["limit"].FirstOrDefault().ObjectToInt(30);
            var where = PredicateBuilder.True<SysUser>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();
            Expression<Func<SysUser, object>> orderEx;
            switch (orderField)
            {
                case "id":
                    orderEx = p => p.id;
                    break;
                case "userName":
                    orderEx = p => p.userName;
                    break;
                case "passWord":
                    orderEx = p => p.passWord;
                    break;
                case "nickName":
                    orderEx = p => p.nickName;
                    break;
                case "avatar":
                    orderEx = p => p.avatar;
                    break;
                case "sex":
                    orderEx = p => p.sex;
                    break;
                case "phone":
                    orderEx = p => p.phone;
                    break;
                case "email":
                    orderEx = p => p.email;
                    break;
                case "emailVerified":
                    orderEx = p => p.emailVerified;
                    break;
                case "trueName":
                    orderEx = p => p.trueName;
                    break;
                case "idCard":
                    orderEx = p => p.idCard;
                    break;
                case "birthday":
                    orderEx = p => p.birthday;
                    break;
                case "introduction":
                    orderEx = p => p.introduction;
                    break;
                case "organizationId":
                    orderEx = p => p.organizationId;
                    break;
                case "state":
                    orderEx = p => p.state;
                    break;
                case "deleted":
                    orderEx = p => p.deleted;
                    break;
                case "createTime":
                    orderEx = p => p.createTime;
                    break;
                case "updateTime":
                    orderEx = p => p.updateTime;
                    break;
                default:
                    orderEx = p => p.id;
                    break;
            }

            //设置排序方式
            var orderDirection = Request.Form["orderDirection"].FirstOrDefault();
            var orderBy = orderDirection switch
            {
                "asc" => OrderByType.Asc,
                "desc" => OrderByType.Desc,
                _ => OrderByType.Desc
            };
            //查询筛选

            //用户id int
            var id = Request.Form["id"].FirstOrDefault().ObjectToInt(0);
            if (id > 0) @where = @where.And(p => p.id == id);
            //账号 nvarchar
            var userName = Request.Form["userName"].FirstOrDefault();
            if (!string.IsNullOrEmpty(userName)) @where = @where.And(p => p.userName.Contains(userName));
            //密码 nvarchar
            var passWord = Request.Form["passWord"].FirstOrDefault();
            if (!string.IsNullOrEmpty(passWord)) @where = @where.And(p => p.passWord.Contains(passWord));
            //昵称 nvarchar
            var nickName = Request.Form["nickName"].FirstOrDefault();
            if (!string.IsNullOrEmpty(nickName)) @where = @where.And(p => p.nickName.Contains(nickName));
            //头像 nvarchar
            var avatar = Request.Form["avatar"].FirstOrDefault();
            if (!string.IsNullOrEmpty(avatar)) @where = @where.And(p => p.avatar.Contains(avatar));
            //性别 int
            var sex = Request.Form["sex"].FirstOrDefault().ObjectToInt(0);
            if (sex > 0) @where = @where.And(p => p.sex == sex);
            //手机号 nvarchar
            var phone = Request.Form["phone"].FirstOrDefault();
            if (!string.IsNullOrEmpty(phone)) @where = @where.And(p => p.phone.Contains(phone));
            //邮箱 nvarchar
            var email = Request.Form["email"].FirstOrDefault();
            if (!string.IsNullOrEmpty(email)) @where = @where.And(p => p.email.Contains(email));
            //邮箱是否验证 bit
            var emailVerified = Request.Form["emailVerified"].FirstOrDefault();
            if (!string.IsNullOrEmpty(emailVerified) && emailVerified.ToLowerInvariant() == "true")
                @where = @where.And(p => p.emailVerified);
            else if (!string.IsNullOrEmpty(emailVerified) && emailVerified.ToLowerInvariant() == "false")
                @where = @where.And(p => p.emailVerified == false);
            //真实姓名 nvarchar
            var trueName = Request.Form["trueName"].FirstOrDefault();
            if (!string.IsNullOrEmpty(trueName)) @where = @where.And(p => p.trueName.Contains(trueName));
            //身份证号 nvarchar
            var idCard = Request.Form["idCard"].FirstOrDefault();
            if (!string.IsNullOrEmpty(idCard)) @where = @where.And(p => p.idCard.Contains(idCard));
            //个人简介 nvarchar
            var introduction = Request.Form["introduction"].FirstOrDefault();
            if (!string.IsNullOrEmpty(introduction)) @where = @where.And(p => p.introduction.Contains(introduction));
            //机构id int
            var organizationId = Request.Form["organizationId"].FirstOrDefault().ObjectToInt(0);
            if (organizationId > 0)
            {
                //where = where.And(p => p.organizationId == organizationId);
                var o = await _sysOrganizationServices.QueryAsync();
                var ids = new List<int>();
                SysOrganizationHelper.GetOrganizeChildIds(o, organizationId, ref ids);
                if (ids.Any())
                {
                    jm.otherData = ids;
                    where = where.And(p => ids.Contains((int)p.organizationId));
                }
            }

            //状态,0正常,1冻结 int
            var state = Request.Form["state"].FirstOrDefault().ObjectToInt(0);
            if (state > 0) @where = @where.And(p => p.state == state);
            //是否删除,0否,1是 bit
            var deleted = Request.Form["deleted"].FirstOrDefault();
            if (!string.IsNullOrEmpty(deleted) && deleted.ToLowerInvariant() == "true")
                @where = @where.And(p => p.deleted);
            else if (!string.IsNullOrEmpty(deleted) && deleted.ToLowerInvariant() == "false")
                @where = @where.And(p => p.deleted == false);
            //注册时间 datetime
            var createTime = Request.Form["createTime"].FirstOrDefault();
            if (!string.IsNullOrEmpty(createTime))
            {
                if (createTime.Contains("到"))
                {
                    var dts = createTime.Split("到");
                    var dtStart = dts[0].Trim().ObjectToDate();
                    where = where.And(p => p.createTime > dtStart);
                    var dtEnd = dts[1].Trim().ObjectToDate();
                    where = where.And(p => p.createTime < dtEnd);
                }
                else
                {
                    var dt = createTime.ObjectToDate();
                    where = where.And(p => p.createTime > dt);
                }
            }

            //修改时间 datetime
            var updateTime = Request.Form["updateTime"].FirstOrDefault();
            if (!string.IsNullOrEmpty(updateTime))
            {
                if (updateTime.Contains("到"))
                {
                    var dts = updateTime.Split("到");
                    var dtStart = dts[0].Trim().ObjectToDate();
                    where = where.And(p => p.updateTime > dtStart);
                    var dtEnd = dts[1].Trim().ObjectToDate();
                    where = where.And(p => p.updateTime < dtEnd);
                }
                else
                {
                    var dt = updateTime.ObjectToDate();
                    where = where.And(p => p.updateTime > dt);
                }
            }

            //获取数据
            var list = await _sysUserServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";

            if (list.Any())
            {
                var sysRoles = await _sysRoleServices.QueryAsync();
                var sysUserRoles = await _sysUserRoleServices.QueryAsync();

                foreach (var user in list)
                {
                    var roleIds = sysUserRoles.Where(p => p.userId == user.id).Select(p => p.roleId).ToList();
                    if (roleIds.Any()) user.roles = sysRoles.Where(p => roleIds.Contains(p.id)).ToList();
                }
            }


            return jm;
        }

        #endregion

        #region 首页数据============================================================

        // POST: Api/SysUser/GetIndex
        /// <summary>
        ///     首页数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("首页数据")]
        public AdminUiCallBack GetIndex()
        {
            //返回数据
            var jm = new AdminUiCallBack { code = 0 };
            return jm;
        }

        #endregion

        #region 创建数据============================================================

        // POST: Api/SysUser/GetCreate
        /// <summary>
        ///     创建数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("创建数据")]
        public async Task<AdminUiCallBack> GetCreate()
        {
            //返回数据
            var userSexTypes = EnumHelper.EnumToList<GlobalEnumVars.UserSexTypes>();
            var roles = await _sysRoleServices.QueryListByClauseAsync(p => p.deleted == false);

            var jm = new AdminUiCallBack { code = 0 };
            jm.data = new { userSexTypes, roles };


            return jm;
        }

        #endregion

        #region 创建提交============================================================

        // POST: Api/SysUser/DoCreate
        /// <summary>
        ///     创建提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("创建提交")]
        public async Task<AdminUiCallBack> DoCreate([FromBody] SysUser entity)
        {
            var jm = new AdminUiCallBack();

            var haveName = await _sysUserServices.ExistsAsync(p => p.userName == entity.userName);
            if (haveName)
            {
                jm.msg = "账号已经存在";
                return jm;
            }

            entity.createTime = DateTime.Now;
            entity.passWord = CommonHelper.Md5For32(entity.passWord);
            var id = await _sysUserServices.InsertAsync(entity);
            if (id > 0 && !string.IsNullOrEmpty(entity.roleIds))
            {
                var strIds = entity.roleIds.Split(",");
                var ids = CommonHelper.StringArrAyToIntArray(strIds);
                if (ids.Any())
                {
                    var userRoles = new List<SysUserRole>();
                    foreach (var itemRoleId in ids)
                        userRoles.Add(new SysUserRole
                        {
                            createTime = DateTime.Now,
                            roleId = itemRoleId,
                            userId = id
                        });
                    if (userRoles.Any()) await _sysUserRoleServices.InsertAsync(userRoles);
                }
            }

            jm.otherData = entity;
            var bl = id > 0;
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.CreateSuccess : GlobalConstVars.CreateFailure;

            return jm;
        }

        #endregion

        #region 编辑数据============================================================

        // POST: Api/SysUser/GetEdit
        /// <summary>
        ///     编辑数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑数据")]
        public async Task<AdminUiCallBack> GetEdit([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _sysUserServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            var userSexTypes = EnumHelper.EnumToList<GlobalEnumVars.UserSexTypes>();
            var userRoles = await _sysUserRoleServices.QueryListByClauseAsync(p => p.userId == model.id);
            var roleIds = userRoles.Select(p => p.roleId).ToList();
            var roles = await _sysRoleServices.QueryListByClauseAsync(p => p.deleted == false);


            jm.code = 0;
            jm.data = new
            {
                model,
                userSexTypes,
                roles,
                roleIds
            };

            return jm;
        }

        #endregion

        #region 编辑提交============================================================

        // POST: Api/SysUser/Edit
        /// <summary>
        ///     编辑提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑提交")]
        public async Task<AdminUiCallBack> DoEdit([FromBody] SysUser entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _sysUserServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }


            if (oldModel.userName != entity.userName)
            {
                var haveName = await _sysUserServices.ExistsAsync(p => p.userName == entity.userName);
                if (haveName)
                {
                    jm.msg = "账号已经存在";
                    return jm;
                }
            }

            //事物处理过程开始
            oldModel.userName = entity.userName;
            if (!string.IsNullOrEmpty(entity.passWord))
            {
                var md5Str = CommonHelper.Md5For32(entity.passWord);
                oldModel.passWord = md5Str;
            }

            oldModel.organizationId = entity.organizationId > 0 ? entity.organizationId : 0;
            oldModel.nickName = entity.nickName;
            oldModel.sex = entity.sex;
            oldModel.phone = entity.phone;
            oldModel.updateTime = DateTime.Now;

            //事物处理过程结束
            var bl = await _sysUserServices.UpdateAsync(oldModel);
            if (bl)
            {
                await _sysUserRoleServices.DeleteAsync(p => p.userId == oldModel.id);
                if (!string.IsNullOrEmpty(entity.roleIds))
                {
                    var strIds = entity.roleIds.Split(",");
                    var ids = CommonHelper.StringArrAyToIntArray(strIds);
                    if (ids.Any())
                    {
                        var userRoles = new List<SysUserRole>();
                        foreach (var itemRoleId in ids)
                            userRoles.Add(new SysUserRole
                            {
                                createTime = DateTime.Now,
                                roleId = itemRoleId,
                                userId = oldModel.id
                            });
                        if (userRoles.Any()) await _sysUserRoleServices.InsertAsync(userRoles);
                    }
                }
            }

            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return jm;
        }

        #endregion

        #region 删除数据============================================================

        // POST: Api/SysUser/DoDelete/10
        /// <summary>
        ///     单选删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("单选删除")]
        public async Task<AdminUiCallBack> DoDelete([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _sysUserServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = GlobalConstVars.DataisNo;
                return jm;
            }

            if (model.id == 1)
            {
                jm.msg = "初始管理员账户禁止删除";
                return jm;
            }

            var bl = await _sysUserServices.DeleteByIdAsync(entity.id);
            if (bl) await _sysUserRoleServices.DeleteAsync(p => p.userId == model.id);

            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;
            return jm;
        }

        #endregion

        #region 设置是否锁定============================================================

        // POST: Api/SysUser/DoSetdeleted/10
        /// <summary>
        ///     设置是否锁定,0否,1是
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("设置是否锁定,0否,1是")]
        public async Task<AdminUiCallBack> DoSetState([FromBody] FMUpdateBoolDataByIntId entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _sysUserServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            oldModel.state = entity.data ? 0 : 1;

            var bl = await _sysUserServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return jm;
        }

        #endregion
    }
}