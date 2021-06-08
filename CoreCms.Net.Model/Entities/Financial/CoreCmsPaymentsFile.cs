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
    /// 文件表
    /// </summary>
    [SugarTable("CoreCmsPaymentsFile",TableDescription = "文件表")]
    public partial class CoreCmsPaymentsFile
    {
        /// <summary>
        /// 文件表
        /// </summary>
        public CoreCmsPaymentsFile()
        {
        }

        /// <summary>
        /// 视频ID
        /// </summary>
        [Display(Name = "视频ID")]
        [SugarColumn(ColumnDescription = "视频ID", IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 id { get; set; }
        /// <summary>
        /// 视频名称
        /// </summary>
        [Display(Name = "视频名称")]
        [SugarColumn(ColumnDescription = "视频名称", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String name { get; set; }
        /// <summary>
        /// 绝对地址
        /// </summary>
        [Display(Name = "绝对地址")]
        [SugarColumn(ColumnDescription = "绝对地址", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String url { get; set; }
        /// <summary>
        /// 物理地址
        /// </summary>
        [Display(Name = "物理地址")]
        [SugarColumn(ColumnDescription = "物理地址", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String path { get; set; }
        /// <summary>
        /// 存储引擎
        /// </summary>
        [Display(Name = "存储引擎")]
        [SugarColumn(ColumnDescription = "存储引擎", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String type { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        [Display(Name = "文件类型")]
        [SugarColumn(ColumnDescription = "文件类型", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String fileType { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        [Display(Name = "是否删除")]
        [SugarColumn(ColumnDescription = "是否删除", IsNullable = true)]
        public System.Int32? isDel { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
        [SugarColumn(ColumnDescription = "", IsNullable = true)]
        public System.DateTime? createTime { get; set; }
    }
}