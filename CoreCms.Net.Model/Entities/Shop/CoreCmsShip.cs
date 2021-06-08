/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com
 *         CreateTime: 2021-06-08 22:14:59
 *        Description: 暂无
***********************************************************************/ 
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    /// 配送方式表
    /// </summary>
    [SugarTable("CoreCmsShip",TableDescription = "配送方式表")]
    public partial class CoreCmsShip
    {
        /// <summary>
        /// 配送方式表
        /// </summary>
        public CoreCmsShip()
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
        /// 配送方式名称
        /// </summary>
        [Display(Name = "配送方式名称")]
        [SugarColumn(ColumnDescription = "配送方式名称")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String name { get; set; }
        /// <summary>
        /// 是否货到付款
        /// </summary>
        [Display(Name = "是否货到付款")]
        [SugarColumn(ColumnDescription = "是否货到付款")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Boolean isCashOnDelivery { get; set; }
        /// <summary>
        /// 首重
        /// </summary>
        [Display(Name = "首重")]
        [SugarColumn(ColumnDescription = "首重")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 firstUnit { get; set; }
        /// <summary>
        /// 续重
        /// </summary>
        [Display(Name = "续重")]
        [SugarColumn(ColumnDescription = "续重")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 continueUnit { get; set; }
        /// <summary>
        /// 是否按地区设置配送费用
        /// </summary>
        [Display(Name = "是否按地区设置配送费用")]
        [SugarColumn(ColumnDescription = "是否按地区设置配送费用")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Boolean isdefaultAreaFee { get; set; }
        /// <summary>
        /// 地区类型
        /// </summary>
        [Display(Name = "地区类型")]
        [SugarColumn(ColumnDescription = "地区类型")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 areaType { get; set; }
        /// <summary>
        /// 首重费用
        /// </summary>
        [Display(Name = "首重费用")]
        [SugarColumn(ColumnDescription = "首重费用")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Decimal firstunitPrice { get; set; }
        /// <summary>
        /// 续重费用
        /// </summary>
        [Display(Name = "续重费用")]
        [SugarColumn(ColumnDescription = "续重费用")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Decimal continueunitPrice { get; set; }
        /// <summary>
        /// 配送费用计算表达式
        /// </summary>
        [Display(Name = "配送费用计算表达式")]
        [SugarColumn(ColumnDescription = "配送费用计算表达式", IsNullable = true)]
        public System.String exp { get; set; }
        /// <summary>
        /// 物流公司名称
        /// </summary>
        [Display(Name = "物流公司名称")]
        [SugarColumn(ColumnDescription = "物流公司名称", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String logiName { get; set; }
        /// <summary>
        /// 物流公司编码
        /// </summary>
        [Display(Name = "物流公司编码")]
        [SugarColumn(ColumnDescription = "物流公司编码", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String logiCode { get; set; }
        /// <summary>
        /// 是否默认
        /// </summary>
        [Display(Name = "是否默认")]
        [SugarColumn(ColumnDescription = "是否默认")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Boolean isDefault { get; set; }
        /// <summary>
        /// 配送方式排序
        /// </summary>
        [Display(Name = "配送方式排序")]
        [SugarColumn(ColumnDescription = "配送方式排序")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 sort { get; set; }
        /// <summary>
        /// 状态1正常2停用
        /// </summary>
        [Display(Name = "状态1正常2停用")]
        [SugarColumn(ColumnDescription = "状态1正常2停用")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 status { get; set; }
        /// <summary>
        /// 是否包邮
        /// </summary>
        [Display(Name = "是否包邮")]
        [SugarColumn(ColumnDescription = "是否包邮")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Boolean isfreePostage { get; set; }
        /// <summary>
        /// 地区配送费用
        /// </summary>
        [Display(Name = "地区配送费用")]
        [SugarColumn(ColumnDescription = "地区配送费用", IsNullable = true)]
        public System.String areaFee { get; set; }
        /// <summary>
        /// 商品总额满多少免运费
        /// </summary>
        [Display(Name = "商品总额满多少免运费")]
        [SugarColumn(ColumnDescription = "商品总额满多少免运费")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Decimal goodsMoney { get; set; }
    }
}