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

namespace CoreCms.Net.Model.FromBody
{
    /// <summary>
    /// 后台审核退款单提交参数
    /// </summary>
    public class FMDoAuditPost
    {
        /// <summary>
        /// 状态
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 退款方式
        /// </summary>
        public string paymentCode { get; set; }

        /// <summary>
        /// 退款单号
        /// </summary>
        public string refundId { get; set; }

    }
}
