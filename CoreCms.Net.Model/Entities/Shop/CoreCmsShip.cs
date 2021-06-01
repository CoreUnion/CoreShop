/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using System.ComponentModel.DataAnnotations;
using SqlSugar;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    ///     配送方式表
    /// </summary>
    public partial class CoreCmsShip
    {
        /// <summary>
        ///     序列
        /// </summary>
        [Display(Name = "序列")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public int id { get; set; }

        /// <summary>
        ///     配送方式名称
        /// </summary>
        [Display(Name = "配送方式名称")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(25, ErrorMessage = "{0}不能超过{1}字")]
        public string name { get; set; }

        /// <summary>
        ///     是否货到付款
        /// </summary>
        [Display(Name = "是否货到付款")]
        [Required(ErrorMessage = "请输入{0}")]
        public bool isCashOnDelivery { get; set; }

        /// <summary>
        ///     首重
        /// </summary>
        [Display(Name = "首重")]
        [Required(ErrorMessage = "请输入{0}")]
        public int firstUnit { get; set; }

        /// <summary>
        ///     续重
        /// </summary>
        [Display(Name = "续重")]
        [Required(ErrorMessage = "请输入{0}")]
        public int continueUnit { get; set; }

        /// <summary>
        ///     是否按地区设置配送费用
        /// </summary>
        [Display(Name = "是否按地区设置配送费用")]
        [Required(ErrorMessage = "请输入{0}")]
        public bool isdefaultAreaFee { get; set; }

        /// <summary>
        ///     地区类型
        /// </summary>
        [Display(Name = "地区类型")]
        [Required(ErrorMessage = "请输入{0}")]
        public int areaType { get; set; }

        /// <summary>
        ///     首重费用
        /// </summary>
        [Display(Name = "首重费用")]
        [Required(ErrorMessage = "请输入{0}")]
        public decimal firstunitPrice { get; set; }

        /// <summary>
        ///     续重费用
        /// </summary>
        [Display(Name = "续重费用")]
        [Required(ErrorMessage = "请输入{0}")]
        public decimal continueunitPrice { get; set; }

        /// <summary>
        ///     配送费用计算表达式
        /// </summary>
        [Display(Name = "配送费用计算表达式")]
        public string exp { get; set; }

        /// <summary>
        ///     物流公司名称
        /// </summary>
        [Display(Name = "物流公司名称")]
        [StringLength(25, ErrorMessage = "{0}不能超过{1}字")]
        public string logiName { get; set; }

        /// <summary>
        ///     物流公司编码
        /// </summary>
        [Display(Name = "物流公司编码")]
        [StringLength(25, ErrorMessage = "{0}不能超过{1}字")]
        public string logiCode { get; set; }

        /// <summary>
        ///     是否默认
        /// </summary>
        [Display(Name = "是否默认")]
        [Required(ErrorMessage = "请输入{0}")]
        public bool isDefault { get; set; }

        /// <summary>
        ///     配送方式排序
        /// </summary>
        [Display(Name = "配送方式排序")]
        [Required(ErrorMessage = "请输入{0}")]
        public int sort { get; set; }

        /// <summary>
        ///     状态1正常2停用
        /// </summary>
        [Display(Name = "状态1正常2停用")]
        [Required(ErrorMessage = "请输入{0}")]
        public int status { get; set; }

        /// <summary>
        ///     是否包邮
        /// </summary>
        [Display(Name = "是否包邮")]
        [Required(ErrorMessage = "请输入{0}")]
        public bool isfreePostage { get; set; }

        /// <summary>
        ///     地区配送费用
        /// </summary>
        [Display(Name = "地区配送费用")]
        public string areaFee { get; set; }

        /// <summary>
        ///     商品总额满多少免运费
        /// </summary>
        [Display(Name = "商品总额满多少免运费")]
        [Required(ErrorMessage = "请输入{0}")]
        public decimal goodsMoney { get; set; }
    }
}