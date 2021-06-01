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
using System.ComponentModel.DataAnnotations;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    /// 代理商品池
    /// </summary>
    public partial class CoreCmsAgentGoods
    {
        /// <summary>
        /// 商品名称
        /// </summary>
        [Display(Name = "商品名称")]
        [SqlSugar.SugarColumn(IsIgnore = true)]
        public string goodName { get; set; }


        /// <summary>
        /// 商品缩略图
        /// </summary>
        [Display(Name = "商品缩略图")]
        [SqlSugar.SugarColumn(IsIgnore = true)]
        public string goodImage { get; set; }


        /// <summary>
        /// 商品最后更新时间
        /// </summary>
        [Display(Name = "商品最后更新时间")]
        [SqlSugar.SugarColumn(IsIgnore = true)]
        public DateTime? goodUpdateTime { get; set; }




    }
}