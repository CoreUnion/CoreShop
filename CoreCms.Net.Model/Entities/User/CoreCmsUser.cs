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
    [SugarTable("CoreCmsUser",TableDescription = "用户表")]
    public partial class CoreCmsUser
    {
        /// <summary>
        /// 用户表
        /// </summary>
        public CoreCmsUser()
        {
        }

        /// <summary>
        /// 用户ID
        /// </summary>
        [Display(Name = "用户ID")]
        [SugarColumn(ColumnDescription = "用户ID", IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 id { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        [Display(Name = "用户名")]
        [SugarColumn(ColumnDescription = "用户名", IsNullable = true)]
        [StringLength(20, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String userName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Display(Name = "密码")]
        [SugarColumn(ColumnDescription = "密码", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String passWord { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        [Display(Name = "手机号")]
        [SugarColumn(ColumnDescription = "手机号", IsNullable = true)]
        [StringLength(15, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String mobile { get; set; }
        /// <summary>
        /// 性别[1男2女3未知]
        /// </summary>
        [Display(Name = "性别[1男2女3未知]")]
        [SugarColumn(ColumnDescription = "性别[1男2女3未知]")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 sex { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        [Display(Name = "生日")]
        [SugarColumn(ColumnDescription = "生日", IsNullable = true)]
        public System.DateTime? birthday { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        [Display(Name = "头像")]
        [SugarColumn(ColumnDescription = "头像", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String avatarImage { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        [Display(Name = "昵称")]
        [SugarColumn(ColumnDescription = "昵称", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String nickName { get; set; }
        /// <summary>
        /// 余额
        /// </summary>
        [Display(Name = "余额")]
        [SugarColumn(ColumnDescription = "余额")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Decimal balance { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        [Display(Name = "积分")]
        [SugarColumn(ColumnDescription = "积分")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 point { get; set; }
        /// <summary>
        /// 用户等级
        /// </summary>
        [Display(Name = "用户等级")]
        [SugarColumn(ColumnDescription = "用户等级")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 grade { get; set; }
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
        public System.DateTime? updataTime { get; set; }
        /// <summary>
        /// 状态[1正常2停用]
        /// </summary>
        [Display(Name = "状态[1正常2停用]")]
        [SugarColumn(ColumnDescription = "状态[1正常2停用]")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 status { get; set; }
        /// <summary>
        /// 推荐人
        /// </summary>
        [Display(Name = "推荐人")]
        [SugarColumn(ColumnDescription = "推荐人")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 parentId { get; set; }
        /// <summary>
        /// 关联三方账户
        /// </summary>
        [Display(Name = "关联三方账户")]
        [SugarColumn(ColumnDescription = "关联三方账户")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 userWx { get; set; }
        /// <summary>
        /// 删除标志 有数据就是删除
        /// </summary>
        [Display(Name = "删除标志 有数据就是删除")]
        [SugarColumn(ColumnDescription = "删除标志 有数据就是删除")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Boolean isDelete { get; set; }
    }
}