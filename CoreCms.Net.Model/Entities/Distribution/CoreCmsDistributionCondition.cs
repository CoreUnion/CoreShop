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
    /// 分销商等级升级条件
    /// </summary>
    [SugarTable("CoreCmsDistributionCondition",TableDescription = "分销商等级升级条件")]
    public partial class CoreCmsDistributionCondition
    {
        /// <summary>
        /// 分销商等级升级条件
        /// </summary>
        public CoreCmsDistributionCondition()
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
        /// 升级条件编码
        /// </summary>
        [Display(Name = "升级条件编码")]
        [SugarColumn(ColumnDescription = "升级条件编码", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String code { get; set; }
        /// <summary>
        /// 其它参数
        /// </summary>
        [Display(Name = "其它参数")]
        [SugarColumn(ColumnDescription = "其它参数", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String parameters { get; set; }
    }
}