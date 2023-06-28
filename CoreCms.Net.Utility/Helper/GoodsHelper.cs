/***********************************************************************
 *            Project: CoreCms.Net                                     *
 *                Web: https://CoreCms.Net                             *
 *        ProjectName: 核心内容管理系统                                *
 *             Author: 大灰灰                                          *
 *              Email: JianWeie@163.com                                *
 *         CreateTime: 2020-03-03 5:04:42
 *        Description: 暂无
 ***********************************************************************/


using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.UI;

namespace CoreCms.Net.Utility.Helper
{
    /// <summary>
    /// 商品分类帮助
    /// </summary>
    public static class GoodsHelper
    {
        #region 获取商品分类下来Dtree============================================================

        /// <summary>
        /// 获取导航下拉上级树
        /// </summary>
        /// <returns></returns>
        [Description("获取导航下拉上级树")]
        public static DTree GetTree(List<CoreCmsGoodsCategory> categories, bool isHaveTop = true)
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
        private static List<dtreeChild> GetMenus(List<CoreCmsGoodsCategory> oldNavs, int parentId)
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
                parentTree.isParent = !parentTree.last;
                parentTree.otherData = item;

                childTree.Add(parentTree);
                parentTree.children = GetMenus(oldNavs, item.id);
            }
            return childTree;
        }

        #endregion

        #region 递归获取下级所有序列
        /// <summary>
        /// 递归获取下级所有序列
        /// </summary>
        /// <returns></returns>
        [Description("递归获取下级所有序列")]
        public static List<int> GetChildIds(List<CoreCmsGoodsCategory> categories, int parentId)
        {
            var ids = new List<int> { parentId };
            ids = GetChildrenIds(categories, parentId, ids);
            return ids;
        }

        /// <summary>
        /// 迭代方法
        /// </summary>
        /// <param name="oldNavs"></param>
        /// <param name="parentId"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        private static List<int> GetChildrenIds(List<CoreCmsGoodsCategory> oldNavs, int parentId, List<int> ids)
        {
            var model = oldNavs.Where(p => p.parentId == parentId).ToList();
            foreach (var item in model)
            {
                ids.Add(item.id);
                GetChildrenIds(oldNavs, item.id, ids);
            }
            return ids;
        }
        #endregion

        #region 获取可用库存
        /// <summary>
        /// 获取可用库存。
        /// 库存机制：商品下单 总库存不变，冻结库存加1，
        /// 商品发货：冻结库存减1，总库存减1，
        /// 商品退款&取消订单：总库存不变，冻结库存减1,
        /// 商品退货：总库存加1，冻结库存不变,
        /// 可销售库存：总库存-冻结库存
        /// </summary>
        /// <param name="stock">库存</param>
        /// <param name="freezeStock">静态库存</param>
        /// <returns></returns>
        public static int GetStock(int stock, int freezeStock)
        {
            return stock - freezeStock;
        }
        #endregion

        #region arr图片提取单张,或设置一张默认
        /// <summary>
        /// arr图片提取单张,或设置一张默认
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetOneImage(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return "/static/images/common/empty.png";
            }
            else if (url.Contains(","))
            {
                return url.Split(",")[0];
            }
            else
            {
                return url;
            }
        }
        #endregion

        #region 小程序端获取编码后的分类集合



        #endregion

        #region 后端判断提交的商品属性值是否符合规则（判断内容，只允许中文，字母，数字，和-，/）

        /// <summary>
        /// 判断内容，只允许中文，字母，数字，和-，/
        /// </summary>
        /// <param name="inputValue">输入字符串</param>
        /// <remarks>判断内容，只允许中文，字母，数字，和-，/</remarks>
        /// <returns></returns>
        public static bool FilterChar(string inputValue)
        {
            return Regex.IsMatch(inputValue, "[`.~!@#$^&*()=|\"{}':;',\\[\\]<>?~！@#￥……&*&;|{}。*-+]+");
        }

        #endregion


    }
}
