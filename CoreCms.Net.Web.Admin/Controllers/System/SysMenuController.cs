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
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoreCms.Net.Configuration;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.Entities.Expression;
using CoreCms.Net.Model.FromBody;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Filter;
using CoreCms.Net.Loging;
using CoreCms.Net.IServices;
using CoreCms.Net.Utility.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;
using SqlSugar;

namespace CoreCms.Net.Web.Admin.Controllers
{
    /// <summary>
    /// 菜单表
    ///</summary>
    [Description("菜单表")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize(Permissions.Name)]
    public class SysMenuController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ISysMenuServices _sysMenuServices;

        /// <summary>
        /// 构造函数
        ///</summary>
        public SysMenuController(IWebHostEnvironment webHostEnvironment
            , ISysMenuServices sysMenuServices
            )
        {
            _webHostEnvironment = webHostEnvironment;
            _sysMenuServices = sysMenuServices;
        }

        #region 获取列表============================================================
        // POST: Api/SysMenu/GetPageList
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("获取列表")]
        public async Task<AdminUiCallBack> GetPageList()
        {
            var jm = new AdminUiCallBack();

            //获取数据
            var list = await _sysMenuServices.QueryListByClauseAsync(p => p.deleted == false, p => p.sortNumber, OrderByType.Asc);

            var type = Request.Form["type"].FirstOrDefault();
            if (!string.IsNullOrEmpty(type) && type == "layui")
            {
                jm.data = SysMenuHelper.GetTree(list, false).data;
            }
            else
            {
                jm.data = list;
            }
            //返回数据
            jm.code = 0;
            jm.count = list.Count;
            jm.msg = "数据调用成功!";
            return jm;
        }
        #endregion

        #region 首页数据============================================================
        // POST: Api/SysMenu/GetIndex
        /// <summary>
        /// 首页数据
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
        // POST: Api/SysMenu/GetCreate
        /// <summary>
        /// 创建数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("创建数据")]
        public AdminUiCallBack GetCreate()
        {
            //返回数据
            var jm = new AdminUiCallBack { code = 0 };

            return jm;
        }
        #endregion

        #region 创建提交============================================================
        // POST: Api/SysMenu/DoCreate
        /// <summary>
        /// 创建提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("创建提交")]
        public async Task<AdminUiCallBack> DoCreate([FromBody] SysMenu entity)
        {
            var jm = new AdminUiCallBack();

            entity.createTime = DateTime.Now; ;
            jm = await _sysMenuServices.InsertAsync(entity);

            return jm;
        }
        #endregion

        #region 编辑数据============================================================
        // POST: Api/SysMenu/GetEdit
        /// <summary>
        /// 编辑数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑数据")]
        public async Task<AdminUiCallBack> GetEdit([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _sysMenuServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            jm.code = 0;
            jm.data = model;

            return jm;
        }
        #endregion

        #region 编辑提交============================================================
        // POST: Api/SysMenu/Edit
        /// <summary>
        /// 编辑提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑提交")]
        public async Task<AdminUiCallBack> DoEdit([FromBody] SysMenu entity)
        {
            var jm = await _sysMenuServices.UpdateAsync(entity);
            return jm;
        }
        #endregion

        #region 删除数据============================================================
        // POST: Api/SysMenu/DoDelete/10
        /// <summary>
        /// 单选删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("单选删除")]
        public async Task<AdminUiCallBack> DoDelete([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            jm = await _sysMenuServices.DeleteByIdAsync(entity.id);




            return jm;




        }
        #endregion

        #region 批量导入Action============================================================
        // POST: Api/SysMenu/DoDelete/10
        /// <summary>
        /// 单选删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("单选删除")]
        public async Task<AdminUiCallBack> ImportButtons([FromBody] FMSysMenuToImportButton entity)
        {
            var jm = new AdminUiCallBack();

            if (entity.data.Count <= 0)
            {
                jm.msg = "请选择要导入的按钮";
                return jm;
            }

            //清空旗下按钮
            await _sysMenuServices.DeleteAsync(p => p.parentId == entity.data[0].menuId && p.menuType == 1);

            var list = new List<SysMenu>();
            for (var index = 0; index < entity.data.Count; index++)
            {
                var p = entity.data[index];
                list.Add(new SysMenu()
                {
                    parentId = p.menuId,
                    identificationCode = p.actionName,
                    menuName = p.description,
                    component = "/Api/" + p.controllerName + "/" + p.actionName,
                    menuType = 1,
                    sortNumber = index,
                    authority = p.controllerName + ":" + p.actionName,
                    hide = false,
                    deleted = false,
                    createTime = DateTime.Now
                });
            }

            var bl = await _sysMenuServices.InsertAsync(list) > 0;
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;
            if (bl)
            {
                await _sysMenuServices.UpdateCaChe();
            }
            return jm;

        }
        #endregion

    }
}
