/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/


namespace CoreCms.Net.Model.ViewModels.Basics
{
    /// <summary>
    ///     验证错误信息视图模型
    /// </summary>
    public class ErrorView
    {
        /// <summary>
        ///     错误字段
        /// </summary>

        public string ErrorName { get; set; }

        /// <summary>
        ///     错误内容ErrorMessage
        /// </summary>

        public string Error { get; set; }
    }
}