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
using CoreCms.Net.Model.Entities;

namespace CoreCms.Net.Model.ViewModels.DTO
{
    public class AreasDto
    {
        public string label { get; set; }
        public int value { get; set; }
        public object children { get; set; }
    }

    public class AreasDtoTh
    {
        public string label { get; set; }
        public int value { get; set; }
    }


    /// <summary>
    ///     后端编辑三级下拉实体
    /// </summary>
    public class AreasDtoForAdminEdit
    {
        public CoreCmsArea info { get; set; } = new();
        public List<CoreCmsArea> list { get; set; } = new();
    }
}