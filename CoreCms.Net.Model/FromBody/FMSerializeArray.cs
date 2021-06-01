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
    /// <summary>
    ///     前端提交标准json键值对内容
    /// </summary>
    public class FmSerializeArray
    {
        public List<FormSerializeArray> entity { get; set; }
    }

    public class FormSerializeArray
    {
        public string name { get; set; }
        public string value { get; set; }
    }
}