/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using System.ComponentModel.DataAnnotations;
using SqlSugar;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    ///     服务购买表
    /// </summary>
    public partial class CoreCmsUserServicesOrder
    {
        /// <summary>
        ///     关联服务
        /// </summary>
        [Display(Name = "关联服务")]
        [SugarColumn(IsIgnore = true)]
        public CoreCmsServices service { get; set; }


        /// <summary>
        ///     状态说明
        /// </summary>
        [Display(Name = "状态说明")]
        [SugarColumn(IsIgnore = true)]
        public string statusStr { get; set; }


        /// <summary>
        ///     服务标题
        /// </summary>
        [Display(Name = "服务标题")]
        [SugarColumn(IsIgnore = true)]
        public string serviceTitle { get; set; }


        /// <summary>
        ///     用户昵称
        /// </summary>
        [Display(Name = "用户昵称")]
        [SugarColumn(IsIgnore = true)]
        public string userName { get; set; }


    }
}