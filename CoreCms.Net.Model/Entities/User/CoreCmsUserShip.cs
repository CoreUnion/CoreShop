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
    ///     用户地址表
    /// </summary>
    public partial class CoreCmsUserShip
    {
        /// <summary>
        ///     序列
        /// </summary>
        [Display(Name = "序列")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public int id { get; set; }

        /// <summary>
        ///     用户id 关联user.id
        /// </summary>
        [Display(Name = "用户id 关联user.id")]
        [Required(ErrorMessage = "请输入{0}")]
        public int userId { get; set; }

        /// <summary>
        ///     收货地区ID
        /// </summary>
        [Display(Name = "收货地区ID")]
        [Required(ErrorMessage = "请输入{0}")]
        public int areaId { get; set; }

        /// <summary>
        ///     收货详细地址
        /// </summary>
        [Display(Name = "收货详细地址")]
        [StringLength(200, ErrorMessage = "{0}不能超过{1}字")]
        public string address { get; set; }

        /// <summary>
        ///     收货人姓名
        /// </summary>
        [Display(Name = "收货人姓名")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]
        public string name { get; set; }

        /// <summary>
        ///     收货电话
        /// </summary>
        [Display(Name = "收货电话")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]
        public string mobile { get; set; }

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
        public DateTime? createTime { get; set; }

        /// <summary>
        ///     更新时间
        /// </summary>
        [Display(Name = "更新时间")]
        public DateTime? updateTime { get; set; }
    }
}