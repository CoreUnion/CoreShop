/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    /// 第三方授权记录表
    /// </summary>
    public partial class CoreCmsApiAccessToken
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CoreCmsApiAccessToken()
        {
        }

        /// <summary>
        /// 序列
        /// </summary>
        [Display(Name = "序列")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 id { get; set; }

        /// <summary>
        /// 第三方应用名称
        /// </summary>
        [Display(Name = "第三方应用名称")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(maximumLength: 50, ErrorMessage = "{0}不能超过{1}字")]
        public System.String name { get; set; }

        /// <summary>
        /// 第三方应用编码
        /// </summary>
        [Display(Name = "第三方应用编码")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(maximumLength: 50, ErrorMessage = "{0}不能超过{1}字")]
        public System.String code { get; set; }

        /// <summary>
        /// 易联云终端号
        /// </summary>
        [Display(Name = "易联云终端号")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(maximumLength: 50, ErrorMessage = "{0}不能超过{1}字")]
        public System.String machineCode { get; set; }

        /// <summary>
        /// 访问令牌，API调用时需要，令牌可以重复使用无失效时间，请开发者全局保存
        /// </summary>
        [Display(Name = "访问令牌，API调用时需要，令牌可以重复使用无失效时间，请开发者全局保存")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(maximumLength: 50, ErrorMessage = "{0}不能超过{1}字")]
        public System.String accessToken { get; set; }

        /// <summary>
        /// 更新access_token所需，有效时间35天
        /// </summary>
        [Display(Name = "更新access_token所需，有效时间35天")]
        [StringLength(maximumLength: 50, ErrorMessage = "{0}不能超过{1}字")]
        public System.String refreshToken { get; set; }

        /// <summary>
        /// 令牌的有效时间，单位秒 (30天),注：该模式下可忽略此参数
        /// </summary>
        [Display(Name = "令牌的有效时间，单位秒 (30天),注：该模式下可忽略此参数")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 expiresIn { get; set; }

        /// <summary>
        /// 有效期截止时间
        /// </summary>
        [Display(Name = "有效期截止时间")]
        public System.DateTime? expiressEndTime { get; set; }

        /// <summary>
        /// 其他参数
        /// </summary>
        [Display(Name = "其他参数")]
        public System.String parameters { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.DateTime createTime { get; set; }
    }
}