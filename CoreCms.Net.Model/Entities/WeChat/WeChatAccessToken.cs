/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统
 *                Web: https://www.corecms.net
 *             Author: 大灰灰
 *              Email: jianweie@163.com
 *         CreateTime: 2021/7/29 1:54:48
 *        Description: 暂无
 ***********************************************************************/

using System.ComponentModel.DataAnnotations;
using SqlSugar;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    ///     微信授权交互
    /// </summary>
    public class WeChatAccessToken
    {
        /// <summary>
        ///     序列
        /// </summary>
        [Display(Name = "序列")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public int id { get; set; }

        /// <summary>
        ///     类型1小程序2公众号
        /// </summary>
        [Display(Name = "类型1小程序2公众号")]
        [Required(ErrorMessage = "请输入{0}")]
        public int appType { get; set; }

        /// <summary>
        ///     微信appId
        /// </summary>
        [Display(Name = "微信appId")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]
        public string appId { get; set; }

        /// <summary>
        ///     微信accessToken
        /// </summary>
        [Display(Name = "微信accessToken")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(250, ErrorMessage = "{0}不能超过{1}字")]
        public string accessToken { get; set; }

        /// <summary>
        ///     截止时间
        /// </summary>
        [Display(Name = "截止时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public long expireTimestamp { get; set; }

        /// <summary>
        ///     更新时间
        /// </summary>
        [Display(Name = "更新时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public long updateTimestamp { get; set; }

        /// <summary>
        ///     创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public long createTimestamp { get; set; }
    }
}