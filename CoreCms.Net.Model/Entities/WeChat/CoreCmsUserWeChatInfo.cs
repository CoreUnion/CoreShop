/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using System;
using System.ComponentModel.DataAnnotations;
using SqlSugar;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    ///     用户表
    /// </summary>
    public class CoreCmsUserWeChatInfo
    {
        /// <summary>
        ///     用户ID
        /// </summary>
        [Display(Name = "用户ID")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public int id { get; set; }

        /// <summary>
        ///     第三方登录类型
        /// </summary>
        [Display(Name = "第三方登录类型")]
        public int? type { get; set; }

        /// <summary>
        ///     关联用户表
        /// </summary>
        [Display(Name = "关联用户表")]
        [Required(ErrorMessage = "请输入{0}")]
        public int userId { get; set; }

        /// <summary>
        ///     openId
        /// </summary>
        [Display(Name = "openId")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]
        public string openid { get; set; }

        /// <summary>
        ///     缓存key
        /// </summary>
        [Display(Name = "缓存key")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]
        public string sessionKey { get; set; }

        /// <summary>
        ///     unionid
        /// </summary>
        [Display(Name = "unionid")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]
        public string unionId { get; set; }

        /// <summary>
        ///     头像
        /// </summary>
        [Display(Name = "头像")]
        [StringLength(255, ErrorMessage = "{0}不能超过{1}字")]
        public string avatar { get; set; }

        /// <summary>
        ///     昵称
        /// </summary>
        [Display(Name = "昵称")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]
        public string nickName { get; set; }

        /// <summary>
        ///     性别
        /// </summary>
        [Display(Name = "性别")]
        [Required(ErrorMessage = "请输入{0}")]
        public int gender { get; set; }

        /// <summary>
        ///     语言
        /// </summary>
        [Display(Name = "语言")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]
        public string language { get; set; }

        /// <summary>
        ///     城市
        /// </summary>
        [Display(Name = "城市")]
        [StringLength(80, ErrorMessage = "{0}不能超过{1}字")]
        public string city { get; set; }

        /// <summary>
        ///     省
        /// </summary>
        [Display(Name = "省")]
        [StringLength(80, ErrorMessage = "{0}不能超过{1}字")]
        public string province { get; set; }

        /// <summary>
        ///     国家
        /// </summary>
        [Display(Name = "国家")]
        [StringLength(80, ErrorMessage = "{0}不能超过{1}字")]
        public string country { get; set; }

        /// <summary>
        ///     手机号码国家编码
        /// </summary>
        [Display(Name = "手机号码国家编码")]
        [StringLength(20, ErrorMessage = "{0}不能超过{1}字")]
        public string countryCode { get; set; }

        /// <summary>
        ///     手机号码
        /// </summary>
        [Display(Name = "手机号码")]
        [StringLength(20, ErrorMessage = "{0}不能超过{1}字")]
        public string mobile { get; set; }

        /// <summary>
        ///     创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        public DateTime? createTime { get; set; }

        /// <summary>
        ///     更新时间
        /// </summary>
        [Display(Name = "更新时间")]
        public DateTime? updateTime { get; set; }
    }
}