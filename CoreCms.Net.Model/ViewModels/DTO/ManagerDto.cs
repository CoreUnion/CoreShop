/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using System.ComponentModel;

namespace CoreCms.Net.Model.ViewModels.DTO
{
    /// <summary>
    /// </summary>
    public class ManagerDto
    {
        /// <summary>
        ///     序列
        /// </summary>
        [DisplayName("序列")]
        public int Id { get; set; }

        /// <summary>
        ///     用户名
        /// </summary>
        [DisplayName("用户名")]
        public string UserName { get; set; }

        /// <summary>
        ///     昵称
        /// </summary>
        [DisplayName("昵称")]
        public string NickName { get; set; }

        /// <summary>
        ///     描述
        /// </summary>
        [DisplayName("描述")]
        public string Description { get; set; }
    }
}