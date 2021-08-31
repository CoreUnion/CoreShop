/***********************************************************************
 *            Project: CoreCms.Net                                     *
 *                Web: https://CoreCms.Net                             *
 *        ProjectName: 核心内容管理系统                                *
 *             Author: 大灰灰                                          *
 *              Email: JianWeie@163.com                                *
 *         CreateTime: 2020-03-21 22:32:23
 *        Description: 暂无
 ***********************************************************************/


using System.Collections.Generic;
using System.Linq;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.DTO;

namespace CoreCms.Net.Utility.Helper
{
    public class AreaHelper
    {
        /// <summary>
        /// 迭代方法
        /// </summary>
        /// <param name="oldNavs"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public static List<AreasDto> GetList(List<CoreCmsArea> oldNavs)
        {
            List<AreasDto> childList = new List<AreasDto>();
            var model1 = oldNavs.Where(p => p.depth == 1).ToList();
            var model2 = oldNavs.Where(p => p.depth == 2).ToList();
            var model3 = oldNavs.Where(p => p.depth == 3).ToList();

            foreach (var item in model1)
            {
                var child = new AreasDto();
                child.value = item.id;
                child.label = item.name;

                var fsChild = new List<AreasDto>();
                var sc = model2.Where(p => p.parentId == item.id).ToList();
                foreach (var sss in sc)
                {
                    var scItem = new AreasDto();
                    scItem.value = sss.id;
                    scItem.label = sss.name;

                    var scChild = new List<AreasDtoTh>();
                    var th = model3.Where(p => p.parentId == sss.id).ToList();
                    foreach (var itsmth in th)
                    {
                        scChild.Add(new AreasDtoTh()
                        {
                            label = itsmth.name,
                            value = itsmth.id
                        });
                    }
                    scItem.children = scChild;
                    fsChild.Add(scItem);
                }

                child.children = fsChild;
                childList.Add(child);
            }
            return childList;
        }


    }
}
