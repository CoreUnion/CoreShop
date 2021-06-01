/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/


using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SqlSugar;

namespace CoreCms.Net.Model.FromBody
{
    /// <summary>
    ///     商品属性后端提交实体
    /// </summary>
    public class FmGoodsTypeSpecInsert
    {
        /// <summary>
        ///     属性名称
        /// </summary>
        [Display(Name = "属性名称")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(30, ErrorMessage = "{0}不能超过{1}字")]
        public string name { get; set; }

        /// <summary>
        ///     属性排序
        /// </summary>
        [Display(Name = "属性排序")]
        [Required(ErrorMessage = "请输入{0}")]
        public int sort { get; set; }

        /// <summary>
        ///     属性值
        /// </summary>
        [Display(Name = "属性值")]
        [Required(ErrorMessage = "请输入{0}")]
        public List<string> value { get; set; }
    }


    /// <summary>
    ///     商品属性后端提交实体
    /// </summary>
    public class FmGoodsTypeSpecUpdate
    {
        /// <summary>
        ///     序列
        /// </summary>
        [Display(Name = "序列")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public int id { get; set; }

        /// <summary>
        ///     属性名称
        /// </summary>
        [Display(Name = "属性名称")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(30, ErrorMessage = "{0}不能超过{1}字")]
        public string name { get; set; }

        /// <summary>
        ///     属性排序
        /// </summary>
        [Display(Name = "属性排序")]
        [Required(ErrorMessage = "请输入{0}")]
        public int sort { get; set; }

        /// <summary>
        ///     属性值
        /// </summary>
        [Display(Name = "属性值")]
        [Required(ErrorMessage = "请输入{0}")]
        public List<string> value { get; set; }
    }

    /// <summary>
    ///     商品类型增加实体
    /// </summary>
    public class FmGoodsTypeInsert
    {
        /// <summary>
        ///     属性名称
        /// </summary>
        [Display(Name = "属性名称")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(30, ErrorMessage = "{0}不能超过{1}字")]
        public string name { get; set; }

        /// <summary>
        ///     参数集合
        /// </summary>
        public List<TypeParams> parameters { get; set; }

        /// <summary>
        ///     属性集合
        /// </summary>
        public List<TypeAttributes> types { get; set; }
    }

    public class TypeParams
    {
        public int paramsId { get; set; }
        public string paramsName { get; set; }
        public string paramsType { get; set; }
        public string paramsValue { get; set; }
    }


    public class TypeAttributes
    {
        public int typeId { get; set; }
        public string typeName { get; set; }
        public string typeType { get; set; }
        public string typeValue { get; set; }
    }
}