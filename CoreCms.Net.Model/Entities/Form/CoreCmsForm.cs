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
    ///     表单
    /// </summary>
    public partial class CoreCmsForm
    {
        /// <summary>
        ///     序列
        /// </summary>
        [Display(Name = "序列")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public int id { get; set; }

        /// <summary>
        ///     表单名称
        /// </summary>
        [Display(Name = "表单名称")]
        [StringLength(100, ErrorMessage = "{0}不能超过{1}字")]
        public string name { get; set; }

        /// <summary>
        ///     表单类型
        /// </summary>
        [Display(Name = "表单类型")]
        [Required(ErrorMessage = "请输入{0}")]
        public int type { get; set; }

        /// <summary>
        ///     表单排序
        /// </summary>
        [Display(Name = "表单排序")]
        [Required(ErrorMessage = "请输入{0}")]
        public int sort { get; set; }

        /// <summary>
        ///     图集
        /// </summary>
        [Display(Name = "图集")]
        public string images { get; set; }

        /// <summary>
        ///     视频地址
        /// </summary>
        [Display(Name = "视频地址")]
        [StringLength(255, ErrorMessage = "{0}不能超过{1}字")]
        public string videoPath { get; set; }

        /// <summary>
        ///     表单描述
        /// </summary>
        [Display(Name = "表单描述")]
        [StringLength(255, ErrorMessage = "{0}不能超过{1}字")]
        public string description { get; set; }

        /// <summary>
        ///     表头类型
        /// </summary>
        [Display(Name = "表头类型")]
        [Required(ErrorMessage = "请输入{0}")]
        public int headType { get; set; }

        /// <summary>
        ///     表单头值
        /// </summary>
        [Display(Name = "表单头值")]
        [StringLength(200, ErrorMessage = "{0}不能超过{1}字")]
        public string headTypeValue { get; set; }

        /// <summary>
        ///     表单视频
        /// </summary>
        [Display(Name = "表单视频")]
        [StringLength(200, ErrorMessage = "{0}不能超过{1}字")]
        public string headTypeVideo { get; set; }

        /// <summary>
        ///     表单提交按钮名称
        /// </summary>
        [Display(Name = "表单提交按钮名称")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]
        public string buttonName { get; set; }

        /// <summary>
        ///     表单按钮颜色
        /// </summary>
        [Display(Name = "表单按钮颜色")]
        [StringLength(30, ErrorMessage = "{0}不能超过{1}字")]
        public string buttonColor { get; set; }

        /// <summary>
        ///     是否需要登录
        /// </summary>
        [Display(Name = "是否需要登录")]
        [Required(ErrorMessage = "请输入{0}")]
        public bool isLogin { get; set; }

        /// <summary>
        ///     可提交次数
        /// </summary>
        [Display(Name = "可提交次数")]
        [Required(ErrorMessage = "请输入{0}")]
        public int times { get; set; }

        /// <summary>
        ///     二维码图片地址
        /// </summary>
        [Display(Name = "二维码图片地址")]
        [StringLength(200, ErrorMessage = "{0}不能超过{1}字")]
        public string qrcode { get; set; }

        /// <summary>
        ///     提交后提示语
        /// </summary>
        [Display(Name = "提交后提示语")]
        [StringLength(200, ErrorMessage = "{0}不能超过{1}字")]
        public string returnMsg { get; set; }

        /// <summary>
        ///     结束时间
        /// </summary>
        [Display(Name = "结束时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public DateTime endDateTime { get; set; }

        /// <summary>
        ///     创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public DateTime createTime { get; set; }

        /// <summary>
        ///     更新时间
        /// </summary>
        [Display(Name = "更新时间")]
        public DateTime? updateTime { get; set; }
    }
}