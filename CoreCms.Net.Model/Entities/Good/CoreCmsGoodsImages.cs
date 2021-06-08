/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com
 *         CreateTime: 2021-06-08 22:14:58
 *        Description: 暂无
***********************************************************************/ 
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    /// 商品图片关联表
    /// </summary>
    [SugarTable("CoreCmsGoodsImages",TableDescription = "商品图片关联表")]
    public partial class CoreCmsGoodsImages
    {
        /// <summary>
        /// 商品图片关联表
        /// </summary>
        public CoreCmsGoodsImages()
        {
        }

        /// <summary>
        /// 商品ID
        /// </summary>
        [Display(Name = "商品ID")]
        [SugarColumn(ColumnDescription = "商品ID")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 goodsId { get; set; }
        /// <summary>
        /// 图片ID
        /// </summary>
        [Display(Name = "图片ID")]
        [SugarColumn(ColumnDescription = "图片ID")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String imageId { get; set; }
        /// <summary>
        /// 图片排序
        /// </summary>
        [Display(Name = "图片排序")]
        [SugarColumn(ColumnDescription = "图片排序")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 sort { get; set; }
    }
}