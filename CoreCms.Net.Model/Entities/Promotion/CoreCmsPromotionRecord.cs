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
    /// 促销活动记录表
    /// </summary>
    [SugarTable("CoreCmsPromotionRecord",TableDescription = "促销活动记录表")]
    public partial class CoreCmsPromotionRecord
    {
        /// <summary>
        /// 促销活动记录表
        /// </summary>
        public CoreCmsPromotionRecord()
        {
        }

        /// <summary>
        /// 记录序列
        /// </summary>
        [Display(Name = "记录序列")]
        [SugarColumn(ColumnDescription = "记录序列", IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 id { get; set; }
        /// <summary>
        /// 促销序列
        /// </summary>
        [Display(Name = "促销序列")]
        [SugarColumn(ColumnDescription = "促销序列")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 promotionId { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        [Display(Name = "用户Id")]
        [SugarColumn(ColumnDescription = "用户Id")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 userId { get; set; }
        /// <summary>
        /// 商品id
        /// </summary>
        [Display(Name = "商品id")]
        [SugarColumn(ColumnDescription = "商品id")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 goodsId { get; set; }
        /// <summary>
        /// 货品id
        /// </summary>
        [Display(Name = "货品id")]
        [SugarColumn(ColumnDescription = "货品id")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 productId { get; set; }
        /// <summary>
        /// 订单id
        /// </summary>
        [Display(Name = "订单id")]
        [SugarColumn(ColumnDescription = "订单id")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String orderId { get; set; }
        /// <summary>
        /// 3团购/4秒杀
        /// </summary>
        [Display(Name = "3团购/4秒杀")]
        [SugarColumn(ColumnDescription = "3团购/4秒杀")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 type { get; set; }
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
        [SugarColumn(ColumnDescription = "更新时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.DateTime updateTime { get; set; }
    }
}