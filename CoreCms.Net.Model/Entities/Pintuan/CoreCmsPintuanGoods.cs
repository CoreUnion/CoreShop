/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using System.ComponentModel.DataAnnotations;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    ///     拼团商品表
    /// </summary>
    public class CoreCmsPinTuanGoods
    {
        /// <summary>
        ///     规则表序列
        /// </summary>
        [Display(Name = "规则表序列")]
        [Required(ErrorMessage = "请输入{0}")]
        public int ruleId { get; set; }

        /// <summary>
        ///     商品序列
        /// </summary>
        [Display(Name = "商品序列")]
        [Required(ErrorMessage = "请输入{0}")]
        public int goodsId { get; set; }
    }
}