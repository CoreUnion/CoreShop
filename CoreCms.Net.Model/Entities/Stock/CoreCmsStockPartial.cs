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
    ///     库存操作表
    /// </summary>
    public partial class CoreCmsStock
    {
        /// <summary>
        ///     操作员昵称
        /// </summary>
        [Display(Name = "操作员昵称")]
        [SugarColumn(IsIgnore = true)]
        public string managerName { get; set; }
    }
}