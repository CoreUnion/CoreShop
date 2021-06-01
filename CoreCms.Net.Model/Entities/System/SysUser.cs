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
    ///     用户表
    /// </summary>
    public partial class SysUser
    {
        /// <summary>
        ///     用户id
        /// </summary>
        [Display(Name = "用户id")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public int id { get; set; }

        /// <summary>
        ///     账号
        /// </summary>
        [Display(Name = "账号")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]
        public string userName { get; set; }

        /// <summary>
        ///     密码
        /// </summary>
        [Display(Name = "密码")]
        [StringLength(100, ErrorMessage = "{0}不能超过{1}字")]
        public string passWord { get; set; }

        /// <summary>
        ///     昵称
        /// </summary>
        [Display(Name = "昵称")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]
        public string nickName { get; set; }

        /// <summary>
        ///     头像
        /// </summary>
        [Display(Name = "头像")]
        [StringLength(255, ErrorMessage = "{0}不能超过{1}字")]
        public string avatar { get; set; }

        /// <summary>
        ///     性别
        /// </summary>
        [Display(Name = "性别")]
        [Required(ErrorMessage = "请输入{0}")]
        public int sex { get; set; }

        /// <summary>
        ///     手机号
        /// </summary>
        [Display(Name = "手机号")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]
        public string phone { get; set; }

        /// <summary>
        ///     邮箱
        /// </summary>
        [Display(Name = "邮箱")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]
        public string email { get; set; }

        /// <summary>
        ///     邮箱是否验证
        /// </summary>
        [Display(Name = "邮箱是否验证")]
        [Required(ErrorMessage = "请输入{0}")]
        public bool emailVerified { get; set; }

        /// <summary>
        ///     真实姓名
        /// </summary>
        [Display(Name = "真实姓名")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]
        public string trueName { get; set; }

        /// <summary>
        ///     身份证号
        /// </summary>
        [Display(Name = "身份证号")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]
        public string idCard { get; set; }

        /// <summary>
        ///     出生日期
        /// </summary>
        [Display(Name = "出生日期")]
        public DateTime? birthday { get; set; }

        /// <summary>
        ///     个人简介
        /// </summary>
        [Display(Name = "个人简介")]
        [StringLength(500, ErrorMessage = "{0}不能超过{1}字")]
        public string introduction { get; set; }

        /// <summary>
        ///     机构id
        /// </summary>
        [Display(Name = "机构id")]
        public int? organizationId { get; set; }

        /// <summary>
        ///     状态,0正常,1冻结
        /// </summary>
        [Display(Name = "状态,0正常,1冻结")]
        [Required(ErrorMessage = "请输入{0}")]
        public int state { get; set; }

        /// <summary>
        ///     是否删除,0否,1是
        /// </summary>
        [Display(Name = "是否删除,0否,1是")]
        [Required(ErrorMessage = "请输入{0}")]
        public bool deleted { get; set; }

        /// <summary>
        ///     注册时间
        /// </summary>
        [Display(Name = "注册时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public DateTime createTime { get; set; }

        /// <summary>
        ///     修改时间
        /// </summary>
        [Display(Name = "修改时间")]
        public DateTime? updateTime { get; set; }
    }
}