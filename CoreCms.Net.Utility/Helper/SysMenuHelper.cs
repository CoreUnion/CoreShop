using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCms.Net.Utility.Helper
{
    public static class SysMenuHelper
    {

        #region 获取商品分类下来Dtree============================================================

        /// <summary>
        /// 获取导航下拉上级树
        /// </summary>
        /// <returns></returns>
        [Description("获取导航下拉上级树")]
        public static DTree GetTree(List<SysMenu> categories, bool isHaveTop = true)
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
        private static List<dtreeChild> GetMenus(List<SysMenu> oldNavs, int parentId)
        {
            List<dtreeChild> childTree = new List<dtreeChild>();
            var model = oldNavs.Where(p => p.parentId == parentId).ToList();
            foreach (var item in model)
            {
                var parentTree = new dtreeChild();
                parentTree.id = item.id.ToString();
                parentTree.title = item.menuName;
                parentTree.parentId = item.parentId.ToString();
                parentTree.last = !oldNavs.Exists(p => p.parentId == item.id);
                parentTree.isParent = !parentTree.last;
                parentTree.otherData = item;

                childTree.Add(parentTree);
                parentTree.children = GetMenus(oldNavs, item.id);
            }
            return childTree;
        }

        #endregion

    }
}
