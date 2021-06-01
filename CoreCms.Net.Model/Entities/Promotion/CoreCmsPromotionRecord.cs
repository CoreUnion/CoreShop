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
    ///     促销活动记录表
    /// </summary>
    public class CoreCmsPromotionRecord
    {
        /// <summary>
        ///     记录序列
        /// </summary>
        [Display(Name = "记录序列")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]


        public int id { get; set; }


        /// <summary>
        ///     促销序列
        /// </summary>
        [Display(Name = "促销序列")]
        [Required(ErrorMessage = "请输入{0}")]


        public int promotionId { get; set; }


        /// <summary>
        ///     用户Id
        /// </summary>
        [Display(Name = "用户Id")]
        [Required(ErrorMessage = "请输入{0}")]


        public int userId { get; set; }


        /// <summary>
        ///     商品id
        /// </summary>
        [Display(Name = "商品id")]
        [Required(ErrorMessage = "请输入{0}")]


        public int goodsId { get; set; }


        /// <summary>
        ///     货品id
        /// </summary>
        [Display(Name = "货品id")]
        [Required(ErrorMessage = "请输入{0}")]


        public int productId { get; set; }


        /// <summary>
        ///     订单id
        /// </summary>
        [Display(Name = "订单id")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(25, ErrorMessage = "{0}不能超过{1}字")]


        public string orderId { get; set; }


        /// <summary>
        ///     3团购/4秒杀
        /// </summary>
        [Display(Name = "3团购/4秒杀")]
        [Required(ErrorMessage = "请输入{0}")]


        public int type { get; set; }


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
        [Required(ErrorMessage = "请输入{0}")]


        public DateTime updateTime { get; set; }
    }
}