/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using System;
using System.ComponentModel.DataAnnotations;
using SqlSugar;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    ///     门店表
    /// </summary>
    public partial class CoreCmsStore
    {
        /// <summary>
        ///     序列
        /// </summary>
        [Display(Name = "序列")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public int id { get; set; }

        /// <summary>
        ///     门店名称
        /// </summary>
        [Display(Name = "门店名称")]
        [StringLength(125, ErrorMessage = "{0}不能超过{1}字")]
        public string storeName { get; set; }

        /// <summary>
        ///     门店电话/手机号
        /// </summary>
        [Display(Name = "门店电话/手机号")]
        [StringLength(13, ErrorMessage = "{0}不能超过{1}字")]
        public string mobile { get; set; }

        /// <summary>
        ///     门店联系人
        /// </summary>
        [Display(Name = "门店联系人")]
        [StringLength(32, ErrorMessage = "{0}不能超过{1}字")]
        public string linkMan { get; set; }

        /// <summary>
        ///     门店logo
        /// </summary>
        [Display(Name = "门店logo")]
        [StringLength(255, ErrorMessage = "{0}不能超过{1}字")]
        public string logoImage { get; set; }

        /// <summary>
        ///     门店地区id
        /// </summary>
        [Display(Name = "门店地区id")]
        [Required(ErrorMessage = "请输入{0}")]
        public int areaId { get; set; }

        /// <summary>
        ///     门店详细地址
        /// </summary>
        [Display(Name = "门店详细地址")]
        [StringLength(200, ErrorMessage = "{0}不能超过{1}字")]
        public string address { get; set; }

        /// <summary>
        ///     坐标位置
        /// </summary>
        [Display(Name = "坐标位置")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]
        public string coordinate { get; set; }

        /// <summary>
        ///     纬度
        /// </summary>
        [Display(Name = "纬度")]
        [StringLength(40, ErrorMessage = "{0}不能超过{1}字")]
        public string latitude { get; set; }

        /// <summary>
        ///     经度
        /// </summary>
        [Display(Name = "经度")]
        [StringLength(40, ErrorMessage = "{0}不能超过{1}字")]
        public string longitude { get; set; }

        /// <summary>
        ///     是否默认
        /// </summary>
        [Display(Name = "是否默认")]
        [Required(ErrorMessage = "请输入{0}")]
        public bool isDefault { get; set; }

        /// <summary>
        ///     创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public DateTime createTime { get; set; }

        /// <summary>
        ///     更新时间
        /// </summary>
        [Display(Name = "更新时间")]
        public DateTime? updateTime { get; set; }


        /// <summary>
        ///     距离
        /// </summary>
        [Display(Name = "距离")]
        [Required(ErrorMessage = "请输入{0}")]
        public decimal distance { get; set; }
    }
}