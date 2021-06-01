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
    ///     商品收藏表
    /// </summary>
    public partial class CoreCmsGoodsCollection
    {
        /// <summary>
        ///     ID
        /// </summary>
        [Display(Name = "ID")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public int id { get; set; }

        /// <summary>
        ///     商品id 关联goods.id
        /// </summary>
        [Display(Name = "商品id 关联goods.id")]
        [Required(ErrorMessage = "请输入{0}")]
        public int goodsId { get; set; }

        /// <summary>
        ///     用户id
        /// </summary>
        [Display(Name = "用户id")]
        [Required(ErrorMessage = "请输入{0}")]
        public int userId { get; set; }

        /// <summary>
        ///     商品名称
        /// </summary>
        [Display(Name = "商品名称")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(200, ErrorMessage = "{0}不能超过{1}字")]
        public string goodsName { get; set; }

        /// <summary>
        ///     创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public DateTime createTime { get; set; }
    }
}