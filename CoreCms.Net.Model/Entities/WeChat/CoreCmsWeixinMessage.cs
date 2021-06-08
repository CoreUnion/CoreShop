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
    /// 微信消息表
    /// </summary>
    [SugarTable("CoreCmsWeixinMessage",TableDescription = "微信消息表")]
    public partial class CoreCmsWeixinMessage
    {
        /// <summary>
        /// 微信消息表
        /// </summary>
        public CoreCmsWeixinMessage()
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
        /// 消息名称
        /// </summary>
        [Display(Name = "消息名称")]
        [SugarColumn(ColumnDescription = "消息名称", IsNullable = true)]
        [StringLength(200, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String name { get; set; }
        /// <summary>
        /// 消息类型
        /// </summary>
        [Display(Name = "消息类型")]
        [SugarColumn(ColumnDescription = "消息类型", IsNullable = true)]
        public System.Int32? type { get; set; }
        /// <summary>
        /// 消息参数
        /// </summary>
        [Display(Name = "消息参数")]
        [SugarColumn(ColumnDescription = "消息参数", IsNullable = true)]
        public System.String parameters { get; set; }
        /// <summary>
        /// 是否关注回复
        /// </summary>
        [Display(Name = "是否关注回复")]
        [SugarColumn(ColumnDescription = "是否关注回复", IsNullable = true)]
        public System.Boolean? isAttention { get; set; }
        /// <summary>
        /// 是否默认回复
        /// </summary>
        [Display(Name = "是否默认回复")]
        [SugarColumn(ColumnDescription = "是否默认回复", IsNullable = true)]
        public System.Boolean? isDefault { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        [Display(Name = "是否启用")]
        [SugarColumn(ColumnDescription = "是否启用", IsNullable = true)]
        public System.Boolean? isEnable { get; set; }
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