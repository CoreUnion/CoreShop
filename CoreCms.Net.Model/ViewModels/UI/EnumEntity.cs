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
    ///     枚举实体
    /// </summary>
    public class EnumEntity
    {
        /// <summary>
        ///     枚举的描述
        /// </summary>
        public string description { set; get; }

        /// <summary>
        ///     枚举名称
        /// </summary>
        public string title { set; get; }

        /// <summary>
        ///     枚举对象的值
        /// </summary>
        public int value { set; get; }
    }
}