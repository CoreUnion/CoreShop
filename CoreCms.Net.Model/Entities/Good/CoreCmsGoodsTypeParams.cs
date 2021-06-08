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
    /// 商品参数类型关系表
    /// </summary>
    [SugarTable("CoreCmsGoodsTypeParams",TableDescription = "商品参数类型关系表")]
    public partial class CoreCmsGoodsTypeParams
    {
        /// <summary>
        /// 商品参数类型关系表
        /// </summary>
        public CoreCmsGoodsTypeParams()
        {
        }

        /// <summary>
        /// 商品参数id
        /// </summary>
        [Display(Name = "商品参数id")]
        [SugarColumn(ColumnDescription = "商品参数id")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 paramsId { get; set; }
        /// <summary>
        /// 商品类型id
        /// </summary>
        [Display(Name = "商品类型id")]
        [SugarColumn(ColumnDescription = "商品类型id")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 typeId { get; set; }
    }
}