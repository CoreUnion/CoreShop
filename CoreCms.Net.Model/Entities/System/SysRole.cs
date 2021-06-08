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
    /// 角色表
    /// </summary>
    [SugarTable("SysRole",TableDescription = "角色表")]
    public partial class SysRole
    {
        /// <summary>
        /// 角色表
        /// </summary>
        public SysRole()
        {
        }

        /// <summary>
        /// 角色id
        /// </summary>
        [Display(Name = "角色id")]
        [SugarColumn(ColumnDescription = "角色id", IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 id { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        [Display(Name = "角色名称")]
        [SugarColumn(ColumnDescription = "角色名称")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String roleName { get; set; }
        /// <summary>
        /// 角色标识
        /// </summary>
        [Display(Name = "角色标识")]
        [SugarColumn(ColumnDescription = "角色标识", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String roleCode { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "备注")]
        [SugarColumn(ColumnDescription = "备注", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
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