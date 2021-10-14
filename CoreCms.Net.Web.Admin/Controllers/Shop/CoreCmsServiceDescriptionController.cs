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
using CoreCms.Net.Utility.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;
using SqlSugar;

namespace CoreCms.Net.Web.Admin.Controllers
{
    /// <summary>
    /// 商城服务说明
    ///</summary>
    [Description("商城服务说明")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize(Permissions.Name)]
    public class CoreCmsServiceDescriptionController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICoreCmsServiceDescriptionServices _coreCmsServiceDescriptionServices;

        /// <summary>
        /// 构造函数
        ///</summary>
        public CoreCmsServiceDescriptionController(IWebHostEnvironment webHostEnvironment
            , ICoreCmsServiceDescriptionServices coreCmsServiceDescriptionServices
            )
        {
            _webHostEnvironment = webHostEnvironment;
            _coreCmsServiceDescriptionServices = coreCmsServiceDescriptionServices;
        }

        #region 获取列表============================================================
        // POST: Api/CoreCmsServiceDescription/GetPageList
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("获取列表")]
        public async Task<AdminUiCallBack> GetPageList()
        {
            var jm = new AdminUiCallBack();
            var pageCurrent = Request.Form["page"].FirstOrDefault().ObjectToInt(1);
            var pageSize = Request.Form["limit"].FirstOrDefault().ObjectToInt(30);
            var where = PredicateBuilder.True<CoreCmsServiceDescription>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();

            Expression<Func<CoreCmsServiceDescription, object>> orderEx = orderField switch
            {
                "id" => p => p.id,
                "title" => p => p.title,
                "type" => p => p.type,
                "description" => p => p.description,
                "isShow" => p => p.isShow,
                "sortId" => p => p.sortId,
                _ => p => p.id
            };

            //设置排序方式
            var orderDirection = Request.Form["orderDirection"].FirstOrDefault();
            var orderBy = orderDirection switch
            {
                "asc" => OrderByType.Asc,
                "desc" => OrderByType.Desc,
                _ => OrderByType.Desc
            };
            //查询筛选

            //序列 int
            var id = Request.Form["id"].FirstOrDefault().ObjectToInt(0);
            if (id > 0)
            {
                where = where.And(p => p.id == id);
            }
            //名称 nvarchar
            var title = Request.Form["title"].FirstOrDefault();
            if (!string.IsNullOrEmpty(title))
            {
                where = where.And(p => p.title.Contains(title));
            }
            //类型 int
            var type = Request.Form["type"].FirstOrDefault().ObjectToInt(0);
            if (type > 0)
            {
                where = where.And(p => p.type == type);
            }
            //描述 nvarchar
            var description = Request.Form["description"].FirstOrDefault();
            if (!string.IsNullOrEmpty(description))
            {
                where = where.And(p => p.description.Contains(description));
            }
            //是否展示 bit
            var isShow = Request.Form["isShow"].FirstOrDefault();
            if (!string.IsNullOrEmpty(isShow) && isShow.ToLowerInvariant() == "true")
            {
                where = where.And(p => p.isShow == true);
            }
            else if (!string.IsNullOrEmpty(isShow) && isShow.ToLowerInvariant() == "false")
            {
                where = where.And(p => p.isShow == false);
            }
            //排序 int
            var sortId = Request.Form["sortId"].FirstOrDefault().ObjectToInt(0);
            if (sortId > 0)
            {
                where = where.And(p => p.sortId == sortId);
            }
            //获取数据
            var list = await _coreCmsServiceDescriptionServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize, true);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return jm;
        }
        #endregion

        #region 首页数据============================================================
        // POST: Api/CoreCmsServiceDescription/GetIndex
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

            var serviceNoteType = EnumHelper.EnumToList<GlobalEnumVars.ShopServiceNoteType>();

            jm.data = new
            {
                serviceNoteType
            };

            return jm;
        }
        #endregion

        #region 创建数据============================================================
        // POST: Api/CoreCmsServiceDescription/GetCreate
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

            var serviceNoteType = EnumHelper.EnumToList<GlobalEnumVars.ShopServiceNoteType>();

            jm.data = new
            {
                serviceNoteType
            };

            return jm;
        }
        #endregion

        #region 创建提交============================================================
        // POST: Api/CoreCmsServiceDescription/DoCreate
        /// <summary>
        /// 创建提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("创建提交")]
        public async Task<AdminUiCallBack> DoCreate([FromBody] CoreCmsServiceDescription entity)
        {
            var jm = await _coreCmsServiceDescriptionServices.InsertAsync(entity);
            return jm;
        }
        #endregion

        #region 编辑数据============================================================
        // POST: Api/CoreCmsServiceDescription/GetEdit
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

            var model = await _coreCmsServiceDescriptionServices.QueryByIdAsync(entity.id, false);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            jm.code = 0;

            var serviceNoteType = EnumHelper.EnumToList<GlobalEnumVars.ShopServiceNoteType>();

            jm.data = new
            {
                model,
                serviceNoteType
            };

            return jm;
        }
        #endregion

        #region 编辑提交============================================================
        // POST: Api/CoreCmsServiceDescription/Edit
        /// <summary>
        /// 编辑提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑提交")]
        public async Task<AdminUiCallBack> DoEdit([FromBody] CoreCmsServiceDescription entity)
        {
            var jm = await _coreCmsServiceDescriptionServices.UpdateAsync(entity);
            return jm;
        }
        #endregion

        #region 删除数据============================================================
        // POST: Api/CoreCmsServiceDescription/DoDelete/10
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

            var model = await _coreCmsServiceDescriptionServices.ExistsAsync(p => p.id == entity.id, true);
            if (!model)
            {
                jm.msg = GlobalConstVars.DataisNo;
                return jm;
            }
            jm = await _coreCmsServiceDescriptionServices.DeleteByIdAsync(entity.id);

            return jm;
        }
        #endregion

        #region 预览数据============================================================
        // POST: Api/CoreCmsServiceDescription/GetDetails/10
        /// <summary>
        /// 预览数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("预览数据")]
        public async Task<AdminUiCallBack> GetDetails([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _coreCmsServiceDescriptionServices.QueryByIdAsync(entity.id, false);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            jm.code = 0;

            var serviceNoteType = EnumHelper.EnumToList<GlobalEnumVars.ShopServiceNoteType>();

            jm.data = new
            {
                model,
                serviceNoteType
            };


            return jm;
        }
        #endregion

        #region 设置是否展示============================================================
        // POST: Api/CoreCmsServiceDescription/DoSetisShow/10
        /// <summary>
        /// 设置是否展示
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("设置是否展示")]
        public async Task<AdminUiCallBack> DoSetisShow([FromBody] FMUpdateBoolDataByIntId entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsServiceDescriptionServices.QueryByIdAsync(entity.id, false);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            oldModel.isShow = (bool)entity.data;

            jm = await _coreCmsServiceDescriptionServices.UpdateAsync(oldModel);

            return jm;
        }
        #endregion


    }
}
