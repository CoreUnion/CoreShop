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
    /// 商品类型属性值表
    /// </summary>
    [SugarTable("CoreCmsGoodsTypeSpecValue",TableDescription = "商品类型属性值表")]
    public partial class CoreCmsGoodsTypeSpecValue
    {
        /// <summary>
        /// 商品类型属性值表
        /// </summary>
        public CoreCmsGoodsTypeSpecValue()
        {
        }

        /// <summary>
        /// 序列
        /// </summary>
        [Display(Name = "序列")]
        [SugarColumn(ColumnDescription = "序列", IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 id { get; set; }
        /// <summary>
        /// 属性ID 关联goods_type_spec.id
        /// </summary>
        [Display(Name = "属性ID 关联goods_type_spec.id")]
        [SugarColumn(ColumnDescription = "属性ID 关联goods_type_spec.id")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 specId { get; set; }
        /// <summary>
        /// 属性值
        /// </summary>
        [Display(Name = "属性值")]
        [SugarColumn(ColumnDescription = "属性值")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String value { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [Display(Name = "排序")]
        [SugarColumn(ColumnDescription = "排序")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 sort { get; set; }
    }
}