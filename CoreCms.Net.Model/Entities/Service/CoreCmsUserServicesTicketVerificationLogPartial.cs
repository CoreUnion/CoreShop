/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using System.ComponentModel.DataAnnotations;
using SqlSugar;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    ///     服务券核验日志
    /// </summary>
    public partial class CoreCmsUserServicesTicketVerificationLog
    {
        /// <summary>
        ///     ticket
        /// </summary>
        [Display(Name = "ticket")]
        [SugarColumn(IsIgnore = true)]
        public CoreCmsUserServicesTicket ticket { get; set; }

        /// <summary>
        ///     service
        /// </summary>
        [Display(Name = "service")]
        [SugarColumn(IsIgnore = true)]
        public CoreCmsServices service { get; set; }

        /// <summary>
        ///     order
        /// </summary>
        [Display(Name = "order")]
        [SugarColumn(IsIgnore = true)]
        public CoreCmsUserServicesOrder order { get; set; }
    }
}