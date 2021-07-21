/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using CoreCms.Net.Caching.AutoMate.RedisCache;
using CoreCms.Net.IServices;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Utility.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using ToolGood.Words;

namespace CoreCms.Net.Web.Admin.Controllers
{
    /// <summary>
    /// 演示类
    /// </summary>
    //[DisableCors]
    public class DemoController : Controller
    {
        private readonly IToolsServices _toolsServices;

        public DemoController(IToolsServices toolsServices)
        {
            _toolsServices = toolsServices;
        }


        /// <summary>
        /// 默认首页
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            var t = await _toolsServices.IllegalWordsReplace("测试");

            //return Content(t);
            return Content(t.ToString());
        }
    }
}
