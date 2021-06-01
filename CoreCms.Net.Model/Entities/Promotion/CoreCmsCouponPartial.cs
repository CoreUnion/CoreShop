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
    ///     优惠券表
    /// </summary>
    public partial class CoreCmsCoupon
    {
        /// <summary>
        ///     优惠券名称
        /// </summary>
        [Display(Name = "优惠券名称")]
        [SugarColumn(IsIgnore = true)]
        public string couponName { get; set; }


        /// <summary>
        ///     领取用户姓名
        /// </summary>
        [Display(Name = "领取用户姓名")]
        [SugarColumn(IsIgnore = true)]
        public string userNickName { get; set; }

        /// <summary>
        ///     关联优惠内容
        /// </summary>
        [Display(Name = "关联优惠内容")]
        [SugarColumn(IsIgnore = true)]
        public CoreCmsPromotion promotion { get; set; }

        /// <summary>
        ///     满足明细
        /// </summary>
        [Display(Name = "满足明细")]
        [SugarColumn(IsIgnore = true)]
        public List<CoreCmsPromotionCondition> conditions { get; set; }


        /// <summary>
        ///     结果列表
        /// </summary>
        [Display(Name = "结果列表")]
        [SugarColumn(IsIgnore = true)]
        public List<CoreCmsPromotionResult> results { get; set; }
    }
}