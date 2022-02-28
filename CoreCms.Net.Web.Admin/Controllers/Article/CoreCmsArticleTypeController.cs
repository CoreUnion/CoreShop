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
using CoreCms.Net.Filter;
using CoreCms.Net.IServices;
using CoreCms.Net.Loging;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.FromBody;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Utility.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;
using SqlSugar;

namespace CoreCms.Net.Web.Admin.Controllers
{
    /// <summary>
    /// 文章分类表
    ///</summary>
    [Description("文章分类表")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize(Permissions.Name)]
    public class CoreCmsArticleTypeController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICoreCmsArticleTypeServices _coreCmsArticleTypeServices;
        private readonly ICoreCmsArticleServices _coreCmsArticleServices;

        ///  <summary>
        ///  构造函数
        /// </summary>
        ///   <param name="webHostEnvironment"></param>
        /// <param name="coreCmsArticleTypeServices"></param>
        ///  <param name="coreCmsArticleServices"></param>
        public CoreCmsArticleTypeController(IWebHostEnvironment webHostEnvironment
            , ICoreCmsArticleTypeServices coreCmsArticleTypeServices
            , ICoreCmsArticleServices coreCmsArticleServices
            )
        {
            _webHostEnvironment = webHostEnvironment;
            _coreCmsArticleTypeServices = coreCmsArticleTypeServices;
            _coreCmsArticleServices = coreCmsArticleServices;
        }

        #region 获取列表============================================================
        // POST: Api/CoreCmsArticleType/GetPageList
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
            var list = await _coreCmsArticleTypeServices.QueryListByClauseAsync(p => p.id > 0, p => p.sort,
                OrderByType.Desc);            
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.msg = "数据调用成功!";
            return jm;
        }
        #endregion

        #region 首页数据============================================================
        // POST: Api/CoreCmsArticleType/GetIndex
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
        // POST: Api/CoreCmsArticleType/GetCreate
        /// <summary>
        /// 创建数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("创建数据")]
        public async Task<AdminUiCallBack> GetCreate()
        {
            //返回数据
            var jm = new AdminUiCallBack { code = 0 };

            var categories = await _coreCmsArticleTypeServices.QueryAsync();
            jm.data = new
            {
                categories = ArticleHelper.GetTree(categories)
            };
            return jm;
        }
        #endregion

        #region 创建提交============================================================
        // POST: Api/CoreCmsArticleType/DoCreate
        /// <summary>
        /// 创建提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("创建提交")]
        public async Task<AdminUiCallBack> DoCreate([FromBody] CoreCmsArticleType entity)
        {
            var jm = new AdminUiCallBack();

            var bl = await _coreCmsArticleTypeServices.InsertAsync(entity) > 0;
            jm.code = bl ? 0 : 1;
            jm.msg = (bl ? GlobalConstVars.CreateSuccess : GlobalConstVars.CreateFailure);

            return jm;
        }
        #endregion

        #region 编辑数据============================================================
        // POST: Api/CoreCmsArticleType/GetEdit
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

            var model = await _coreCmsArticleTypeServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            jm.code = 0;

            var categories = await _coreCmsArticleTypeServices.QueryAsync();
            jm.data = new
            {
                model,
                categories = ArticleHelper.GetTree(categories)
            };

            return jm;
        }
        #endregion

        #region 编辑提交============================================================
        // POST: Admins/CoreCmsArticleType/Edit
        /// <summary>
        /// 编辑提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑提交")]
        public async Task<AdminUiCallBack> DoEdit([FromBody] CoreCmsArticleType entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsArticleTypeServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            //事物处理过程开始
            oldModel.id = entity.id;
            oldModel.name = entity.name;
            oldModel.parentId = entity.parentId;
            oldModel.sort = entity.sort;

            //事物处理过程结束
            var bl = await _coreCmsArticleTypeServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return jm;
        }
        #endregion

        #region 删除数据============================================================
        // POST: Api/CoreCmsArticleType/DoDelete/10
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

            var model = await _coreCmsArticleTypeServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = GlobalConstVars.DataisNo;
                return jm;
            }

            if (await _coreCmsArticleTypeServices.ExistsAsync(p => p.parentId == entity.id))
            {
                jm.msg = GlobalConstVars.DeleteIsHaveChildren;
                return jm;
            }

            if (await _coreCmsArticleServices.ExistsAsync(p => p.typeId == entity.id))
            {
                jm.msg = "栏目下有文章禁止删除";
                return jm;
            }

            var bl = await _coreCmsArticleTypeServices.DeleteByIdAsync(entity.id);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;

            return jm;
        }
        #endregion

    }
}
