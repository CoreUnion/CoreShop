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
using SqlSugar;

namespace CoreCms.Net.Model.ViewModels.Basics
{
    /// <summary>
    ///     代码生成器下拉数据列表实体
    /// </summary>
    public class DbTableInfoTree
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public DbObjectType DbObjectType { get; set; }
    }

    /// <summary>
    ///     表名带字段
    /// </summary>
    public class DbTableInfoAndColumns
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<DbColumnInfo> columns { get; set; } = null;
        public DbObjectType DbObjectType { get; set; }
    }
}