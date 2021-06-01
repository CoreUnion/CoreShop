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
    ///     购物车表
    /// </summary>
    public class CoreCmsCart
    {
        /// <summary>
        ///     序列
        /// </summary>
        [Display(Name = "序列")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public int id { get; set; }

        /// <summary>
        ///     用户序列
        /// </summary>
        [Display(Name = "用户序列")]
        [Required(ErrorMessage = "请输入{0}")]
        public int userId { get; set; }

        /// <summary>
        ///     货品序列
        /// </summary>
        [Display(Name = "货品序列")]
        [Required(ErrorMessage = "请输入{0}")]
        public int productId { get; set; }

        /// <summary>
        ///     货品数量
        /// </summary>
        [Display(Name = "货品数量")]
        [Required(ErrorMessage = "请输入{0}")]
        public int nums { get; set; }

        /// <summary>
        ///     购物车类型
        /// </summary>
        [Display(Name = "购物车类型")]
        [Required(ErrorMessage = "请输入{0}")]
        public int type { get; set; }
    }
}