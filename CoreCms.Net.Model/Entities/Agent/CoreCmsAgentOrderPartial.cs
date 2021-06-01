/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/
using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    /// 分销商订单记录表
    /// </summary>
    public partial class CoreCmsAgentOrder
    {

        /// <summary>
        /// 购买人昵称
        /// </summary>
        [Display(Name = "购买人昵称")]
        [SugarColumn(IsIgnore = true)]
        public System.String buyUserNickName { get; set; }

        /// <summary>
        /// 分销商
        /// </summary>
        [Display(Name = "分销商")]
        [SugarColumn(IsIgnore = true)]
        public System.String distributorName { get; set; }

    }
}
