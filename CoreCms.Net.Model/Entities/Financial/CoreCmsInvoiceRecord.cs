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
    /// 发票信息记录
    /// </summary>
    [SugarTable("CoreCmsInvoiceRecord",TableDescription = "发票信息记录")]
    public partial class CoreCmsInvoiceRecord
    {
        /// <summary>
        /// 发票信息记录
        /// </summary>
        public CoreCmsInvoiceRecord()
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
        /// 发票抬头
        /// </summary>
        [Display(Name = "发票抬头")]
        [SugarColumn(ColumnDescription = "发票抬头")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(80, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String name { get; set; }
        /// <summary>
        /// 发票税号
        /// </summary>
        [Display(Name = "发票税号")]
        [SugarColumn(ColumnDescription = "发票税号")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(30, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String code { get; set; }
        /// <summary>
        /// 被使用次数
        /// </summary>
        [Display(Name = "被使用次数")]
        [SugarColumn(ColumnDescription = "被使用次数")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 frequency { get; set; }
    }
}