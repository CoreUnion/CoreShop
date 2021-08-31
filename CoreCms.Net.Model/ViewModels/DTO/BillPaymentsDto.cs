/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using System.Collections.Generic;

namespace CoreCms.Net.Model.ViewModels.DTO
{
    /// <summary>
    ///     支付确认页面返回实体
    /// </summary>
    public class CheckPayDTO
    {
        public int userId { get; set; } = 0;

        public decimal money { get; set; } = 0;

        public List<rel> rel { get; set; } = new();
    }

    public class rel
    {
        /// <summary>
        ///     关联资源序列
        /// </summary>
        public string sourceId { get; set; }

        /// <summary>
        ///     金额
        /// </summary>
        public decimal money { get; set; }
    }
}