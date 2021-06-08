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
    /// 服务项目表
    /// </summary>
    [SugarTable("CoreCmsServices",TableDescription = "服务项目表")]
    public partial class CoreCmsServices
    {
        /// <summary>
        /// 服务项目表
        /// </summary>
        public CoreCmsServices()
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
        /// 项目名称
        /// </summary>
        [Display(Name = "项目名称")]
        [SugarColumn(ColumnDescription = "项目名称")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String title { get; set; }
        /// <summary>
        /// 项目缩略图
        /// </summary>
        [Display(Name = "项目缩略图")]
        [SugarColumn(ColumnDescription = "项目缩略图")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String thumbnail { get; set; }
        /// <summary>
        /// 项目概述
        /// </summary>
        [Display(Name = "项目概述")]
        [SugarColumn(ColumnDescription = "项目概述", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String description { get; set; }
        /// <summary>
        /// 项目详细说明
        /// </summary>
        [Display(Name = "项目详细说明")]
        [SugarColumn(ColumnDescription = "项目详细说明")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.String contentBody { get; set; }
        /// <summary>
        /// 允许购买会员级别
        /// </summary>
        [Display(Name = "允许购买会员级别")]
        [SugarColumn(ColumnDescription = "允许购买会员级别")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String allowedMembership { get; set; }
        /// <summary>
        /// 可消费门店
        /// </summary>
        [Display(Name = "可消费门店")]
        [SugarColumn(ColumnDescription = "可消费门店")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String consumableStore { get; set; }
        /// <summary>
        /// 项目状态
        /// </summary>
        [Display(Name = "项目状态")]
        [SugarColumn(ColumnDescription = "项目状态")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 status { get; set; }
        /// <summary>
        /// 项目重复购买次数
        /// </summary>
        [Display(Name = "项目重复购买次数")]
        [SugarColumn(ColumnDescription = "项目重复购买次数")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 maxBuyNumber { get; set; }
        /// <summary>
        /// 项目可销售数量
        /// </summary>
        [Display(Name = "项目可销售数量")]
        [SugarColumn(ColumnDescription = "项目可销售数量")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 amount { get; set; }
        /// <summary>
        /// 项目开始时间
        /// </summary>
        [Display(Name = "项目开始时间")]
        [SugarColumn(ColumnDescription = "项目开始时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.DateTime startTime { get; set; }
        /// <summary>
        /// 项目截止时间
        /// </summary>
        [Display(Name = "项目截止时间")]
        [SugarColumn(ColumnDescription = "项目截止时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.DateTime endTime { get; set; }
        /// <summary>
        /// 核销有效期类型
        /// </summary>
        [Display(Name = "核销有效期类型")]
        [SugarColumn(ColumnDescription = "核销有效期类型")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 validityType { get; set; }
        /// <summary>
        /// 核销开始时间
        /// </summary>
        [Display(Name = "核销开始时间")]
        [SugarColumn(ColumnDescription = "核销开始时间", IsNullable = true)]
        public System.DateTime? validityStartTime { get; set; }
        /// <summary>
        /// 核销结束时间
        /// </summary>
        [Display(Name = "核销结束时间")]
        [SugarColumn(ColumnDescription = "核销结束时间", IsNullable = true)]
        public System.DateTime? validityEndTime { get; set; }
        /// <summary>
        /// 核销服务券数量
        /// </summary>
        [Display(Name = "核销服务券数量")]
        [SugarColumn(ColumnDescription = "核销服务券数量")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 ticketNumber { get; set; }
        /// <summary>
        /// 项目创建时间
        /// </summary>
        [Display(Name = "项目创建时间")]
        [SugarColumn(ColumnDescription = "项目创建时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.DateTime createTime { get; set; }
        /// <summary>
        /// 项目更新时间
        /// </summary>
        [Display(Name = "项目更新时间")]
        [SugarColumn(ColumnDescription = "项目更新时间", IsNullable = true)]
        public System.DateTime? updateTime { get; set; }
        /// <summary>
        /// 售价
        /// </summary>
        [Display(Name = "售价")]
        [SugarColumn(ColumnDescription = "售价")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Decimal money { get; set; }
    }
}