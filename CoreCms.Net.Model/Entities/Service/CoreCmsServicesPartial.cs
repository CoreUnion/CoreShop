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
    ///     服务项目表
    /// </summary>
    public partial class CoreCmsServices
    {
        /// <summary>
        ///     倒计时时间戳
        /// </summary>
        [Display(Name = "倒计时时间戳")]
        [SugarColumn(IsIgnore = true)]
        public int timestamp { get; set; } = 0;

        /// <summary>
        ///     允许购买用户等级
        /// </summary>
        [Display(Name = "允许购买用户等级")]
        [SugarColumn(IsIgnore = true)]
        public List<string> allowedMemberships { get; set; }

        /// <summary>
        ///     核销门店
        /// </summary>
        [Display(Name = "核销门店")]
        [SugarColumn(IsIgnore = true)]
        public List<string> consumableStores { get; set; }
    }
}