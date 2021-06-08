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
    /// 拼团记录表
    /// </summary>
    [SugarTable("CoreCmsPinTuanRecord",TableDescription = "拼团记录表")]
    public partial class CoreCmsPinTuanRecord
    {
        /// <summary>
        /// 拼团记录表
        /// </summary>
        public CoreCmsPinTuanRecord()
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
        /// 团序列
        /// </summary>
        [Display(Name = "团序列")]
        [SugarColumn(ColumnDescription = "团序列")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 teamId { get; set; }
        /// <summary>
        /// 用户序列
        /// </summary>
        [Display(Name = "用户序列")]
        [SugarColumn(ColumnDescription = "用户序列")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 userId { get; set; }
        /// <summary>
        /// 规则表序列
        /// </summary>
        [Display(Name = "规则表序列")]
        [SugarColumn(ColumnDescription = "规则表序列")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 ruleId { get; set; }
        /// <summary>
        /// 商品序列
        /// </summary>
        [Display(Name = "商品序列")]
        [SugarColumn(ColumnDescription = "商品序列")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 goodsId { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        [Display(Name = "状态")]
        [SugarColumn(ColumnDescription = "状态")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 status { get; set; }
        /// <summary>
        /// 订单序列
        /// </summary>
        [Display(Name = "订单序列")]
        [SugarColumn(ColumnDescription = "订单序列")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(20, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String orderId { get; set; }
        /// <summary>
        /// 拼团人数Json
        /// </summary>
        [Display(Name = "拼团人数Json")]
        [SugarColumn(ColumnDescription = "拼团人数Json", IsNullable = true)]
        [StringLength(200, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String parameters { get; set; }
        /// <summary>
        /// 关闭时间
        /// </summary>
        [Display(Name = "关闭时间")]
        [SugarColumn(ColumnDescription = "关闭时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.DateTime closeTime { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [SugarColumn(ColumnDescription = "创建时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.DateTime createTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        [Display(Name = "更新时间")]
        [SugarColumn(ColumnDescription = "更新时间", IsNullable = true)]
        public System.DateTime? updateTime { get; set; }
    }
}