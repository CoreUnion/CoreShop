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
    /// 商品类型
    /// </summary>
    [SugarTable("CoreCmsGoodsType",TableDescription = "商品类型")]
    public partial class CoreCmsGoodsType
    {
        /// <summary>
        /// 商品类型
        /// </summary>
        public CoreCmsGoodsType()
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
        /// 类型名称
        /// </summary>
        [Display(Name = "类型名称")]
        [SugarColumn(ColumnDescription = "类型名称", IsNullable = true)]
        [StringLength(20, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String name { get; set; }
        /// <summary>
        /// 参数序列号数组
        /// </summary>
        [Display(Name = "参数序列号数组")]
        [SugarColumn(ColumnDescription = "参数序列号数组", IsNullable = true)]
        public System.String parameters { get; set; }
    }
}