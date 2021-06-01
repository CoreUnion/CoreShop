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
    ///     评论
    /// </summary>
    public partial class CoreCmsGoodsComment
    {
        /// <summary>
        ///     图集数组
        /// </summary>
        [Display(Name = "图集数组")]
        [SugarColumn(IsIgnore = true)]
        public string[] imagesArr { get; set; }

        /// <summary>
        ///     用户头像
        /// </summary>
        [Display(Name = "用户头像")]
        [SugarColumn(IsIgnore = true)]
        public string avatarImage { get; set; }

        /// <summary>
        ///     用户昵称
        /// </summary>
        [Display(Name = "用户昵称")]
        [SugarColumn(IsIgnore = true)]
        public string nickName { get; set; }

        /// <summary>
        ///     用户手机
        /// </summary>
        [Display(Name = "用户手机")]
        [SugarColumn(IsIgnore = true)]
        public string mobile { get; set; }

        /// <summary>
        ///     商品名称
        /// </summary>
        [Display(Name = "商品名称")]
        [SugarColumn(IsIgnore = true)]
        public string goodName { get; set; }
    }
}