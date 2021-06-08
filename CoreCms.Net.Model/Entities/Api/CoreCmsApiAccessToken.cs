/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com
 *         CreateTime: 2021-06-08 22:14:58
 *        Description: 暂无
***********************************************************************/ 
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    /// 第三方授权记录表
    /// </summary>
    [SugarTable("CoreCmsApiAccessToken",TableDescription = "第三方授权记录表")]
    public partial class CoreCmsApiAccessToken
    {
        /// <summary>
        /// 第三方授权记录表
        /// </summary>
        public CoreCmsApiAccessToken()
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
        /// 第三方应用名称
        /// </summary>
        [Display(Name = "第三方应用名称")]
        [SugarColumn(ColumnDescription = "第三方应用名称")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String name { get; set; }
        /// <summary>
        /// 第三方应用编码
        /// </summary>
        [Display(Name = "第三方应用编码")]
        [SugarColumn(ColumnDescription = "第三方应用编码")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String code { get; set; }
        /// <summary>
        /// 易联云终端号
        /// </summary>
        [Display(Name = "易联云终端号")]
        [SugarColumn(ColumnDescription = "易联云终端号")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String machineCode { get; set; }
        /// <summary>
        /// 访问令牌，API调用时需要，令牌可以重复使用无失效时间，请开发者全局保存
        /// </summary>
        [Display(Name = "访问令牌，API调用时需要，令牌可以重复使用无失效时间，请开发者全局保存")]
        [SugarColumn(ColumnDescription = "访问令牌，API调用时需要，令牌可以重复使用无失效时间，请开发者全局保存")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String accessToken { get; set; }
        /// <summary>
        /// 更新access_token所需，有效时间35天
        /// </summary>
        [Display(Name = "更新access_token所需，有效时间35天")]
        [SugarColumn(ColumnDescription = "更新access_token所需，有效时间35天", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String refreshToken { get; set; }
        /// <summary>
        /// 令牌的有效时间，单位秒 (30天),注：该模式下可忽略此参数
        /// </summary>
        [Display(Name = "令牌的有效时间，单位秒 (30天),注：该模式下可忽略此参数")]
        [SugarColumn(ColumnDescription = "令牌的有效时间，单位秒 (30天),注：该模式下可忽略此参数")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 expiresIn { get; set; }
        /// <summary>
        /// 有效期截止时间
        /// </summary>
        [Display(Name = "有效期截止时间")]
        [SugarColumn(ColumnDescription = "有效期截止时间", IsNullable = true)]
        public System.DateTime? expiressEndTime { get; set; }
        /// <summary>
        /// 其他参数
        /// </summary>
        [Display(Name = "其他参数")]
        [SugarColumn(ColumnDescription = "其他参数", IsNullable = true)]
        public System.String parameters { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [SugarColumn(ColumnDescription = "创建时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.DateTime createTime { get; set; }
    }
}