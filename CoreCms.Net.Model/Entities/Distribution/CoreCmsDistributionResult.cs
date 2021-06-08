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
    /// 等级佣金表
    /// </summary>
    [SugarTable("CoreCmsDistributionResult",TableDescription = "等级佣金表")]
    public partial class CoreCmsDistributionResult
    {
        /// <summary>
        /// 等级佣金表
        /// </summary>
        public CoreCmsDistributionResult()
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
        /// 会员等级Id
        /// </summary>
        [Display(Name = "会员等级Id")]
        [SugarColumn(ColumnDescription = "会员等级Id")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 gradeId { get; set; }
        /// <summary>
        /// 佣金编码
        /// </summary>
        [Display(Name = "佣金编码")]
        [SugarColumn(ColumnDescription = "佣金编码", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String code { get; set; }
        /// <summary>
        /// 佣金设置序列化参数
        /// </summary>
        [Display(Name = "佣金设置序列化参数")]
        [SugarColumn(ColumnDescription = "佣金设置序列化参数", IsNullable = true)]
        public System.String parameters { get; set; }
    }
}