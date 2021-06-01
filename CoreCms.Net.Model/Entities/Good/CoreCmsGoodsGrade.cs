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
    ///     商品会员价表
    /// </summary>
    public partial class CoreCmsGoodsGrade
    {
        /// <summary>
        ///     序列
        /// </summary>
        [Display(Name = "序列")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public int id { get; set; }

        /// <summary>
        ///     商品id
        /// </summary>
        [Display(Name = "商品id")]
        [Required(ErrorMessage = "请输入{0}")]
        public int goodsId { get; set; }

        /// <summary>
        ///     会员等级id
        /// </summary>
        [Display(Name = "会员等级id")]
        [Required(ErrorMessage = "请输入{0}")]
        public int gradeId { get; set; }

        /// <summary>
        ///     会员价
        /// </summary>
        [Display(Name = "会员价")]
        [Required(ErrorMessage = "请输入{0}")]
        public decimal gradePrice { get; set; }
    }
}