/***********************************************************************
 *            Project: CoreCms.Net                                     *
 *                Web: https://CoreCms.Net                             *
 *        ProjectName: 核心内容管理系统                                *
 *             Author: 大灰灰                                          *
 *              Email: JianWeie@163.com                                *
 *         CreateTime: 2020-03-13 1:50:16
 *        Description: 暂无
 ***********************************************************************/


using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.UI;

namespace CoreCms.Net.Utility.Helper
{
    public class ArticleHelper
    {
        #region 获取文章分类下来Dtree============================================================

        /// <summary>
        /// 获取导航下拉上级树
        /// </summary>
        /// <returns></returns>
        [Description("获取导航下拉上级树")]
        public static DTree GetTree(List<CoreCmsArticleType> categories, bool isHaveTop = true)
        {
            var model = new DTree();
            model.status = new dtreeStatus() { code = 200, message = "操作成功" };
            var list = GetMenus(categories, 0);
            if (isHaveTop)
            {
                list.Insert(0, new dtreeChild()
                {
                    id = "0",
                    last = true,
                    parentId = "0",
                    title = "无父级",
                    children = new List<dtreeChild>()
                });
            }
            model.data = list;
            return model;
        }

        /// <summary>
        /// 迭代方法
        /// </summary>
        /// <param name="oldNavs"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        private static List<dtreeChild> GetMenus(List<CoreCmsArticleType> oldNavs, int parentId)
        {
            List<dtreeChild> childTree = new List<dtreeChild>();
            var model = oldNavs.Where(p => p.parentId == parentId).ToList();
            foreach (var item in model)
            {
                var parentTree = new dtreeChild();
                parentTree.id = item.id.ToString();
                parentTree.title = item.name;
                parentTree.parentId = item.parentId.ToString();
                parentTree.last = !oldNavs.Exists(p => p.parentId == item.id);

                childTree.Add(parentTree);
                parentTree.children = GetMenus(oldNavs, item.id);
            }
            return childTree;
        }
        #endregion
    }
}
