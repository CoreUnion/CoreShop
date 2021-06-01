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
    ///     微信小程序消息模板
    /// </summary>
    public partial class CoreCmsUserWeChatMsgTemplate
    {
        /// <summary>
        ///     是否订阅
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public bool @is { get; set; }
    }
}