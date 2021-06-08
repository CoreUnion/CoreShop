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
    /// 物流公司表
    /// </summary>
    [SugarTable("CoreCmsLogistics",TableDescription = "物流公司表")]
    public partial class CoreCmsLogistics
    {
        /// <summary>
        /// 物流公司表
        /// </summary>
        public CoreCmsLogistics()
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
        /// 物流公司名称
        /// </summary>
        [Display(Name = "物流公司名称")]
        [SugarColumn(ColumnDescription = "物流公司名称")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String logiName { get; set; }
        /// <summary>
        /// 物流公司编码
        /// </summary>
        [Display(Name = "物流公司编码")]
        [SugarColumn(ColumnDescription = "物流公司编码")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String logiCode { get; set; }
        /// <summary>
        /// 物流logo
        /// </summary>
        [Display(Name = "物流logo")]
        [SugarColumn(ColumnDescription = "物流logo", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String imgUrl { get; set; }
        /// <summary>
        /// 物流电话
        /// </summary>
        [Display(Name = "物流电话")]
        [SugarColumn(ColumnDescription = "物流电话", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String phone { get; set; }
        /// <summary>
        /// 物流网址
        /// </summary>
        [Display(Name = "物流网址")]
        [SugarColumn(ColumnDescription = "物流网址", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String url { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [Display(Name = "排序")]
        [SugarColumn(ColumnDescription = "排序")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 sort { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        [Display(Name = "是否删除")]
        [SugarColumn(ColumnDescription = "是否删除")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Boolean isDelete { get; set; }
    }
}