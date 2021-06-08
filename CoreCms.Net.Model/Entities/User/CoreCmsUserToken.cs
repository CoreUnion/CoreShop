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
    /// 用户token
    /// </summary>
    [SugarTable("CoreCmsUserToken",TableDescription = "用户token")]
    public partial class CoreCmsUserToken
    {
        /// <summary>
        /// 用户token
        /// </summary>
        public CoreCmsUserToken()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
        [SugarColumn(ColumnDescription = "", IsPrimaryKey = true)]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(32, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String token { get; set; }
        /// <summary>
        /// 用户序列
        /// </summary>
        [Display(Name = "用户序列")]
        [SugarColumn(ColumnDescription = "用户序列")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 userId { get; set; }
        /// <summary>
        /// 平台类型，1就是默认，2就是微信小程序
        /// </summary>
        [Display(Name = "平台类型，1就是默认，2就是微信小程序")]
        [SugarColumn(ColumnDescription = "平台类型，1就是默认，2就是微信小程序")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int16 platform { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [SugarColumn(ColumnDescription = "创建时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.DateTime createTime { get; set; }
    }
}