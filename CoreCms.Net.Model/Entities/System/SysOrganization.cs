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
    /// 组织机构表
    /// </summary>
    [SugarTable("SysOrganization",TableDescription = "组织机构表")]
    public partial class SysOrganization
    {
        /// <summary>
        /// 组织机构表
        /// </summary>
        public SysOrganization()
        {
        }

        /// <summary>
        /// 机构id
        /// </summary>
        [Display(Name = "机构id")]
        [SugarColumn(ColumnDescription = "机构id", IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 id { get; set; }
        /// <summary>
        /// 上级id,0是顶级
        /// </summary>
        [Display(Name = "上级id,0是顶级")]
        [SugarColumn(ColumnDescription = "上级id,0是顶级")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 parentId { get; set; }
        /// <summary>
        /// 机构名称
        /// </summary>
        [Display(Name = "机构名称")]
        [SugarColumn(ColumnDescription = "机构名称", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String organizationName { get; set; }
        /// <summary>
        /// 机构名称
        /// </summary>
        [Display(Name = "机构名称")]
        [SugarColumn(ColumnDescription = "机构名称", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String organizationFullName { get; set; }
        /// <summary>
        /// 机构类型
        /// </summary>
        [Display(Name = "机构类型")]
        [SugarColumn(ColumnDescription = "机构类型")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 organizationType { get; set; }
        /// <summary>
        /// 负责人id
        /// </summary>
        [Display(Name = "负责人id")]
        [SugarColumn(ColumnDescription = "负责人id", IsNullable = true)]
        public System.Int32? leaderId { get; set; }
        /// <summary>
        /// 排序号
        /// </summary>
        [Display(Name = "排序号")]
        [SugarColumn(ColumnDescription = "排序号")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 sortNumber { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "备注")]
        [SugarColumn(ColumnDescription = "备注", IsNullable = true)]
        [StringLength(500, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String comments { get; set; }
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