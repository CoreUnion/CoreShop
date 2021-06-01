/***********************************************************************
 *            Project: CoreCms.Net                                     *
 *                Web: https://CoreCms.Net                             *
 *        ProjectName: 核心内容管理系统                                *
 *             Author: 大灰灰                                          *
 *              Email: JianWeie@163.com                                *
 *         CreateTime: 2020-07-16 1:39:49
 *        Description: 暂无
 ***********************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreCms.Net.Model.Entities;

namespace CoreCms.Net.Utility.Helper
{
    /// <summary>
    /// 组织机构帮助类
    /// </summary>
    public static class SysOrganizationHelper
    {

        /// <summary>
        /// 获取组织结构树
        /// </summary>
        /// <param name="list"></param>
        /// <param name="id"></param>
        /// <param name="treeNodes"></param>
        /// <returns></returns>
        public static void GetOrganizeChildIds(List<SysOrganization> list, int id, ref List<int> treeNodes)
        {
            treeNodes.Add(id);
            if (list == null) return;
            List<SysOrganization> sublist;
            sublist = list.Where(t => t.parentId == id).ToList();
            if (!sublist.Any()) return;
            foreach (var item in sublist)
            {
                GetOrganizeChildIds(list, item.id, ref treeNodes);
            }
        }

    }
}
