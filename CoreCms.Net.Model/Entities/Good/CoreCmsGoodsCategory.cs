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
    ///     商品分类
    /// </summary>
    public partial class CoreCmsGoodsCategory
    {
        /// <summary>
        ///     序列
        /// </summary>
        [Display(Name = "序列")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public int id { get; set; }

        /// <summary>
        ///     上级分类id
        /// </summary>
        [Display(Name = "上级分类id")]
        [Required(ErrorMessage = "请输入{0}")]
        public int parentId { get; set; }

        /// <summary>
        ///     分类名称
        /// </summary>
        [Display(Name = "分类名称")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(20, ErrorMessage = "{0}不能超过{1}字")]
        public string name { get; set; }

        /// <summary>
        ///     类型ID 关联 goods_type.id
        /// </summary>
        [Display(Name = "类型ID 关联 goods_type.id")]
        [Required(ErrorMessage = "请输入{0}")]
        public int typeId { get; set; }

        /// <summary>
        ///     分类排序
        /// </summary>
        [Display(Name = "分类排序")]
        [Required(ErrorMessage = "请输入{0}")]
        public int sort { get; set; }

        /// <summary>
        ///     分类图片ID
        /// </summary>
        [Display(Name = "分类图片ID")]
        [StringLength(255, ErrorMessage = "{0}不能超过{1}字")]
        public string imageUrl { get; set; }

        /// <summary>
        ///     是否显示
        /// </summary>
        [Display(Name = "是否显示")]
        [Required(ErrorMessage = "请输入{0}")]
        public bool isShow { get; set; }

        /// <summary>
        ///     创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        public DateTime? createTime { get; set; }
    }
}