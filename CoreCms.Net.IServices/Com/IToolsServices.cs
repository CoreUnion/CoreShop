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
using System.Threading.Tasks;
using CoreCms.Net.Model.Entities;

namespace CoreCms.Net.IServices
{
    /// <summary>
    ///     标签表 服务工厂接口
    /// </summary>
    public interface IToolsServices
    {


        /// <summary>
        /// 查询是否存在违规内容并进行替换
        /// </summary>
        /// <returns></returns>
        Task<String> IllegalWordsReplace(string oldString, char symbol = '*');


        /// <summary>
        /// 查询是否存在违规内容
        /// </summary>
        /// <returns></returns>
        Task<bool> IllegalWordsContainsAny(string oldString);


    }
}