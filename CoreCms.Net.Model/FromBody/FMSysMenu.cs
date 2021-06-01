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

namespace CoreCms.Net.Model.FromBody
{
    public class FMSysMenuToImportButtonData
    {
        public int menuId { get; set; }
        public string controllerName { get; set; }
        public string actionName { get; set; }
        public string description { get; set; }
    }


    public class FMSysMenuToImportButton
    {
        public List<FMSysMenuToImportButtonData> data { get; set; }
    }
}