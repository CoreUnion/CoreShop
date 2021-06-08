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
    /// 店铺设置表
    /// </summary>
    [SugarTable("CoreCmsSetting",TableDescription = "店铺设置表")]
    public partial class CoreCmsSetting
    {
        /// <summary>
        /// 店铺设置表
        /// </summary>
        public CoreCmsSetting()
        {
        }

        /// <summary>
        /// 键
        /// </summary>
        [Display(Name = "键")]
        [SugarColumn(ColumnDescription = "键", IsPrimaryKey = true)]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String sKey { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        [Display(Name = "值")]
        [SugarColumn(ColumnDescription = "值", IsNullable = true)]
        public System.String sValue { get; set; }
    }
}