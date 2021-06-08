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
    /// 商城服务说明
    /// </summary>
    [SugarTable("CoreCmsServiceDescription",TableDescription = "商城服务说明")]
    public partial class CoreCmsServiceDescription
    {
        /// <summary>
        /// 商城服务说明
        /// </summary>
        public CoreCmsServiceDescription()
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
        [StringLength(100, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String title { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        [Display(Name = "类型")]
        [SugarColumn(ColumnDescription = "类型")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 type { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [Display(Name = "描述")]
        [SugarColumn(ColumnDescription = "描述")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(500, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String description { get; set; }
        /// <summary>
        /// 是否展示
        /// </summary>
        [Display(Name = "是否展示")]
        [SugarColumn(ColumnDescription = "是否展示")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Boolean isShow { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [Display(Name = "排序")]
        [SugarColumn(ColumnDescription = "排序")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 sortId { get; set; }
    }
}