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

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    /// 商品图片关联表
    /// </summary>
    public partial class CoreCmsBillAftersalesImages
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CoreCmsBillAftersalesImages()
        {
        }

        /// <summary>
        /// 售后单id
        /// </summary>
        [Display(Name = "售后单id")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(maximumLength: 20, ErrorMessage = "{0}不能超过{1}字")]
        public System.String aftersalesId { get; set; }

        /// <summary>
        /// 图片地址
        /// </summary>
        [Display(Name = "图片地址")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(maximumLength: 255, ErrorMessage = "{0}不能超过{1}字")]
        public System.String imageUrl { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Display(Name = "排序")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 sortId { get; set; }
    }
}