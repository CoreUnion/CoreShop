/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/6/21 1:16:08
 *        Description: 暂无
 ***********************************************************************/

using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    /// 接龙活动商品表
    /// </summary>
    public partial class CoreCmsSolitaireItems
    {

        /// <summary>
        /// 关联货品
        /// </summary>
        [Display(Name = "关联货品")]
        [SqlSugar.SugarColumn(IsIgnore = true)]
        public CoreCmsProducts productObj { get; set; }

        /// <summary>
        /// 关联商品
        /// </summary>
        [Display(Name = "关联商品")]
        [SqlSugar.SugarColumn(IsIgnore = true)]
        public CoreCmsGoods goodObj { get; set; }

    }
}
