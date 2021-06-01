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
    ///     表单
    /// </summary>
    public partial class CoreCmsForm
    {
        /// <summary>
        ///     表单字段
        /// </summary>
        [Display(Name = "表单字段")]
        [SugarColumn(IsIgnore = true)]
        public List<CoreCmsFormItem> Items { get; set; }
    }
}