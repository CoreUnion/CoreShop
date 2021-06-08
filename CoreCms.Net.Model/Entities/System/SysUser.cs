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
    /// 用户表
    /// </summary>
    [SugarTable("SysUser",TableDescription = "用户表")]
    public partial class SysUser
    {
        /// <summary>
        /// 用户表
        /// </summary>
        public SysUser()
        {
        }

        /// <summary>
        /// 用户id
        /// </summary>
        [Display(Name = "用户id")]
        [SugarColumn(ColumnDescription = "用户id", IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 id { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        [Display(Name = "账号")]
        [SugarColumn(ColumnDescription = "账号", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String userName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Display(Name = "密码")]
        [SugarColumn(ColumnDescription = "密码", IsNullable = true)]
        [StringLength(100, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String passWord { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        [Display(Name = "昵称")]
        [SugarColumn(ColumnDescription = "昵称", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String nickName { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        [Display(Name = "头像")]
        [SugarColumn(ColumnDescription = "头像", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String avatar { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        [Display(Name = "性别")]
        [SugarColumn(ColumnDescription = "性别")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 sex { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        [Display(Name = "手机号")]
        [SugarColumn(ColumnDescription = "手机号", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String phone { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        [Display(Name = "邮箱")]
        [SugarColumn(ColumnDescription = "邮箱", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String email { get; set; }
        /// <summary>
        /// 邮箱是否验证
        /// </summary>
        [Display(Name = "邮箱是否验证")]
        [SugarColumn(ColumnDescription = "邮箱是否验证")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Boolean emailVerified { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        [Display(Name = "真实姓名")]
        [SugarColumn(ColumnDescription = "真实姓名", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String trueName { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        [Display(Name = "身份证号")]
        [SugarColumn(ColumnDescription = "身份证号", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String idCard { get; set; }
        /// <summary>
        /// 出生日期
        /// </summary>
        [Display(Name = "出生日期")]
        [SugarColumn(ColumnDescription = "出生日期", IsNullable = true)]
        public System.DateTime? birthday { get; set; }
        /// <summary>
        /// 个人简介
        /// </summary>
        [Display(Name = "个人简介")]
        [SugarColumn(ColumnDescription = "个人简介", IsNullable = true)]
        [StringLength(500, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String introduction { get; set; }
        /// <summary>
        /// 机构id
        /// </summary>
        [Display(Name = "机构id")]
        [SugarColumn(ColumnDescription = "机构id", IsNullable = true)]
        public System.Int32? organizationId { get; set; }
        /// <summary>
        /// 状态,0正常,1冻结
        /// </summary>
        [Display(Name = "状态,0正常,1冻结")]
        [SugarColumn(ColumnDescription = "状态,0正常,1冻结")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 state { get; set; }
        /// <summary>
        /// 是否删除,0否,1是
        /// </summary>
        [Display(Name = "是否删除,0否,1是")]
        [SugarColumn(ColumnDescription = "是否删除,0否,1是")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Boolean deleted { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        [Display(Name = "注册时间")]
        [SugarColumn(ColumnDescription = "注册时间")]
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