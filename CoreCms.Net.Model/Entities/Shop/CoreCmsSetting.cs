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
    ///     店铺设置表
    /// </summary>
    public class CoreCmsSetting
    {
        /// <summary>
        ///     键
        /// </summary>
        [Display(Name = "键")]
        [SugarColumn(IsPrimaryKey = true)]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]
        public string sKey { get; set; }

        /// <summary>
        ///     值
        /// </summary>
        [Display(Name = "值")]
        public string sValue { get; set; }
    }
}