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
    ///     用户表
    /// </summary>
    public partial class CoreCmsUser
    {
        /// <summary>
        ///     来源类型
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public int type { get; set; }

        /// <summary>
        ///     下级用户数量
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public int childNum { get; set; }

        /// <summary>
        ///     父级名称
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string parentNickName { get; set; }
    }
}