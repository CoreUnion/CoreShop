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
    /// 用户地址表
    /// </summary>
    [SugarTable("CoreCmsUserShip",TableDescription = "用户地址表")]
    public partial class CoreCmsUserShip
    {
        /// <summary>
        /// 用户地址表
        /// </summary>
        public CoreCmsUserShip()
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
        /// 用户id 关联user.id
        /// </summary>
        [Display(Name = "用户id 关联user.id")]
        [SugarColumn(ColumnDescription = "用户id 关联user.id")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 userId { get; set; }
        /// <summary>
        /// 收货地区ID
        /// </summary>
        [Display(Name = "收货地区ID")]
        [SugarColumn(ColumnDescription = "收货地区ID")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 areaId { get; set; }
        /// <summary>
        /// 收货详细地址
        /// </summary>
        [Display(Name = "收货详细地址")]
        [SugarColumn(ColumnDescription = "收货详细地址", IsNullable = true)]
        [StringLength(200, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String address { get; set; }
        /// <summary>
        /// 收货人姓名
        /// </summary>
        [Display(Name = "收货人姓名")]
        [SugarColumn(ColumnDescription = "收货人姓名", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String name { get; set; }
        /// <summary>
        /// 收货电话
        /// </summary>
        [Display(Name = "收货电话")]
        [SugarColumn(ColumnDescription = "收货电话", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String mobile { get; set; }
        /// <summary>
        /// 是否默认
        /// </summary>
        [Display(Name = "是否默认")]
        [SugarColumn(ColumnDescription = "是否默认")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Boolean isDefault { get; set; }
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