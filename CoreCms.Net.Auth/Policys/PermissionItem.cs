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
using System.Collections.Generic;
using System.Text;

namespace CoreCms.Net.Auth.Policys
{
    /// <summary>
    /// 用户或角色或其他凭据实体
    /// </summary>
    public class PermissionItem
    {
        /// <summary>
        /// 用户或角色或其他凭据名称
        /// </summary>
        public virtual string Role { get; set; }
        /// <summary>
        /// 请求Url
        /// </summary>
        public virtual string Url { get; set; }
        /// <summary>
        /// 权限标识
        /// </summary>
        public virtual string Authority { get; set; }
        /// <summary>
        /// 路由标识Url
        /// </summary>
        public virtual string RouteUrl { get; set; }
    }
}
