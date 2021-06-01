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

namespace CoreCms.Net.Model.FromBody
{
    /// <summary>
    ///     APi
    /// </summary>
    public class EditInfoPost
    {
        public DateTime? birthday { get; set; }

        public string nickname { get; set; }

        public int sex { get; set; }
    }

    /// <summary>
    ///     编辑后端登录个人账户密码
    /// </summary>
    public class EditPwdPost
    {
        public string newpwd { get; set; }

        public string repwd { get; set; }


        public string pwd { get; set; }
    }


    /// <summary>
    ///     编辑登录用户个人信息
    /// </summary>
    public class EditLoginUserInfo
    {
        /// <summary>
        ///     昵称
        /// </summary>
        public string nickName { get; set; }

        /// <summary>
        ///     头像
        /// </summary>
        public string avatar { get; set; }

        /// <summary>
        ///     性别
        /// </summary>
        public int sex { get; set; }

        /// <summary>
        ///     手机号
        /// </summary>
        public string phone { get; set; }

        /// <summary>
        ///     邮箱
        /// </summary>
        public string email { get; set; }

        /// <summary>
        ///     真实姓名
        /// </summary>
        public string trueName { get; set; }

        /// <summary>
        ///     身份证号
        /// </summary>
        public string idCard { get; set; }

        /// <summary>
        ///     出生日期
        /// </summary>
        public DateTime? birthday { get; set; }

        /// <summary>
        ///     个人简介
        /// </summary>
        public string introduction { get; set; }
    }

    /// <summary>
    ///     API取回密码提交参数
    /// </summary>
    public class FMForgetPwdPost
    {
        /// <summary>
        ///     电话号码
        /// </summary>
        public string mobile { get; set; }

        /// <summary>
        ///     编码
        /// </summary>
        public string code { get; set; }

        /// <summary>
        ///     新密码
        /// </summary>
        public string newpwd { get; set; }

        /// <summary>
        ///     确认新密码
        /// </summary>
        public string repwd { get; set; }
    }

    /// <summary>
    ///     API取回密码提交参数
    /// </summary>
    public class FMGetBalancePost
    {
        /// <summary>
        ///     类型
        /// </summary>
        public int id { get; set; } = 0;

        /// <summary>
        ///     页码
        /// </summary>
        public int page { get; set; } = 1;

        /// <summary>
        ///     每页条数
        /// </summary>
        public int limit { get; set; } = 10;

        /// <summary>
        ///     时间范围
        /// </summary>
        public string propsDate { get; set; }
    }
}