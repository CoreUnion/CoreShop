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
    /// 商品会员价表
    /// </summary>
    [SugarTable("CoreCmsGoodsGrade",TableDescription = "商品会员价表")]
    public partial class CoreCmsGoodsGrade
    {
        /// <summary>
        /// 商品会员价表
        /// </summary>
        public CoreCmsGoodsGrade()
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
        /// 商品id
        /// </summary>
        [Display(Name = "商品id")]
        [SugarColumn(ColumnDescription = "商品id")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 goodsId { get; set; }
        /// <summary>
        /// 会员等级id
        /// </summary>
        [Display(Name = "会员等级id")]
        [SugarColumn(ColumnDescription = "会员等级id")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 gradeId { get; set; }
        /// <summary>
        /// 会员价
        /// </summary>
        [Display(Name = "会员价")]
        [SugarColumn(ColumnDescription = "会员价")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Decimal gradePrice { get; set; }
    }
}