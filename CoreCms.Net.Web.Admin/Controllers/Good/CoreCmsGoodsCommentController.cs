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
    /// 商品评价表
    ///</summary>
    [Description("商品评价表")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize(Permissions.Name)]
    public class CoreCmsGoodsCommentController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICoreCmsGoodsCommentServices _coreCmsGoodsCommentServices;

        /// <summary>
        /// 构造函数
        ///</summary>
        public CoreCmsGoodsCommentController(IWebHostEnvironment webHostEnvironment
            , ICoreCmsGoodsCommentServices coreCmsGoodsCommentServices
            )
        {
            _webHostEnvironment = webHostEnvironment;
            _coreCmsGoodsCommentServices = coreCmsGoodsCommentServices;
        }

        #region 获取列表============================================================
        // POST: Api/CoreCmsGoodsComment/GetPageList
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
            var where = PredicateBuilder.True<CoreCmsGoodsComment>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();
            Expression<Func<CoreCmsGoodsComment, object>> orderEx;
            switch (orderField)
            {
                case "id":
                    orderEx = p => p.id;
                    break;
                case "commentId":
                    orderEx = p => p.commentId;
                    break;
                case "score":
                    orderEx = p => p.score;
                    break;
                case "userId":
                    orderEx = p => p.userId;
                    break;
                case "goodsId":
                    orderEx = p => p.goodsId;
                    break;
                case "orderId":
                    orderEx = p => p.orderId;
                    break;
                case "addon":
                    orderEx = p => p.addon;
                    break;
                case "images":
                    orderEx = p => p.images;
                    break;
                case "contentBody":
                    orderEx = p => p.contentBody;
                    break;
                case "sellerContent":
                    orderEx = p => p.sellerContent;
                    break;
                case "isDisplay":
                    orderEx = p => p.isDisplay;
                    break;
                case "createTime":
                    orderEx = p => p.createTime;
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

            //序列 int
            var id = Request.Form["id"].FirstOrDefault().ObjectToInt(0);
            if (id > 0)
            {
                where = where.And(p => p.id == id);
            }
            //父级评价ID int
            var commentId = Request.Form["commentId"].FirstOrDefault().ObjectToInt(0);
            if (commentId > 0)
            {
                where = where.And(p => p.commentId == commentId);
            }
            //评价1-5星 int
            var score = Request.Form["score"].FirstOrDefault().ObjectToInt(0);
            if (score > 0)
            {
                where = where.And(p => p.score == score);
            }
            //评价用户ID int
            var userId = Request.Form["userId"].FirstOrDefault().ObjectToInt(0);
            if (userId > 0)
            {
                where = where.And(p => p.userId == userId);
            }
            //商品ID int
            var goodsId = Request.Form["goodsId"].FirstOrDefault().ObjectToInt(0);
            if (goodsId > 0)
            {
                where = where.And(p => p.goodsId == goodsId);
            }
            //评价订单ID nvarchar
            var orderId = Request.Form["orderId"].FirstOrDefault();
            if (!string.IsNullOrEmpty(orderId))
            {
                where = where.And(p => p.orderId.Contains(orderId));
            }
            //货品规格序列号存储 nvarchar
            var addon = Request.Form["addon"].FirstOrDefault();
            if (!string.IsNullOrEmpty(addon))
            {
                where = where.And(p => p.addon.Contains(addon));
            }
            //评价图片逗号分隔最多五张 nvarchar
            var images = Request.Form["images"].FirstOrDefault();
            if (!string.IsNullOrEmpty(images))
            {
                where = where.And(p => p.images.Contains(images));
            }
            //评价内容 nvarchar
            var contentBody = Request.Form["contentBody"].FirstOrDefault();
            if (!string.IsNullOrEmpty(contentBody))
            {
                where = where.And(p => p.contentBody.Contains(contentBody));
            }
            //商家回复 nvarchar
            var sellerContent = Request.Form["sellerContent"].FirstOrDefault();
            if (!string.IsNullOrEmpty(sellerContent))
            {
                where = where.And(p => p.sellerContent.Contains(sellerContent));
            }


            //用户昵称 nvarchar
            var nickName = Request.Form["nickName"].FirstOrDefault();
            if (!string.IsNullOrEmpty(nickName))
            {
                where = where.And(p => p.nickName.Contains(nickName));
            }
            //商品名称 nvarchar
            var goodName = Request.Form["goodName"].FirstOrDefault();
            if (!string.IsNullOrEmpty(goodName))
            {
                where = where.And(p => p.goodName.Contains(goodName));
            }


            //前台显示 bit
            var isDisplay = Request.Form["isDisplay"].FirstOrDefault();
            if (!string.IsNullOrEmpty(isDisplay) && isDisplay.ToLowerInvariant() == "true")
            {
                where = where.And(p => p.isDisplay == true);
            }
            else if (!string.IsNullOrEmpty(isDisplay) && isDisplay.ToLowerInvariant() == "false")
            {
                where = where.And(p => p.isDisplay == false);
            }
            //创建时间 datetime
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
            //获取数据
            var list = await _coreCmsGoodsCommentServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize, true);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return jm;
        }
        #endregion

        #region 首页数据============================================================
        // POST: Api/CoreCmsGoodsComment/GetIndex
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

        #region 编辑数据============================================================
        // POST: Api/CoreCmsGoodsComment/GetEdit
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

            var model = await _coreCmsGoodsCommentServices.DetailsByIdAsync(p => p.id == entity.id, p => p.id, OrderByType.Desc);
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
        // POST: Api/CoreCmsGoodsComment/Edit
        /// <summary>
        /// 编辑提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑提交")]
        public async Task<AdminUiCallBack> DoEdit([FromBody] FMIntId entity)
        {
            var jm = await _coreCmsGoodsCommentServices.Reply(entity.id, entity.data.ToString());
            return jm;
        }
        #endregion

        #region 删除数据============================================================
        // POST: Api/CoreCmsGoodsComment/DoDelete/10
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

            var model = await _coreCmsGoodsCommentServices.ExistsAsync(p => p.id == entity.id, true);
            if (!model)
            {
                jm.msg = GlobalConstVars.DataisNo;
                return jm;
            }
            jm = await _coreCmsGoodsCommentServices.DeleteByIdAsync(entity.id);

            return jm;
        }
        #endregion

        #region 预览数据============================================================
        // POST: Api/CoreCmsGoodsComment/GetDetails/10
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

            var model = await _coreCmsGoodsCommentServices.DetailsByIdAsync(p => p.id == entity.id, p => p.id, OrderByType.Desc);
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

        #region 设置前台显示============================================================
        // POST: Api/CoreCmsGoodsComment/DoSetisDisplay/10
        /// <summary>
        /// 设置前台显示
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("设置前台显示")]
        public async Task<AdminUiCallBack> DoSetisDisplay([FromBody] FMUpdateBoolDataByIntId entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsGoodsCommentServices.QueryByIdAsync(entity.id, false);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            oldModel.isDisplay = (bool)entity.data;

            jm = await _coreCmsGoodsCommentServices.UpdateAsync(oldModel);

            return jm;
        }
        #endregion

    }
}
