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
    /// 商品参数表
    /// </summary>
    [SugarTable("CoreCmsGoodsParams",TableDescription = "商品参数表")]
    public partial class CoreCmsGoodsParams
    {
        /// <summary>
        /// 商品参数表
        /// </summary>
        public CoreCmsGoodsParams()
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
        /// 参数名称
        /// </summary>
        [Display(Name = "参数名称")]
        [SugarColumn(ColumnDescription = "参数名称", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String name { get; set; }
        /// <summary>
        /// 参数值
        /// </summary>
        [Display(Name = "参数值")]
        [SugarColumn(ColumnDescription = "参数值", IsNullable = true)]
        public System.String value { get; set; }
        /// <summary>
        /// 参数类型
        /// </summary>
        [Display(Name = "参数类型")]
        [SugarColumn(ColumnDescription = "参数类型", IsNullable = true)]
        [StringLength(10, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String type { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [SugarColumn(ColumnDescription = "创建时间", IsNullable = true)]
        public System.DateTime? createTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        [Display(Name = "更新时间")]
        [SugarColumn(ColumnDescription = "更新时间", IsNullable = true)]
        public System.DateTime? updateTime { get; set; }
    }
}