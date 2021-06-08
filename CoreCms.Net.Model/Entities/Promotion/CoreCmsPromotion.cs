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
    /// 促销表
    /// </summary>
    [SugarTable("CoreCmsPromotion",TableDescription = "促销表")]
    public partial class CoreCmsPromotion
    {
        /// <summary>
        /// 促销表
        /// </summary>
        public CoreCmsPromotion()
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
        /// 促销名称
        /// </summary>
        [Display(Name = "促销名称")]
        [SugarColumn(ColumnDescription = "促销名称")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(40, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String name { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        [Display(Name = "类型")]
        [SugarColumn(ColumnDescription = "类型")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 type { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [Display(Name = "排序")]
        [SugarColumn(ColumnDescription = "排序")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 sort { get; set; }
        /// <summary>
        /// 其它参数
        /// </summary>
        [Display(Name = "其它参数")]
        [SugarColumn(ColumnDescription = "其它参数", IsNullable = true)]
        public System.String parameters { get; set; }
        /// <summary>
        /// 每人限购数量
        /// </summary>
        [Display(Name = "每人限购数量")]
        [SugarColumn(ColumnDescription = "每人限购数量")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 maxNums { get; set; }
        /// <summary>
        /// 每个商品活动数量
        /// </summary>
        [Display(Name = "每个商品活动数量")]
        [SugarColumn(ColumnDescription = "每个商品活动数量")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 maxGoodsNums { get; set; }
        /// <summary>
        /// 最大领取数量
        /// </summary>
        [Display(Name = "最大领取数量")]
        [SugarColumn(ColumnDescription = "最大领取数量")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 maxRecevieNums { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        [Display(Name = "开始时间")]
        [SugarColumn(ColumnDescription = "开始时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.DateTime startTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        [Display(Name = "结束时间")]
        [SugarColumn(ColumnDescription = "结束时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.DateTime endTime { get; set; }
        /// <summary>
        /// 是否排他
        /// </summary>
        [Display(Name = "是否排他")]
        [SugarColumn(ColumnDescription = "是否排他")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Boolean isExclusive { get; set; }
        /// <summary>
        /// 是否自动领取
        /// </summary>
        [Display(Name = "是否自动领取")]
        [SugarColumn(ColumnDescription = "是否自动领取")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Boolean isAutoReceive { get; set; }
        /// <summary>
        /// 是否开启
        /// </summary>
        [Display(Name = "是否开启")]
        [SugarColumn(ColumnDescription = "是否开启")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Boolean isEnable { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        [Display(Name = "是否删除")]
        [SugarColumn(ColumnDescription = "是否删除")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Boolean isDel { get; set; }
        /// <summary>
        /// 有效天数
        /// </summary>
        [Display(Name = "有效天数")]
        [SugarColumn(ColumnDescription = "有效天数")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 effectiveDays { get; set; }
        /// <summary>
        /// 有效小时
        /// </summary>
        [Display(Name = "有效小时")]
        [SugarColumn(ColumnDescription = "有效小时")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 effectiveHours { get; set; }
    }
}