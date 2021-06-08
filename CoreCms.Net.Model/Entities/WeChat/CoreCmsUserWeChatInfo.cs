/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com
 *         CreateTime: 2021-06-08 22:14:59
 *        Description: 暂无
***********************************************************************/ 
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    /// 用户表
    /// </summary>
    [SugarTable("CoreCmsUserWeChatInfo",TableDescription = "用户表")]
    public partial class CoreCmsUserWeChatInfo
    {
        /// <summary>
        /// 用户表
        /// </summary>
        public CoreCmsUserWeChatInfo()
        {
        }

        /// <summary>
        /// 用户ID
        /// </summary>
        [Display(Name = "用户ID")]
        [SugarColumn(ColumnDescription = "用户ID", IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 id { get; set; }
        /// <summary>
        /// 第三方登录类型
        /// </summary>
        [Display(Name = "第三方登录类型")]
        [SugarColumn(ColumnDescription = "第三方登录类型", IsNullable = true)]
        public System.Int32? type { get; set; }
        /// <summary>
        /// 关联用户表
        /// </summary>
        [Display(Name = "关联用户表")]
        [SugarColumn(ColumnDescription = "关联用户表")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 userId { get; set; }
        /// <summary>
        /// openId
        /// </summary>
        [Display(Name = "openId")]
        [SugarColumn(ColumnDescription = "openId", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String openid { get; set; }
        /// <summary>
        /// 缓存key
        /// </summary>
        [Display(Name = "缓存key")]
        [SugarColumn(ColumnDescription = "缓存key", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String sessionKey { get; set; }
        /// <summary>
        /// unionid
        /// </summary>
        [Display(Name = "unionid")]
        [SugarColumn(ColumnDescription = "unionid", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String unionId { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        [Display(Name = "头像")]
        [SugarColumn(ColumnDescription = "头像", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String avatar { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        [Display(Name = "昵称")]
        [SugarColumn(ColumnDescription = "昵称", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String nickName { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        [Display(Name = "性别")]
        [SugarColumn(ColumnDescription = "性别")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 gender { get; set; }
        /// <summary>
        /// 语言
        /// </summary>
        [Display(Name = "语言")]
        [SugarColumn(ColumnDescription = "语言", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String language { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        [Display(Name = "城市")]
        [SugarColumn(ColumnDescription = "城市", IsNullable = true)]
        [StringLength(80, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String city { get; set; }
        /// <summary>
        /// 省
        /// </summary>
        [Display(Name = "省")]
        [SugarColumn(ColumnDescription = "省", IsNullable = true)]
        [StringLength(80, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String province { get; set; }
        /// <summary>
        /// 国家
        /// </summary>
        [Display(Name = "国家")]
        [SugarColumn(ColumnDescription = "国家", IsNullable = true)]
        [StringLength(80, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String country { get; set; }
        /// <summary>
        /// 手机号码国家编码
        /// </summary>
        [Display(Name = "手机号码国家编码")]
        [SugarColumn(ColumnDescription = "手机号码国家编码", IsNullable = true)]
        [StringLength(20, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String countryCode { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        [Display(Name = "手机号码")]
        [SugarColumn(ColumnDescription = "手机号码", IsNullable = true)]
        [StringLength(20, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String mobile { get; set; }
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