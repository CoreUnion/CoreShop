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
    /// 拼团规则表
    /// </summary>
    [SugarTable("CoreCmsPinTuanRule",TableDescription = "拼团规则表")]
    public partial class CoreCmsPinTuanRule
    {
        /// <summary>
        /// 拼团规则表
        /// </summary>
        public CoreCmsPinTuanRule()
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
        /// 活动名称
        /// </summary>
        [Display(Name = "活动名称")]
        [SugarColumn(ColumnDescription = "活动名称")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String name { get; set; }
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
        /// 人数2-10人
        /// </summary>
        [Display(Name = "人数2-10人")]
        [SugarColumn(ColumnDescription = "人数2-10人")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 peopleNumber { get; set; }
        /// <summary>
        /// 单位分钟
        /// </summary>
        [Display(Name = "单位分钟")]
        [SugarColumn(ColumnDescription = "单位分钟")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 significantInterval { get; set; }
        /// <summary>
        /// 优惠金额
        /// </summary>
        [Display(Name = "优惠金额")]
        [SugarColumn(ColumnDescription = "优惠金额")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Decimal discountAmount { get; set; }
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
        /// 排序
        /// </summary>
        [Display(Name = "排序")]
        [SugarColumn(ColumnDescription = "排序")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 sort { get; set; }
        /// <summary>
        /// 是否开启
        /// </summary>
        [Display(Name = "是否开启")]
        [SugarColumn(ColumnDescription = "是否开启")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Boolean isStatusOpen { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [SugarColumn(ColumnDescription = "创建时间", IsNullable = true)]
        public System.DateTime? createTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        [Display(Name = "更新时间")]
        [SugarColumn(ColumnDescription = "更新时间", IsNullable = true)]
        public System.DateTime? updateTime { get; set; }
    }
}