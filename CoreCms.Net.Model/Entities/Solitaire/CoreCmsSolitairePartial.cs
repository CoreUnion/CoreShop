/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统
 *                Web: https://www.corecms.net
 *             Author: 大灰灰
 *              Email: jianweie@163.com
 *         CreateTime: 2021/6/14 23:17:57
 *        Description: 暂无
 ***********************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SqlSugar;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    ///     接龙活动表
    /// </summary>
    public partial class CoreCmsSolitaire
    {
        /// <summary>
        ///     货品明细
        /// </summary>
        [Display(Name = "货品明细")]
        [SqlSugar.SugarColumn(IsIgnore = true)]
        public List<CoreCmsSolitaireItems> items { get; set; }
    }
}