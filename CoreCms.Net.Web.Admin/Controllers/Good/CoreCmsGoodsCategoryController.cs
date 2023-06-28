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
using AutoMapper;
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
    /// 商品分类
    ///</summary>
    [Description("商品分类")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize(Permissions.Name)]
    public class CoreCmsGoodsCategoryController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICoreCmsGoodsCategoryServices _coreCmsGoodsCategoryServices;
        private readonly ICoreCmsGoodsServices _goodsServices;
        private IMapper _mapper;

        ///  <summary>
        ///  构造函数
        /// </summary>
        public CoreCmsGoodsCategoryController(IWebHostEnvironment webHostEnvironment
            , ICoreCmsGoodsCategoryServices coreCmsGoodsCategoryServices
            , IMapper mapper, ICoreCmsGoodsServices goodsServices)
        {
            _webHostEnvironment = webHostEnvironment;
            _coreCmsGoodsCategoryServices = coreCmsGoodsCategoryServices;
            _mapper = mapper;
            _goodsServices = goodsServices;
        }

        #region 获取列表============================================================
        // POST: Api/CoreCmsGoodsCategory/GetPageList
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
            var list = await _coreCmsGoodsCategoryServices.QueryListByClauseAsync(p => p.id > 0, p => p.sort, OrderByType.Desc);
            //返回数据
            var type = Request.Form["type"].FirstOrDefault();
            if (!string.IsNullOrEmpty(type) && type == "layui")
            {
                //返回数据
                jm.data = GoodsHelper.GetTree(list, false).data;
            }
            else
            {
                jm.data = list;
            }
            jm.code = 0;
            jm.msg = "数据调用成功!";
            return jm;
        }
        #endregion

        #region 首页数据============================================================
        // POST: Api/CoreCmsGoodsCategory/GetIndex
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
        // POST: Api/CoreCmsGoodsCategory/GetCreate
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
            var categories = await _coreCmsGoodsCategoryServices.QueryListByClauseAsync(p => p.isShow == true, p => p.sort,
                OrderByType.Asc);
            jm.data = new
            {
                categories = GoodsHelper.GetTree(categories),
            };
            return jm;
        }
        #endregion

        #region 创建提交============================================================
        // POST: Api/CoreCmsGoodsCategory/DoCreate
        /// <summary>
        /// 创建提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("创建提交")]
        public async Task<AdminUiCallBack> DoCreate([FromBody] CoreCmsGoodsCategory entity)
        {
            var jm = new AdminUiCallBack();

            var result = await _coreCmsGoodsCategoryServices.InsertAsync(entity);
            var bl = result.code == 0;
            jm.code = bl ? 0 : 1;
            jm.msg = (bl ? GlobalConstVars.CreateSuccess : GlobalConstVars.CreateFailure);
            if (bl)
            {
                var categories = await _coreCmsGoodsCategoryServices.QueryListByClauseAsync(p => p.isShow == true, p => p.sort,
                    OrderByType.Asc);
                jm.data = new
                {
                    categories = GoodsHelper.GetTree(categories, false)
                };
            }

            return jm;
        }
        #endregion

        #region 编辑数据============================================================
        // POST: Api/CoreCmsGoodsCategory/GetEdit
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

            var model = await _coreCmsGoodsCategoryServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            jm.code = 0;

            var categories = await _coreCmsGoodsCategoryServices.QueryListByClauseAsync(p => p.isShow == true, p => p.sort,
                OrderByType.Asc); ;
            jm.data = new
            {
                model,
                categories = GoodsHelper.GetTree(categories),
            };

            return jm;
        }
        #endregion

        #region 编辑提交============================================================
        // POST: Admins/CoreCmsGoodsCategory/Edit
        /// <summary>
        /// 编辑提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑提交")]
        public async Task<AdminUiCallBack> DoEdit([FromBody] CoreCmsGoodsCategory entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsGoodsCategoryServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            if (entity.id == entity.parentId)
            {
                jm.msg = "上级不能为本类";
                return jm;
            }

            //事物处理过程开始
            oldModel.parentId = entity.parentId;
            oldModel.name = entity.name;
            oldModel.typeId = entity.typeId;
            oldModel.sort = entity.sort;
            oldModel.imageUrl = entity.imageUrl;
            oldModel.isShow = entity.isShow;
            oldModel.createTime = entity.createTime;

            //事物处理过程结束
            var result = await _coreCmsGoodsCategoryServices.UpdateAsync(oldModel);
            var bl = result.code == 0;
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return jm;
        }
        #endregion

        #region 删除数据============================================================
        // POST: Api/CoreCmsGoodsCategory/DoDelete/10
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

            var model = await _coreCmsGoodsCategoryServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = GlobalConstVars.DataisNo;
                return jm;
            }

            if (await _coreCmsGoodsCategoryServices.ExistsAsync(p => p.parentId == entity.id))
            {
                jm.msg = GlobalConstVars.DeleteIsHaveChildren;
                return jm;
            }

            if (await _goodsServices.ExistsAsync(p => p.goodsCategoryId == entity.id && !p.isDel))
            {
                jm.msg = "有商品关联此栏目,禁止删除";
                return jm;
            }

            var result = await _coreCmsGoodsCategoryServices.DeleteByIdAsync(entity.id);
            var bl = result.code == 0;
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;
            return jm;

        }
        #endregion

        #region 设置是否显示============================================================
        // POST: Api/CoreCmsGoodsCategory/DoSetisShow/10
        /// <summary>
        /// 设置是否显示
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("设置是否显示")]
        public async Task<AdminUiCallBack> DoSetisShow([FromBody] FMUpdateBoolDataByIntId entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsGoodsCategoryServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            oldModel.isShow = (bool)entity.data;

            var result = await _coreCmsGoodsCategoryServices.UpdateAsync(oldModel);
            var bl = result.code == 0;
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return jm;
        }
        #endregion



    }
}
