/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using Microsoft.AspNetCore.Mvc;

namespace CoreCms.Net.Web.WebApi.Controllers
{
    /// <summary>
    ///     默认接口示例
    /// </summary>
    public class DemoController : ControllerBase
    {
        /// <summary>
        ///     默认首页
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return Content("已结束");
        }
    }
}