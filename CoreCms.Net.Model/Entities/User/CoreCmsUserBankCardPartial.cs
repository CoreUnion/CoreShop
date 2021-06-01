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
using SqlSugar;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    ///     用户银行卡
    /// </summary>
    public partial class CoreCmsUserBankCard
    {
        /// <summary>
        ///     区域名称
        /// </summary>
        [Display(Name = "区域名称")]
        [SugarColumn(IsIgnore = true)]
        public string bankAreaName { get; set; }

        /// <summary>
        ///     银行卡类型
        /// </summary>
        [Display(Name = "银行卡类型")]
        [SugarColumn(IsIgnore = true)]
        public string cardTypeName { get; set; }

        /// <summary>
        ///     银行图片
        /// </summary>
        [Display(Name = "银行图片")]
        [SugarColumn(IsIgnore = true)]
        public string bankLogo { get; set; }
    }
}