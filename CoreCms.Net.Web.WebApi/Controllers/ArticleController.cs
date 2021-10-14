/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using System.Linq;
using System.Threading.Tasks;
using CoreCms.Net.Auth.HttpContextUser;
using CoreCms.Net.IServices;
using CoreCms.Net.Model.FromBody;
using CoreCms.Net.Model.ViewModels.UI;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace CoreCms.Net.Web.WebApi.Controllers
{
    /// <summary>
    /// 文章api控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {

        private IHttpContextUser _user;
        private readonly ICoreCmsArticleServices _articleServices;
        private readonly ICoreCmsArticleTypeServices _articleTypeServices;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="user"></param>
        /// <param name="articleServices"></param>
        /// <param name="articleTypeServices"></param>
        public ArticleController(IHttpContextUser user, ICoreCmsArticleServices articleServices, ICoreCmsArticleTypeServices articleTypeServices)
        {
            _user = user;
            _articleServices = articleServices;
            _articleTypeServices = articleTypeServices;
        }



        #region 获取通知列表
        /// <summary>
        /// 获取通知列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<WebApiCallBack> NoticeList([FromBody] FMPageByIntId entity)
        {
            var jm = new WebApiCallBack();

            var list = await _articleServices.QueryPageAsync(p => p.isDel == false, p => p.createTime, OrderByType.Desc,
                entity.page, entity.limit);
            jm.status = true;
            jm.data = list;

            return jm;
        }

        #endregion


        #region 获取文章列表
        /// <summary>
        /// 获取文章列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<WebApiCallBack> GetArticleList([FromBody] FMPageByIntId entity)
        {
            var jm = new WebApiCallBack();

            var list = await _articleServices.QueryPageAsync(p => p.isDel == false && p.typeId == entity.id, p => p.createTime, OrderByType.Desc,
                entity.page, entity.limit);

            var articleType = await _articleTypeServices.QueryAsync();
            var typeName = string.Empty;
            if (articleType.Any())
            {
                var type = articleType.Find(p => p.id == entity.id);
                typeName = type != null ? type.name : "";
            }
            jm.status = true;
            jm.data = new
            {
                list,
                articleType,
                type_name = typeName,
                count = list.TotalCount
            };

            return jm;
        }

        #endregion


        /// <summary>
        /// 获取单个文章内容
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<WebApiCallBack> GetArticleDetail([FromBody] FMIntId entity)
        {
            var jm = new WebApiCallBack();

            var model = await _articleServices.ArticleDetail(entity.id);
            if (model == null)
            {
                jm.msg = "数据获取失败";
                return jm;
            }
            jm.status = true;
            jm.data = model;
            return jm;

        }
    }
}
