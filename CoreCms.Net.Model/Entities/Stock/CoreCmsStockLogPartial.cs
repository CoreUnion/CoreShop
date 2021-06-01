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
using SqlSugar;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    ///     库存操作详情表
    /// </summary>
    public partial class CoreCmsStockLog
    {
        /// <summary>
        ///     操作类型
        /// </summary>
        [Display(Name = "操作类型")]
        [SugarColumn(IsIgnore = true)]

        public int type { get; set; }

        /// <summary>
        ///     操作员
        /// </summary>
        [Display(Name = "操作员")]
        [SugarColumn(IsIgnore = true)]

        public int manager { get; set; }

        /// <summary>
        ///     备注
        /// </summary>
        [Display(Name = "备注")]
        [SugarColumn(IsIgnore = true)]
        public string memo { get; set; }

        /// <summary>
        ///     创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [SugarColumn(IsIgnore = true)]

        public DateTime createTime { get; set; }
    }
}