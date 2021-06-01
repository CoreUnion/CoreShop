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
using System.ComponentModel.DataAnnotations;
using SqlSugar;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    ///     促销表
    /// </summary>
    public partial class CoreCmsPromotion
    {
        /// <summary>
        ///     已领取数量
        /// </summary>
        [Display(Name = "已领取数量")]
        [SugarColumn(IsIgnore = true)]
        public int getNumber { get; set; }


        /// <summary>
        ///     条件
        /// </summary>
        [Display(Name = "条件")]
        [SugarColumn(IsIgnore = true)]
        public string expression1 { get; set; }

        /// <summary>
        ///     结果
        /// </summary>
        [Display(Name = "结果")]
        [SugarColumn(IsIgnore = true)]
        public string expression2 { get; set; }


        /// <summary>
        ///     条件
        /// </summary>
        [Display(Name = "条件集合")]
        [SugarColumn(IsIgnore = true)]
        public List<string> conditions { get; set; } = new();

        /// <summary>
        ///     结果
        /// </summary>
        [Display(Name = "结果集合")]
        [SugarColumn(IsIgnore = true)]
        public List<string> results { get; set; } = new();

        /// <summary>
        ///     条件
        /// </summary>
        [Display(Name = "条件集合")]
        [SugarColumn(IsIgnore = true)]
        public List<CoreCmsPromotionCondition> promotionCondition { get; set; }

        /// <summary>
        ///     条件
        /// </summary>
        [Display(Name = "条件集合")]
        [SugarColumn(IsIgnore = true)]
        public List<CoreCmsPromotionResult> promotionResult { get; set; }
    }
}