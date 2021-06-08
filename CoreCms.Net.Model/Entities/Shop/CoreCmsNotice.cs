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
    /// 公告表
    /// </summary>
    [SugarTable("CoreCmsNotice",TableDescription = "公告表")]
    public partial class CoreCmsNotice
    {
        /// <summary>
        /// 公告表
        /// </summary>
        public CoreCmsNotice()
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
        /// 公告标题
        /// </summary>
        [Display(Name = "公告标题")]
        [SugarColumn(ColumnDescription = "公告标题")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(200, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String title { get; set; }
        /// <summary>
        /// 公告内容
        /// </summary>
        [Display(Name = "公告内容")]
        [SugarColumn(ColumnDescription = "公告内容")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.String contentBody { get; set; }
        /// <summary>
        /// 公告类型
        /// </summary>
        [Display(Name = "公告类型")]
        [SugarColumn(ColumnDescription = "公告类型", IsNullable = true)]
        public System.Int32? type { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [Display(Name = "排序")]
        [SugarColumn(ColumnDescription = "排序", IsNullable = true)]
        public System.Int32? sort { get; set; }
        /// <summary>
        /// 软删除位  有时间代表已删除
        /// </summary>
        [Display(Name = "软删除位  有时间代表已删除")]
        [SugarColumn(ColumnDescription = "软删除位  有时间代表已删除", IsNullable = true)]
        public System.Boolean? isDel { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [SugarColumn(ColumnDescription = "创建时间", IsNullable = true)]
        public System.DateTime? createTime { get; set; }
    }
}