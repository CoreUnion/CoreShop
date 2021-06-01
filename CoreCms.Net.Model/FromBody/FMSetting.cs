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
using CoreCms.Net.Model.ViewModels.Basics;

namespace CoreCms.Net.Model.FromBody
{
    /// <summary>
    ///     配置文件更新类
    /// </summary>
    public class FMCoreCmsSettingDoSaveModel
    {
        /// <summary>
        ///     列表
        /// </summary>
        public List<DictionaryKeyValues> entity { get; set; }
    }
}