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
    /// 广告表
    /// </summary>
    [SugarTable("CoreCmsAdvertisement", TableDescription = "广告表")]
    public partial class CoreCmsAdvertisement
    {
        /// <summary>
        /// 广告表
        /// </summary>
        public CoreCmsAdvertisement()
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
        /// 位置序列
        /// </summary>
        [Display(Name = "位置序列")]
        [SugarColumn(ColumnDescription = "位置序列")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 positionId { get; set; }
        /// <summary>
        /// 广告名称
        /// </summary>
        [Display(Name = "广告名称")]
        [SugarColumn(ColumnDescription = "广告名称")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String name { get; set; }
        /// <summary>
        /// 广告图片id
        /// </summary>
        [Display(Name = "广告图片id")]
        [SugarColumn(ColumnDescription = "广告图片id")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String imageUrl { get; set; }
        /// <summary>
        /// 属性值
        /// </summary>
        [Display(Name = "属性值")]
        [SugarColumn(ColumnDescription = "属性值")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String val { get; set; }
        /// <summary>
        /// 属性值说明
        /// </summary>
        [Display(Name = "属性值说明")]
        [SugarColumn(ColumnDescription = "属性值说明", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String valDes { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [Display(Name = "排序")]
        [SugarColumn(ColumnDescription = "排序")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 sort { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        [Display(Name = "添加时间")]
        [SugarColumn(ColumnDescription = "添加时间", IsNullable = true)]
        public System.DateTime? createTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        [Display(Name = "更新时间")]
        [SugarColumn(ColumnDescription = "更新时间", IsNullable = true)]
        public System.DateTime? updateTime { get; set; }
        /// <summary>
        /// 广告位置编码
        /// </summary>
        [Display(Name = "广告位置编码")]
        [SugarColumn(ColumnDescription = "广告位置编码", IsNullable = true)]
        [StringLength(32, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String code { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        [Display(Name = "类型")]
        [SugarColumn(ColumnDescription = "类型")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 type { get; set; }
    }
}