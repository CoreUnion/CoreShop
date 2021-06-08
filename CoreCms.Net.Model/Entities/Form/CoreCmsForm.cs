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
    /// 表单
    /// </summary>
    [SugarTable("CoreCmsForm",TableDescription = "表单")]
    public partial class CoreCmsForm
    {
        /// <summary>
        /// 表单
        /// </summary>
        public CoreCmsForm()
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
        /// 表单名称
        /// </summary>
        [Display(Name = "表单名称")]
        [SugarColumn(ColumnDescription = "表单名称", IsNullable = true)]
        [StringLength(100, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String name { get; set; }
        /// <summary>
        /// 表单类型
        /// </summary>
        [Display(Name = "表单类型")]
        [SugarColumn(ColumnDescription = "表单类型")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 type { get; set; }
        /// <summary>
        /// 表单排序
        /// </summary>
        [Display(Name = "表单排序")]
        [SugarColumn(ColumnDescription = "表单排序")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 sort { get; set; }
        /// <summary>
        /// 图集
        /// </summary>
        [Display(Name = "图集")]
        [SugarColumn(ColumnDescription = "图集", IsNullable = true)]
        public System.String images { get; set; }
        /// <summary>
        /// 视频地址
        /// </summary>
        [Display(Name = "视频地址")]
        [SugarColumn(ColumnDescription = "视频地址", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String videoPath { get; set; }
        /// <summary>
        /// 表单描述
        /// </summary>
        [Display(Name = "表单描述")]
        [SugarColumn(ColumnDescription = "表单描述", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String description { get; set; }
        /// <summary>
        /// 表头类型
        /// </summary>
        [Display(Name = "表头类型")]
        [SugarColumn(ColumnDescription = "表头类型")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 headType { get; set; }
        /// <summary>
        /// 表单头值
        /// </summary>
        [Display(Name = "表单头值")]
        [SugarColumn(ColumnDescription = "表单头值", IsNullable = true)]
        [StringLength(200, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String headTypeValue { get; set; }
        /// <summary>
        /// 表单视频
        /// </summary>
        [Display(Name = "表单视频")]
        [SugarColumn(ColumnDescription = "表单视频", IsNullable = true)]
        [StringLength(200, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String headTypeVideo { get; set; }
        /// <summary>
        /// 表单提交按钮名称
        /// </summary>
        [Display(Name = "表单提交按钮名称")]
        [SugarColumn(ColumnDescription = "表单提交按钮名称", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String buttonName { get; set; }
        /// <summary>
        /// 表单按钮颜色
        /// </summary>
        [Display(Name = "表单按钮颜色")]
        [SugarColumn(ColumnDescription = "表单按钮颜色", IsNullable = true)]
        [StringLength(30, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String buttonColor { get; set; }
        /// <summary>
        /// 是否需要登录
        /// </summary>
        [Display(Name = "是否需要登录")]
        [SugarColumn(ColumnDescription = "是否需要登录")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Boolean isLogin { get; set; }
        /// <summary>
        /// 可提交次数
        /// </summary>
        [Display(Name = "可提交次数")]
        [SugarColumn(ColumnDescription = "可提交次数")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 times { get; set; }
        /// <summary>
        /// 二维码图片地址
        /// </summary>
        [Display(Name = "二维码图片地址")]
        [SugarColumn(ColumnDescription = "二维码图片地址", IsNullable = true)]
        [StringLength(200, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String qrcode { get; set; }
        /// <summary>
        /// 提交后提示语
        /// </summary>
        [Display(Name = "提交后提示语")]
        [SugarColumn(ColumnDescription = "提交后提示语", IsNullable = true)]
        [StringLength(200, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String returnMsg { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        [Display(Name = "结束时间")]
        [SugarColumn(ColumnDescription = "结束时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.DateTime endDateTime { get; set; }
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
    }
}