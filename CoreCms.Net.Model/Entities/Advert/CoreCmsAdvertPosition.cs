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
    /// 广告位置表
    /// </summary>
    [SugarTable("CoreCmsAdvertPosition",TableDescription = "广告位置表")]
    public partial class CoreCmsAdvertPosition
    {
        /// <summary>
        /// 广告位置表
        /// </summary>
        public CoreCmsAdvertPosition()
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
        /// 名称
        /// </summary>
        [Display(Name = "名称")]
        [SugarColumn(ColumnDescription = "名称")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(120, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String name { get; set; }
        /// <summary>
        /// 位置编码
        /// </summary>
        [Display(Name = "位置编码")]
        [SugarColumn(ColumnDescription = "位置编码")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(32, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String code { get; set; }
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
        /// 是否启用
        /// </summary>
        [Display(Name = "是否启用")]
        [SugarColumn(ColumnDescription = "是否启用")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Boolean isEnable { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [Display(Name = "排序")]
        [SugarColumn(ColumnDescription = "排序")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 sort { get; set; }
    }
}