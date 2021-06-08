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
    /// 获取授权方的帐号基本信息表
    /// </summary>
    [SugarTable("CoreCmsWeixinAuthor",TableDescription = "获取授权方的帐号基本信息表")]
    public partial class CoreCmsWeixinAuthor
    {
        /// <summary>
        /// 获取授权方的帐号基本信息表
        /// </summary>
        public CoreCmsWeixinAuthor()
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
        /// 授权方昵称
        /// </summary>
        [Display(Name = "授权方昵称")]
        [SugarColumn(ColumnDescription = "授权方昵称", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String nickName { get; set; }
        /// <summary>
        /// 授权方头像
        /// </summary>
        [Display(Name = "授权方头像")]
        [SugarColumn(ColumnDescription = "授权方头像", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String headImg { get; set; }
        /// <summary>
        /// 默认为0
        /// </summary>
        [Display(Name = "默认为0")]
        [SugarColumn(ColumnDescription = "默认为0", IsNullable = true)]
        [StringLength(10, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String serviceTypeInfo { get; set; }
        /// <summary>
        /// 授权方认证类型
        /// </summary>
        [Display(Name = "授权方认证类型")]
        [SugarColumn(ColumnDescription = "授权方认证类型", IsNullable = true)]
        public System.Int32? verifyTypeInfo { get; set; }
        /// <summary>
        /// 小程序的原始ID
        /// </summary>
        [Display(Name = "小程序的原始ID")]
        [SugarColumn(ColumnDescription = "小程序的原始ID", IsNullable = true)]
        [StringLength(200, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String userName { get; set; }
        /// <summary>
        /// 帐号介绍
        /// </summary>
        [Display(Name = "帐号介绍")]
        [SugarColumn(ColumnDescription = "帐号介绍", IsNullable = true)]
        public System.String signature { get; set; }
        /// <summary>
        /// 小程序的主体名称
        /// </summary>
        [Display(Name = "小程序的主体名称")]
        [SugarColumn(ColumnDescription = "小程序的主体名称", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String principalName { get; set; }
        /// <summary>
        /// 功能的开通状况（0代表未开通，1代表已开通）： open_store:是否开通微信门店功能 open_scan:是否开通微信扫商品功能 open_pay:是否开通微信支付功能 open_card:是否开通微信卡券功能 open_shake:是否开通微信摇一摇功能
        /// </summary>
        [Display(Name = "功能的开通状况（0代表未开通，1代表已开通）： open_store:是否开通微信门店功能 open_scan:是否开通微信扫商品功能 open_pay:是否开通微信支付功能 open_card:是否开通微信卡券功能 open_shake:是否开通微信摇一摇功能")]
        [SugarColumn(ColumnDescription = "功能的开通状况（0代表未开通，1代表已开通）： open_store:是否开通微信门店功能 open_scan:是否开通微信扫商品功能 open_pay:是否开通微信支付功能 open_card:是否开通微信卡券功能 open_shake:是否开通微信摇一摇功能", IsNullable = true)]
        public System.String businessInfo { get; set; }
        /// <summary>
        /// 二维码图片的URL
        /// </summary>
        [Display(Name = "二维码图片的URL")]
        [SugarColumn(ColumnDescription = "二维码图片的URL", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String qrcodeUrl { get; set; }
        /// <summary>
        /// 授权信息
        /// </summary>
        [Display(Name = "授权信息")]
        [SugarColumn(ColumnDescription = "授权信息", IsNullable = true)]
        public System.String authorizationInfo { get; set; }
        /// <summary>
        /// 授权方appid
        /// </summary>
        [Display(Name = "授权方appid")]
        [SugarColumn(ColumnDescription = "授权方appid", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String appId { get; set; }
        /// <summary>
        /// 授权方AppSecret
        /// </summary>
        [Display(Name = "授权方AppSecret")]
        [SugarColumn(ColumnDescription = "授权方AppSecret", IsNullable = true)]
        [StringLength(100, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String appSecret { get; set; }
        /// <summary>
        /// 可根据这个字段判断是否为小程序类型授权,有值为小程序
        /// </summary>
        [Display(Name = "可根据这个字段判断是否为小程序类型授权,有值为小程序")]
        [SugarColumn(ColumnDescription = "可根据这个字段判断是否为小程序类型授权,有值为小程序", IsNullable = true)]
        public System.String miniprograminfo { get; set; }
        /// <summary>
        /// 小程序授权给开发者的权限集列表，ID为17到19时分别代表： 17.帐号管理权限 18.开发管理权限 19.客服消息管理权限 请注意： 1）该字段的返回不会考虑小程序是否具备该权限集的权限（因为可能部分具备）
        /// </summary>
        [Display(Name = "小程序授权给开发者的权限集列表，ID为17到19时分别代表： 17.帐号管理权限 18.开发管理权限 19.客服消息管理权限 请注意： 1）该字段的返回不会考虑小程序是否具备该权限集的权限（因为可能部分具备）")]
        [SugarColumn(ColumnDescription = "小程序授权给开发者的权限集列表，ID为17到19时分别代表： 17.帐号管理权限 18.开发管理权限 19.客服消息管理权限 请注意： 1）该字段的返回不会考虑小程序是否具备该权限集的权限（因为可能部分具备）", IsNullable = true)]
        public System.String funcInfo { get; set; }
        /// <summary>
        /// 刷新token
        /// </summary>
        [Display(Name = "刷新token")]
        [SugarColumn(ColumnDescription = "刷新token", IsNullable = true)]
        [StringLength(200, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String authorizerRefreshToken { get; set; }
        /// <summary>
        /// token
        /// </summary>
        [Display(Name = "token")]
        [SugarColumn(ColumnDescription = "token", IsNullable = true)]
        [StringLength(200, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String authorizerAccessToken { get; set; }
        /// <summary>
        /// 绑定类型，1为第三方授权绑定，2为自助绑定
        /// </summary>
        [Display(Name = "绑定类型，1为第三方授权绑定，2为自助绑定")]
        [SugarColumn(ColumnDescription = "绑定类型，1为第三方授权绑定，2为自助绑定", IsNullable = true)]
        public System.Int32? bindType { get; set; }
        /// <summary>
        /// 授权类型，默认b2c
        /// </summary>
        [Display(Name = "授权类型，默认b2c")]
        [SugarColumn(ColumnDescription = "授权类型，默认b2c", IsNullable = true)]
        [StringLength(10, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String authorType { get; set; }
        /// <summary>
        /// 绑定授权到期时间
        /// </summary>
        [Display(Name = "绑定授权到期时间")]
        [SugarColumn(ColumnDescription = "绑定授权到期时间", IsNullable = true)]
        public System.Int32? expiresIn { get; set; }
        /// <summary>
        /// 小程序授权时间
        /// </summary>
        [Display(Name = "小程序授权时间")]
        [SugarColumn(ColumnDescription = "小程序授权时间", IsNullable = true)]
        public System.DateTime? createTime { get; set; }
        /// <summary>
        /// 小程序更新时间
        /// </summary>
        [Display(Name = "小程序更新时间")]
        [SugarColumn(ColumnDescription = "小程序更新时间", IsNullable = true)]
        public System.DateTime? updateTime { get; set; }
    }
}