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
    /// 门店表
    /// </summary>
    [SugarTable("CoreCmsStore",TableDescription = "门店表")]
    public partial class CoreCmsStore
    {
        /// <summary>
        /// 门店表
        /// </summary>
        public CoreCmsStore()
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
        /// 门店名称
        /// </summary>
        [Display(Name = "门店名称")]
        [SugarColumn(ColumnDescription = "门店名称", IsNullable = true)]
        [StringLength(125, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String storeName { get; set; }
        /// <summary>
        /// 门店电话/手机号
        /// </summary>
        [Display(Name = "门店电话/手机号")]
        [SugarColumn(ColumnDescription = "门店电话/手机号", IsNullable = true)]
        [StringLength(13, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String mobile { get; set; }
        /// <summary>
        /// 门店联系人
        /// </summary>
        [Display(Name = "门店联系人")]
        [SugarColumn(ColumnDescription = "门店联系人", IsNullable = true)]
        [StringLength(32, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String linkMan { get; set; }
        /// <summary>
        /// 门店logo
        /// </summary>
        [Display(Name = "门店logo")]
        [SugarColumn(ColumnDescription = "门店logo", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String logoImage { get; set; }
        /// <summary>
        /// 门店地区id
        /// </summary>
        [Display(Name = "门店地区id")]
        [SugarColumn(ColumnDescription = "门店地区id")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 areaId { get; set; }
        /// <summary>
        /// 门店详细地址
        /// </summary>
        [Display(Name = "门店详细地址")]
        [SugarColumn(ColumnDescription = "门店详细地址", IsNullable = true)]
        [StringLength(200, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String address { get; set; }
        /// <summary>
        /// 坐标位置
        /// </summary>
        [Display(Name = "坐标位置")]
        [SugarColumn(ColumnDescription = "坐标位置", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String coordinate { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        [Display(Name = "纬度")]
        [SugarColumn(ColumnDescription = "纬度", IsNullable = true)]
        [StringLength(40, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String latitude { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        [Display(Name = "经度")]
        [SugarColumn(ColumnDescription = "经度", IsNullable = true)]
        [StringLength(40, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String longitude { get; set; }
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
        /// 距离
        /// </summary>
        [Display(Name = "距离")]
        [SugarColumn(ColumnDescription = "距离")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Decimal distance { get; set; }
    }
}