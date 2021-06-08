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
    /// 分销商表
    /// </summary>
    [SugarTable("CoreCmsDistribution",TableDescription = "分销商表")]
    public partial class CoreCmsDistribution
    {
        /// <summary>
        /// 分销商表
        /// </summary>
        public CoreCmsDistribution()
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
        /// 用户Id
        /// </summary>
        [Display(Name = "用户Id")]
        [SugarColumn(ColumnDescription = "用户Id")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 userId { get; set; }
        /// <summary>
        /// 分销商名称
        /// </summary>
        [Display(Name = "分销商名称")]
        [SugarColumn(ColumnDescription = "分销商名称", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String name { get; set; }
        /// <summary>
        /// 分销等级
        /// </summary>
        [Display(Name = "分销等级")]
        [SugarColumn(ColumnDescription = "分销等级")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 gradeId { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        [Display(Name = "手机号")]
        [SugarColumn(ColumnDescription = "手机号", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String mobile { get; set; }
        /// <summary>
        /// 微信号
        /// </summary>
        [Display(Name = "微信号")]
        [SugarColumn(ColumnDescription = "微信号", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String weixin { get; set; }
        /// <summary>
        /// qq号
        /// </summary>
        [Display(Name = "qq号")]
        [SugarColumn(ColumnDescription = "qq号", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String qq { get; set; }
        /// <summary>
        /// 店铺名称
        /// </summary>
        [Display(Name = "店铺名称")]
        [SugarColumn(ColumnDescription = "店铺名称", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String storeName { get; set; }
        /// <summary>
        /// 店铺Logo
        /// </summary>
        [Display(Name = "店铺Logo")]
        [SugarColumn(ColumnDescription = "店铺Logo", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String storeLogo { get; set; }
        /// <summary>
        /// 店铺Banner
        /// </summary>
        [Display(Name = "店铺Banner")]
        [SugarColumn(ColumnDescription = "店铺Banner", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String storeBanner { get; set; }
        /// <summary>
        /// 店铺简介
        /// </summary>
        [Display(Name = "店铺简介")]
        [SugarColumn(ColumnDescription = "店铺简介", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String storeDesc { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        [Display(Name = "审核状态")]
        [SugarColumn(ColumnDescription = "审核状态")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 verifyStatus { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [SugarColumn(ColumnDescription = "创建时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.DateTime createTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        [Display(Name = "更新时间")]
        [SugarColumn(ColumnDescription = "更新时间", IsNullable = true)]
        public System.DateTime? updateTime { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        [Display(Name = "审核时间")]
        [SugarColumn(ColumnDescription = "审核时间", IsNullable = true)]
        public System.DateTime? verifyTime { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        [Display(Name = "是否删除")]
        [SugarColumn(ColumnDescription = "是否删除")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Boolean isDelete { get; set; }
    }
}