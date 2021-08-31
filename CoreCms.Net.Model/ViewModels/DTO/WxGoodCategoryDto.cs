/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/


using System.Collections.Generic;

namespace CoreCms.Net.Model.ViewModels.DTO
{
    public class WxGoodCategoryDto
    {
        /// <summary>
        ///     序列
        /// </summary>
        public int id { get; set; }

        /// <summary>
        ///     标题
        /// </summary>
        public string name { get; set; }

        /// <summary>
        ///     排序
        /// </summary>
        public int sort { get; set; }

        /// <summary>
        ///     图片地址
        /// </summary>
        public string imageUrl { get; set; }

        public List<WxGoodCategoryChild> child { get; set; }
    }

    public class WxGoodCategoryChild
    {
        /// <summary>
        ///     序列
        /// </summary>
        public int id { get; set; }

        /// <summary>
        ///     标题
        /// </summary>
        public string name { get; set; }

        /// <summary>
        ///     排序
        /// </summary>
        public int sort { get; set; }

        /// <summary>
        ///     图片地址
        /// </summary>
        public string imageUrl { get; set; }
    }
}