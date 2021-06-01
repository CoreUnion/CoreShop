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
    ///     商品类型和属性关联表
    /// </summary>
    public class CoreCmsGoodsTypeSpecRel
    {
        /// <summary>
        ///     属性ID
        /// </summary>
        [Display(Name = "属性ID")]
        [Required(ErrorMessage = "请输入{0}")]
        public int specId { get; set; }

        /// <summary>
        ///     类型ID
        /// </summary>
        [Display(Name = "类型ID")]
        [Required(ErrorMessage = "请输入{0}")]
        public int typeId { get; set; }
    }
}