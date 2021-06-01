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
    ///     分销商等级升级条件
    /// </summary>
    public partial class CoreCmsDistributionCondition
    {
        /// <summary>
        ///     Code转码
        /// </summary>
        [Display(Name = "Code转码")]
        [SugarColumn(IsIgnore = true)]
        public string name { get; set; }
    }
}