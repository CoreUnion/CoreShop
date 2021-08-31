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
using System.ComponentModel;

namespace CoreCms.Net.Model.ViewModels.DTO
{
    /// <summary>
    /// </summary>
    public class ManagerLogDto
    {
        /// <summary>
        ///     序列
        /// </summary>
        [DisplayName("序列")]
        public int Id { get; set; }

        /// <summary>
        ///     关联用户
        /// </summary>
        [DisplayName("关联用户")]
        public int UserId { get; set; }

        /// <summary>
        ///     用户账户
        /// </summary>
        [DisplayName("用户账户")]
        public string UserName { get; set; }

        /// <summary>
        ///     操作类型
        /// </summary>
        [DisplayName("操作类型")]
        public string ActionType { get; set; }

        /// <summary>
        ///     备注
        /// </summary>
        [DisplayName("备注")]
        public string Remark { get; set; }

        /// <summary>
        ///     用户IP
        /// </summary>
        [DisplayName("用户IP")]
        public string UserIp { get; set; }

        /// <summary>
        ///     创建时间
        /// </summary>
        [DisplayName("创建时间")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        ///     关联控制器
        /// </summary>
        [DisplayName("关联控制器")]
        public string ControllerName { get; set; }

        /// <summary>
        ///     关联操作
        /// </summary>
        [DisplayName("关联操作")]
        public string ActionName { get; set; }
    }
}