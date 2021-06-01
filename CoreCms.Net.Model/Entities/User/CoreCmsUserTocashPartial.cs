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
    ///     用户提现表
    /// </summary>
    public partial class CoreCmsUserTocash
    {
        /// <summary>
        ///     状态说明
        /// </summary>
        [Display(Name = "状态说明")]
        [SugarColumn(IsIgnore = true)]
        public string statusName { get; set; }


        /// <summary>
        ///     用户昵称
        /// </summary>
        [Display(Name = "用户昵称")]
        [SugarColumn(IsIgnore = true)]
        public string userNickName { get; set; }
    }
}