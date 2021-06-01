/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using SqlSugar;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    ///     订单记录表
    /// </summary>
    public partial class CoreCmsOrderLog
    {
        /// <summary>
        ///     类型说明
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string typeText { get; set; }
    }
}