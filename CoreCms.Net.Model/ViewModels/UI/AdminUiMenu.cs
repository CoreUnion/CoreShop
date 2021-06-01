/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

namespace CoreCms.Net.Model.ViewModels.UI
{
    /// <summary>
    ///     layUIAdmin左侧导航实体
    /// </summary>
    public class AdminUiMenu
    {
        /// <summary>
        ///     标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        ///     英文标识符
        /// </summary>
        public string name { get; set; }

        /// <summary>
        ///     图标
        /// </summary>
        public string icon { get; set; }

        /// <summary>
        ///     跳转地址(如home/homepage1)
        /// </summary>
        public string jump { get; set; }

        /// <summary>
        ///     是否展开当前列表
        /// </summary>
        public bool spread { get; set; } = false;

        /// <summary>
        ///     子类(防止json循环使用object类型)
        /// </summary>
        public object list { get; set; }
    }
}