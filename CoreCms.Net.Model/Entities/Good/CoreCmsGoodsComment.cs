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
    /// 商品评价表
    /// </summary>
    [SugarTable("CoreCmsGoodsComment",TableDescription = "商品评价表")]
    public partial class CoreCmsGoodsComment
    {
        /// <summary>
        /// 商品评价表
        /// </summary>
        public CoreCmsGoodsComment()
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
        /// 父级评价ID
        /// </summary>
        [Display(Name = "父级评价ID")]
        [SugarColumn(ColumnDescription = "父级评价ID")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 commentId { get; set; }
        /// <summary>
        /// 评价1-5星
        /// </summary>
        [Display(Name = "评价1-5星")]
        [SugarColumn(ColumnDescription = "评价1-5星")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 score { get; set; }
        /// <summary>
        /// 评价用户ID
        /// </summary>
        [Display(Name = "评价用户ID")]
        [SugarColumn(ColumnDescription = "评价用户ID")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 userId { get; set; }
        /// <summary>
        /// 商品ID
        /// </summary>
        [Display(Name = "商品ID")]
        [SugarColumn(ColumnDescription = "商品ID")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 goodsId { get; set; }
        /// <summary>
        /// 评价订单ID
        /// </summary>
        [Display(Name = "评价订单ID")]
        [SugarColumn(ColumnDescription = "评价订单ID")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(32, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String orderId { get; set; }
        /// <summary>
        /// 货品规格序列号存储
        /// </summary>
        [Display(Name = "货品规格序列号存储")]
        [SugarColumn(ColumnDescription = "货品规格序列号存储", IsNullable = true)]
        public System.String addon { get; set; }
        /// <summary>
        /// 评价图片逗号分隔最多五张
        /// </summary>
        [Display(Name = "评价图片逗号分隔最多五张")]
        [SugarColumn(ColumnDescription = "评价图片逗号分隔最多五张", IsNullable = true)]
        public System.String images { get; set; }
        /// <summary>
        /// 评价内容
        /// </summary>
        [Display(Name = "评价内容")]
        [SugarColumn(ColumnDescription = "评价内容", IsNullable = true)]
        public System.String contentBody { get; set; }
        /// <summary>
        /// 商家回复
        /// </summary>
        [Display(Name = "商家回复")]
        [SugarColumn(ColumnDescription = "商家回复", IsNullable = true)]
        public System.String sellerContent { get; set; }
        /// <summary>
        /// 前台显示
        /// </summary>
        [Display(Name = "前台显示")]
        [SugarColumn(ColumnDescription = "前台显示")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Boolean isDisplay { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [SugarColumn(ColumnDescription = "创建时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.DateTime createTime { get; set; }
    }
}