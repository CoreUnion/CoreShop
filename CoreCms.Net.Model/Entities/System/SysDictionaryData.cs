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
    /// 数据字典项表
    /// </summary>
    [SugarTable("SysDictionaryData",TableDescription = "数据字典项表")]
    public partial class SysDictionaryData
    {
        /// <summary>
        /// 数据字典项表
        /// </summary>
        public SysDictionaryData()
        {
        }

        /// <summary>
        /// 字典项id
        /// </summary>
        [Display(Name = "字典项id")]
        [SugarColumn(ColumnDescription = "字典项id", IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 id { get; set; }
        /// <summary>
        /// 字典id
        /// </summary>
        [Display(Name = "字典id")]
        [SugarColumn(ColumnDescription = "字典id", IsNullable = true)]
        public System.Int32? dictId { get; set; }
        /// <summary>
        /// 字典项标识
        /// </summary>
        [Display(Name = "字典项标识")]
        [SugarColumn(ColumnDescription = "字典项标识", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String dictDataCode { get; set; }
        /// <summary>
        /// 字典项名称
        /// </summary>
        [Display(Name = "字典项名称")]
        [SugarColumn(ColumnDescription = "字典项名称", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String dictDataName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "备注")]
        [SugarColumn(ColumnDescription = "备注", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String comments { get; set; }
        /// <summary>
        /// 排序号
        /// </summary>
        [Display(Name = "排序号")]
        [SugarColumn(ColumnDescription = "排序号")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 sortNumber { get; set; }
        /// <summary>
        /// 是否删除,0否,1是
        /// </summary>
        [Display(Name = "是否删除,0否,1是")]
        [SugarColumn(ColumnDescription = "是否删除,0否,1是")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Boolean deleted { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [SugarColumn(ColumnDescription = "创建时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.DateTime createTime { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        [Display(Name = "修改时间")]
        [SugarColumn(ColumnDescription = "修改时间", IsNullable = true)]
        public System.DateTime? updateTime { get; set; }
    }
}