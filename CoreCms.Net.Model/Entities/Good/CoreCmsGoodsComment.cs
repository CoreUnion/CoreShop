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
    ///     商品评价表
    /// </summary>
    public partial class CoreCmsGoodsComment
    {
        /// <summary>
        ///     序列
        /// </summary>
        [Display(Name = "序列")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public int id { get; set; }

        /// <summary>
        ///     父级评价ID
        /// </summary>
        [Display(Name = "父级评价ID")]
        [Required(ErrorMessage = "请输入{0}")]
        public int commentId { get; set; }

        /// <summary>
        ///     评价1-5星
        /// </summary>
        [Display(Name = "评价1-5星")]
        [Required(ErrorMessage = "请输入{0}")]
        public int score { get; set; }

        /// <summary>
        ///     评价用户ID
        /// </summary>
        [Display(Name = "评价用户ID")]
        [Required(ErrorMessage = "请输入{0}")]
        public int userId { get; set; }

        /// <summary>
        ///     商品ID
        /// </summary>
        [Display(Name = "商品ID")]
        [Required(ErrorMessage = "请输入{0}")]
        public int goodsId { get; set; }

        /// <summary>
        ///     评价订单ID
        /// </summary>
        [Display(Name = "评价订单ID")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(16, ErrorMessage = "{0}不能超过{1}字")]
        public string orderId { get; set; }

        /// <summary>
        ///     货品规格序列号存储
        /// </summary>
        [Display(Name = "货品规格序列号存储")]
        public string addon { get; set; }

        /// <summary>
        ///     评价图片逗号分隔最多五张
        /// </summary>
        [Display(Name = "评价图片逗号分隔最多五张")]
        public string images { get; set; }

        /// <summary>
        ///     评价内容
        /// </summary>
        [Display(Name = "评价内容")]
        public string contentBody { get; set; }

        /// <summary>
        ///     商家回复
        /// </summary>
        [Display(Name = "商家回复")]
        public string sellerContent { get; set; }

        /// <summary>
        ///     前台显示
        /// </summary>
        [Display(Name = "前台显示")]
        [Required(ErrorMessage = "请输入{0}")]
        public bool isDisplay { get; set; }

        /// <summary>
        ///     创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public DateTime createTime { get; set; }
    }
}