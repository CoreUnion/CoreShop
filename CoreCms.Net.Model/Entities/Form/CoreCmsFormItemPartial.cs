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

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    ///     表单项表
    /// </summary>
    public partial class CoreCmsFormItem
    {
        /// <summary>
        ///     商品
        /// </summary>
        [Display(Name = "商品")]
        [SugarColumn(IsIgnore = true)]
        public CoreCmsGoods good { get; set; }

        /// <summary>
        ///     商品购物车数量
        /// </summary>
        [Display(Name = "商品购物车数量")]
        [SugarColumn(IsIgnore = true)]
        public int cartCount { get; set; } = 0;


        /// <summary>
        ///     复选内容
        /// </summary>
        [Display(Name = "复选内容")]
        [SugarColumn(IsIgnore = true)]
        public List<TempCheckbox> checkboxValue { get; set; }


        /// <summary>
        ///     单选内容
        /// </summary>
        [Display(Name = "单选内容")]
        [SugarColumn(IsIgnore = true)]
        public List<string> radioValue { get; set; }
    }

    /// <summary>
    ///     表单checkbox选中集合实体
    /// </summary>
    public class TempCheckbox
    {
        public bool @checked { get; set; }
        public string value { get; set; }
    }
}